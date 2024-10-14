using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class PositionAnimation : Vector3Animation
    {
        public override IEnumerator Animate()
        {
            target.localPosition = properties.startValue;

            while (delayTimer < initialDelay)
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                while (timer < 1)
                {
                    target.localPosition = OverLerp.Lerp( properties.bounceCurve.Evaluate(timer), properties.startValue, properties.endValue );
                    timer += Time.deltaTime / duration;
                    yield return waitForEndOfFrame;
                }
                
                timer = 0;
                target.localPosition = properties.endValue;

            } while ( loop && this.enabled );

            yield return null;
        }
    }
}
