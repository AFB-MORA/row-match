using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.AFBiyik.UIComponents
{
    /// <summary>
    /// Scroll view with recycling cells.
    /// <br/>
    /// Improvements:
    /// - Horizontal layout
    /// - Check runtime data source updates
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class RecycleView : UIBehaviour,
        IDragHandler, IBeginDragHandler, IEndDragHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        /// Serialize Fields
        [SerializeField]
        protected RectTransform content;
        [SerializeField]
        protected float spacing;
        [SerializeField, Tooltip("Top, Bottom")]
        protected Vector2 padding;
        [SerializeField]
        private float bounceSpeed = 15;
        [SerializeField]
        protected float decelerationRate = 0.135f;
        [SerializeField]
        protected float scrollLimit = 2f;

        // Protected Fields
        protected RectTransform rectTransform;
        protected BoxCollider2D boxCollider;
        protected IRecycleViewDataSource dataSource;

        // Private Fields
        // For scroll
        private Vector2 prevPointerPos;
        private bool isDragging;
        private int firstItemIndex;
        private int visibleItemCount;
        private List<GameObject> cells;

        // For spring
        private int springDirection; // -1 down, 1 up, 0 none

        // For inertia
        private DateTime pevTimeStamp;
        private float inertiaSpeed;
        private int inertiaDirection;

        // Protected Properties
        protected RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }

                return rectTransform;
            }
        }

        protected BoxCollider2D BoxCollider
        {
            get
            {
                if (boxCollider == null)
                {
                    boxCollider = GetComponent<BoxCollider2D>();
                }

                return boxCollider;
            }
        }

        // Public Properties
        /// <summary>
        /// Called first
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            SetBounds();
        }

        /// <summary>
        /// Called when rect transform changes
        /// </summary>
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            SetBounds();
        }

        /// <summary>
        /// Sets bounds for sprite renderer and collider
        /// </summary>
        private void SetBounds()
        {
            // Get size
            var size = RectTransform.rect.size;

            // Set collideer
            BoxCollider.size = size;
            BoxCollider.offset = new Vector2(0, 0);
        }

        /// <summary>
        /// Initializes recycle view
        /// </summary>
        /// <param name="dataSource"></param>
        public virtual void Initialize(IRecycleViewDataSource dataSource)
        {
            // Set data source
            this.dataSource = dataSource;

            // Get item count
            int numberOfItems = dataSource.NumberOfItems;
            // Get item height
            float itemHeight = dataSource.ItemHeight;
            // Calculate content height
            float contentHeight = numberOfItems * itemHeight + (numberOfItems - 1) * spacing + padding.x + padding.y;
            // Set content height
            content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);
            // Create Items
            CreateItems();
        }

        /// <summary>
        /// Updates content position
        /// </summary>
        /// <param name="anchoredPosition">Content anchored position</param>
        public void SetContentPosition(Vector2 anchoredPosition)
        {
            // Update position
            content.anchoredPosition = anchoredPosition;
            CheckItemPositions();
        }

        /// <summary>
        /// Creates initial items for scroll
        /// </summary>
        protected virtual void CreateItems()
        {
            // Destroy content
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            // Get height
            float height = RectTransform.rect.height;
            // Get item height
            float itemHeight = dataSource.ItemHeight;

            // Calculate visible item count
            visibleItemCount = Mathf.CeilToInt(height / (itemHeight + spacing)) + 1;
            // Set first item index
            firstItemIndex = -1;
            // Set cells
            cells = new List<GameObject>();

            // For number of visible items
            for (int i = 0; i < visibleItemCount; i++)
            {
                // Create view
                var view = dataSource.CreateView();
                cells.Add(view);

                // Set parent
                RectTransform rt = (RectTransform)view.transform;
                rt.SetParent(content, false);
            }

            CheckItemPositions();
        }

        /// <summary>
        /// Checks and updates items when scroll
        /// </summary>
        protected virtual void CheckItemPositions()
        {
            // Get current pos
            float contentPos = content.anchoredPosition.y;

            // Check bounds
            if (contentPos < 0 || content.rect.height - contentPos < RectTransform.rect.height)
            {
                return;
            }

            // Get item index
            int itemIndex = Mathf.Max(0,
                Mathf.FloorToInt((contentPos - padding.x) / (dataSource.ItemHeight + spacing)));

            // Check item index
            if (firstItemIndex == itemIndex)
            {
                return;
            }

            // Update item index
            firstItemIndex = itemIndex;

            // Get item height
            float itemHeight = dataSource.ItemHeight;

            // For each visible items
            for (int i = 0; i < visibleItemCount; i++)
            {
                SetItemPosition(itemHeight, i);
            }
        }

        /// <summary>
        /// Sets position of the item
        /// </summary>
        /// <param name="itemHeight"><see cref="dataSource.ItemHeight"/></param>
        /// <param name="i">Cell index</param>
        private void SetItemPosition(float itemHeight, int i)
        {
            // Get view
            var view = cells[i];
            // Get index
            int index = firstItemIndex + i;

            // Check index is valid
            if (index < dataSource.NumberOfItems)
            {
                view.name = $"index ({index})";
                // Activate view
                view.SetActive(true);

                // Get rect transform
                RectTransform rt = (RectTransform)view.transform;

                // Update data
                dataSource.SetView(view, index);

                // Set position
                // height + padding + spacing
                float position = index * itemHeight +
                    padding.x +
                    (index == 0 ? 0 : index * spacing);

                // Set position
                rt.anchoredPosition = new Vector2(0, -position);
            }
            else
            {
                // Deactivate view
                view.SetActive(false);
            }
        }

        /// <summary>
        /// Called when pointer up
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            // For pointer down
        }

        /// <summary>
        /// Called when pointer down
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            // Reset Spring
            springDirection = 0;
            // Reset inertia
            inertiaSpeed = 0;
        }

        /// <summary>
        /// Called when drag begins
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            // Get previous position
            prevPointerPos = GetPointerPosition(eventData);
            // Update time
            pevTimeStamp = DateTime.Now;
            // Start dragging
            isDragging = true;
            // Reset spring direction
            springDirection = 0;
            // Reset inertia
            inertiaSpeed = 0;
        }

        /// <summary>
        /// Called when drag
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnDrag(PointerEventData eventData)
        {
            // Get position
            var pos = GetPointerPosition(eventData);
            // Get position difference
            float diff = pos.y - prevPointerPos.y;
            // Update previous position
            prevPointerPos = pos;

            // Update position
            content.anchoredPosition += new Vector2(0, diff);
            // Update time
            pevTimeStamp = DateTime.Now;

            CheckItemPositions();

        }

        /// <summary>
        /// Called when drag ends
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnEndDrag(PointerEventData eventData)
        {
            // Get position
            var pos = GetPointerPosition(eventData);
            // Get position difference
            float posDiff = pos.y - prevPointerPos.y;
            // Get time
            var time = DateTime.Now;
            // Get time difference
            float timeDiff = (float)(time - pevTimeStamp).TotalSeconds;
            // Calculate inertia speed and direction
            inertiaSpeed = Mathf.Abs(posDiff / timeDiff);
            inertiaDirection = (int)Mathf.Sign(posDiff);

            // Stop dragging
            isDragging = false;
        }

        /// <summary>
        /// Converts input position to world position
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        protected virtual Vector2 GetPointerPosition(PointerEventData eventData)
        {
            Vector3 screenPos = eventData.position;
            screenPos.z = Camera.main.nearClipPlane;
            return Camera.main.ScreenToWorldPoint(screenPos);
        }

        /// <summary>
        /// Called evenry frame
        /// </summary>
        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif

            CheckLimit();

            if (isDragging)
            {
                return;
            }

            ApplyInertia();
            SpringBack();
        }

        /// <summary>
        /// Check content limit
        /// </summary>
        private void CheckLimit()
        {
            // If upper limit
            if (content.anchoredPosition.y < -2)
            {
                // Stop inertia
                inertiaSpeed = 0;
                // Set position
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, -scrollLimit);
            }
            // If lower limit
            else if (content.rect.height - content.anchoredPosition.y < RectTransform.rect.height - scrollLimit)
            {
                // If content height is longer than height
                if (content.rect.height >= ((RectTransform)transform).rect.height)
                {
                    // Stop inertia
                    inertiaSpeed = 0;
                    // Set position
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.rect.height - RectTransform.rect.height + scrollLimit);
                }
                // If content height is shorter than height and down limit
                else
                {
                    // Stop inertia
                    inertiaSpeed = 0;
                    // Set position
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
                }
            }
        }

        /// <summary>
        /// Apply inertia after drag end
        /// </summary>
        private void ApplyInertia()
        {
            // if has inertia
            if (inertiaSpeed > 0)
            {
                // Update inertia speed
                inertiaSpeed *= Mathf.Pow(decelerationRate, Time.deltaTime);
                // Set position
                content.anchoredPosition += new Vector2(0, inertiaDirection * inertiaSpeed * Time.deltaTime);

                CheckItemPositions();
            }
        }

        /// <summary>
        /// Spring back to position
        /// </summary>
        private void SpringBack()
        {
            // If no spring
            if (springDirection == 0)
            {
                // If up
                if (content.anchoredPosition.y < 0)
                {
                    // Reset inertia
                    inertiaSpeed = 0;
                    // Down spring
                    springDirection = -1;
                }
                // If down
                else if (content.rect.height >= RectTransform.rect.height &&
                    content.rect.height - content.anchoredPosition.y < RectTransform.rect.height)
                {
                    // Reset inertia
                    inertiaSpeed = 0;
                    // Up spring
                    springDirection = 1;
                }
            }
            // If up spring
            else if (springDirection == -1)
            {
                if (content.anchoredPosition.y < 0)
                {
                    // Update position
                    content.anchoredPosition += new Vector2(0, bounceSpeed * Time.deltaTime);
                }
                // If up spring stop
                else
                {
                    // Stop spring
                    springDirection = 0;
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
                }
            }
            // If down spring
            else if (springDirection == 1)
            {
                if (content.rect.height - content.anchoredPosition.y < RectTransform.rect.height)
                {
                    // Update position
                    content.anchoredPosition -= new Vector2(0, bounceSpeed * Time.deltaTime);
                }
                // If down spring stop
                else
                {
                    // Stop spring
                    springDirection = 0;
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.rect.height - RectTransform.rect.height);
                }
            }
        }
    }
}
