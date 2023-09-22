using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUD_ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ScrollRect scroll;
    public float scrollYVelocity = 150f;
    public float expandSpeed = 0.15f;

    private void Update()
    {
    }
    public void OnScrollButtonUp()
    {
        scroll.velocity = new Vector2(0, -scrollYVelocity);
    }
    public void OnScrollButtonDown()
    {
        scroll.velocity = new Vector2(0, scrollYVelocity);
    }
    public void ToggleButton(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scaleX(gameObject, 1.25f, expandSpeed);
        LeanTween.scaleY(gameObject, 1.25f, expandSpeed);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scaleX(gameObject, 1f, expandSpeed);
        LeanTween.scaleY(gameObject, 1f, expandSpeed);
    }
}
