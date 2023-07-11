using UnityEngine;

namespace Com.AFBiyik.MatchRow.View
{
    /// <summary>
    /// Calculates bounds responsively.
    /// <br/>
    /// Similar with rect transform strech.
    /// </summary>
    [ExecuteAlways]
    public class BoundsView : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField, Tooltip("Left, Right, Top, Bottom")]
        protected Vector4 padding;

        // Private Fields
        protected Rect? rect;

        // Public Properties
        public Rect Rect => rect ??= GetRect();

        /// <summary>
        /// Calculates rect with screen aspect.
        /// </summary>
        /// <returns></returns>
        protected virtual Rect GetRect()
        {
            // Get cam
            Camera cam = Camera.main;

            // Get sizes
            float aspect = cam.aspect;
            float cameraHeight = cam.orthographicSize * 2;
            float cameraWidth = aspect * cameraHeight;

            // Get position
            Vector2 position = transform.localPosition;

            // Calculate size by camera and padding
            Vector2 size = new Vector2(cameraWidth - padding.x - padding.y,
                cameraHeight - padding.z - padding.w);

            // Calculate offset by posiyion and padding
            Vector2 offset = position + new Vector2(padding.x / 2f - padding.y / 2f,
                padding.w / 2f - padding.z / 2f);

            // Return rect
            return new Rect(offset, size);
        }

#if UNITY_EDITOR
        protected virtual void Update()
        {
            if (!Application.isPlaying)
            {
                // Update rect.
                // Editor only
                rect = GetRect();
            }
        }

        protected virtual void OnDrawGizmos()
        {
            // Draw bounds
            Vector2 size2 = Rect.size / 2;

            Vector2 topLeft = Rect.position + new Vector2(-size2.x, size2.y);
            Vector2 topRight = Rect.position + new Vector2(size2.x, size2.y);
            Vector2 bottomLeft = Rect.position + new Vector2(-size2.x, -size2.y);
            Vector2 bottomRight = Rect.position + new Vector2(size2.x, -size2.y);

            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(topLeft, 0.2f);
            Gizmos.DrawSphere(topRight, 0.2f);
            Gizmos.DrawSphere(bottomLeft, 0.2f);
            Gizmos.DrawSphere(bottomRight, 0.2f);

            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
#endif
    }
}