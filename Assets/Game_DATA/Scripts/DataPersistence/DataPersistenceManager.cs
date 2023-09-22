using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
using TMPro;

public class DataPersistenceManager : MonoBehaviour
{
    public int mapPointPosition_ID;
    public List<int> unlockedMapPoint_ID_List;

    //equipped auras
    public HeadAura_ScriptableObject equippedAura_Head;
    public LeftArmAura_ScriptableObject equippedAura_LeftArm;
    public RightArmAura_ScriptableObject equippedAura_RightArm;
    public TorsoAura_ScriptableObject equippedAura_Torso;

    public List<ScriptableObject> availableAuraList;
    public List<ScriptableObject> allGameAurasList;


    private IDataService DataService = new JsonDataService();
    private bool EncryptionEnabled;
    [HideInInspector]
    public bool initialized = false;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogError("Encontrou mais de uma Data Persistence Manager na cena");
            Destroy(this);
        }
        GetAllGameAuras();
        initialized = true;
    }
    private void Start()
    {
    }

    public void GetAllGameAuras()
    {
        string aurasFolderPath = "Assets/Game_DATA/Prefabs/Auras";
        DirectoryInfo dir = new DirectoryInfo(aurasFolderPath);
        FileInfo[] info = dir.GetFiles("*.*");

        //limpar a lista para evitar duplicatas
        allGameAurasList = new List<ScriptableObject>();
        foreach (FileInfo f in info)
        {
            if (f.Extension == ".asset")
            {
                if (AssetDatabase.LoadAssetAtPath(
                    aurasFolderPath + "/" + f.Name,
                    typeof(ScriptableObject)).GetType()
                    == typeof(HeadAura_ScriptableObject))
                {
                    allGameAurasList.Add((HeadAura_ScriptableObject)AssetDatabase.LoadAssetAtPath(aurasFolderPath + "/" + f.Name, typeof(HeadAura_ScriptableObject)));
                }
                else if (AssetDatabase.LoadAssetAtPath(
                    aurasFolderPath + "/" + f.Name,
                    typeof(ScriptableObject)).GetType()
                    == typeof(LeftArmAura_ScriptableObject))
                {
                    allGameAurasList.Add((LeftArmAura_ScriptableObject)AssetDatabase.LoadAssetAtPath(aurasFolderPath + "/" + f.Name, typeof(LeftArmAura_ScriptableObject)));
                }
                else if (AssetDatabase.LoadAssetAtPath(
                    aurasFolderPath + "/" + f.Name,
                    typeof(ScriptableObject)).GetType()
                    == typeof(RightArmAura_ScriptableObject))
                {
                    allGameAurasList.Add((RightArmAura_ScriptableObject)AssetDatabase.LoadAssetAtPath(aurasFolderPath + "/" + f.Name, typeof(RightArmAura_ScriptableObject)));
                }
                else if (AssetDatabase.LoadAssetAtPath(
                   aurasFolderPath + "/" + f.Name,
                   typeof(ScriptableObject)).GetType()
                   == typeof(TorsoAura_ScriptableObject))
                {
                    allGameAurasList.Add((TorsoAura_ScriptableObject)AssetDatabase.LoadAssetAtPath(aurasFolderPath + "/" + f.Name, typeof(TorsoAura_ScriptableObject)));
                }
            }
        }
    }
    public void ToggleEncyption(bool EncryptionEnabled)
    {
        this.EncryptionEnabled = EncryptionEnabled;
    }

    public void SaveJson()
    {
        List<string> avAura_ID_List = new List<string>();
        foreach (ScriptableObject auraSO in availableAuraList)
        {
            avAura_ID_List.Add(auraSO.name);
        }
        SaveGameData saveGameData = new SaveGameData
        {
            mapPointPosition_ID = mapPointPosition_ID,
            unlockedMapPoint_ID_List = unlockedMapPoint_ID_List,
            equippedAura_Head_Name = equippedAura_Head.name,
            equippedAura_LeftArm_Name = equippedAura_LeftArm.name,
            equippedAura_RightArm_Name = equippedAura_RightArm.name,
            equippedAura_Torso_Name = equippedAura_Torso.name,
            availableAura_Name_List = avAura_ID_List
        };

        long startTime = DateTime.Now.Ticks;
        if (DataService.SaveData("/save.json", saveGameData, EncryptionEnabled))
        {
            Debug.Log("GameData Saved!");
            //autoload em seguida
            LoadJson();
        }
        else
        {
            Debug.LogError("Error saving data!");
        }
    }

    public void LoadJson()
    {
        try
        {
            SaveGameData data = DataService.LoadData<SaveGameData>("/save.json", EncryptionEnabled);

            Debug.Log("Loaded from file:\r\n" + JsonConvert.SerializeObject(data, Formatting.Indented));
            Debug.Log("Game Data Loaded");

            //precisa tratar o DATA(SaveGameData) que recebeu para colocar no DataPersistenceManager
            mapPointPosition_ID = data.mapPointPosition_ID;
            unlockedMapPoint_ID_List = data.unlockedMapPoint_ID_List;
            //carregar auras equipadas
            foreach (ScriptableObject auraSO in allGameAurasList)
            {
                if (auraSO.name == data.equippedAura_Head_Name)
                {
                    equippedAura_Head = (HeadAura_ScriptableObject)auraSO;
                }
                else if (auraSO.name == data.equippedAura_LeftArm_Name)
                {
                    equippedAura_LeftArm = (LeftArmAura_ScriptableObject)auraSO;
                }
                else if (auraSO.name == data.equippedAura_RightArm_Name)
                {
                    equippedAura_RightArm = (RightArmAura_ScriptableObject)auraSO;
                }
                else if (auraSO.name == data.equippedAura_Torso_Name)
                {
                    equippedAura_Torso = (TorsoAura_ScriptableObject)auraSO;
                }
            }
            //limpar lista para evitar duplicatas
            availableAuraList = new List<ScriptableObject>();
            //carregar as auras disponíveis
            foreach (ScriptableObject auraSO in allGameAurasList)
            {
                if (data.availableAura_Name_List.Contains(auraSO.name))
                {
                    availableAuraList.Add(auraSO);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading save file!");
        }
        //fim do load
    }

    private class SaveGameData{
        public int mapPointPosition_ID;
        public List<int> unlockedMapPoint_ID_List;

        //equipped auras
        public string equippedAura_Head_Name;
        public string equippedAura_LeftArm_Name;
        public string equippedAura_RightArm_Name;
        public string equippedAura_Torso_Name;

        public List<string> availableAura_Name_List;
    }
}