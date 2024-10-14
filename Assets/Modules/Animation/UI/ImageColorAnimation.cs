using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Animation
{
    public class ImageColorAnimation : BaseAnimation
    {
        [SerializeField] protected float duration = 0;
        [SerializeField] public Color startValue, endValue;
        [SerializeField] protected AnimationCurve bounceCurve;
        [SerializeField] protected Image target;
        [SerializeField] protected bool loop = false;
        [SerializeField] protected bool lockToEndValue = false;

        private bool isPlaying = false;

        public override void Reset()
        {
            StopCoroutine( Animate() );
            target.color = startValue;
        }

        public override void Play()
        {
            Reset();
            base.Play();
        }

        public override IEnumerator Animate()
        {
            if ( isPlaying ) yield break;

            isPlaying = true;
            target.color = startValue;

            while ( delayTimer < initialDelay )
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            yield return null;

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

            isPlaying = false;
        }
    }
}
