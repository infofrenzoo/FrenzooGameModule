using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class RotationAnimation : Vector3Animation
    {
        public override IEnumerator Animate()
        {
            target.localRotation  = Quaternion.Euler( properties.startValue );

            while (delayTimer < initialDelay)
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                while (timer < 1)
                {
                    target.localRotation = Quaternion.Euler(OverLerp.Lerp( properties.bounceCurve.Evaluate(timer), properties.startValue, properties.endValue ));
                    timer += Time.deltaTime / duration;
                    yield return waitForEndOfFrame;
                }

                timer = 0;
                target.localRotation = Quaternion.Euler( properties.endValue );

            } while ( loop && this.enabled);

            yield return null;
        }
    }
}
