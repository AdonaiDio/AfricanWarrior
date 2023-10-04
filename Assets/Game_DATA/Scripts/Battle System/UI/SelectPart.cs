using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SelectPart : MonoBehaviour
{
    private BattleSystem_FSM BS_FSM;
    public List<GameObject> gameObjectsSelected;
    public AuraParts aura;
    private Character_Base charBase; //talvez não use aqui

    //HP BAR
    public GameObject healthBar_prefab;
    private GameObject healthBar;

    private GameObject hb_full;
    private GameObject hb_empty;
    private GameObject hb_bar;

    //Floating TEXT
    public GameObject floatigText_prefab;
    private GameObject floatigText;
    private Canvas canvas;

    private Targets auraTarget = Targets.none;

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
        charBase = GetComponentInParent<Character_Base>();
        BS_FSM = FindObjectOfType<BattleSystem_FSM>();
        canvas = FindObjectOfType<Canvas>();
    }

    void OnMouseEnter()
    {
        void OverAuraPart()
        {
            if (charBase.IsSelectable)
            {
                ChangePartsColor(Color.yellow);

                Events.onMouseOverPartEvent.Invoke(auraTarget, charBase);
            }
            ShowHPBar();
            
        }
        OnMouseAuraPart_SwitchCase(() => { 
                                       OverAuraPart();
        },
                                   () => {
                                       ChangePartsColor(Color.gray);
                                       ShowHPBar(); 
                                   });
    }
    void OnMouseExit()
    {
        void ExitAuraPart()
        {
            if (charBase.IsSelectable)
            {
                ChangePartsColor(Color.white);
            }
            Destroy(healthBar);
        }
        OnMouseAuraPart_SwitchCase(() => {
                                       ExitAuraPart();
                                   },
                                   () => {
                                       ChangePartsColor(Color.gray);
                                       Destroy(healthBar);
                                   });
    }

    private void OnMouseDown()
    {
        void ClickPart()
        {
            if (charBase.IsSelectable && (BS_FSM.currentTurnStep == TurnStep.SelectPlayer
                                          || BS_FSM.currentTurnStep == TurnStep.SelectEnemy))
            {
                Events.onMouseClickPartEvent.Invoke(auraTarget, charBase);
                if (healthBar != null)
                {
                    ChangePartsColor(Color.white);
                    Destroy(healthBar);
                }
            }
        }
        OnMouseAuraPart_SwitchCase(()=>{ 
                                      ClickPart();
                                   },
                                   ()=> {
                                       ChangePartsColor(Color.gray);
                                       //som de 'não pode clicar'
                                   });
        
    }
    private void OnMouseAuraPart_SwitchCase(Action actionIf, Action actionElse)
    {
        switch (aura)
        {
            case AuraParts.Head:
                if (charBase.is_head_ON)
                {
                    actionIf();
                }
                else
                {
                    actionElse();
                }
                break;
            case AuraParts.LeftArm:
                if (charBase.is_leftArm_ON)
                {
                    actionIf();
                }
                else
                {
                    actionElse();
                }
                break;
            case AuraParts.RightArm:
                if (charBase.is_rightArm_ON)
                {
                    actionIf();
                }
                else
                {
                    actionElse();
                }
                break;
            case AuraParts.Torso:
                if (charBase.is_torso_ON)
                {
                    actionIf();
                }
                else
                {
                    actionElse();
                }
                break;
        }
    }

    public void ShowHPBar()
    {
        if (healthBar == null)
        {
            healthBar = Instantiate<GameObject>(healthBar_prefab);

            hb_empty = healthBar.transform.GetChild(0).Find("emptyPos").gameObject;
            hb_full = healthBar.transform.GetChild(0).Find("fullPos").gameObject;
            hb_bar = healthBar.transform.GetChild(0).Find("bar").gameObject;
        }

        healthBar.transform.position = transform.position + new Vector3(0, 1f, 0);
        float percent = 0;
        switch (aura)
        {
            case AuraParts.Head:
                auraTarget = Targets.Head;
                percent = (float)charBase.headHp / (float)charBase.head_Aura.hp;
                break;
            case AuraParts.LeftArm:
                auraTarget = Targets.LeftArm;
                percent = (float)charBase.leftArmHp / (float)charBase.left_Arm_Aura.hp;
                break;
            case AuraParts.RightArm:
                auraTarget = Targets.RightArm;
                percent = (float)charBase.rightArmHp / (float)charBase.right_Arm_Aura.hp;
                break;
            case AuraParts.Torso:
                auraTarget = Targets.Torso;
                percent = (float)charBase.torsoHp / (float)charBase.torso_Aura.hp;
                break;
        }
        hb_bar.transform.localPosition = Vector3.Lerp(hb_empty.transform.localPosition,
                                                hb_full.transform.localPosition,
                                                percent);
    }

    private void DisablePart(AuraParts _aura, Character_Base _charBase)
    {
        
        if (aura == _aura && charBase == _charBase)
        {
            switch (_aura)
            {
                case AuraParts.Head:
                    _charBase.is_head_ON = false;
                    break;
                case AuraParts.LeftArm:
                    _charBase.is_leftArm_ON = false;
                    break;
                case AuraParts.RightArm:
                    _charBase.is_rightArm_ON = false;
                    break;
                case AuraParts.Torso:
                    _charBase.is_torso_ON = false;
                    break;
            }
            ChangePartsColor(Color.gray);
        }
    }

    public void ChangePartsColor(Color color)
    {
        foreach (GameObject go in gameObjectsSelected)
        {
            LeanTween.color(go, color, 0.15f);
        }
    }

    public void SpawnDamageText(int amount)
    {
        if (floatigText == null)
        {
            floatigText = Instantiate<GameObject>(floatigText_prefab, canvas.transform);
        }
        floatigText.GetComponent<TMP_Text>().color = Color.red;
        floatigText.GetComponent<TMP_Text>().text = "-"+amount;
        LeanTween.move(floatigText, new Vector3(transform.position.x,
            transform.position.y, transform.position.z), 0f);
        LeanTween.move(floatigText, new Vector3(transform.position.x, 7f, 0), 2f).setEaseInOutSine().setOnComplete(() => {
            Destroy(floatigText);
        });
    }
    public void SpawnHealText(int amount)
    {
        if (floatigText == null)
        {
            floatigText = Instantiate<GameObject>(floatigText_prefab, canvas.transform);
        }
        floatigText.GetComponent<TMP_Text>().color = Color.green;
        floatigText.GetComponent<TMP_Text>().text = "+" + amount;
        LeanTween.move(floatigText, new Vector3(transform.position.x, 
            transform.position.y, transform.position.z), 0f);
        LeanTween.move(floatigText, new Vector3(transform.position.x,7f,0), 2f).setEaseInOutSine().setOnComplete(() => {
            Destroy(floatigText);
        });
    }
}
