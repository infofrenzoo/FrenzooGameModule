using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class OverLerp
    {
        public static Vector3 Lerp(float evaluatedValue, Vector3 start, Vector3 end)
        {
            if (evaluatedValue >= 0 && evaluatedValue <= 1)
            {
                return Vector3.Lerp(start, end, evaluatedValue);
            }
            else if (evaluatedValue > 1)
            {
                Vector3 offsetVector = start - Vector3.Lerp(start, end, evaluatedValue - 1);
                offsetVector = new Vector3(Mathf.Abs(offsetVector.x), Mathf.Abs(offsetVector.y), Mathf.Abs(offsetVector.z));
                return end + offsetVector;
            }
            else
            {
                Vector3 offsetVector = end - Vector3.Lerp(start, end, evaluatedValue + 1);
                offsetVector = new Vector3(Mathf.Abs(offsetVector.x), Mathf.Abs(offsetVector.y), Mathf.Abs(offsetVector.z));
                return start.magnitude > end.magnitude ? start + offsetVector : start - offsetVector;
            }
        }

        public static float Lerp(float evaluatedValue, float start, float end)
        {
            if (evaluatedValue >= 0 && evaluatedValue <= 1)
            {
                return Mathf.Lerp(start, end, evaluatedValue);
            }
            else if (evaluatedValue > 1)
            {
                float offsetValue = start - Mathf.Lerp(start, end, evaluatedValue - 1);
                offsetValue = Mathf.Abs(offsetValue);
                return end + offsetValue;
            }
            else
            {
                float offsetValue = end - Mathf.Lerp(start, end, evaluatedValue + 1);
                offsetValue = Mathf.Abs(offsetValue);
                return start - offsetValue;
            }
        }

        public static Quaternion Lerp(float evaluatedValue, Quaternion start, Quaternion end)
        {
            if (evaluatedValue >= 0 && evaluatedValue <= 1)
            {
                return Quaternion.Lerp(start, end, evaluatedValue);
            }
            else if (evaluatedValue > 1)
            {
                Quaternion offsetQuaternion = start * Quaternion.Inverse(Quaternion.Lerp(start, end, evaluatedValue - 1));
                offsetQuaternion = new Quaternion(Mathf.Abs(offsetQuaternion.x), Mathf.Abs(offsetQuaternion.y), Mathf.Abs(offsetQuaternion.z), Mathf.Abs(offsetQuaternion.w));
                return end * offsetQuaternion;
            }
            else
            {
                Quaternion offsetQuaternion = end * Quaternion.Inverse(Quaternion.Lerp(start, end, evaluatedValue + 1));
                offsetQuaternion = new Quaternion(Mathf.Abs(offsetQuaternion.x), Mathf.Abs(offsetQuaternion.y), Mathf.Abs(offsetQuaternion.z), Mathf.Abs(offsetQuaternion.w));
                return start * Quaternion.Inverse(offsetQuaternion);
            }
        }
    }
}