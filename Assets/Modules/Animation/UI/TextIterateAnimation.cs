using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Common.Animation
{
    public class TextIterateAnimation : BaseAnimation
    {
        [SerializeField] protected float duration = 0;
        [SerializeField] protected AnimationCurve bounceCurve;
        [SerializeField] protected TMP_Text target;

        private string originalText;
        private float actualDuration = 0;

        public bool isAnimating { get; private set; }

        public override IEnumerator Animate()
        {
            isAnimating = true;
            originalText = target.text;
            actualDuration = duration;

            while ( delayTimer < initialDelay )
            {
                delayTimer += Time.deltaTime;
                yield return waitForEndOfFrame;
            }

            target.text = "";

            StringBuilder sb = new StringBuilder();
            
            while ( timer < 1 )
            {
                float t = OverLerp.Lerp( bounceCurve.Evaluate( timer ), 0, 1 );
                target.text = originalText.Substring( 0, Mathf.FloorToInt( t * originalText.Length ) );
                timer += Time.deltaTime / actualDuration;
                yield return waitForEndOfFrame;
            }

            isAnimating = false;
            target.text = originalText;

            yield return null;
        }

        public void SkipAnimation()
        {
            actualDuration = 0.01f;
            target.text = originalText;
        }
    }
}
