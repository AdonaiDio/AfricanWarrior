using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//respons�vel por segurar a DATA do personagem.
//Coisa que precisa persistir e ser alterado entre mapas e salvamento do jogo.
public class Character_Base : MonoBehaviour
{
    //os dados configurados pelo Scriptable object.
    public Character_ScriptableObject characterScriptableObject;
    //Os mesmo dados mas pertencentes a inst�ncia atual. Ser�o iniciados com as informa��es do SO.
    public string characterName;
    [HideInInspector] public int maxHp;
    public int hp; //valor vari�vel
    [HideInInspector] public int attack;
    [HideInInspector] public int initialActionPoints;
    [HideInInspector] public int maxActionPoints;
    public int actionPoints; //valor vari�vel

    private void Awake()
    {
    }
    private void Start()
    {
    }

    public void SetCharacterAttributes()
    {
        characterName = characterScriptableObject.name;
        maxHp = characterScriptableObject.maxHp;
        attack = characterScriptableObject.attack;
        initialActionPoints = characterScriptableObject.initialActionPoints;
        maxActionPoints = characterScriptableObject.maxActionPoints;
        hp = maxHp;
        actionPoints = initialActionPoints;
    }
}
