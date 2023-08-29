using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn_State : BaseState
{
    private BattleSystem_FSM _fsm;
    public EnemyTurn_State(BattleSystem_FSM finiteStateMachine) : base("Enemy Turn", finiteStateMachine) {
        _fsm = finiteStateMachine;
    }
    public override void _Enter()
    {
        base._Enter();
        bool stop = _fsm.CheckForEndBattleCondition();
        if (stop) { return; }
        Debug.Log(this.name);
    }
    public override void _Update()
    {
        base._Update();
        //if (true)
        //{
        //    _fsm.ChangeState(_fsm.playerTurnState);
        //}
    }
    public override void _Exit()
    {
        base._Exit();
    }
}
