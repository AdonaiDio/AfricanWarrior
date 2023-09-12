using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TorsoAuraDATA", menuName = "Character/Aura/New Torso Aura Data")]
public class TorsoAura_ScriptableObject : ScriptableObject
{
    public int hp;
    public string torsoDescription;
    public int initialActionPoints;
    public int maxActionPoints;
    public int actionPointsPerTurn;
}
