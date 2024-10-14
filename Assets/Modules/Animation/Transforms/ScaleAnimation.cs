using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class ScaleAnimation : Vector3Animation
    {
        public override IEnumerator Animate()
        {
            target.localScale = properties.startValue;
            while (delayTimer < initialDelay)
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }
            target.localScale = properties.startValue;

            do
            {
                while (timer < 1f)
                {
                    target.localScale = OverLerp.Lerp( properties.bounceCurve.Evaluate(timer), properties.startValue, properties.endValue );
                    timer += Time.deltaTime / duration;
                    yield return waitForEndOfFrame;
                }

                timer = 0;
                target.localScale = properties.endValue;

            } while ( loop && this.enabled);

            yield return null;
        }
    }
}
