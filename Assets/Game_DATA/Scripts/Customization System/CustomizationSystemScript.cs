using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;

public class CustomizationSystemScript : MonoBehaviour
{
    private string aurasFolderPath = "Assets/Game_DATA/Prefabs/Auras";
    [HideInInspector] public List<HeadAura_ScriptableObject> headAuras;
    [HideInInspector] public List<LeftArmAura_ScriptableObject> leftArmAuras;
    [HideInInspector] public List<RightArmAura_ScriptableObject> rightArmAuras;
    [HideInInspector] public List<TorsoAura_ScriptableObject> torsoAuras;

    public GameObject AuraSetPrefab;
    public Transform containerAuraSetParent;

    public HeadAura_ScriptableObject currentHeadAura;
    public LeftArmAura_ScriptableObject currentLeftArmAura;
    public RightArmAura_ScriptableObject currentRightArmAura;
    public TorsoAura_ScriptableObject currentTorsoAura;

    public Image headSlotImg;
    public Image leftSlotImg;
    public Image rightSlotImg;
    public Image torsoSlotImg;

    public List<string> animalTypeList;

    public List<ScriptableObject> HumanAuras;
    public List<ScriptableObject> LoboGuaraAuras;
    public List<ScriptableObject> TatuAuras;
    public List<ScriptableObject> OnçaAuras;

    private void Start()
    {
        GetAuraScriptableObjectsFromFolder();
        CustomizationSceneSetup();
    }

    private void GetAuraScriptableObjectsFromFolder()
    {
        DirectoryInfo dir = new DirectoryInfo(aurasFolderPath);
        FileInfo[] info = dir.GetFiles("*.*");

        foreach (FileInfo f in info)
        {
            if (f.Extension == ".asset")
            {
                if (AssetDatabase.LoadAssetAtPath(
                    aurasFolderPath + "/" + f.Name,
                    typeof(ScriptableObject)).GetType()
                    == typeof(HeadAura_ScriptableObject))
                {
                    headAuras.Add((HeadAura_ScriptableObject)AssetDatabase.LoadAssetAtPath(aurasFolderPath + "/" + f.Name, typeof(HeadAura_ScriptableObject)));
                }
                else if (AssetDatabase.LoadAssetAtPath(
                    aurasFolderPath + "/" + f.Name,
                    typeof(ScriptableObject)).GetType()
                    == typeof(LeftArmAura_ScriptableObject))
                {
                    leftArmAuras.Add((LeftArmAura_ScriptableObject)AssetDatabase.LoadAssetAtPath(aurasFolderPath + "/" + f.Name, typeof(LeftArmAura_ScriptableObject)));
                }
                else if (AssetDatabase.LoadAssetAtPath(
                    aurasFolderPath + "/" + f.Name,
                    typeof(ScriptableObject)).GetType()
                    == typeof(RightArmAura_ScriptableObject))
                {
                    rightArmAuras.Add((RightArmAura_ScriptableObject)AssetDatabase.LoadAssetAtPath(aurasFolderPath + "/" + f.Name, typeof(RightArmAura_ScriptableObject)));
                }
                else if (AssetDatabase.LoadAssetAtPath(
                    aurasFolderPath + "/" + f.Name,
                    typeof(ScriptableObject)).GetType()
                    == typeof(TorsoAura_ScriptableObject))
                {
                    torsoAuras.Add((TorsoAura_ScriptableObject)AssetDatabase.LoadAssetAtPath(aurasFolderPath + "/" + f.Name, typeof(TorsoAura_ScriptableObject)));
                }
            }
        }
    }


    /// _UI_
    private void CustomizationSceneSetup()
    {
        //Load de todos os sets de auras
        StartAuraSets();
        //Carregar auras atuais nos slots correspondentes. (puxar do PlayerGameDATA)
        headSlotImg.sprite = currentHeadAura.sprite;
        leftSlotImg.sprite = currentLeftArmAura.sprite;
        rightSlotImg.sprite = currentRightArmAura.sprite;
        torsoSlotImg.sprite = currentTorsoAura.sprite;
    }

    private void switchAnimalAura(string animal, ScriptableObject SO)
    {
        HeadAura_ScriptableObject genSO = (HeadAura_ScriptableObject)SO;
        if (genSO.animalType.ToString() == animal)
        {
            Debug.Log(animal);
            switch (animal)
            {
                case "Human":
                    HumanAuras.Add(SO);
                    break;
                case "LoboGuara":
                    LoboGuaraAuras.Add(SO);
                    break;
                case "Tatu":
                    TatuAuras.Add(SO);
                    break;
                case "Onça":
                    OnçaAuras.Add(SO);
                    break;
            }
        }
    }

    private void StartAuraSets()
    {
        //acessar lista de auras disponíveis
        //guardar em grupos de animais

        foreach (string animal in animalTypeList)
        {
            foreach (HeadAura_ScriptableObject SO in headAuras)
            {
                switchAnimalAura(animal, SO);
            }
            foreach (LeftArmAura_ScriptableObject SO in leftArmAuras)
            {
                switchAnimalAura(animal, SO);
            }
            foreach (RightArmAura_ScriptableObject SO in rightArmAuras)
            {
                switchAnimalAura(animal, SO);
            }
            foreach (TorsoAura_ScriptableObject SO in torsoAuras)
            {
                switchAnimalAura(animal, SO);
            }
        }
        //instanciar um AuraSet por grupo de animal disponível
        if(HumanAuras.Count != 0)
        {
            GameObject humanSet = Instantiate(AuraSetPrefab, containerAuraSetParent);
        }
        if (LoboGuaraAuras.Count != 0)
        {
            GameObject loboGuaraSet = Instantiate(AuraSetPrefab, containerAuraSetParent);
        }
        if (TatuAuras.Count != 0)
        {
            GameObject tatuSet = Instantiate(AuraSetPrefab, containerAuraSetParent);
        }
        if (OnçaAuras.Count != 0)
        {
            GameObject oncaSet = Instantiate(AuraSetPrefab, containerAuraSetParent);
        }
        //instanciar auras do tipo do animal como filhos desse AuraSet
    }

    public void NextButtonLeft()
    {
        //animar movendo da esquerda para o centro
        //trocar a imagem da aura pela de index anterior na lista
        //evento de aura destacada (para a infoBox saber qual info por) (para o player TEMP DATA atualizar a aura ativa)
    }
    public void NextButtonRight()
    {
        //animar movendo da direita para o centro
        //trocar a imagem da aura pela de index anterior na lista
        //evento de aura destacada (para a infoBox saber qual info por) (para o player TEMP DATA atualizar a aura ativa)
    }
}
