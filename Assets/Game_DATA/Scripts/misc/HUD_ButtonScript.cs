using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ButtonScript : MonoBehaviour
{
    public ScrollRect scroll;
    public float scrollYVelocity = 150f;

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

}
