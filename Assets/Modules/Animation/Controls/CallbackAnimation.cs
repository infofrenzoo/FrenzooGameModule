using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common.Animation
{
	public class CallbackAnimation : BaseAnimation {
		
		public UnityEvent callback;
		public UnityEvent stopCallback;

		public override void Stop()
        {
            base.Stop();

			if (this.stopCallback != null)
			{
				this.stopCallback.Invoke();
			}
		}

		public override IEnumerator Animate()
		{
			while (delayTimer < initialDelay)
			{
				delayTimer += Time.deltaTime;
				yield return waitForEndOfFrame;
			}

			this.callback.Invoke();
			yield return null;
		}
	}
}
