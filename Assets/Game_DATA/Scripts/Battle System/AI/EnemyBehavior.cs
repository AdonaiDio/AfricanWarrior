using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private BattleSystem_FSM BS_FSM;
    private int executing = 0;

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Awake()
    {
        BS_FSM = FindObjectOfType<BattleSystem_FSM>();    
    }
    public void TurnSetUp()
    {
        BS_FSM.ResetAP(BS_FSM.enemyCharBase);
        BS_FSM.battleSystemUI.selected_Acting_Char = BS_FSM.enemyCharBase;
        executing = 0;
    }
    public void Execute()
    {
        executing++;
        if (executing == 1)
        {
            Debug.Log("Execute");
            StartCoroutine(ExecuteARandomCommand());
        }
    }
    public IEnumerator ExecuteARandomCommand()
    {
        yield return new WaitForSeconds(2f);
        randomCommand();
    }
    public void CheckForEndTurn()
    {
        BS_FSM.battleSystemUI.NotEnoughAPThisTurn();
    }
    public void EndTurn()
    {
        BS_FSM.EndTurn();
    }

    //private void Command_Attack_With_Head()
    //{
    //    Debug.LogWarning("Ocorre Command_Attack_With_Head");
    //    //aleatoriamente escolher 1 alvo
    //    Targets[] target = new Targets[1];
        
    //    randomTarget(target);
    //    Events.onDamageEvent.Invoke(BS_FSM.enemyCharBase, BS_FSM.enemyCharBase.head_Aura,
    //                                            BS_FSM.playerCharBase, target, "atk");

    //}

    private void Command_Attack_With_LeftArm()
    {
        Debug.LogWarning("Ocorre Command_Attack_With_LeftArm");
        Targets[] target = new Targets[1];

        randomTarget(target);
        Events.onDamageEvent.Invoke(BS_FSM.enemyCharBase, BS_FSM.enemyCharBase.left_Arm_Aura,
                                                BS_FSM.playerCharBase, target, "atk");

    }
    private void Command_Attack_With_RightArm()
    {
        Debug.LogWarning("Ocorre Command_Attack_With_RightArm");
        Targets[] target = new Targets[1];

        randomTarget(target);
        Events.onDamageEvent.Invoke(BS_FSM.enemyCharBase, BS_FSM.enemyCharBase.right_Arm_Aura,
                                                BS_FSM.playerCharBase, target, "atk");
    }

    private void Command_Skill_With_Head()
    {
        Debug.LogWarning("Ocorre Command_Skill_With_Head");
        HeadAura_ScriptableObject _selected_acting_auraSO = BS_FSM.enemyCharBase.head_Aura;
        GetSkillTarget_InvokeEvent(_selected_acting_auraSO);
    }
    private void Command_Skill_With_LeftArm()
    {
        Debug.LogWarning("Ocorre Command_Skill_With_LeftArm");
        LeftArmAura_ScriptableObject _selected_acting_auraSO = BS_FSM.enemyCharBase.left_Arm_Aura;
        GetSkillTarget_InvokeEvent(_selected_acting_auraSO);
    }
    private void Command_Skill_With_RightArm()
    {
        Debug.LogWarning("Ocorre Command_Skill_With_RightArm");
        RightArmAura_ScriptableObject _selected_acting_auraSO = BS_FSM.enemyCharBase.right_Arm_Aura;
        GetSkillTarget_InvokeEvent(_selected_acting_auraSO);
    }
    private void Command_EndTurn()
    {
        Debug.LogWarning("Ocorre Command_EndTurn");
        EndTurn();
    }
    private void GetSkillTarget_InvokeEvent(GenericAura_ScriptableObject _selected_acting_auraSO)
    {
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
            Events.onDamageEvent.Invoke(BS_FSM.enemyCharBase, _selected_acting_auraSO,
                                                BS_FSM.playerCharBase, _target_auraParts, "skill");
        }
        else if (_selected_acting_auraSO.skill.skillType == SkillType.Heal)
        {
            Events.onHealEvent.Invoke(BS_FSM.enemyCharBase, _selected_acting_auraSO,
                                                BS_FSM.enemyCharBase, _target_auraParts, "skill");
        }
    }
    private void randomTarget(Targets[] targets)
    {
        int randomNumber = UnityEngine.Random.Range(0, 4);
        switch (randomNumber)
        {
            case 0:
                //checar se é um alvo válido
                if (!BS_FSM.playerCharBase.is_head_ON)
                {
                    randomTarget(targets);
                }
                targets[0] = Targets.Head;
                break;
            case 1:
                if (!BS_FSM.playerCharBase.is_leftArm_ON)
                {
                    randomTarget(targets);
                }
                targets[0] = Targets.LeftArm;
                break;
            case 2:
                if (!BS_FSM.playerCharBase.is_rightArm_ON)
                {
                    randomTarget(targets);
                }
                targets[0] = Targets.RightArm;
                break;
            case 3:
                if (!BS_FSM.playerCharBase.is_torso_ON)
                {
                    randomTarget(targets);
                }
                targets[0] = Targets.Torso;
                break;
        }
    }
    private void randomCommand()
    {
        int randomNumber = UnityEngine.Random.Range(1, 7);
        switch (randomNumber)
        {
            case 1:
                if (!BS_FSM.playerCharBase.is_leftArm_ON)
                {
                    StartCoroutine(ExecuteARandomCommand());
                }
                else
                {
                    Command_Attack_With_LeftArm();
                    if (EnoughAPToPlayThisTurn())
                    {
                        executing = 0;
                        StartCoroutine(ExecuteARandomCommand());
                    }
                    else
                    {
                        EndTurn();
                    }
                }
                break;
            case 2:
                if (!BS_FSM.playerCharBase.is_rightArm_ON)
                {
                    StartCoroutine(ExecuteARandomCommand());
                }
                else
                {
                    Command_Attack_With_RightArm();
                    if (EnoughAPToPlayThisTurn())
                    {
                        executing = 0;
                        StartCoroutine(ExecuteARandomCommand());
                    }
                    else
                    {
                        EndTurn();
                    }
                }
                break;
            case 3:
                //checar se é um alvo válido
                if (!BS_FSM.playerCharBase.is_head_ON)
                {
                    StartCoroutine(ExecuteARandomCommand());
                }
                else
                {
                    Command_Skill_With_Head();
                    if (EnoughAPToPlayThisTurn())
                    {
                        executing = 0;
                        StartCoroutine(ExecuteARandomCommand());
                    }
                    else
                    {
                        EndTurn();
                    }
                }
                break;
            case 4:
                if (!BS_FSM.playerCharBase.is_leftArm_ON)
                {
                    StartCoroutine(ExecuteARandomCommand());
                }
                else
                {
                    Command_Skill_With_LeftArm();
                    if (EnoughAPToPlayThisTurn())
                    {
                        executing = 0;
                        StartCoroutine(ExecuteARandomCommand());
                    }
                    else
                    {
                        EndTurn();
                    }
                }
                break;
            case 5:
                if (!BS_FSM.playerCharBase.is_rightArm_ON)
                {
                    StartCoroutine(ExecuteARandomCommand());
                }
                else
                {
                    Command_Skill_With_RightArm();
                    if (EnoughAPToPlayThisTurn())
                    {
                        executing = 0;
                        StartCoroutine(ExecuteARandomCommand());
                    }
                    else
                    {
                        EndTurn();
                    }
                }
                break;
            case 6:
                executing = 0;
                EndTurn();
                break;
        }
    }

    public bool EnoughAPToPlayThisTurn()
    {
        bool canPlayThisTurn = false;

        List<Targets> _tList = new List<Targets>();
        _tList.Add(Targets.Head);
        _tList.Add(Targets.LeftArm);
        _tList.Add(Targets.RightArm);

        void IfAuraPartEnabled(Targets t)
        {
            //se a opção existe mesmo aí testa, se não, nem faz nada.
            if (BS_FSM.battleSystemUI.GetAura(t, BS_FSM.enemyCharBase).skill.skillType != SkillType.none)
            {
                if (BS_FSM.battleSystemUI.IsAPEnough(t, BS_FSM.enemyCharBase, "skill"))
                {
                    canPlayThisTurn = true;
                }
            }
            if(BS_FSM.battleSystemUI.GetAura(t, BS_FSM.enemyCharBase).attackAPCost != 0)
            {
                if (BS_FSM.battleSystemUI.IsAPEnough(t, BS_FSM.enemyCharBase, "atk"))
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
                    if (BS_FSM.enemyCharBase.is_head_ON)
                    {
                        IfAuraPartEnabled(t);
                    }
                    break;
                case Targets.LeftArm:
                    if (BS_FSM.enemyCharBase.is_leftArm_ON)
                    {
                        IfAuraPartEnabled(t);
                    }
                    break;
                case Targets.RightArm:
                    if (BS_FSM.enemyCharBase.is_rightArm_ON)
                    {
                        IfAuraPartEnabled(t);
                    }
                    break;
            }
        }

        if (BS_FSM.enemyCharBase.actionPoints >= 1 && canPlayThisTurn)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
