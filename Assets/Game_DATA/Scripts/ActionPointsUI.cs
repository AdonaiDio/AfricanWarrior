using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointsUI : MonoBehaviour
{
    public BattleSystem_FSM BS_FSM;
    public Slider slider;

    private void Update()
    {
        slider.value = (float)BS_FSM.playerCharBase.actionPoints / 10;
    }
}
