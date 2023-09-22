using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class Skill
{
    public SkillType skillType;
    public int skillAmount;
    public Targets targets;
    public bool selfTarget;
}

public enum SkillType
{
    none,
    Damage,
    Heal,
    Disable
}

[Flags]
public enum Targets
{
    none = 0,
    Head = 1,
    LeftArm = 2,
    RightArm = 4,
    Torso = 8,
    Target = 16
}
