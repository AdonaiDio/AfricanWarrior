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
    public int GiveDamage() { 
        return characterBase.attack; }
    public void ReceiveDamage(int damage) {
        characterBase.hp -= damage; 
    }
}
