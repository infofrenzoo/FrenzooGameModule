using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class LevelAsset : ScriptableObject
{
    [SerializeField]
    public List<LevelInfo> LevelInfoList;
}
