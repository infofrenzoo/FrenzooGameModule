using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Animation
{
    public class RawImageRectAnimation : BaseAnimation
    {
        [SerializeField] protected float duration = 0;
        [SerializeField] public Vector4 startValue, endValue;
        [SerializeField] protected AnimationCurve bounceCurve;
        [SerializeField] protected RawImage target;
        [SerializeField] protected bool loop = false;
        [SerializeField] protected bool lockToEndValue = false;

        public override IEnumerator Animate()
        {
            target.uvRect = new Rect( startValue.x, startValue.y, startValue.z, startValue.w );

            while ( delayTimer < initialDelay )
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                while ( timer < 1 )
                {
                    float x = OverLerp.Lerp( bounceCurve.Evaluate( timer ), startValue.x, endValue.x );
                    float y = OverLerp.Lerp( bounceCurve.Evaluate( timer ), startValue.y, endValue.y );
                    float z = OverLerp.Lerp( bounceCurve.Evaluate( timer ), startValue.z, endValue.z );
                    float w = OverLerp.Lerp( bounceCurve.Evaluate( timer ), startValue.w, endValue.w );
                    target.uvRect = new Rect( x, y, z, w );
                    timer += Time.deltaTime / duration;
                    yield return waitForEndOfFrame;
                }

                timer = 0;
                if ( lockToEndValue )
                {
                    target.uvRect = new Rect( endValue.x, endValue.y, endValue.z, endValue.w );
                }

            } while ( loop && this.enabled );
        }
    }
}
