using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_ScrollButtonScript : MonoBehaviour
{
    public ScrollRect scroll;
    public float scrollYVelocity;
    public void OnButtonUp()
    {
        scroll.velocity = new Vector2(0, scrollYVelocity);
    }
    public void OnButtonDown()
    {
        scroll.velocity = new Vector2(0, -scrollYVelocity);
    }
}
