using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;
using TMPro;

public class CustomizationSystemScript : MonoBehaviour
{
    private string aurasFolderPath = "Assets/Game_DATA/Prefabs/Auras";
    public DataPersistenceManager dataPersistence;
    //Todas as auras por partes
    [HideInInspector]
    public List<HeadAura_ScriptableObject> headAuras;
    [HideInInspector]
    public List<LeftArmAura_ScriptableObject> leftArmAuras;
    [HideInInspector]
    public List<RightArmAura_ScriptableObject> rightArmAuras;
    [HideInInspector]
    public List<TorsoAura_ScriptableObject> torsoAuras;

    //public GameObject AuraSetPrefab;
    //public Transform containerAuraSetParent;

    [Header("Auras Equipadas")]
    private HeadAura_ScriptableObject currentHeadAura;
    private LeftArmAura_ScriptableObject currentLeftArmAura;
    private RightArmAura_ScriptableObject currentRightArmAura;
    private TorsoAura_ScriptableObject currentTorsoAura;


    [Header("imagem da aura equipada")]
    public Image headSlotImg;
    public Image leftSlotImg;
    public Image rightSlotImg;
    public Image torsoSlotImg;
    [Header("botões da aura equipada")]
    public GameObject headSlotBtns;
    public GameObject leftSlotBtns;
    public GameObject rightSlotBtns;
    public GameObject torsoSlotBtns;

    [Header("Nomes dos animais")]
    public List<string> animalTypeList;

    ////Auras por grupo de animal
    //private List<ScriptableObject> HumanAuras;
    //private List<ScriptableObject> LoboGuaraAuras;
    //private List<ScriptableObject> TatuAuras;
    //private List<ScriptableObject> OnçaAuras;


    [Header("GameObject dos Sets")]
    public GameObject LoboSet_divAura;
    public GameObject Onca_divAura;
    public GameObject Tatu_divAura;


    [Header("aura equipada")]
    public float slideVelocity = 0.5f;


    [Header("Descrição das auras")]
    //Description Box
    public Image descBox_icon;
    public TMP_Text descBox_title;
    public TMP_Text descBox_PV;
    public GameObject descBox_PA_Atk_GO;
    public TMP_Text descBox_PA_Atk;
    public GameObject descBox_PA_Skill_GO;
    public TMP_Text descBox_PA_Skill;
    public TMP_Text descBox_Description;

    private void Awake()
    {
        dataPersistence = FindObjectOfType<DataPersistenceManager>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => dataPersistence.initialized);

        SetEquippedAuras();

        ArrangeAurasByParts();

        //StoreAurasByAnimalGroup();

        CustomizationSceneSetup();

        DisableAuralist(LoboSet_divAura);
        DisableAuralist(Onca_divAura);
        DisableAuralist(Tatu_divAura);

        RefreshDescriptionBoxInfo(currentHeadAura);
    }

    private void SetEquippedAuras()
    {
        currentHeadAura = dataPersistence.equippedAura_Head;
        currentLeftArmAura = dataPersistence.equippedAura_LeftArm;
        currentRightArmAura = dataPersistence.equippedAura_RightArm;
        currentTorsoAura = dataPersistence.equippedAura_Torso;
    }

    private void DisableAuralist(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    public void DisableAllArrows()
    {
        headSlotBtns.SetActive(false);
        leftSlotBtns.SetActive(false);
        rightSlotBtns.SetActive(false);
        torsoSlotBtns.SetActive(false);
    }
    public void RefreshHeadAura()
    {
        RefreshDescriptionBoxInfo(currentHeadAura);
    }
    public void RefreshLeftArmAura()
    {
        RefreshDescriptionBoxInfo(currentLeftArmAura);
    }
    public void RefreshRightArmAura()
    {
        RefreshDescriptionBoxInfo(currentRightArmAura);
    }
    public void RefreshTorsoAura()
    {
        RefreshDescriptionBoxInfo(currentTorsoAura);
    }
    public void ChangeHeadEquipped(HeadAura_ScriptableObject headSO)
    {
        currentHeadAura = headSO;
    }
    public void ChangeLeftEquipped(LeftArmAura_ScriptableObject leftSO)
    {
        currentLeftArmAura = leftSO;
    }
    public void ChangeRightEquipped(RightArmAura_ScriptableObject rightSO)
    {
        currentRightArmAura = rightSO;
    }
    public void ChangeTorsoEquipped(TorsoAura_ScriptableObject torsoSO)
    {
        currentTorsoAura = torsoSO;
    }
    private void ArrangeAurasByParts() {
        foreach (ScriptableObject auraSO in dataPersistence.allGameAurasList)
        {
            if (auraSO.GetType() == typeof(HeadAura_ScriptableObject))
            {
                if (dataPersistence.availableAuraList.Contains(auraSO))
                {
                    headAuras.Add((HeadAura_ScriptableObject)auraSO);
                }
            }
            else if (auraSO.GetType() == typeof(LeftArmAura_ScriptableObject))
            {
                if (dataPersistence.availableAuraList.Contains(auraSO))
                {
                    leftArmAuras.Add((LeftArmAura_ScriptableObject)auraSO);
                }
            }
            else if (auraSO.GetType() == typeof(RightArmAura_ScriptableObject))
            {
                if (dataPersistence.availableAuraList.Contains(auraSO))
                {
                    rightArmAuras.Add((RightArmAura_ScriptableObject)auraSO);
                }
            }
            else if (auraSO.GetType() == typeof(TorsoAura_ScriptableObject))
            {
                if (dataPersistence.availableAuraList.Contains(auraSO))
                {
                    torsoAuras.Add((TorsoAura_ScriptableObject)auraSO);
                }
            }
        }
    }
    public void YesSaveModifications()
    {
        //guardar as auras equipadas no Data
        dataPersistence.equippedAura_Head = currentHeadAura;
        dataPersistence.equippedAura_LeftArm = currentLeftArmAura;
        dataPersistence.equippedAura_RightArm = currentRightArmAura;
        dataPersistence.equippedAura_Torso = currentTorsoAura;
        //salvar o json do Data
        dataPersistence.SaveJson();
    }
    public void DontSaveModifications()
    {
        Debug.Log("Não chamou salvou");
    }

    /// _UI_
    private void CustomizationSceneSetup()
    {
        //Load de todos os sets de auras
        //StartAuraSets();
        //Carregar auras atuais nos slots correspondentes. (puxar do PlayerGameDATA)
        headSlotImg.sprite = currentHeadAura.sprite;
        leftSlotImg.sprite = currentLeftArmAura.sprite;
        rightSlotImg.sprite = currentRightArmAura.sprite;
        torsoSlotImg.sprite = currentTorsoAura.sprite;
    }

    #region Aura Button left and right Bulshit
    private void NextButtonLeft(GameObject auraImage, string auraPart)
    {
        if (auraImage.transform.localPosition.x == 0)
        {
            //animar movendo da esquerda para o centro
            LeanTween.moveLocalX(auraImage, -130f, slideVelocity).setOnComplete(() =>
           {
               LeanTween.moveLocalX(auraImage, 130f, 0f).setOnComplete(() =>
               {
                   switch (auraPart)
                   {
                       case "head":
                           if (headAuras.Count > 1) //se tem com quem trocar
                            {
                               int _currentIndex = headAuras.IndexOf(currentHeadAura);
                               if (_currentIndex == 0)
                               {
                                    //se for o primeiro volta o ultimo index
                                    //troca o equipado e corrige a imagem
                                    currentHeadAura = headAuras[headAuras.Count - 1];
                                   auraImage.GetComponent<Image>().sprite = headAuras[headAuras.Count - 1].sprite;
                               }
                               else
                               {
                                    //acrescenta -1 no index
                                    //troca o equipado e corrige a imagem
                                    currentHeadAura = headAuras[_currentIndex - 1];
                                   auraImage.GetComponent<Image>().sprite = headAuras[_currentIndex - 1].sprite;
                               }
                               RefreshDescriptionBoxInfo(currentHeadAura);
                           }
                           break;
                       case "left":
                           if (leftArmAuras.Count > 1) //se tem com quem trocar
                            {
                               int _currentIndex = leftArmAuras.IndexOf(currentLeftArmAura);
                               if (_currentIndex == 0)
                               {
                                    //se for o ultimo volta a 0 index
                                    //troca o equipado e corrige a imagem
                                    currentLeftArmAura = leftArmAuras[headAuras.Count - 1];
                                   auraImage.GetComponent<Image>().sprite = leftArmAuras[headAuras.Count - 1].sprite;
                               }
                               else
                               {
                                    //acrescenta +1 no index
                                    //troca o equipado e corrige a imagem
                                    currentLeftArmAura = leftArmAuras[_currentIndex - 1];
                                   auraImage.GetComponent<Image>().sprite = leftArmAuras[_currentIndex - 1].sprite;
                               }
                               RefreshDescriptionBoxInfo(currentLeftArmAura);
                           }
                           break;
                       case "right":
                           if (rightArmAuras.Count > 1) //se tem com quem trocar
                            {
                               int _currentIndex = rightArmAuras.IndexOf(currentRightArmAura);
                               if (_currentIndex == 0)
                               {
                                    //se for o ultimo volta a 0 index
                                    //troca o equipado e corrige a imagem
                                    currentRightArmAura = rightArmAuras[headAuras.Count - 1];
                                   auraImage.GetComponent<Image>().sprite = rightArmAuras[headAuras.Count - 1].sprite;
                               }
                               else
                               {
                                    //acrescenta +1 no index
                                    //troca o equipado e corrige a imagem
                                    currentRightArmAura = rightArmAuras[_currentIndex - 1];
                                   auraImage.GetComponent<Image>().sprite = rightArmAuras[_currentIndex - 1].sprite;
                               }
                               RefreshDescriptionBoxInfo(currentRightArmAura);
                           }
                           break;
                       case "torso":
                           if (torsoAuras.Count > 1) //se tem com quem trocar
                            {
                               int _currentIndex = torsoAuras.IndexOf(currentTorsoAura);
                               if (_currentIndex == 0)
                               {
                                    //se for o ultimo volta a 0 index
                                    //troca o equipado e corrige a imagem
                                    currentTorsoAura = torsoAuras[headAuras.Count - 1];
                                   auraImage.GetComponent<Image>().sprite = torsoAuras[headAuras.Count - 1].sprite;
                               }
                               else
                               {
                                    //acrescenta +1 no index
                                    //troca o equipado e corrige a imagem
                                    currentTorsoAura = torsoAuras[_currentIndex - 1];
                                   auraImage.GetComponent<Image>().sprite = torsoAuras[_currentIndex - 1].sprite;
                               }
                               RefreshDescriptionBoxInfo(currentTorsoAura);
                           }
                           break;
                   }
                   LeanTween.moveLocalX(auraImage, 0f, slideVelocity);
               });
           });
        }
    }

    private void NextButtonRight(GameObject auraImage, string auraPart)
    {
        if (auraImage.transform.localPosition.x == 0)
        {
            //animar movendo da direita para o centro
            LeanTween.moveLocalX(auraImage, 130f, slideVelocity).setOnComplete(() =>
            {
                LeanTween.moveLocalX(auraImage, -130f, 0f).setOnComplete(() =>
                {
                    switch (auraPart)
                    {
                        case "head":
                            if (headAuras.Count > 1) //se tem com quem trocar
                            {
                                int _currentIndex = headAuras.IndexOf(currentHeadAura);
                                if (_currentIndex == headAuras.Count - 1)
                                {
                                    //se for o ultimo volta a 0 index
                                    //troca o equipado e corrige a imagem
                                    currentHeadAura = headAuras[0];
                                    auraImage.GetComponent<Image>().sprite = headAuras[0].sprite;
                                }
                                else
                                {
                                    //acrescenta +1 no index
                                    //troca o equipado e corrige a imagem
                                    currentHeadAura = headAuras[_currentIndex + 1];
                                    auraImage.GetComponent<Image>().sprite = headAuras[_currentIndex + 1].sprite;
                                }
                                RefreshDescriptionBoxInfo(currentHeadAura);
                            }
                            break;
                        case "left":
                            if (leftArmAuras.Count > 1) //se tem com quem trocar
                            {
                                int _currentIndex = leftArmAuras.IndexOf(currentLeftArmAura);
                                if (_currentIndex == leftArmAuras.Count - 1)
                                {
                                    //se for o ultimo volta a 0 index
                                    //troca o equipado e corrige a imagem
                                    currentLeftArmAura = leftArmAuras[0];
                                    auraImage.GetComponent<Image>().sprite = leftArmAuras[0].sprite;
                                }
                                else
                                {
                                    //acrescenta +1 no index
                                    //troca o equipado e corrige a imagem
                                    currentLeftArmAura = leftArmAuras[_currentIndex + 1];
                                    auraImage.GetComponent<Image>().sprite = leftArmAuras[_currentIndex + 1].sprite;
                                }
                                RefreshDescriptionBoxInfo(currentLeftArmAura);
                            }
                            break;
                        case "right":
                            if (rightArmAuras.Count > 1) //se tem com quem trocar
                            {
                                int _currentIndex = rightArmAuras.IndexOf(currentRightArmAura);
                                if (_currentIndex == rightArmAuras.Count - 1)
                                {
                                    //se for o ultimo volta a 0 index
                                    //troca o equipado e corrige a imagem
                                    currentRightArmAura = rightArmAuras[0];
                                    auraImage.GetComponent<Image>().sprite = rightArmAuras[0].sprite;
                                }
                                else
                                {
                                    //acrescenta +1 no index
                                    //troca o equipado e corrige a imagem
                                    currentRightArmAura = rightArmAuras[_currentIndex + 1];
                                    auraImage.GetComponent<Image>().sprite = rightArmAuras[_currentIndex + 1].sprite;
                                }
                                RefreshDescriptionBoxInfo(currentRightArmAura);
                            }
                            break;
                        case "torso":
                            if (torsoAuras.Count > 1) //se tem com quem trocar
                            {
                                int _currentIndex = torsoAuras.IndexOf(currentTorsoAura);
                                if (_currentIndex == torsoAuras.Count - 1)
                                {
                                    //se for o ultimo volta a 0 index
                                    //troca o equipado e corrige a imagem
                                    currentTorsoAura = torsoAuras[0];
                                    auraImage.GetComponent<Image>().sprite = torsoAuras[0].sprite;
                                }
                                else
                                {
                                    //acrescenta +1 no index
                                    //troca o equipado e corrige a imagem
                                    currentTorsoAura = torsoAuras[_currentIndex + 1];
                                    auraImage.GetComponent<Image>().sprite = torsoAuras[_currentIndex + 1].sprite;
                                }
                                RefreshDescriptionBoxInfo(currentTorsoAura);
                            }
                            break;
                    }
                    LeanTween.moveLocalX(auraImage, 0f, slideVelocity);
                });
            });
        }
    }
    public void HeadAura_Btn_Left(GameObject auraImage)
    {
        NextButtonLeft(auraImage, "head");
    }
    public void LeftArmAura_Btn_Left(GameObject auraImage)
    {
        NextButtonLeft(auraImage, "left");
    }
    public void RightArmAura_Btn_Left(GameObject auraImage)
    {
        NextButtonLeft(auraImage, "right");
    }
    public void TorsoAura_Btn_Left(GameObject auraImage)
    {
        NextButtonLeft(auraImage, "torso");
    }
    public void HeadAura_Btn_Right(GameObject auraImage)
    {
        NextButtonRight(auraImage, "head");
    }
    public void LeftArmAura_Btn_Right(GameObject auraImage)
    {
        NextButtonRight(auraImage, "left");
    }
    public void RightArmAura_Btn_Right(GameObject auraImage)
    {
        NextButtonRight(auraImage, "right");
    }
    public void TorsoAura_Btn_Right(GameObject auraImage)
    {
        NextButtonRight(auraImage, "torso");
    }
    #endregion


    public void RefreshDescriptionBoxInfo(ScriptableObject aura_SO)
    {
        if (aura_SO.GetType() == typeof(HeadAura_ScriptableObject))
        {
            HeadAura_ScriptableObject headAura_temp = ((HeadAura_ScriptableObject)aura_SO);
            //subistituir o icone Image
            descBox_icon.sprite = headAura_temp.sprite;
            //subistituir o Titulo Text
            descBox_title.text = headAura_temp.auraName;
            //subistituir o PV Text
            descBox_PV.text = headAura_temp.hp.ToString();
            //subistituir o PA ataque Text (se tiver ativar se não desativar)
            descBox_PA_Atk_GO.SetActive(false);
            //subistituir o PA skill Text (se tiver ativar se não desativar)
            descBox_PA_Skill_GO.SetActive(true);
            descBox_PA_Skill.text = headAura_temp.skillAPCost.ToString();
            //subistituir o description Text
            descBox_Description.text = "Descrição: " + headAura_temp.description;
        }
        else if (aura_SO.GetType() == typeof(LeftArmAura_ScriptableObject))
        {
            LeftArmAura_ScriptableObject leftArmAura_temp = ((LeftArmAura_ScriptableObject)aura_SO);
            //subistituir o icone Image
            descBox_icon.sprite = leftArmAura_temp.sprite;
            //subistituir o Titulo Text
            descBox_title.text = leftArmAura_temp.auraName;
            //subistituir o PV Text
            descBox_PV.text = leftArmAura_temp.hp.ToString();
            //subistituir o PA ataque Text (se tiver ativar se não desativar)
            descBox_PA_Atk_GO.SetActive(true);
            descBox_PA_Atk.text = leftArmAura_temp.attackAPCost.ToString();
            //subistituir o PA skill Text (se tiver ativar se não desativar)
            descBox_PA_Skill_GO.SetActive(true);
            descBox_PA_Skill.text = leftArmAura_temp.skillAPCost.ToString();
            //subistituir o description Text
            descBox_Description.text = "Descrição: " + leftArmAura_temp.description;
        }
        else if (aura_SO.GetType() == typeof(RightArmAura_ScriptableObject))
        {
            RightArmAura_ScriptableObject rightArmAura_temp = ((RightArmAura_ScriptableObject)aura_SO);
            //subistituir o icone Image
            descBox_icon.sprite = rightArmAura_temp.sprite;
            //subistituir o Titulo Text
            descBox_title.text = rightArmAura_temp.auraName;
            //subistituir o PV Text
            descBox_PV.text = rightArmAura_temp.hp.ToString();
            //subistituir o PA ataque Text (se tiver ativar se não desativar)
            descBox_PA_Atk_GO.SetActive(true);
            descBox_PA_Atk.text = rightArmAura_temp.attackAPCost.ToString();
            //subistituir o PA skill Text (se tiver ativar se não desativar)
            descBox_PA_Skill_GO.SetActive(true);
            descBox_PA_Skill.text = rightArmAura_temp.skillAPCost.ToString();
            //subistituir o description Text
            descBox_Description.text = "Descrição: " + rightArmAura_temp.description;
        }
        else if (aura_SO.GetType() == typeof(TorsoAura_ScriptableObject))
        {
            TorsoAura_ScriptableObject torsoAura_temp = ((TorsoAura_ScriptableObject)aura_SO);
            //subistituir o icone Image
            descBox_icon.sprite = torsoAura_temp.sprite;
            //subistituir o Titulo Text
            descBox_title.text = torsoAura_temp.auraName;
            //subistituir o PV Text
            descBox_PV.text = torsoAura_temp.hp.ToString();
            //subistituir o PA ataque Text (se tiver ativar se não desativar)
            descBox_PA_Atk_GO.SetActive(false);
            //subistituir o PA skill Text (se tiver ativar se não desativar)
            descBox_PA_Skill_GO.SetActive(false);
            //subistituir o description Text
            descBox_Description.text = "Descrição: " + torsoAura_temp.description;
        }
    }

}
