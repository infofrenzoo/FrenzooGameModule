using System.Collections;
using UnityEngine;

namespace Common
{
    public abstract class Vector3Animation : BaseAnimation
    {
        public float duration = 0;
        public Vector3Properties properties;
        public Transform target;
        public bool loop = false;
        public bool lockEndValue = true;
        public AnimationEvent OnAnimationEnd = null;

        protected override void Awake()
        {
            base.Awake();

            if ( target == null)
            {
                target = this.transform;
            }
        }
    }

    [System.Serializable]
    public struct Vector3Properties
    {
        public Vector3 startValue, endValue;
        public AnimationCurve bounceCurve;
    }
}
