using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Com.AFBiyik.MatchRow.Global.Util
{
    public class NumberTween
    {
        public static async UniTask TweenInt(int from, int to, float time, Action<int> onUpdate)
        {
            int value = from;
            await DOTween.To(() => value, (int newValue) => value = newValue, to, time)
                .OnUpdate(() => onUpdate?.Invoke(value));
        }

        public static async UniTask TweenFloat(float from, float to, float time, Action<float> onUpdate)
        {
            float value = from;
            await DOTween.To(() => value, (float newValue) => value = newValue, to, time)
                .OnUpdate(() => onUpdate?.Invoke(value));
        }
    }
}
