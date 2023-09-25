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

    //battle info?
    private Animator animator;
    public bool IsSelectable = false;//ativa a seleção no selectPart
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        actionPoints = characterScriptableObject.torso_Aura.actionPointsPerTurn;
    }

    public void AtackAnim()
    {
        animator.Play("bonecoTeste_atk");
    }
    public int GiveDamageFromAura(ScriptableObject auraSOSelected)
    {
        switch (auraSOSelected)
        {
            case LeftArmAura_ScriptableObject:
                //gastar PA
                actionPoints -= left_Arm_Aura.attackAPCost;
                return left_Arm_Aura.attack;
            case RightArmAura_ScriptableObject:
                actionPoints -= right_Arm_Aura.attackAPCost;
                return right_Arm_Aura.attack;
            default:
                return 0;
        }
    }
    public void ReceiveDamage(int damage, ScriptableObject auraSOTarget)
    {
        switch (auraSOTarget)
        {
            case HeadAura_ScriptableObject:
                headHp -= damage;
                break;
            case LeftArmAura_ScriptableObject:
                leftArmHp -= damage;
                break;
            case RightArmAura_ScriptableObject:
                rightArmHp -= damage;
                break;
            default:
                torsoHp -= damage;
                break;
        }
    }
}
