using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn_State : BaseState
{
    private BattleSystem_FSM _fsm;
    public PlayerTurn_State(BattleSystem_FSM finiteStateMachine) : base("Player Turn", finiteStateMachine) {
        _fsm = finiteStateMachine;
    }

    private bool onEndTurn = false;

    public override void _Enter()
    {
        base._Enter();
        bool stop = _fsm.CheckForEndBattleCondition();
        if (stop) { return; }
        Debug.Log(this.name);
        Events.onEndTurnEvent.AddListener(OnEndTurn);//se turno terminou chama OnEndTurn
        //_fsm.currentTurnStep = TurnStep.ChooseMove;
        _fsm.ResetAP(_fsm.playerCharBase);
        Events.onTurnStepChange.Invoke(TurnStep.ChooseMove);

        _fsm.battleSystemUI.playerTurnLabel.SetActive(true);
        LeanTween.scale(_fsm.battleSystemUI.playerTurnLabel, new Vector3(1, 1, 1), 1.5f).setEaseOutSine().setOnComplete(() =>
        {
            _fsm.battleSystemUI.playerTurnLabel.SetActive(false);
        });

    }
    public override void _Update()
    {
        base._Update();
        if (onEndTurn)
        {
            _fsm.ChangeState(_fsm.enemyTurnState);
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
