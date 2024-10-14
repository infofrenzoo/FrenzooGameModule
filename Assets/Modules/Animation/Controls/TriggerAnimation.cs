using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
	public class TriggerAnimation : BaseAnimation {

        [SerializeField] protected Animator animator;
        [SerializeField] protected string stateName;
        [SerializeField] protected float speed = 1.0f;
        
		public override void Stop()
		{
			this.delayTimer = 0;

			this.animator.Play(stateName, -1, 0f);
			this.animator.speed = 0.0f;
		}

		public override IEnumerator Animate()
		{
			while (delayTimer < initialDelay)
			{
				delayTimer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

			this.animator.speed = speed;

			yield return null;
		}
	}
}