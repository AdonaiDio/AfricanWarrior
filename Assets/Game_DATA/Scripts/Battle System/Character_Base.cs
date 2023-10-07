using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsável por segurar a DATA do personagem.
//Coisa que precisa persistir e ser alterado entre mapas e salvamento do jogo.
public class Character_Base : MonoBehaviour
{
    private BattleSystem_FSM BS_FSM;
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
    //auxiliar bool para habilitar/desabilitar partes
    public bool is_head_ON = true;
    public bool is_leftArm_ON = true;
    public bool is_rightArm_ON = true;
    public bool is_torso_ON = true;

    //battle info?
    private Animator animator;

    public bool IsSelectable = false;//ativa a seleção no selectPart
    private void Awake()
    {
        animator = GetComponent<Animator>();
        BS_FSM = FindObjectOfType<BattleSystem_FSM>();
    }
    private void OnEnable()
    {
        Events.onDamageEvent.AddListener(DamageHandler);
        Events.onHealEvent.AddListener(HealHandler);
    }


    private void OnDisable()
    {
        Events.onDamageEvent.RemoveListener(DamageHandler);
        Events.onHealEvent.RemoveListener(HealHandler);
    }
    private void Start()
    {
    }

    public void SetCharacterAttributes()
    {
        characterName = characterScriptableObject.name;
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
    #region Heal Handler
    private void HealHandler(Character_Base acting_charBase, GenericAura_ScriptableObject acting_auraSO, Character_Base target_charBase, Targets[] target_auraPart, string choosenButton)
    {
        //resolver os calculos
        if (this == target_charBase && this != acting_charBase)//this é target
        {
            TakeHeal(acting_charBase, acting_auraSO, target_charBase, target_auraPart, choosenButton);
        }
        else if(this == acting_charBase && this != target_charBase)//this é acting
        {
            GiveHeal(acting_charBase, acting_auraSO, target_charBase, target_auraPart, choosenButton);
        }
        else if (this == acting_charBase && this == target_charBase)//this é os dois
        {
            TakeHeal(acting_charBase, acting_auraSO, target_charBase, target_auraPart, choosenButton);
            GiveHeal(acting_charBase, acting_auraSO, target_charBase, target_auraPart, choosenButton);
        }
    }

    private void GiveHeal(Character_Base acting_charBase, GenericAura_ScriptableObject acting_auraSO, Character_Base target_charBase, Targets[] target_auraPart, string choosenButton)
    {
        StartCoroutine(GiveHealSkillAnim( () =>
        {
            actionPoints -= acting_auraSO.skillAPCost;
            BS_FSM.battleSystemUI.UpdateAPCount();
            BS_FSM.CheckForEndTurnCondition();
        }));
    }

    private void TakeHeal(Character_Base acting_charBase, GenericAura_ScriptableObject acting_auraSO, Character_Base target_charBase, Targets[] target_auraPart, string choosenButton)
    {
        int ClampHP(int hp, int amount, int auraPartHP){
            if (hp + amount > auraPartHP)
            {
                hp = auraPartHP;
            }
            else
            {
                hp += amount;
            }
            return hp;
        }
        void UpdatePartHP(int i, int healAmount)
        {
            if (target_auraPart[i] == Targets.Head)
            {
                headHp = ClampHP(headHp, healAmount, target_charBase.head_Aura.hp);
            }
            else if (target_auraPart[i] == Targets.LeftArm)
            {
                leftArmHp = ClampHP(leftArmHp, healAmount, target_charBase.left_Arm_Aura.hp);
            }
            else if (target_auraPart[i] == Targets.RightArm)
            {
                rightArmHp = ClampHP(rightArmHp, healAmount, target_charBase.right_Arm_Aura.hp);
            }
            else if (target_auraPart[i] == Targets.Torso)
            {
                torsoHp = ClampHP(torsoHp, healAmount, target_charBase.torso_Aura.hp);
            }
        }

        string _allAuraPartsListStrings = "";

        for (int i = 0; i < target_auraPart.Length; i++)
        {
            UpdatePartHP(i, acting_auraSO.skill.skillAmount);
            _allAuraPartsListStrings += ("'" + target_auraPart[i].ToString() + "', ");
        }

        StartCoroutine(TakeHealSkillAnim(target_charBase, acting_auraSO, target_auraPart, ()=> {

        }));
        _allAuraPartsListStrings = _allAuraPartsListStrings.Remove(_allAuraPartsListStrings.Length - 2) + " ";
        Debug.Log(acting_charBase.characterName + " usou " + acting_auraSO.auraName + " que curou " + acting_auraSO.skill.skillAmount + " de PV no " + _allAuraPartsListStrings + "do " + target_charBase.characterName);
    }
    #endregion
    #region Damage Handler
    private void DamageHandler(Character_Base acting_charBase, GenericAura_ScriptableObject acting_auraSO, Character_Base target_charBase, Targets[] target_auraPart, string choosenButton)
    {
        //resolver os calculos
        ///esse script é do Acting ou do Target?
        if (this == target_charBase && this != acting_charBase)
        {
            TakeDamage(acting_charBase, acting_auraSO, target_charBase, target_auraPart, choosenButton);
        }
        else if (this == acting_charBase && this != target_charBase)
        {
            GiveDamage(acting_charBase, acting_auraSO, target_charBase, target_auraPart, choosenButton);
        }
        else if (this == acting_charBase && this == target_charBase)// this é ambos
        {
            TakeDamage(acting_charBase, acting_auraSO, target_charBase, target_auraPart, choosenButton);
            GiveDamage(acting_charBase, acting_auraSO, target_charBase, target_auraPart, choosenButton);
        }
    }

    private void GiveDamage(Character_Base acting_charBase, GenericAura_ScriptableObject acting_auraSO, Character_Base target_charBase, Targets[] target_auraPart, string choosenButton)
    {
        if (choosenButton == "skill")
        {
            //anim
            StartCoroutine(GiveDamageSkillAnim(() => {
                actionPoints -= acting_auraSO.skillAPCost;
                BS_FSM.battleSystemUI.UpdateAPCount();
                BS_FSM.CheckForEndTurnCondition();
            }));
        }
        else//atk
        {
            StartCoroutine(GiveAttackAnim(() => {
                actionPoints -= acting_auraSO.attackAPCost;
                BS_FSM.battleSystemUI.UpdateAPCount();
                BS_FSM.CheckForEndTurnCondition();
            }));
        }
    }

    private void TakeDamage(Character_Base acting_charBase, GenericAura_ScriptableObject acting_auraSO, Character_Base target_charBase, Targets[] target_auraPart, string choosenButton)
    {

        int ClampHP(int hp, int amount)
        {
            if (hp - amount <= 0)
            {
                hp = 0;
            }
            else
            {
                hp -= amount;
            }
            return hp;
        }
        void UpdatePartHP(int i, int damageAmount)
        {
            if (target_auraPart[i] == Targets.Head)
            {
                headHp = ClampHP(headHp, damageAmount);
                if (headHp == 0)
                {
                    //disable auraPart
                    //is_head_ON = false;
                    Events.onDisablePart.Invoke(AuraParts.Head, target_charBase);
                }
            }
            else if (target_auraPart[i] == Targets.LeftArm)
            {
                leftArmHp = ClampHP(leftArmHp, damageAmount);
                if (leftArmHp == 0)
                {
                    //disable auraPart
                    //is_leftArm_ON = false;
                    Events.onDisablePart.Invoke(AuraParts.LeftArm, target_charBase);
                }
            }
            else if (target_auraPart[i] == Targets.RightArm)
            {
                rightArmHp = ClampHP(rightArmHp, damageAmount);
                if (rightArmHp == 0)
                {
                    //disable auraPart
                    //is_rightArm_ON = false;
                    Events.onDisablePart.Invoke(AuraParts.RightArm, target_charBase);
                }
            }
            else if (target_auraPart[i] == Targets.Torso)
            {
                torsoHp = ClampHP(torsoHp, damageAmount);
                if (torsoHp == 0)
                {
                    //disable auraPart
                    //is_torso_ON = false;
                    Events.onDisablePart.Invoke(AuraParts.Torso, target_charBase);
                }
            }
        }

        string _allAuraPartsListStrings = "";
        //é dano de skill ou de atk?
        if (choosenButton == "skill")
        {
            for(int i=0; i < target_auraPart.Length; i++)
            {
                UpdatePartHP(i, acting_auraSO.skill.skillAmount);
                _allAuraPartsListStrings += ("'" + target_auraPart[i].ToString() +"', ");
                StartCoroutine(TakeDamageSkillAnim(acting_auraSO, target_auraPart,() => {

                }));
            }
            if (_allAuraPartsListStrings != "")
            {
                _allAuraPartsListStrings = _allAuraPartsListStrings.Remove(_allAuraPartsListStrings.Length - 2)+" ";
            }
            Debug.Log(acting_charBase.characterName +" usou "+ acting_auraSO.auraName +" que causou "+ acting_auraSO.skill.skillAmount +" de dano no " + _allAuraPartsListStrings + " do " + target_charBase.characterName);
        }
        else//atk
        {
            UpdatePartHP(0, acting_auraSO.attack);
            _allAuraPartsListStrings = target_auraPart[0].ToString();
            StartCoroutine(TakeAttackAnim(target_charBase, acting_auraSO, target_auraPart, () => {
                Debug.Log(acting_charBase.characterName +" usou "+ acting_auraSO.auraName +" que causou "+ acting_auraSO.attack +" de dano no " + _allAuraPartsListStrings + " do " + target_charBase.characterName);

            }));
        }
    }
    #endregion
    #region Animation
    #region Damage Skill
    public IEnumerator TakeDamageSkillAnim(GenericAura_ScriptableObject auraSO, Targets[] target_auraPart_List, Action action)
    {
        animator.Play("hit");

        yield return new WaitWhile(() =>
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
        });
        DamageFeedback(auraSO, target_auraPart_List);

        action();
    }

    private void DamageFeedback(GenericAura_ScriptableObject auraSO, Targets[] target_auraPart_List)
    {
        List<GameObject> _targetGOs = new List<GameObject>();
        List<GameObject> _main_targetGOs = new List<GameObject>();
        SearchTargetGameObjects(target_auraPart_List, _targetGOs, _main_targetGOs);
        foreach (GameObject go in _targetGOs)
        {
            LeanTween.color(go, Color.red, 0.7f).setEaseOutExpo().setOnComplete(() =>
            {
                switch (go.GetComponent<SelectPart>().aura)
                {
                    case AuraParts.Head when !is_head_ON:
                    case AuraParts.LeftArm when !is_leftArm_ON:
                    case AuraParts.RightArm when !is_rightArm_ON:
                    case AuraParts.Torso when !is_torso_ON:
                        LeanTween.color(go, Color.gray, 0.7f).setEaseInOutSine();
                        break;
                    default:
                        LeanTween.color(go, Color.white, 0.7f).setEaseInOutSine();
                        break;
                }
            });
        }
        foreach (GameObject go in _main_targetGOs)
        {
            go.GetComponent<SelectPart>().SpawnDamageText(auraSO.attack);
        }
    }


    public IEnumerator GiveDamageSkillAnim(Action action)
    {
        animator.Play("attack");
        yield return new WaitWhile(() => {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
        });
        yield return new WaitForSeconds(1f);
        action();
    }
    #endregion
    #region Attack
    public IEnumerator TakeAttackAnim(Character_Base target_charBase, GenericAura_ScriptableObject auraSO, Targets[] target_auraPart_List, Action action)
    {
        animator.Play("hit");
        yield return new WaitWhile(()=> { 
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f; 
        });

        DamageFeedback(auraSO, target_auraPart_List);

        action();
    }
    public IEnumerator GiveAttackAnim(Action action)
    {
        animator.Play("attack");
        yield return new WaitWhile(() => {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
        });
        yield return new WaitForSeconds(1f);
        action();
    }
    #endregion
    #region heal
    public IEnumerator TakeHealSkillAnim(Character_Base target_charBase, GenericAura_ScriptableObject auraSO, Targets[] target_auraPart_List, Action action)
    {
        if (this == target_charBase)
        {
            List<GameObject> _targetGOs = new List<GameObject>();
            List<GameObject> _main_targetGOs = new List<GameObject>();
            SearchTargetGameObjects(target_auraPart_List, _targetGOs, _main_targetGOs);

            foreach (GameObject go in _targetGOs)
            {
                LeanTween.color(go, Color.cyan, 0.7f).setEaseOutExpo().setOnComplete(()=> {
                    LeanTween.color(go, Color.white, 0.7f).setEaseInOutSine();
                });
            }
            foreach (GameObject go in _main_targetGOs)
            {
                go.GetComponent<SelectPart>().SpawnHealText(auraSO.skill.skillAmount);
            }
        }
        yield return null;
        action();
    }
    public IEnumerator GiveHealSkillAnim(Action action)
    {
        //animator.Play("bonecoTeste_atk");
        //yield return new WaitWhile(() => {
        //    return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
        //});
        yield return new WaitForSeconds(1f);
        action();
    }
    private void SearchTargetGameObjects(Targets[] target_auraPart_List, List<GameObject> _targetGOs, List<GameObject> _main_targetGOs)
    {
        for (int i = 0; i < target_auraPart_List.Length; i++)
        {
            if (target_auraPart_List[i] == Targets.Head)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<SelectPart>() != null
                        && child.GetComponent<SelectPart>().aura == AuraParts.Head)
                    {
                        _targetGOs.Add(child.gameObject);
                        bool _main_gotOne = false;
                        foreach (GameObject go in _main_targetGOs)
                        {
                            if (go.GetComponent<SelectPart>() != null
                                && go.GetComponent<SelectPart>().aura == AuraParts.Head)
                            {
                                _main_gotOne = true;
                            }
                        }
                        if (_main_gotOne == false)
                        {
                            _main_targetGOs.Add(child.gameObject);
                        }
                    }
                }
                //_targetGOs.Add(transform.GetChild(0).gameObject);
                //_main_targetGOs.Add(transform.GetChild(0).gameObject);
                //_targetGOs.Add(transform.GetChild(1).gameObject);
            }
            if (target_auraPart_List[i] == Targets.LeftArm)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<SelectPart>() != null
                        && child.GetComponent<SelectPart>().aura == AuraParts.LeftArm)
                    {
                        _targetGOs.Add(child.gameObject);
                        bool _main_gotOne = false;
                        foreach (GameObject go in _main_targetGOs)
                        {
                            if (go.GetComponent<SelectPart>() != null
                                && go.GetComponent<SelectPart>().aura == AuraParts.LeftArm)
                            {
                                _main_gotOne = true;
                            }
                        }
                        if (_main_gotOne == false)
                        {
                            _main_targetGOs.Add(child.gameObject);
                        }
                    }
                }
                //_targetGOs.Add(transform.GetChild(9).gameObject);
                //_targetGOs.Add(transform.GetChild(10).gameObject);
                //_main_targetGOs.Add(transform.GetChild(10).gameObject);
                //_targetGOs.Add(transform.GetChild(11).gameObject);
            }
            if (target_auraPart_List[i] == Targets.RightArm)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<SelectPart>() != null
                        && child.GetComponent<SelectPart>().aura == AuraParts.RightArm)
                    {
                        _targetGOs.Add(child.gameObject);
                        bool _main_gotOne = false;
                        foreach (GameObject go in _main_targetGOs)
                        {
                            if (go.GetComponent<SelectPart>() != null
                                && go.GetComponent<SelectPart>().aura == AuraParts.RightArm)
                            {
                                _main_gotOne = true;
                            }
                        }
                        if (_main_gotOne == false)
                        {
                            _main_targetGOs.Add(child.gameObject);
                        }
                    }
                }
                //_targetGOs.Add(transform.GetChild(2).gameObject);
                //_targetGOs.Add(transform.GetChild(3).gameObject);
                //_main_targetGOs.Add(transform.GetChild(3).gameObject);
                //_targetGOs.Add(transform.GetChild(4).gameObject);
            }
            if (target_auraPart_List[i] == Targets.Torso)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<SelectPart>() != null
                        && child.GetComponent<SelectPart>().aura == AuraParts.Torso)
                    {
                        _targetGOs.Add(child.gameObject);
                        bool _main_gotOne = false;
                        foreach (GameObject go in _main_targetGOs)
                        {
                            if (go.GetComponent<SelectPart>() != null
                                && go.GetComponent<SelectPart>().aura == AuraParts.Torso)
                            {
                                _main_gotOne = true;
                            }
                        }
                        if (_main_gotOne == false)
                        {
                            _main_targetGOs.Add(child.gameObject);
                        }
                    }
                }
                //_targetGOs.Add(transform.GetChild(8).gameObject);
                //_main_targetGOs.Add(transform.GetChild(8).gameObject);
            }
        }
    }
    #endregion
    #endregion

}
