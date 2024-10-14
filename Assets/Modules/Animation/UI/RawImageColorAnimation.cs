using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Animation
{
    public class RawImageColorAnimation : BaseAnimation
    {
        [SerializeField] protected float duration = 0;
        [SerializeField] public Color startValue, endValue;
        [SerializeField] protected AnimationCurve bounceCurve;
        [SerializeField] protected RawImage target;
        [SerializeField] protected bool loop = false;
        [SerializeField] protected bool lockToEndValue = false;

        public override IEnumerator Animate()
        {
            target.color = startValue;

            while ( delayTimer < initialDelay )
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                while ( timer < 1 )
                {
                    target.color = Color.Lerp( startValue, endValue, bounceCurve.Evaluate( timer ) );
                    timer += Time.deltaTime / duration;
                    yield return waitForEndOfFrame;
                }

                timer = 0;
                if( lockToEndValue )
                {
                    target.color = endValue;
                }

            } while ( loop && this.enabled );
        }
    }
}
