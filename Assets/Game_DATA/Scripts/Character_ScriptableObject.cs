using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "characterAttributes", menuName = "Character/New Character Data")]
public class Character_ScriptableObject : ScriptableObject
{
    public string name;
    public HeadAura_ScriptableObject head_Aura;
    public TorsoAura_ScriptableObject torso_Aura;
    public LeftArmAura_ScriptableObject left_Arm_Aura;
    public RightArmAura_ScriptableObject right_Arm_Aura;
}
