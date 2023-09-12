using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RightArmAuraDATA", menuName = "Character/Aura/New Right Arm Aura Data")]
public class RightArmAura_ScriptableObject : ScriptableObject
{
    public int hp;
    //public Skill
    public string skillDescription;
    public int attack;
    public int attackActionPointsCost;
    public int skillActionPointsCost;
}
