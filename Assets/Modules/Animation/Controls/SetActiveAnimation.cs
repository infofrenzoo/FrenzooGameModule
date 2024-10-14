using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class SetActiveAnimation : BaseAnimation
    {
        [SerializeField] protected List<RectTransform> target;
        [SerializeField] protected bool endState = true;

        public override IEnumerator Animate()
        {
            for (int i = 0; i < target.Count; i++)
            {
                target[i].gameObject.SetActive(!endState);
            }

            while (delayTimer < initialDelay)
            {
                delayTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }


            for (int i = 0; i < target.Count; i++)
            {
                target[i].gameObject.SetActive(endState);
            }

            yield return null;
        }
    }
}
