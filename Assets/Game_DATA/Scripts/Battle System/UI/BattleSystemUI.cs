using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleSystemUI : MonoBehaviour
{
    private BattleSystem_FSM BS_FSM;
    //Turn States Texts
    public GameObject wonUI;
    public GameObject lostUI;
    public GameObject playerTurnLabel;
    public GameObject enemyTurnLabel;
    //panel
    public GameObject controlPanel;
    //buttons
    public GameObject SnA_Buttons;
    public GameObject skillGO;
    public GameObject attackGO;

    public GameObject skipTurnButton;

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
    public Targets selected_Acting_AuraPart = Targets.LeftArm;
    public Character_Base selected_Acting_Char;

    //choosen button
    public string buttonChoosed = "";

    //description of skill
    public GameObject skillInfo;

    private void OnEnable()
    {
        Events.onMouseOverPartEvent.AddListener(RefreshAuraInfo);
        Events.onMouseClickPartEvent.AddListener(AuraClickToResolve);
        Events.onDisablePart.AddListener(DisableSnAButtons);
    }
    private void OnDisable()
    {
        Events.onMouseOverPartEvent.RemoveListener(RefreshAuraInfo);
        Events.onMouseClickPartEvent.RemoveListener(AuraClickToResolve);
        Events.onDisablePart.RemoveListener(DisableSnAButtons);
    }

    private void Awake()
    {
        BS_FSM = gameObject.GetComponent<BattleSystem_FSM>();
    }

    public void AuraClickToResolve(Targets target_auraPart, Character_Base target_charBase)
    {
        GenericAura_ScriptableObject _acting_auraSO = GetAura(selected_Acting_AuraPart, selected_Acting_Char);
        //converter em array
        Targets[] _target_auraPart = new Targets[1];
        _target_auraPart[0] = target_auraPart;


        if (buttonChoosed == "skill")
        {
            //evento de quem agiu com que aura, contra que alvo
            switch (_acting_auraSO.skill.skillType)
            {
                case SkillType.Damage:
                    Events.onDamageEvent.Invoke(selected_Acting_Char, _acting_auraSO,
                                                target_charBase, _target_auraPart, "skill");
                    break;
                case SkillType.Heal:
                    Events.onHealEvent.Invoke(selected_Acting_Char, _acting_auraSO,
                                              target_charBase, _target_auraPart, "skill");
                    break;
            }
        }
        else//atk
        {
            Events.onDamageEvent.Invoke(selected_Acting_Char, _acting_auraSO,
                                        target_charBase, _target_auraPart, "atk");
        }

    }

    public void RefreshAuraInfo(Targets auraPart, Character_Base charBase) //só jogador usa isso. pois usa mouse
    {
        if (BS_FSM.currentTurnStep == TurnStep.ChooseMove)
        {
            switch (auraPart)
            {
                case Targets.Head:
                    EnableSnAButtons();
                    auraTitle_text.text = charBase.head_Aura.auraName;
                    HPPositionHandler(charBase.head_Aura.hp, charBase.headHp);

                    skillGO.GetComponent<Image>().sprite = IsSkillNone(auraPart, charBase) ? sprites[4] : sprites[0];
                    attackGO.GetComponent<Image>().sprite = sprites[4];
                    attackGO.GetComponent<Button>().interactable = false;
                    break;
                case Targets.LeftArm:
                    EnableSnAButtons();
                    auraTitle_text.text = charBase.left_Arm_Aura.auraName;
                    HPPositionHandler(charBase.left_Arm_Aura.hp, charBase.leftArmHp);

                    skillGO.GetComponent<Image>().sprite = IsSkillNone(auraPart, charBase) ? sprites[4] : sprites[1];
                    attackGO.GetComponent<Image>().sprite = sprites[1];
                    attackGO.GetComponent<Button>().interactable = true;
                    break;
                case Targets.RightArm:
                    EnableSnAButtons();
                    auraTitle_text.text = charBase.right_Arm_Aura.auraName;
                    HPPositionHandler(charBase.right_Arm_Aura.hp, charBase.rightArmHp);

                    skillGO.GetComponent<Image>().sprite = IsSkillNone(auraPart, charBase) ? sprites[4] : sprites[2];
                    attackGO.GetComponent<Image>().sprite = sprites[2];
                    attackGO.GetComponent<Button>().interactable = true;
                    break;
                case Targets.Torso:
                    auraTitle_text.text = charBase.torso_Aura.auraName;
                    HPPositionHandler(charBase.torso_Aura.hp, charBase.torsoHp);
                    DisableSnAButtons();
                    break;
            }

            selected_Acting_AuraPart = auraPart;
        }
    }

    public void OnUseAttack() //clicar é coisa de player somente
    {
        buttonChoosed = "atk";
        Events.onTurnStepChange.Invoke(TurnStep.SelectEnemy);
    }

    public void OnUseSkill()
    {
        buttonChoosed = "skill";

        List<AuraParts> _auraPartsList = new List<AuraParts>();
        _auraPartsList.Add(AuraParts.Head);
        _auraPartsList.Add(AuraParts.LeftArm);
        _auraPartsList.Add(AuraParts.RightArm);

        void HandleSkillTargetType(GenericAura_ScriptableObject _selected_acting_aura) //qual o tipo de skill (target ou Multitarget)
        {
            //checar se tem AP suficiente para usar nessa aura
            if (IsAPEnough(selected_Acting_AuraPart, selected_Acting_Char, buttonChoosed))
            {
                //se a habilidade for self
                bool is_ActingChar_SelfTarget = _selected_acting_aura.skill.selfTarget;
                //para retornar o valor ao personagem correto
                bool playerIsTarget = selected_Acting_Char == BS_FSM.playerCharBase ? is_ActingChar_SelfTarget : !is_ActingChar_SelfTarget;

                foreach (Targets auraPart in _auraPartsList)
                {
                    if (selected_Acting_AuraPart == auraPart)
                    {
                        //
                        if (_selected_acting_aura.skill.targets > 0) //é válido
                        {
                            if (_selected_acting_aura.skill.targets.HasFlag(Targets.Target))
                            {
                                TurnStep _turnStep = playerIsTarget ? TurnStep.SelectPlayer : TurnStep.SelectEnemy;
                                Events.onTurnStepChange.Invoke(_turnStep);
                            }
                            else
                            {
                                Events.onTurnStepChange.Invoke(TurnStep.Resolve);
                                SkillMultiTargetResolve(_selected_acting_aura);
                            }
                        }
                    }
                }
            }
        }

        GenericAura_ScriptableObject _selected_acting_aura = GetAura(selected_Acting_AuraPart, selected_Acting_Char);
        HandleSkillTargetType(_selected_acting_aura);
    }

    private void SkillMultiTargetResolve(GenericAura_ScriptableObject _selected_acting_auraSO)
    {
        //primeiro descubro quem são as partes alvo. Depois eu chamo o evento de dano ou cura da parte alvo
        List<Targets> _targetsList = new List<Targets>();
        _targetsList.Add(Targets.Head);
        _targetsList.Add(Targets.LeftArm);
        _targetsList.Add(Targets.RightArm);
        _targetsList.Add(Targets.Torso);

        //se a habilidade for self
        bool is_ActingChar_SelfTarget = _selected_acting_auraSO.skill.selfTarget;
        //para retornar o valor ao personagem correto
        bool playerIsTarget = false;
        Character_Base _target_charBase;
        int _oponentPartHP = 0;

        playerIsTarget = selected_Acting_Char == BS_FSM.playerCharBase ? is_ActingChar_SelfTarget : !is_ActingChar_SelfTarget;//bool
        _target_charBase = (playerIsTarget ? BS_FSM.playerCharBase : BS_FSM.enemyCharBase);//charBase

        //salvar todas as partes que são alvo da skill
        List<Targets> _auraPartsList = new List<Targets>();


        if (_selected_acting_auraSO.skill.targets.HasFlag(Targets.Head))
        {
            _auraPartsList.Add(Targets.Head);
        }
        if (_selected_acting_auraSO.skill.targets.HasFlag(Targets.LeftArm))
        {
            _auraPartsList.Add(Targets.LeftArm);
        }
        if (_selected_acting_auraSO.skill.targets.HasFlag(Targets.RightArm))
        {
            _auraPartsList.Add(Targets.RightArm);
        }
        if (_selected_acting_auraSO.skill.targets.HasFlag(Targets.Torso))
        {
            _auraPartsList.Add(Targets.Torso);
        }
        //criar uma array pois é o formato que o event recebe
        Targets[] _target_auraParts = new Targets[_auraPartsList.Count];//array do tamanho de partes targets

        //baseado nas partes alvo da auraSO, vamos deixar somente as auras corretas
        for (int i = 0; i < _auraPartsList.Count; i++)
        {
            _target_auraParts[i] = _auraPartsList[i];
        }
        if (_selected_acting_auraSO.skill.skillType == SkillType.Damage)
        {
            Events.onDamageEvent.Invoke(selected_Acting_Char, _selected_acting_auraSO,
                                        _target_charBase, _target_auraParts, "skill");
        }
        else if (_selected_acting_auraSO.skill.skillType == SkillType.Heal)
        {
            Events.onHealEvent.Invoke(selected_Acting_Char, _selected_acting_auraSO,
                                        _target_charBase, _target_auraParts, "skill");
        }
        //disable, status...
    }

    public GenericAura_ScriptableObject GetAura(Targets auraPart, Character_Base charBase)
    {
        List<Targets> _auraPartsList = new List<Targets>();
        _auraPartsList.Add(Targets.Head);
        _auraPartsList.Add(Targets.LeftArm);
        _auraPartsList.Add(Targets.RightArm);
        _auraPartsList.Add(Targets.Torso);

        if (auraPart == Targets.Head)
        {
            HeadAura_ScriptableObject _new_aura = charBase.head_Aura;
            return _new_aura;
        }
        else if (auraPart == Targets.LeftArm)
        {
            LeftArmAura_ScriptableObject _new_aura = charBase.left_Arm_Aura;
            return _new_aura;
        }
        else if (auraPart == Targets.RightArm)
        {
            RightArmAura_ScriptableObject _new_aura = charBase.right_Arm_Aura;
            return _new_aura;
        }
        else if (auraPart == Targets.Torso)
        {
            TorsoAura_ScriptableObject _new_aura = charBase.torso_Aura;
            return _new_aura;
        }

        return new GenericAura_ScriptableObject();
    }
    public void DisableSkipTurnButton()
    {
        skipTurnButton.SetActive(false);
    }
    public void EnableSkipTurnButton()
    {
        skipTurnButton.SetActive(true);
    }
    public void DisableSkillInfo()
    {
        skillInfo.SetActive(false);
    }
    public void DisableAllPanel()
    {
        controlPanel.SetActive(false);
    }
    public void EnableAllPanel()
    {
        controlPanel.SetActive(true);
    }
    public void DisableSnAButtons(AuraParts arg1 = default, Character_Base arg2 = null)
    {
        SnA_Buttons.SetActive(false);
    }
    public void EnableSnAButtons()
    {
        if (selected_Acting_AuraPart != Targets.none && 
            selected_Acting_Char != null)
        {
            SnA_Buttons.SetActive(true);
        }
    }
    public void UpdateAPCount()
    {
        int _maxAP = BS_FSM.playerCharBase.characterScriptableObject.torso_Aura.actionPointsPerTurn;
        //atualiza o numero maximo
        maxAP.text = "/" + _maxAP.ToString();
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
    public bool IsSkillNone(Targets auraPart, Character_Base charBase)
    {
        switch (auraPart)
        {
            case Targets.Head:
                if (SkillType.none == charBase.head_Aura.skill.skillType) { return true; }
                break;
            case Targets.LeftArm:
                if (SkillType.none == charBase.left_Arm_Aura.skill.skillType) { return true; }
                break;
            case Targets.RightArm:
                if (SkillType.none == charBase.right_Arm_Aura.skill.skillType) { return true; }
                break;
        }
        return false;
    }
    public bool IsAPEnough(Targets auraPart, Character_Base charBase, string buttonChoosed)
    {
        //acessar aura para ver quantos PA é gasto
        int _AP_Cost = 0;

        GenericAura_ScriptableObject _aux_aura = GetAura(auraPart, charBase);

        List<AuraParts> _auraPartsList = new List<AuraParts>();
        _auraPartsList.Add(AuraParts.Head);
        _auraPartsList.Add(AuraParts.LeftArm);
        _auraPartsList.Add(AuraParts.RightArm);

        foreach (AuraParts p in _auraPartsList)
        {
            if (_aux_aura.auraPart == p)
            {
                _AP_Cost = buttonChoosed == "skill" ? _aux_aura.skillAPCost : _aux_aura.attackAPCost;
            }
        }
        return charBase.actionPoints >= _AP_Cost && _AP_Cost > 0;
    }
    private void HPPositionHandler(int maxHP, int currentHP)
    {
        float percent = (float)currentHP / (float)maxHP;
        hpBar.transform.localPosition = Vector3.Lerp(emptyPosHP.transform.localPosition,
                                                     fullPosHP.transform.localPosition,
                                                     percent);
    }

    public void NotEnoughAPThisTurn()
    {
        bool canPlayThisTurn = false;

        List<Targets> _tList = new List<Targets>();
        _tList.Add(Targets.Head);
        _tList.Add(Targets.LeftArm);
        _tList.Add(Targets.RightArm);

        void IfAuraPartEnabled(Targets t)
        {
            //se a opção existe mesmo aí testa, se não, nem faz nada.
            if (GetAura(t, selected_Acting_Char).skill.skillType != SkillType.none)
            { 
                if (IsAPEnough(t, selected_Acting_Char, "skill"))
                {
                    canPlayThisTurn = true;
                }
            }
            if (GetAura(t, selected_Acting_Char).attackAPCost != 0)
            {
                if (IsAPEnough(t, selected_Acting_Char, "atk"))
                {
                    canPlayThisTurn = true;
                }
            }
        }
        foreach (Targets t in _tList)
        {
            //check se está ativa a aura
            switch (t)
            {
                case Targets.Head:
                    if (selected_Acting_Char.is_head_ON)
                    {
                        IfAuraPartEnabled(t);
                    }
                    break;
                case Targets.LeftArm:
                    if (selected_Acting_Char.is_leftArm_ON)
                    {
                        IfAuraPartEnabled(t);
                    }
                    break;
                case Targets.RightArm:
                    if (selected_Acting_Char.is_rightArm_ON)
                    {
                        IfAuraPartEnabled(t);
                    }
                    break;
            }
        }

        if (selected_Acting_Char.actionPoints >= 1 && canPlayThisTurn)
        {
            Debug.Log("change step choose move");
            Events.onTurnStepChange.Invoke(TurnStep.ChooseMove);
        }
        else
        {
            BS_FSM.EndTurn();
            Debug.Log("End turn");
        }
    }

    //TEMPORARIO vvv
    public void ClickDisable(int auraPart)
    {
        if (auraPart == (int)AuraParts.Head)
        {
            Events.onDisablePart.Invoke(AuraParts.Head, BS_FSM.playerCharBase);
        }
        else if (auraPart == (int)AuraParts.LeftArm)
        {
            Events.onDisablePart.Invoke(AuraParts.LeftArm, BS_FSM.playerCharBase);
        }
        else if (auraPart == (int)AuraParts.RightArm)
        {
            Events.onDisablePart.Invoke(AuraParts.RightArm, BS_FSM.playerCharBase);
        }
        else if (auraPart == (int)AuraParts.Torso)
        {
            Events.onDisablePart.Invoke(AuraParts.Torso, BS_FSM.playerCharBase);
        }
    }
}
