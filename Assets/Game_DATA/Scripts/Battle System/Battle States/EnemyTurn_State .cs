using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn_State : BaseState
{
    private BattleSystem_FSM _fsm;
    public EnemyTurn_State(BattleSystem_FSM finiteStateMachine) : base("Enemy Turn", finiteStateMachine) {
        _fsm = finiteStateMachine;
    }

    private bool onEndTurn = false;

    public override void _Enter()
    {
        base._Enter();
        bool stop = _fsm.CheckForEndBattleCondition();
        if (stop) { return; }
        Debug.Log(this.name);
        Events.onEndTurnEvent.AddListener(OnEndTurn);
        Events.onTurnStepChange.Invoke(TurnStep.ChooseMove);

        _fsm.battleSystemUI.enemyTurnLabel.SetActive(true);
        LeanTween.scale(_fsm.battleSystemUI.enemyTurnLabel, new Vector3(1, 1, 1), 1.5f).setEaseOutSine().setOnComplete(() =>
        {
            _fsm.battleSystemUI.enemyTurnLabel.SetActive(false);
        });
        _fsm.enemyChar.GetComponent<EnemyBehavior>().Execute();
    }
    public override void _Update()
    {
        base._Update();
        if (onEndTurn)
        {
            _fsm.ChangeState(_fsm.playerTurnState);
        }
    }
    public override void _Exit()
    {
        base._Exit();
        onEndTurn = false;
        Events.onEndTurnEvent.RemoveListener(OnEndTurn);
    }

    private void OnEndTurn()
    {
        onEndTurn = true;
    }
}
