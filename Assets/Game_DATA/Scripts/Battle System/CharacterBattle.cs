using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    //recebe o DATA do personagem para fazer as manipulações necessárias
    private Character_Base characterBase;

    private Animator animator;

    private void Awake()
    {
        characterBase = GetComponent<Character_Base>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
    }
    //a ideia é settar as sprites corretas do personagem.
    //public void Setup(bool isPlayerTeam)
    //{
    //    if (isPlayerTeam)
    //    {
    //    }
    //    else
    //    {
    //    }
    //}

    public void AtackAnim()
    {
        animator.Play("bonecoTeste_atk");
    }
    public int GiveDamageFromAura(ScriptableObject auraSOSelected) {
        switch (auraSOSelected)
        {
            case LeftArmAura_ScriptableObject:
                //gastar PA
                characterBase.actionPoints -= characterBase.left_Arm_Aura.attackActionPointsCost;
                return characterBase.left_Arm_Aura.attack;
            case RightArmAura_ScriptableObject:
                characterBase.actionPoints -= characterBase.right_Arm_Aura.attackActionPointsCost;
                return characterBase.right_Arm_Aura.attack;
            default:
                return 0;
        }
    }
    public void ReceiveDamage(int damage, ScriptableObject auraSOTarget) {
        switch (auraSOTarget)
        {
            case HeadAura_ScriptableObject:
                characterBase.headHp -= damage;
                break;
            case LeftArmAura_ScriptableObject:
                characterBase.leftArmHp -= damage; 
                break;
            case RightArmAura_ScriptableObject:
                characterBase.rightArmHp -= damage; 
                break;
            default:
                characterBase.torsoHp -= damage; 
                break;
        }
    }
}
