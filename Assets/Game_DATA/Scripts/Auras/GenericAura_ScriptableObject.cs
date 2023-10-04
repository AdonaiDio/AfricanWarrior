using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAura_ScriptableObject : ScriptableObject
{
    [SerializeField]public string auraName;
    [SerializeField]public AnimalType animalType;
    [SerializeField]public Sprite sprite;
    [SerializeField]public int hp;
    [SerializeField]public Skill skill; //
    [SerializeField]public string description;
    [SerializeField]public int attack; //
    [SerializeField]public int attackAPCost; //
    [SerializeField]public int skillAPCost; //
    [SerializeField]public int actionPointsPerTurn; //
    [HideInInspector]public AuraParts auraPart;
}
