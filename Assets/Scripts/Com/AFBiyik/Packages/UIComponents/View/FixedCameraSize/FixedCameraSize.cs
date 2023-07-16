using UnityEngine;

namespace Com.AFBiyik.UIComponents.View
{
    /// <summary>
    /// Fixes camera's size to given size.
    /// <br/>
    /// If vertical camera height = longSize.
    /// <br/>
    /// If horizontal orientation camera width = longSize.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class FixedCameraSize : MonoBehaviour
    {
        // Serialize Fields
        [SerializeField]
        private float longSize = 16;

        // Private Fields
        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            SetCameraSize();
        }

#if UNITY_EDITOR
        private void Update()
        {
            SetCameraSize();
        }
#endif

        /// <summary>
        /// Sets camera size
        /// </summary>
        private void SetCameraSize()
        {
            // Get aspect
            float aspect = cam.aspect;

            // If horizontal orientation
            if (aspect > 1)
            {
                // Calculate height
                float height = longSize / aspect;
                // Set orthographicSize
                cam.orthographicSize = height / 2f;
            }
            // If vertical orientation
            else
            {
                // Set orthographicSize
                cam.orthographicSize = longSize / 2f;
            }
        }
    }
}
