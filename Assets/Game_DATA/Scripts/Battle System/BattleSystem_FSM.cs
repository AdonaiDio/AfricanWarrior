using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem_FSM : FiniteStateMachine
{
    [SerializeField]private string _currentState;
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
    public GameObject playerChar;
    [HideInInspector] public Character_Base playerCharBase;
    public GameObject enemyChar;
    [HideInInspector] public Character_Base enemyCharBase;

    //prefab for char spawn
    public GameObject pf_characterBattle;
    public Transform position1;
    public Transform position2;
    //dados dos personagens dessa batalha.
    //Esses dados são colhidos de outra cena quando inicia combate
    public Character_ScriptableObject playerScriptableObject;
    public Character_ScriptableObject enemyScriptableObject;
    //Dados do jogo
    private DataPersistenceManager dataPersistence;


    private void OnEnable()
    {
        Events.onTurnStepChange.AddListener(TurnStepHandler);
        Events.onChangeState.AddListener(UpdateStateName);
    }
    private void OnDisable()
    {
        Events.onTurnStepChange.RemoveListener(TurnStepHandler);
        Events.onChangeState.RemoveListener(UpdateStateName);
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
        //playerChar = Instantiate(pf_characterBattle, position1.position, Quaternion.identity);

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
        //enemyChar = Instantiate(pf_characterBattle, position2.position, Quaternion.identity);

        enemyCharBase = enemyChar.GetComponent<Character_Base>();

        enemyCharBase.characterScriptableObject = enemyScriptableObject;
        enemyCharBase.SetCharacterAttributes();
        playerChar.SetActive(true);
        enemyChar.SetActive(true);
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
                battleSystemUI.DisableAllPanel();
                battleSystemUI.DisableSkillInfo();
                battleSystemUI.DisableSkipTurnButton();

                break;
            case TurnStep.ChooseMove:
                if (currentState.name == "Player Turn")
                {
                    battleSystemUI.EnableAllPanel();
                    battleSystemUI.EnableSkipTurnButton();
                    
                    Debug.Log("Player Turn ChooseMove");
                    playerCharBase.IsSelectable = true;
                    enemyCharBase.IsSelectable = false;
                    battleSystemUI.selected_Acting_Char = playerCharBase;
                }
                else if (currentState.name == "Enemy Turn")
                {
                    battleSystemUI.DisableSkipTurnButton();
                    battleSystemUI.controlPanel.SetActive(false);

                    Debug.Log("Enemy Turn ChooseMove");
                    playerCharBase.IsSelectable = false;
                    enemyCharBase.IsSelectable = false;
                    battleSystemUI.selected_Acting_Char = enemyCharBase;
                }
                break;
            case TurnStep.SelectPlayer:
                battleSystemUI.DisableAllPanel();
                battleSystemUI.DisableSkipTurnButton();
                battleSystemUI.playerTurnLabel.SetActive(false);
                battleSystemUI.enemyTurnLabel.SetActive(false);
                if (currentState.name == "Player Turn")
                {
                    Debug.Log("Player Turn SelectPlayer");
                    playerCharBase.IsSelectable = true;
                    enemyCharBase.IsSelectable = false;
                }
                else if (currentState.name == "Enemy Turn")
                {
                    Debug.Log("Enemy Turn SelectPlayer");
                    playerCharBase.IsSelectable = false;
                    enemyCharBase.IsSelectable = false;
                }

                break;
            case TurnStep.SelectEnemy:
                battleSystemUI.DisableSkipTurnButton();
                battleSystemUI.DisableAllPanel();
                battleSystemUI.playerTurnLabel.SetActive(false);
                battleSystemUI.enemyTurnLabel.SetActive(false);
                if (currentState.name == "Player Turn")
                {
                    Debug.Log("Player Turn SelectEnemy");
                    playerCharBase.IsSelectable = false;
                    enemyCharBase.IsSelectable = true;
                }
                else if (currentState.name == "Enemy Turn")
                {
                    Debug.Log("Enemy Turn SelectEnemy");
                    playerCharBase.IsSelectable = false;
                    enemyCharBase.IsSelectable = false;
                }

                break;
            case TurnStep.Resolve:
                battleSystemUI.DisableSnAButtons();
                battleSystemUI.DisableAllPanel();
                battleSystemUI.DisableSkillInfo();
                battleSystemUI.DisableSkipTurnButton();
                battleSystemUI.playerTurnLabel.SetActive(false);
                battleSystemUI.enemyTurnLabel.SetActive(false);
                playerCharBase.IsSelectable = false;
                enemyCharBase.IsSelectable = false;
                if (currentState.name == "Player Turn")
                {
                    Debug.Log("Player Turn Resolve");
                }
                else if (currentState.name == "Enemy Turn")
                {
                    Debug.Log("Enemy Turn Resolve");
                }

                break;
        }
    }

    public void ResetAP(Character_Base charBase)
    {
        charBase.actionPoints = charBase.torso_Aura.actionPointsPerTurn;
    }

    public void EndTurn()
    {
        Events.onEndTurnEvent.Invoke();
    }

    public void CheckForEndTurnCondition()
    {
        battleSystemUI.NotEnoughAPThisTurn();
    }
    public bool CheckForEndBattleCondition()
    {
        if (playerCharBase.torsoHp <= 0)
        {
            //Events.onLostBattleEvent.Invoke();
            battleSystemUI.lostUI.SetActive(true);
            ChangeState(lostState);
            return true;
        }
        else if (playerCharBase.headHp <= 0 && playerCharBase.leftArmHp <= 0 && playerCharBase.rightArmHp <= 0)
        {
            //Events.onLostBattleEvent.Invoke();
            battleSystemUI.lostUI.SetActive(true);
            ChangeState(lostState);
            return true;
        }
        else if (enemyCharBase.torsoHp <= 0)
        {
            //Events.onWonBattleEvent.Invoke();
            battleSystemUI.wonUI.SetActive(true);
            ChangeState(wonState);
            return true;
        }
        else if (enemyCharBase.headHp <= 0 && enemyCharBase.leftArmHp <= 0 && enemyCharBase.rightArmHp <= 0)
        {
            //Events.onWonBattleEvent.Invoke();
            battleSystemUI.wonUI.SetActive(true);
            ChangeState(wonState);
            return true;
        }
        return false;
    }

    void UpdateStateName(BaseState currentState)
    {
        _currentState = currentState.name;
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