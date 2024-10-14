using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class AnchorPositionAnimation : Vector3Animation
    {
        RectTransform rectTransform = null;

        public override void Play()
        {
            if( target != null )
            {
                rectTransform = ( RectTransform )target;
            }
            base.Play();
        }

        public override IEnumerator Animate()
        {
            rectTransform.anchoredPosition = properties.startValue;

            while (delayTimer < initialDelay)
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                while (timer < 1)
                {
                    rectTransform.anchoredPosition = OverLerp.Lerp( properties.bounceCurve.Evaluate(timer), properties.startValue, properties.endValue );
                    timer += Time.deltaTime / duration;
                    yield return waitForEndOfFrame;
                }
                
                timer = 0;
                rectTransform.anchoredPosition = properties.endValue;

            } while ( loop && this.enabled );

            yield return null;
        }
    }
}
