using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name;

    protected FiniteStateMachine finiteStateMachine;

    public BaseState(string name, FiniteStateMachine finiteStateMachine) {
        this.name = name;
        this.finiteStateMachine = finiteStateMachine;
    }

    public virtual void _Enter() { }
    public virtual void _Update() { }
    public virtual void _Exit() { }
}
