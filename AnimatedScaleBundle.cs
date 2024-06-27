using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimatedScaleBundle 
{
    public List<RectTransform> Components;// UI objects that should be animated
    public float animationTime; 
    public AnimationCurve scaleOpenCurve;
    public AnimationCurve scaleCloseCurve;
    public float animationStartOnOpen; // when should the animation start. Value between 0 and 1. 
    public float animationStartOnClose;
    public float minScaleMultiplier;
    public float maxScaleMultiplier;

    [HideInInspector] public List<Vector3> originalScales;
    [HideInInspector] public bool started;
}
