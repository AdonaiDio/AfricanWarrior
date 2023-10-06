using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle_State : BaseState
{
    private BattleSystem_FSM _fsm;
    private bool started = false;
    public StartBattle_State(BattleSystem_FSM finiteStateMachine) : base("Start Battle", finiteStateMachine) {
        _fsm = finiteStateMachine;
    }
    public override void _Enter()
    {
        base._Enter();
        Debug.Log(name);
        started = _fsm.SetUpBattlefield();
    }

    public override void _Update()
    {
        base._Update();
        Debug.Log("update " + name);
        if (started)
        {
            //int randNum = UnityEngine.Random.Range(0, 2); //de 0 a 1
            int randNum = 1;
            _fsm.StartUI();
            _fsm.ChangeState(randNum == 0 ? _fsm.enemyTurnState : _fsm.playerTurnState);
        }
    }
    public override void _Exit()
    {
        base._Exit();
    }

}
