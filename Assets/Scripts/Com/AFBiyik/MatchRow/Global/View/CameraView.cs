using UnityEngine;

namespace Com.AFBiyik.MatchRow.Global.View
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        [SerializeField]
        private float longSize = 16;

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

        private void SetCameraSize()
        {
            var aspect = cam.aspect;

            if (aspect > 1)
            {
                float height = longSize / aspect;
                cam.orthographicSize = height / 2f;
            }
            else
            {
                cam.orthographicSize = longSize / 2f;
            }
        }
    }
}
