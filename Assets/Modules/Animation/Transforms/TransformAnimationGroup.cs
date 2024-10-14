using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class TransformAnimationGroup : MonoBehaviour
    {
        public Transform target = null;
        public TransformPropertiesName[] TransformProperties;
        private TransformAnimation transformAnimation = null;

        public void Awake()
        {
            transformAnimation = GetComponent<TransformAnimation>();
        }

        public void Play( string name, AnimationEvent onAnimationEnd = null )
        {
            for( int i = 0 ; i < TransformProperties.Length ; ++i )
            {
                if( TransformProperties[ i ].name == name )
                {
                    SetupAnimation( TransformProperties[ i ].properties, onAnimationEnd );
                    transformAnimation.Play();
                    return;
                }
            }
        }

        private void SetupAnimation( TransformProperties properties, AnimationEvent onAnimationEnd )
        {
            transformAnimation = transformAnimation ?? gameObject.AddComponent<TransformAnimation>();
            transformAnimation.properties = properties;
            transformAnimation.target = target;
            transformAnimation.OnAnimationEnd = onAnimationEnd;
        }
    }

    [System.Serializable]
    public struct TransformPropertiesName
    {
        public string name;
        public TransformProperties properties;
    }
}
