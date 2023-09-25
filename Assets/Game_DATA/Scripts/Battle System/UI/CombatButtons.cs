using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombatButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool expandable = false;
    public float expandSpeed = 0.15f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (expandable)
        {
            LeanTween.scaleX(gameObject, 1.25f, expandSpeed);
            LeanTween.scaleY(gameObject, 1.25f, expandSpeed);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (expandable)
        {
            LeanTween.scaleX(gameObject, 1f, expandSpeed);
            LeanTween.scaleY(gameObject, 1f, expandSpeed);
        }
    }
}
