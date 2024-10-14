using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class AnchorPointsAnimations : Vector3Animation
    {
        public List<Vector2> points;
        public float delayBetweenPoints = 0f;

        RectTransform rectTransform = null;

        public override void Play()
        {
            if ( target != null )
            {
                rectTransform = ( RectTransform )target;
            }
            base.Play();
        }

        public override IEnumerator Animate()
        {
            rectTransform.anchoredPosition = properties.startValue;
            var pointsWait = new WaitForSeconds( delayBetweenPoints );

            while ( delayTimer < initialDelay )
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            do
            {
                if( points.Count < 2 )
                {
                    break;
                }

                for ( int i = 1 ; i < points.Count ; i++ )
                {
                    timer = 0;
                    var startPoint = points[ i - 1 ];
                    var endPoint = points[ i ];
                    while ( timer < 1 )
                    {
                        rectTransform.anchoredPosition = OverLerp.Lerp( properties.bounceCurve.Evaluate( timer ), startPoint, endPoint );
                        timer += Time.deltaTime / duration;
                        yield return waitForEndOfFrame;
                    }

                    yield return pointsWait;
                    rectTransform.anchoredPosition = endPoint;
                }

            } while ( loop && this.enabled );

            yield return null;

            if ( OnAnimationEnd != null )
            {
                OnAnimationEnd.Invoke();
                OnAnimationEnd = null;
            }
        }
    }
}
