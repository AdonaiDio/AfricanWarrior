using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lost_State : BaseState
{
    private BattleSystem_FSM _fsm;
    public Lost_State(BattleSystem_FSM finiteStateMachine) : base("Lost", finiteStateMachine) {
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
