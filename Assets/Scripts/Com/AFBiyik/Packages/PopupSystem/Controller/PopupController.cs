using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.AFBiyik.AssetSystem;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Com.AFBiyik.PopupSystem
{
    public class PopupController : IPopupController
    {
        // Protected Readonly Properties
        protected readonly IAssetLoader assetLoader;
        protected readonly IFactory<GameObject, Transform, GameObject> prefabFactory;
        protected readonly Dictionary<string, Stack<IPopup>> popupCache = new Dictionary<string, Stack<IPopup>>();
        protected readonly Stack<IPopup> popupStack = new Stack<IPopup>();
        protected readonly Subject<PopupData> onPopupClose = new Subject<PopupData>();
        protected readonly Subject<PopupData> onPopupOpen = new Subject<PopupData>();

        // Protected Properties
        protected Transform popupParent;
        protected PopupConfig popupConfig;

        // Public Properties

        /// <inheritdoc/>
        public IObservable<PopupData> OnPopupClose => onPopupClose;
        /// <inheritdoc/>
        public IObservable<PopupData> OnPopupOpen => onPopupOpen;

        // Constructor
        public PopupController(IAssetLoader assetLoader, IFactory<GameObject, Transform, GameObject> prefabFactory)
        {
            this.assetLoader = assetLoader;
            this.prefabFactory = prefabFactory;

            SceneManager.sceneLoaded += OnSceneLoaded;

            Initialize();
        }

        // Protected Methods

        /// <summary>
        /// Async initialize method. Called in constructor.
        /// </summary>
        protected virtual async void Initialize()
        {
            popupConfig = await PopupConfig.Load(assetLoader);

            var go = popupConfig.PopupCanvas;
            var canvas = GameObject.Instantiate(go);
            GameObject.DontDestroyOnLoad(canvas);
            popupParent = canvas.transform;
        }

        /// <summary>
        /// Called when scene changes. See <see cref="SceneManager.sceneLoaded"/>.
        /// Closes active popups on scene change.
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            CloseAll();
        }

        /// <summary>
        /// Adds popup to cache.
        /// </summary>
        /// <param name="popup"></param>
        protected virtual void AddToCache(IPopup popup)
        {
            if (!popupCache.ContainsKey(popup.PopupKey))
            {
                popupCache[popup.PopupKey] = new Stack<IPopup>();
            }

            popupCache[popup.PopupKey].Push(popup);
        }

        /// <summary>
        /// If cache contains popup, gets first popup from cache.
        /// </summary>
        /// <param name="key">Key of popup</param>
        /// <param name="popup">If cache has popup, returns popup. Otherwise null</param>
        /// <returns>True popup is retrieved.</returns>
        protected virtual bool PopFromCache(string key, out IPopup popup)
        {
            popup = null;

            if (popupCache.ContainsKey(key))
            {
                if (popupCache[key].Count > 0)
                {
                    popup = popupCache[key].Pop();

                    if (popupCache[key].Count == 0)
                    {
                        popupCache.Remove(key);
                    }

                    return true;
                }
                else
                {
                    if (popupCache[key].Count == 0)
                    {
                        popupCache.Remove(key);
                    }
                }
            }

            return false;
        }

        // Public Methods

        /// <inheritdoc/>
        public virtual async void Close(Hashtable args = null)
        {
            if (popupStack.Count == 0)
            {
                Debug.LogError("You try to close popup. But stack is empty.");
                return;
            }

            IPopup popupToClose = popupStack.Pop();

            await popupToClose.Close();

            AddToCache(popupToClose);

            IPopup topPopup = null;
            if (popupStack.Count > 0)
            {
                topPopup = popupStack.Peek();
                topPopup.OnRefocus(args);
            }

            onPopupClose.OnNext(new PopupData(topPopup, popupToClose));
        }

        /// <inheritdoc/>
        public virtual void CloseAll()
        {
            while (popupStack.Count > 0)
            {
                Close();
            }
        }

        /// <inheritdoc/>
        public virtual void Open(string key, Hashtable args = null)
        {
            CloseAll();
            OpenTop(key, args);
        }

        /// <inheritdoc/>
        public virtual async void OpenTop(string key, Hashtable args = null)
        {
            try
            {
                IPopup prevPopup = null;
                if (popupStack.Count > 0)
                {
                    prevPopup = popupStack.Peek();
                    prevPopup.OnUnfocus();
                }

                IPopup popupToOpen;
                if (!PopFromCache(key, out popupToOpen))
                {
                    var prefab = await assetLoader.LoadPrefab(key);

                    var go = prefabFactory.Create(prefab, popupParent);
                    popupToOpen = go.GetComponent<IPopup>();
                }

                popupStack.Push(popupToOpen);

                await popupToOpen.Show(args);

                onPopupOpen.OnNext(new PopupData(popupToOpen, prevPopup));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <inheritdoc/>
        public virtual void SendMessage(string key, Hashtable args)
        {
            var popups = popupStack.Where(p => p.PopupKey == key).ToList();

            foreach (var popup in popups)
            {
                popup.ProcessMessage(args);
            }
        }

        /// <inheritdoc/>
        public virtual bool IsPopupActive(string key)
        {
            return popupStack.Any(el => el.PopupKey == key);
        }

        /// <inheritdoc/>
        public virtual bool HasPopup()
        {
            return popupStack.Count > 0;
        }

        /// <inheritdoc/>
        public virtual void ClearCache()
        {
            foreach (var stack in popupCache.Values)
            {
                foreach (var popup in stack)
                {
                    GameObject.Destroy(popup.gameObject);
                }
            }

            popupCache.Clear();
        }

        /// <inheritdoc/>
        public virtual void ClearStack()
        {
            foreach (var activePopup in popupStack)
            {
                GameObject.Destroy(activePopup.gameObject);
            }

            popupStack.Clear();
        }

        /// <inheritdoc/>
        public virtual void ClearAll()
        {
            ClearStack();
            ClearCache();
        }

        /// <inheritdoc/>
        public async UniTask PreloadPopups(IList<string> keys)
        {
            UniTask[] tasks = new UniTask[keys.Count];

            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                var t = PreloadPopup(key);
                tasks[i] = t;
            }

            await UniTask.WhenAll(tasks);
        }

        /// <inheritdoc/>
        public async UniTask PreloadPopup(string key)
        {
            IPopup popupToCache;
            if (!PopFromCache(key, out popupToCache))
            {
                var prefab = await assetLoader.LoadPrefab(key);

                var go = prefabFactory.Create(prefab, popupParent);
                popupToCache = go.GetComponent<IPopup>();
                popupToCache.gameObject.SetActive(false);
                AddToCache(popupToCache);
            }
        }
    }
}
