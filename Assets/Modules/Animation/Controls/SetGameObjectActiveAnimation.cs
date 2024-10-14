using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class SetGameObjectActiveAnimation : BaseAnimation
    {
        [SerializeField] protected List<GameObject> target;
        [SerializeField] protected bool endState = true;

        public override IEnumerator Animate()
        {
            for (int i = 0; i < target.Count; i++)
            {
                target[i].SetActive(!endState);
            }

            while (delayTimer < initialDelay)
            {
                delayTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }


            for (int i = 0; i < target.Count; i++)
            {
                target[i].SetActive(endState);
            }

            yield return null;
        }
    }
}


