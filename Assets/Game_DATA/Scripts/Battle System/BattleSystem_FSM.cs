using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem_FSM : FiniteStateMachine
{
    //setup states
    [HideInInspector] public StartBattle_State startBattleState;
    [HideInInspector] public PlayerTurn_State playerTurnState;
    [HideInInspector] public EnemyTurn_State enemyTurnState;
    [HideInInspector] public Won_State wonState;
    [HideInInspector] public Lost_State lostState;

    //referencias dos personagens em cena
    private GameObject playerChar;
    private CharacterBattle playerCharBattle;
    private Character_Base playerCharBase;
    private GameObject enemyChar;
    private CharacterBattle enemyCharBattle;
    private Character_Base enemyCharBase;

    //
    public GameObject pf_characterBattle;
    public Transform position1;
    public Transform position2;
    //dados dos personagens dessa batalha.
    //Esses dados são colhidos de outra cena quando inicia combate
    public Character_ScriptableObject playerScriptableObject;
    public Character_ScriptableObject enemyScriptableObject;

    //UI
    public GameObject wonUI;
    public GameObject lostUI;

    private void Awake()
    {
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

        playerCharBattle = playerChar.GetComponent<CharacterBattle>();
        playerCharBase = playerChar.GetComponent<Character_Base>();

        playerCharBase.characterScriptableObject = playerScriptableObject;
        playerCharBase.SetCharacterAttributes();

        //configura tudo do enemy
        enemyChar = Instantiate(pf_characterBattle, position2.position, Quaternion.identity);

        enemyCharBattle = enemyChar.GetComponent<CharacterBattle>();
        enemyCharBase = enemyChar.GetComponent<Character_Base>();

        enemyCharBase.characterScriptableObject = enemyScriptableObject;
        enemyCharBase.SetCharacterAttributes();
        return true;
    }

    //botão só o player usa
    public void AtackButton()
    {
        playerCharBattle.AtackAnim();
        enemyCharBattle.ReceiveDamage(playerCharBattle.GiveDamage());
    }
    public void EndTurn()
    {
        Events.onEndTurnEvent.Invoke();
    }
    public bool CheckForEndBattleCondition()
    {
        if (playerCharBase.hp <= 0)
        {
            //Events.onLostBattleEvent.Invoke();
            lostUI.SetActive(true);
            ChangeState(lostState);
            return true;
        } else if (enemyCharBase.hp <= 0)
        {
            //Events.onWonBattleEvent.Invoke();
            wonUI.SetActive(true);
            ChangeState(wonState);
            return true;
        }

        return false;
    }
}
