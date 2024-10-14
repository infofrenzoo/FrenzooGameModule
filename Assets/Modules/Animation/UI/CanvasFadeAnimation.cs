using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Animation
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFadeAnimation : BaseAnimation
    {
        [SerializeField] protected float duration = 0;
        [SerializeField] public float startValue, endValue;
        [SerializeField] protected AnimationCurve bounceCurve;
        [SerializeField] protected CanvasGroup target;
        [SerializeField] protected bool loop = false;

        public override IEnumerator Animate()
        {
            target.alpha = startValue;

            while (delayTimer < initialDelay)
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                while (timer < 1)
                {
                    target.alpha = OverLerp.Lerp(bounceCurve.Evaluate(timer), startValue, endValue);
                    timer += Time.deltaTime / duration;
                    yield return waitForEndOfFrame;
                }

                timer = 0;
                target.alpha = endValue;

            } while (loop && this.enabled);

            yield return null;
        }
    }
}
