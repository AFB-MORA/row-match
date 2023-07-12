using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
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
        protected SpriteRenderer spriteRenderer;
        protected IRecycleViewDataSource dataSource;

        // Private Fields
        // For scroll
        private Vector2 prevPos;
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

        protected SpriteRenderer SpriteRenderer
        {
            get
            {
                if (spriteRenderer == null)
                {
                    spriteRenderer = GetComponent<SpriteRenderer>();
                }

                return spriteRenderer;
            }
        }

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
            // Set collideer
            BoxCollider.size = RectTransform.sizeDelta;
            BoxCollider.offset = new Vector2(0, 0);
            // Set sprite rendere
            SpriteRenderer.size = RectTransform.sizeDelta;
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
        /// Creates initial items for scroll
        /// </summary>
        protected virtual void CreateItems()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            float height = RectTransform.rect.height;
            float itemHeight = dataSource.ItemHeight;

            visibleItemCount = Mathf.CeilToInt(height / (itemHeight + spacing)) + 1;
            firstItemIndex = -1;
            cells = new List<GameObject>();

            for (int i = 0; i < visibleItemCount; i++)
            {
                var view = dataSource.CreateView();
                cells.Add(view);
                RectTransform rt = (RectTransform)view.transform;
                rt.SetParent(content, false);
                float position = i * itemHeight + padding.x + (i == 0 ? 0 : i - 1 * spacing);
                rt.anchoredPosition = new Vector2(0, -position);
            }

            CheckItems();
        }

        /// <summary>
        /// Checks and updates items when scroll
        /// </summary>
        protected virtual void CheckItems()
        {
            float contentPos = content.anchoredPosition.y;

            int itemIndex = Mathf.FloorToInt((contentPos - padding.y) / (dataSource.ItemHeight + spacing));

            if (firstItemIndex != itemIndex)
            {
                float itemHeight = dataSource.ItemHeight;
                firstItemIndex = itemIndex;

                for (int i = 0; i < visibleItemCount; i++)
                {
                    var view = cells[i];
                    RectTransform rt = (RectTransform)view.transform;

                    int index = firstItemIndex + i;
                    dataSource.SetView(view, index);
                    float position = index * itemHeight + padding.x + (i == 0 ? 0 : i - 1 * spacing);
                    rt.anchoredPosition = new Vector2(0, -position);
                }
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
            prevPos = GetPointerPosition(eventData);
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
            float diff = pos.y - prevPos.y;
            // Update previous position
            prevPos = pos;

            // Update position
            content.anchoredPosition += new Vector2(0, diff);
            // Update time
            pevTimeStamp = DateTime.Now;

            CheckItems();

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
            float posDiff = pos.y - prevPos.y;
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
            if (!Application.isPlaying)
            {
                return;
            }

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
                    content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
                }
            }
        }
    }
}
