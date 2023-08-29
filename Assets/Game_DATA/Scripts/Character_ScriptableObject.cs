using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "characterAttributes", menuName = "Character/New Character Data")]
public class Character_ScriptableObject : ScriptableObject
{
    public string name;
    public int maxHp;
    public int attack;
    public int initialActionPoints;
    public int maxActionPoints;
}
