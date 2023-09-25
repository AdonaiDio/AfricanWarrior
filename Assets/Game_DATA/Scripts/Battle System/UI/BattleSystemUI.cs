using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleSystemUI : MonoBehaviour
{
    private BattleSystem_FSM BS_FSM;
    public GameObject controlPanel;
    //buttons
    public GameObject SnA_Buttons;
    public GameObject skillGO;
    public GameObject attackGO;
    public List<Sprite> sprites;
    //aura info
    public TMP_Text auraTitle_text;
    public GameObject hpBar;
    public GameObject fullPosHP;
    public GameObject emptyPosHP;
    //aura counter
    public GameObject actionPointsCounter;
    public List<GameObject> actionPointsList;
    public TMP_Text maxAP;

    //choosen aura
    public AuraParts selectedAuraPart;
    public Character_Base selectedChar;

    private void OnEnable()
    {
        Events.onMouseOverPartEvent.AddListener(RefreshAuraInfo);
    }
    private void OnDisable()
    {
        Events.onMouseOverPartEvent.RemoveListener(RefreshAuraInfo);
    }

    private void Awake()
    {
        BS_FSM = gameObject.GetComponent<BattleSystem_FSM>(); 
    }
    public void DisableAllPanel()
    {
        controlPanel.SetActive(false);
    }
    public void EnableAllPanel()
    {
        controlPanel.SetActive(true);
    }
    public void DisableSnAButtons()
    {
        SnA_Buttons.SetActive(false);
    }
    public void EnableSnAButtons()
    {
        SnA_Buttons.SetActive(true);
    }
    public void UpdateAPCount()
    {
        int _maxAP = BS_FSM.playerCharBase.characterScriptableObject.torso_Aura.actionPointsPerTurn;
        //atualiza o numero maximo
        maxAP.text = "/"+_maxAP.ToString();
        //habilita apenas o numero correto de pontos de ação
        foreach (GameObject go in actionPointsList)
        {
            go.SetActive(false);
        }
        for (int i = 0; i < BS_FSM.playerCharBase.actionPoints; i++)
        {
            actionPointsList[i].SetActive(true);
        }
    }
    public void RefreshAuraInfo(AuraParts auraPart, Character_Base charBase) {
        if (charBase == BS_FSM.playerCharBase)
        {
            switch (auraPart)
            {
                case AuraParts.Head:
                    EnableSnAButtons();
                    auraTitle_text.text = charBase.head_Aura.auraName;
                    HPPositionHandler(charBase.head_Aura.hp, charBase.headHp);
                    skillGO.GetComponent<Image>().sprite = sprites[0];
                    attackGO.GetComponent<Image>().sprite = sprites[0];
                    break;
                case AuraParts.LeftArm:
                    EnableSnAButtons();
                    auraTitle_text.text = charBase.left_Arm_Aura.auraName;
                    HPPositionHandler(charBase.left_Arm_Aura.hp, charBase.leftArmHp);
                    skillGO.GetComponent<Image>().sprite = sprites[1];
                    attackGO.GetComponent<Image>().sprite = sprites[1];
                    break;
                case AuraParts.rightArm:
                    EnableSnAButtons();
                    auraTitle_text.text = charBase.right_Arm_Aura.auraName;
                    HPPositionHandler(charBase.right_Arm_Aura.hp, charBase.rightArmHp);
                    skillGO.GetComponent<Image>().sprite = sprites[2];
                    attackGO.GetComponent<Image>().sprite = sprites[2];
                    break;
                case AuraParts.Torso:
                    auraTitle_text.text = charBase.torso_Aura.auraName;
                    HPPositionHandler(charBase.torso_Aura.hp, charBase.torsoHp);
                    DisableSnAButtons();
                    break;
            }
        }
        selectedAuraPart = auraPart;
        selectedChar = charBase;
    }

    private void HPPositionHandler(int maxHP, int currentHP)
    {
        float percent = (float)currentHP / (float)maxHP;
        hpBar.transform.localPosition = Vector3.Lerp(emptyPosHP.transform.localPosition,
                                                     fullPosHP.transform.localPosition, 
                                                     percent);
    }
}
