using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class AnimationProfile : MonoBehaviour
    {
        public AnimationGroup[] animationGroups = null;

        private AnimationEvent animationEvent = null;
        private Coroutine callAnimationEnd = null;
        
        public void Play( string name, AnimationEvent onAnimationEnd = null )
        {
            this.animationEvent = onAnimationEnd;

            for ( int i = 0 ; i < animationGroups.Length ; ++i )
            {
                AnimationGroup ag = animationGroups[ i ];

                if ( ag.name == name )
                {
                    for( int j = 0 ; j < ag.animations.Length ; ++j )
                    {
                        ag.animations[ j ].Play();
                    }

                    if ( callAnimationEnd != null )
                    {
                        StopCoroutine( callAnimationEnd );
                    }

                    ag.animationEvent.RemoveListener( OnAnimationEnd );
                    ag.animationEvent.AddListener( OnAnimationEnd );

                    callAnimationEnd = StartCoroutine( CallAnimationEnd( ag.animationEvent, ag.eventDelay ) );
                    return;
                }
            }
        }

        private IEnumerator CallAnimationEnd( UnityEvent animEvent, float seconds )
        {
            yield return new WaitForSeconds( seconds );

            if ( animEvent != null )
            {
                animEvent.Invoke();
            }
        }

        private void OnAnimationEnd()
        {
            if ( animationEvent != null )
            {
                animationEvent.Invoke();
            }
        }
    }

    [System.Serializable]
    public struct AnimationGroup
    {
        public string name;
        public BaseAnimation[] animations;
        public float eventDelay;
        public UnityEvent animationEvent;
    }
}
