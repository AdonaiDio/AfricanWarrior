using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsável por segurar a DATA do personagem.
//Coisa que precisa persistir e ser alterado entre mapas e salvamento do jogo.
public class Character_Base : MonoBehaviour
{
    //os dados configurados pelo Scriptable object.
    public Character_ScriptableObject characterScriptableObject;
    //Os mesmo dados mas pertencentes a instância atual. Serão iniciados com as informações do SO.
    public string characterName;
    [HideInInspector] public int maxHp;
    public int hp; //valor variável
    [HideInInspector] public int attack;
    [HideInInspector] public int initialActionPoints;
    [HideInInspector] public int maxActionPoints;
    public int actionPoints; //valor variável

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
