using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Animation
{
    public class CopyTransformControlledAnimation : BaseAnimation
    {
        [SerializeField] public Transform target;
        [SerializeField] public Transform startTransform, endTransform;
        [SerializeField] protected AnimationCurve bounceCurve;
        [SerializeField] public float t = 0;

        public override IEnumerator Animate()
        {
            target.position = startTransform.position;
            target.rotation = startTransform.rotation;

            while (delayTimer < initialDelay)
            {
                delayTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            while (t <= 1 && gameObject.activeSelf)
            {
                target.position = OverLerp.Lerp(bounceCurve.Evaluate(t), startTransform.position, endTransform.position);
                target.rotation = OverLerp.Lerp(bounceCurve.Evaluate(t), startTransform.rotation, endTransform.rotation);
                yield return new WaitForEndOfFrame();
            }

            target.position = endTransform.position;
            target.rotation = endTransform.rotation;

            yield return null;
        }
    }
}
