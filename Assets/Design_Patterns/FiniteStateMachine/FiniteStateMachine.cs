using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    public BaseState currentState;

    void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState._Enter();
    }

    void Update()
    {
        if (currentState != null)
            currentState._Update();
    }

    //void LateUpdate()
    //{
    //    if (currentState != null)
    //        currentState.UpdatePhysics();
    //}

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    public void ChangeState(BaseState newState) //state transition
    {
        currentState._Exit();

        currentState = newState;
        newState._Enter();
    }

    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(10f, 10f, 200f, 100f));
    //    string content = currentState != null ? currentState.name : "(no current state)";
    //    GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    //    GUILayout.EndArea();
    //}
}
