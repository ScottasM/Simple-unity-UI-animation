using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float openTime;
    [SerializeField] private float animationTimeStep;

    [Header("Scale anim settings")]
    [SerializeField] private bool scale;
    [SerializeField] private AnimationCurve scaleOpenCurve;
    [SerializeField] private AnimationCurve scaleCloseCurve;

    [Header("Slide anim settings")]
    [SerializeField] private bool slide;
    [SerializeField] private AnimationCurve slideOpenCurve;
    [SerializeField] private AnimationCurve slideCloseCurve;
    [SerializeField] private Vector3 slidePosition; // where the starting position for the UI object is. TODO: add some functionality for this to be relative to screen sizes. 
    [Space]

    [Header("Child animation")]
    [SerializeField] private List<AnimatedScaleBundle> animatedScaleBundles = new List<AnimatedScaleBundle>();

    private Vector3 _originalPosition;
    private Vector3 _originalScale;
    private RectTransform _rect;
    private bool _shown = false;


    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _originalPosition = _rect.localPosition;
        _originalScale = _rect.localScale;

        if (scale) _rect.localScale = Vector3.zero;
        if (slide) _rect.localPosition = slidePosition;

        foreach(AnimatedScaleBundle bundle in animatedScaleBundles) { // get the original scales for all the child UI objects we animate
            for(int i = 0; i < bundle.Components.Count; i++) {
                bundle.originalScales.Add(bundle.Components[i].localScale);
            }
        }
    }

    public void Show(bool immediately = false, Action OnOpen = null)
    {
        if (_shown) return;
        _shown = true;
        if (immediately) {
            _rect.localPosition = _originalPosition;
            _rect.localScale = _originalScale;
            return;
        }

        StartCoroutine(Open(OnOpen));
    }

    private IEnumerator Open(Action OnOpen = null)
    {
        float timePassed = 0;

        if(slide)_rect.localPosition = slidePosition;
        if(scale)_rect.localScale = Vector3.zero;

        while (true) {
            yield return new WaitForSeconds(animationTimeStep);
            timePassed += animationTimeStep/openTime;

            if (!_shown)
                break;

            if (slide) _rect.localPosition = Vector3.Lerp(slidePosition, _originalPosition, slideOpenCurve.Evaluate(timePassed));
            if (scale) _rect.localScale = scaleOpenCurve.Evaluate(timePassed) * _originalScale;

            foreach (AnimatedScaleBundle bundle in animatedScaleBundles) {
                if (bundle.animationStartOnOpen <= timePassed && !bundle.started) {
                    StartCoroutine(AnimateBundleScale(bundle, bundle.scaleOpenCurve));
                }
            }

            if (timePassed >= 1) {
                _rect.localPosition = _originalPosition;
                _rect.localScale = _originalScale;
                OnOpen?.Invoke();
                break;
            }
        }
    }

    public void Hide(bool immediately = false, Action OnClose = null)
    {
        _shown = false;
        if (immediately) {
            if (slide)
                _rect.localPosition = slidePosition;
            if (scale)
                _rect.localScale = Vector3.zero;
        }
        StartCoroutine(Close(OnClose));
    }

    private IEnumerator Close(Action OnClose = null)
    {
        float timePassed = 0;

        _rect.localScale = _originalScale;
        _rect.localPosition = _originalPosition;

        while (true) {
            yield return new WaitForSeconds(animationTimeStep);
            timePassed += animationTimeStep/openTime;

            if (_shown)
                break;

            if (slide) _rect.localPosition = Vector3.Lerp(_originalPosition, slidePosition, slideCloseCurve.Evaluate(timePassed));
            if (scale) _rect.localScale = (1f-scaleCloseCurve.Evaluate(timePassed)) * _originalScale;

            foreach (AnimatedScaleBundle bundle in animatedScaleBundles) {
                if (bundle.animationStartOnClose <= timePassed && !bundle.started) {
                    StartCoroutine(AnimateBundleScale(bundle, bundle.scaleOpenCurve));
                }
            }

            if (timePassed >= 1) {
                _rect.localPosition = slidePosition;
                _rect.localScale = Vector3.zero;
                OnClose?.Invoke();
                break;
            }
        }
    }

    private IEnumerator AnimateBundleScale(AnimatedScaleBundle bundle,AnimationCurve curve)
    {
        float timePassed = 0;
        bundle.started = true;

        float difference = bundle.maxScaleMultiplier - bundle.minScaleMultiplier;

        while (true) {
            yield return new WaitForSeconds(animationTimeStep);
            timePassed += animationTimeStep / bundle.animationTime;

            if (!bundle.started)
                break;

            for(int i = 0;i< bundle.Components.Count;i++) {
                bundle.Components[i].localScale = bundle.originalScales[i] * (bundle.minScaleMultiplier + difference * curve.Evaluate(timePassed));
            }

            if (timePassed >= 1)
                break;
        }
        bundle.started = false;

        for (int i = 0; i < bundle.Components.Count; i++) {
            bundle.Components[i].localScale = bundle.originalScales[i];
        }
    }
}
