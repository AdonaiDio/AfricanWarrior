using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsável por segurar a DATA do personagem.
//Coisa que precisa persistir e ser alterado entre mapas e salvamento do jogo.
public class Character_Base : MonoBehaviour
{
    //os dados configurados pelo Scriptable object.
    public Character_ScriptableObject characterScriptableObject;
    //Os mesmos dados mas pertencentes a instância atual. Serão iniciados com as informações do SO.
    public string characterName;
    //[HideInInspector] public int maxHp = 100; //TEMP
    //public int hp; //valor variável //TEMP
    //[HideInInspector] public int attack; //TEMP
    //[HideInInspector] public int initialActionPoints; //TEMP
    //[HideInInspector] public int maxActionPoints; //TEMP
    public int actionPoints; //valor variável

    //as 4 partes/auras
    public HeadAura_ScriptableObject head_Aura;
    public TorsoAura_ScriptableObject torso_Aura;
    public LeftArmAura_ScriptableObject left_Arm_Aura;
    public RightArmAura_ScriptableObject right_Arm_Aura;
    public int headHp;
    public int leftArmHp;
    public int rightArmHp;
    public int torsoHp;

    private void Awake()
    {
    }
    private void Start()
    {
    }

    public void SetCharacterAttributes()
    {
        characterName = characterScriptableObject.name;
        //maxHp = characterScriptableObject.maxHp;
        //attack = characterScriptableObject.attack;
        //initialActionPoints = characterScriptableObject.initialActionPoints;
        //maxActionPoints = characterScriptableObject.maxActionPoints;
        head_Aura = characterScriptableObject.head_Aura;
        headHp = head_Aura.hp;
        torso_Aura = characterScriptableObject.torso_Aura;
        torsoHp = torso_Aura.hp;
        left_Arm_Aura = characterScriptableObject.left_Arm_Aura;
        leftArmHp = left_Arm_Aura.hp;
        right_Arm_Aura = characterScriptableObject.right_Arm_Aura;
        rightArmHp = right_Arm_Aura.hp;
        actionPoints = characterScriptableObject.torso_Aura.initialActionPoints;
    }
}
