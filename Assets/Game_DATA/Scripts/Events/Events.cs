using System;
using UnityEngine;
using UnityEngine.Events;

public static class Events
{
    public static readonly Evt onEndTurnEvent = new Evt();


    public static readonly Evt<BaseState> onChangeState = new Evt<BaseState>();

    public static readonly Evt onWonBattleEvent = new Evt();
    public static readonly Evt onLostBattleEvent = new Evt();

    public static readonly Evt<Targets, Character_Base> onMouseOverPartEvent = new Evt<Targets, Character_Base>();
    public static readonly Evt<Targets, Character_Base> onMouseClickPartEvent = new Evt<Targets, Character_Base>();

    public static readonly Evt<TurnStep> onTurnStepChange = new Evt<TurnStep>();

    public static readonly Evt<Character_Base, GenericAura_ScriptableObject, Character_Base,
        Targets[], string> onDamageEvent = new Evt<Character_Base, GenericAura_ScriptableObject, Character_Base, Targets[], string>();
    
    public static readonly Evt<Character_Base, GenericAura_ScriptableObject, Character_Base,
        Targets[], string> onHealEvent = new Evt<Character_Base, GenericAura_ScriptableObject, Character_Base, Targets[], string>();

    public static readonly Evt<AuraParts, Character_Base> onDisablePart = new Evt<AuraParts, Character_Base>();

    //public static readonly Evt onPlayerBusyEvent = new Evt();
    //public static readonly Evt onPlayerBusyEventExit = new Evt();

    //public static readonly Evt<MonoBehaviour> onDialogueEventExit = new Evt<MonoBehaviour>();
}
public class Evt
{
    private event Action _action = delegate { };

    public void Invoke() => _action.Invoke();
    public void AddListener(Action listener) => _action += listener;
    public void RemoveListener(Action listener) => _action -= listener;
}
public class Evt<T>
{
    private event Action<T> _action = delegate { };

    public void Invoke(T param) => _action.Invoke(param);
    public void AddListener(Action<T> listener) => _action += listener;
    public void RemoveListener(Action<T> listener) => _action -= listener;
}

public class Evt<T0, T1>
{
    private event Action<T0, T1> _action = delegate { };

    public void Invoke(T0 param1, T1 param2) => _action.Invoke(param1, param2);
    public void AddListener(Action<T0, T1> listener) => _action += listener;
    public void RemoveListener(Action<T0, T1> listener) => _action -= listener;
}
public class Evt<T0, T1, T2>
{
    private event Action<T0, T1, T2> _action = delegate { };

    public void Invoke(T0 param1, T1 param2, T2 param3) => _action.Invoke(param1, param2, param3);
    public void AddListener(Action<T0, T1, T2> listener) => _action += listener;
    public void RemoveListener(Action<T0, T1, T2> listener) => _action -= listener;
}
public class Evt<T0, T1, T2, T3>
{
    private event Action<T0, T1, T2, T3> _action = delegate { };

    public void Invoke(T0 param1, T1 param2, T2 param3, T3 param4) => _action.Invoke(param1, param2, param3, param4);
    public void AddListener(Action<T0, T1, T2, T3> listener) => _action += listener;
    public void RemoveListener(Action<T0, T1, T2, T3> listener) => _action -= listener;
}
public class Evt<T0, T1, T2, T3, T4>
{
    private event Action<T0, T1, T2, T3, T4> _action = delegate { };

    public void Invoke(T0 param1, T1 param2, T2 param3, T3 param4, T4 param5) => _action.Invoke(param1, param2, param3, param4, param5);
    public void AddListener(Action<T0, T1, T2, T3, T4> listener) => _action += listener;
    public void RemoveListener(Action<T0, T1, T2, T3, T4> listener) => _action -= listener;
}


