using System.Collections;
using System.Collections.Generic;
using AnimationInstancing;
using UnityEngine;
using AnimationInfo = AnimationInstancing.AnimationInfo;

public class AnimationInfoContainer : ScriptableObject
{
    public List<AnimationInfo> AnimationInfos;
    public ExtraBoneInfo ExtraBoneInfo;
}
