using System.Collections;
using UnityEngine;

namespace Common
{
    public delegate void AnimationEvent();
    public delegate void AnimationEvent<T>();

    public abstract class BaseAnimation : MonoBehaviour, IAnimation
    {
        [SerializeField] protected bool playOnEnable = false;
        [SerializeField] public float initialDelay = 0;
        protected WaitForEndOfFrame waitForEndOfFrame = null;

        protected float timer;
        protected float delayTimer;

        protected virtual void Awake()
        {
            waitForEndOfFrame = new WaitForEndOfFrame();
        }

        protected virtual void OnEnable()
        {
            if (playOnEnable)
            {
                Play();
            }
        }

        public virtual void Play()
        {
            Stop();
            if ( gameObject.activeInHierarchy )
            {
                StartCoroutine( Animate() );
            }
        }
        public virtual void Reset()
        {
        }

        public virtual void Stop()
		{
			this.timer = 0;
            this.delayTimer = 0;
            StopCoroutine(Animate());
        }

        public virtual IEnumerator Animate()
        {
            yield return null;
        }
    }
}
