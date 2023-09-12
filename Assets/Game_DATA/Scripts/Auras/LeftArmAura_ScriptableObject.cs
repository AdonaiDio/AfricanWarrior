using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LeftArmAuraDATA", menuName = "Character/Aura/New Left Arm Aura Data")]
public class LeftArmAura_ScriptableObject : ScriptableObject
{
    public int hp;
    //public Skill
    public string skillDescription;
    public int attack;
    public int attackActionPointsCost;
    public int skillActionPointsCost;
}
