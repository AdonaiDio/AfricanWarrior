using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem_FSM : FiniteStateMachine
{
    public BattleSystemUI battleSystemUI;
    //setup states
    [HideInInspector] public StartBattle_State startBattleState;
    [HideInInspector] public PlayerTurn_State playerTurnState;
    [HideInInspector] public EnemyTurn_State enemyTurnState;
    [HideInInspector] public Won_State wonState;
    [HideInInspector] public Lost_State lostState;
    // semi state??
    public TurnStep currentTurnStep = TurnStep.none;

    //referencias dos personagens em cena
    private GameObject playerChar;
    [HideInInspector] public Character_Base playerCharBase;
    private GameObject enemyChar;
    private Character_Base enemyCharBase;

    //
    public GameObject pf_characterBattle;
    public Transform position1;
    public Transform position2;
    //dados dos personagens dessa batalha.
    //Esses dados são colhidos de outra cena quando inicia combate
    public Character_ScriptableObject playerScriptableObject;
    public Character_ScriptableObject enemyScriptableObject;
    //Dados do jogo
    private DataPersistenceManager dataPersistence;

    //UI
    public GameObject wonUI;
    public GameObject lostUI;

    private void OnEnable()
    {
        Events.onTurnStepChange.AddListener(TurnStepHandler);
    }
    private void OnDisable()
    {
        Events.onTurnStepChange.RemoveListener(TurnStepHandler);
    }


    private void Awake()
    {
        dataPersistence = FindObjectOfType<DataPersistenceManager>();
        battleSystemUI = GetComponent<BattleSystemUI>();

        startBattleState = new StartBattle_State(this);
        playerTurnState = new PlayerTurn_State(this);
        enemyTurnState = new EnemyTurn_State(this);
        wonState = new Won_State(this);
        lostState = new Lost_State(this);
    }

    protected override BaseState GetInitialState()
    {
        return startBattleState;
    }

    public bool SetUpBattlefield() {
        //configura tudo do player
        playerChar = Instantiate(pf_characterBattle, position1.position, Quaternion.identity);

        playerCharBase = playerChar.GetComponent<Character_Base>();

        //atualizar os dados do DataPersistence no meu player
        playerScriptableObject.head_Aura = dataPersistence.equippedAura_Head;
        playerScriptableObject.left_Arm_Aura = dataPersistence.equippedAura_LeftArm;
        playerScriptableObject.right_Arm_Aura = dataPersistence.equippedAura_RightArm;
        playerScriptableObject.torso_Aura = dataPersistence.equippedAura_Torso;
        //associar scriptablesObjects do char_base
        playerCharBase.characterScriptableObject = playerScriptableObject;
        playerCharBase.SetCharacterAttributes();

        //configura tudo do enemy
        enemyChar = Instantiate(pf_characterBattle, position2.position, Quaternion.identity);

        enemyCharBase = enemyChar.GetComponent<Character_Base>();

        enemyCharBase.characterScriptableObject = enemyScriptableObject;
        enemyCharBase.SetCharacterAttributes();
        return true;

    }
    public void StartUI()
    {
        //UI setup
        
        battleSystemUI.UpdateAPCount();
    }

    private void TurnStepHandler(TurnStep turnStep)
    {
        currentTurnStep = turnStep;
        switch (turnStep)
        {
            case TurnStep.none:
                battleSystemUI.DisableSnAButtons();

                break;
            case TurnStep.ChooseMove:
                battleSystemUI.EnableSnAButtons();
                if (currentState.name == "Player Turn")
                {
                    Debug.Log("Player Turn ChooseMove");
                    playerCharBase.IsSelectable = true;
                    enemyCharBase.IsSelectable = false;
                }
                if (currentState.name == "Enemy Turn")
                {
                    Debug.Log("Enemy Turn ChooseMove");
                    playerCharBase.IsSelectable = false;
                    enemyCharBase.IsSelectable = true;
                }
                break;
            case TurnStep.SelectPlayer:
                battleSystemUI.EnableSnAButtons();
                if (currentState.name == "Player Turn")
                {
                    Debug.Log("Player Turn SelectPlayer");
                    battleSystemUI.DisableAllPanel();
                    playerCharBase.IsSelectable = true;
                    enemyCharBase.IsSelectable = false;
                }

                break;
            case TurnStep.SelectEnemy:
                battleSystemUI.EnableSnAButtons();
                if (currentState.name == "Player Turn")
                {
                    Debug.Log("Player Turn SelectEnemy");
                    battleSystemUI.DisableAllPanel();
                    playerCharBase.IsSelectable = false;
                    enemyCharBase.IsSelectable = true;

                }

                break;
            case TurnStep.Resolve:
                battleSystemUI.DisableSnAButtons();
                if (currentState.name == "Player Turn")
                {
                    Debug.Log("Player Turn Resolve");
                    battleSystemUI.DisableAllPanel();
                    playerCharBase.IsSelectable = false;
                    enemyCharBase.IsSelectable = false;
                }

                break;
        }
    }


    //botão só o player usa
    public void LeftArmAtackButton()
    {
        playerCharBase.AtackAnim();
        enemyCharBase.ReceiveDamage(playerCharBase.GiveDamageFromAura(playerCharBase.left_Arm_Aura), 
                                      enemyCharBase.torso_Aura);
        CheckForEndTurnCondition();
    }
    public void RightArmAtackButton()
    {
        playerCharBase.AtackAnim();
        enemyCharBase.ReceiveDamage(playerCharBase.GiveDamageFromAura(playerCharBase.right_Arm_Aura),
                                      enemyCharBase.torso_Aura);
        CheckForEndTurnCondition();
    }
    public void EndTurn()
    {
        Events.onEndTurnEvent.Invoke();
    }
    public void CheckForEndTurnCondition() 
    {
        if (playerCharBase.actionPoints < 1)
        {
            EndTurn();
        }
    }
    public bool CheckForEndBattleCondition()
    {
        if (playerCharBase.torsoHp <= 0)
        {
            //Events.onLostBattleEvent.Invoke();
            lostUI.SetActive(true);
            ChangeState(lostState);
            return true;
        } else if (enemyCharBase.torsoHp <= 0)
        {
            //Events.onWonBattleEvent.Invoke();
            wonUI.SetActive(true);
            ChangeState(wonState);
            return true;
        }
        return false;
    }
}

public enum TurnStep
{
    none,//quando não é o seu turno
    ChooseMove, //1
    SelectPlayer, //2
    SelectEnemy, //2
    Resolve //3
}