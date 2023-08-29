using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Won_State : BaseState
{
    private BattleSystem_FSM _fsm;
    public Won_State(BattleSystem_FSM finiteStateMachine) : base("Won", finiteStateMachine) {
        _fsm = finiteStateMachine;
    }
    public override void _Enter()
    {
        base._Enter();
        Debug.Log(this.name);
    }
    public override void _Update()
    {
        base._Update();
    }
    public override void _Exit()
    {
        base._Exit();
    }
}
