using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Animation
{
    public class ImageFillAnimation : BaseAnimation
    {
        [SerializeField] protected float duration = 0;
        [SerializeField] public float startValue, endValue;
        [SerializeField] protected AnimationCurve bounceCurve;
        [SerializeField] protected Image target;
        [SerializeField] protected bool loop = false;
        [SerializeField] protected bool lockToEndValue = false;

        public override IEnumerator Animate()
        {
            target.fillAmount = startValue;

            while ( delayTimer < initialDelay )
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                while ( timer < 1 )
                {
                    target.fillAmount = Mathf.Lerp( startValue, endValue, bounceCurve.Evaluate( timer ) );
                    timer += Time.deltaTime / duration;
                    yield return waitForEndOfFrame;
                }

                timer = 0;
                if( lockToEndValue )
                {
                    target.fillAmount = endValue;
                }

            } while ( loop && this.enabled );
        }
    }
}
