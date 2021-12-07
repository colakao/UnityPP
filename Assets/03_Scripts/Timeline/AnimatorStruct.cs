using System;
using UnityEngine;

[Serializable]
public struct AnimatorStruct
{
    public Animator anim;
    public int startFrame;
    readonly int endFrame;
}