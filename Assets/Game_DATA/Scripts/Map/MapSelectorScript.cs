using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectorScript : MonoBehaviour
{
    public Image image;
    private GameObject imageGO;
    public float tweenTime;
    public Gradient colorsGradient;
    public AnimationCurve animationCurve;

    private void Awake()
    {
        imageGO = image.gameObject;
    }
    private void Start()
    {
        ColorChange();
        IdleAnimation();
    }
    public void TweemPause()
    {
        LeanTween.pause(imageGO);
    }
    public void TweemResume()
    {
        LeanTween.resume(imageGO);
    }

    public void IdleAnimation()
    {
        LeanTween.moveLocal(imageGO, new Vector3(0,90,0) ,1.25f)
            .setLoopPingPong()
            .setEase(animationCurve);
    }

    public void ColorChange()
    {
        LeanTween.value(imageGO, 0f, 1, tweenTime).setLoopClamp().setOnUpdate((value) =>
       {
           image.color = colorsGradient.Evaluate(value);
       });

    }
}
