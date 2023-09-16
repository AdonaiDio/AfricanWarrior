using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGameDATA
{
    public int mapPointPosition_ID;
    public List<int> unlockedMapPoint_ID_List;

    public HeadAura_ScriptableObject equippedAura_Head;
    public LeftArmAura_ScriptableObject equippedAura_LeftArm;
    public RightArmAura_ScriptableObject equippedAura_RightArm;
    public TorsoAura_ScriptableObject equippedAura_Torso;

    public List<ScriptableObject> availableAuraList;

    public void SaveEquippedAuras(  HeadAura_ScriptableObject _equippedAura_Head,
                                    LeftArmAura_ScriptableObject _equippedAura_LeftArm,
                                    RightArmAura_ScriptableObject _equippedAura_RightArm,
                                    TorsoAura_ScriptableObject _equippedAura_Torso)
    {
        equippedAura_Head = _equippedAura_Head; 
        equippedAura_LeftArm = _equippedAura_LeftArm; 
        equippedAura_RightArm = _equippedAura_RightArm; 
        equippedAura_Torso = _equippedAura_Torso; 
    }

    public void SaveAvailableAuras(List<ScriptableObject> _availableAuraList)
    {
        availableAuraList = _availableAuraList;
    }

    public void SaveMapPosition(int _mapPointPosition_ID)
    {
        mapPointPosition_ID = _mapPointPosition_ID;
    }
    public void SaveUnlockedMapPoint(List<int> _unlockedMapPoint_ID_List)
    {
        unlockedMapPoint_ID_List = _unlockedMapPoint_ID_List;
    }
}
