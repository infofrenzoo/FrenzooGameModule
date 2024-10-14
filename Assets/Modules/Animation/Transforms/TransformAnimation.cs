using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    [System.Serializable]
    public class TransformAnimation : BaseAnimation
    {
        public TransformProperties properties;
        public Transform target = null;
        public bool loop = false;
        public AnimationEvent OnAnimationEnd = null;

        public override void Play()
        {
            Stop();
            StartCoroutine( Animate() );
        }

        public override IEnumerator Animate()
        {
            while ( delayTimer < initialDelay )
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                while ( timer < 1 )
                {
                    target.localPosition = OverLerp.Lerp( properties.position.bounceCurve.Evaluate( timer ), properties.position.startValue, properties.position.endValue );
                    target.localRotation = Quaternion.Euler( OverLerp.Lerp( properties.rotation.bounceCurve.Evaluate( timer ), properties.rotation.startValue, properties.rotation.endValue ) );
                    target.localScale = OverLerp.Lerp( properties.scale.bounceCurve.Evaluate( timer ), properties.scale.startValue, properties.scale.endValue );
                    timer += Time.deltaTime / properties.duration;
                    yield return waitForEndOfFrame;
                }

                timer = 0;
                target.localPosition = properties.position.endValue;
                target.localRotation = Quaternion.Euler( properties.rotation.endValue );
                target.localScale = properties.scale.endValue;

            } while ( loop && this.enabled );

            yield return null;

            if( OnAnimationEnd != null )
            {
                OnAnimationEnd.Invoke();
                OnAnimationEnd = null;
            }
        }
    }


    [System.Serializable]
    public struct TransformProperties
    {
        public float duration;
        public Vector3Properties position;
        public Vector3Properties rotation;
        public Vector3Properties scale;
    }
}
