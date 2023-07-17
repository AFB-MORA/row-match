using UnityEngine;

namespace Com.AFBiyik.MatchRow.Global.Util
{
    public static class VectorExtensions
    {
        public static Vector3 ScreenToWorldPosition(this Vector2 screenPosition)
        {
            Camera camera = Camera.main;
            Vector3 screenPosition3 = screenPosition;
            screenPosition3.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(screenPosition3);
        }
    }
}