using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPart : MonoBehaviour
{
    public List<GameObject> gameObjectsSelected;
    public AuraParts aura;
    private Character_Base charBase; //talvez não use aqui

    public GameObject healthBar_prefab;
    private GameObject healthBar;

    private GameObject hb_full;
    private GameObject hb_empty;
    private GameObject hb_bar;

    private void Awake()
    {
        charBase = GetComponentInParent<Character_Base>();
    }

    void OnMouseEnter()
    {
        if (charBase.IsSelectable)
        {
            foreach (GameObject go in gameObjectsSelected)
            {
                LeanTween.color(go, new Color(1, 0, 0), 0.15f);
            }
            if (healthBar == null)
            {
                healthBar = Instantiate<GameObject>(healthBar_prefab);

                hb_empty = healthBar.transform.GetChild(0).Find("emptyPos").gameObject;
                hb_full = healthBar.transform.GetChild(0).Find("fullPos").gameObject;
                hb_bar = healthBar.transform.GetChild(0).Find("bar").gameObject;
            }

            healthBar.transform.position = transform.position + new Vector3(0,1f,0);
            float percent = 0;
            switch (aura)
            {
                case AuraParts.Head:
                    percent = (float)charBase.headHp / (float)charBase.head_Aura.hp;
                    break;
                case AuraParts.LeftArm:
                    percent = (float)charBase.leftArmHp / (float)charBase.left_Arm_Aura.hp;
                    break;
                case AuraParts.rightArm:
                    percent = (float)charBase.rightArmHp / (float)charBase.right_Arm_Aura.hp;
                    break;
                case AuraParts.Torso:
                    percent = (float)charBase.torsoHp / (float)charBase.torso_Aura.hp;
                    break;
            }
            hb_bar.transform.localPosition = Vector3.Lerp(hb_empty.transform.localPosition, 
                                                    hb_full.transform.localPosition,
                                                    percent);

            Events.onMouseOverPartEvent.Invoke(aura, charBase);
        }
    }

    void OnMouseExit()
    {
        if (charBase.IsSelectable)
        {
            foreach (GameObject go in gameObjectsSelected)
            {
                LeanTween.color(go, new Color(1, 1, 1), 0.15f);
            }
            Destroy(healthBar);
        }
    }

    private void OnMouseDown()
    {
        if (charBase.IsSelectable)
        {
            Events.onMouseClickPartEvent.Invoke(aura, charBase);
            if (healthBar != null)
            {
                Destroy(healthBar);
            }
        }
    }

}
