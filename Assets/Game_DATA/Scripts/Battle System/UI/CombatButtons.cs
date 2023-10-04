using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class CombatButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool expandable = false;
    public float expandSpeed = 0.15f;
    public bool isSkill;
    
    private BattleSystem_FSM battleSystem;
    public List<GameObject> actionPoints_icons;

    private void OnEnable()
    {
        Events.onDisablePart.AddListener(DisablePart);
    }

    private void OnDisable()
    {
        Events.onDisablePart.RemoveListener(DisablePart);
    }

    private void Awake()
    {
        battleSystem = FindObjectOfType<BattleSystem_FSM>();
    }
    private void Update()
    {
        if (gameObject.GetComponent<Image>().sprite.name == "Aura_frame_none")
        {
            expandable = false;
        }
        else { 
            expandable = true;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (isSkill)
        {
            battleSystem.battleSystemUI.buttonChoosed = "skill";
            battleSystem.battleSystemUI.skillInfo.SetActive(true);
        }
        else
        {
            battleSystem.battleSystemUI.buttonChoosed = "atk";
        }
        //---
        int _AP_Cost = GetAPCost();

        int _count = 0;
        foreach (GameObject ap in actionPoints_icons)
        {
            if (ap.activeSelf &&
                _AP_Cost > _count)
            {
                _count++;
                ap.GetComponent<Image>().color = Color.red;
            }
        }
        if (_AP_Cost > _count)
        {
            foreach (GameObject ap in actionPoints_icons)
            {
                if (!ap.activeSelf &&
                    _AP_Cost > _count)
                {
                    _count++;
                    ap.SetActive(true);
                    ap.GetComponent<Image>().color = new Color(1,0,0,0.5f);
                }
            }
        }

        //}


        //animar expanção
        if (expandable)
        {
            LeanTween.scaleX(gameObject, 1.25f, expandSpeed);
            LeanTween.scaleY(gameObject, 1.25f, expandSpeed);
        }
    }

    private int GetAPCost()
    {
        //acessar aura para ver quantos PA é gasto
        int _AP_Cost = 0;
        switch (battleSystem.battleSystemUI.selected_Acting_AuraPart)
        {
            case Targets.Head:
                if (isSkill)
                {
                    _AP_Cost = battleSystem.playerCharBase.head_Aura.skillAPCost;
                    battleSystem.battleSystemUI.skillInfo
                        .GetComponent<TMP_Text>()
                        .text = battleSystem.playerCharBase.head_Aura.description;
                }
                break;
            case Targets.LeftArm:
                if (isSkill)
                {
                    _AP_Cost = battleSystem.playerCharBase.left_Arm_Aura.skillAPCost;
                    battleSystem.battleSystemUI.skillInfo
                        .GetComponent<TMP_Text>()
                        .text = battleSystem.playerCharBase.left_Arm_Aura.description;
                }
                else
                {
                    _AP_Cost = battleSystem.playerCharBase.left_Arm_Aura.attackAPCost;
                }
                break;
            case Targets.RightArm:
                if (isSkill)
                {
                    _AP_Cost = battleSystem.playerCharBase.right_Arm_Aura.skillAPCost;
                    battleSystem.battleSystemUI.skillInfo
                        .GetComponent<TMP_Text>()
                        .text = battleSystem.playerCharBase.right_Arm_Aura.description;
                }
                else
                {
                    _AP_Cost = battleSystem.playerCharBase.right_Arm_Aura.attackAPCost;
                }
                break;
        }

        return _AP_Cost;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        battleSystem.battleSystemUI.skillInfo.SetActive(false);
        foreach (GameObject ap in actionPoints_icons)
        {
            ap.GetComponent<Image>().color = Color.white;
        }
        battleSystem.battleSystemUI.UpdateAPCount();
        if (expandable)
        {
            LeanTween.scaleX(gameObject, 1f, expandSpeed);
            LeanTween.scaleY(gameObject, 1f, expandSpeed);
        }
    }

    private void DisablePart(AuraParts auraPart, Character_Base charBase)
    {

    }

}
