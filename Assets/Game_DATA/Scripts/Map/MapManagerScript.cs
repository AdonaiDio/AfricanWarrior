using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapManagerScript : MonoBehaviour
{
    public List<MapPoint> mapPoints;
    public MapSelectorScript selector;
    public MapPoint currentPoint;

    private DataPersistenceManager dataPersistence;
    
    public TMP_Text currentMapName;

    public GameObject battlePopUp;

    private void Awake()
    {
        dataPersistence = FindObjectOfType<DataPersistenceManager>();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => dataPersistence.initialized);
        
        SpawnOnMapPoint(mapPoints[dataPersistence.mapPointPosition_ID]);
        currentPoint = mapPoints[dataPersistence.mapPointPosition_ID];

    }

    public void CheckPossibleMoviment(MapPoint mapPoint)
    {
        //checa se é um adjacente e se está desbloqueado sim pode andar pra lá
        foreach  (MapPointDictionary pointDictionary in currentPoint.pontosAdjacentes)
        {
            if (pointDictionary.pontoNoMapa == mapPoint 
                && dataPersistence.unlockedMapPoint_ID_List.Contains(mapPoint.mapPointID) )
            {
                GoToPoint(mapPoint);
            }
        }
    }
    
    public void GoToPoint(MapPoint mp)
    {
        LeanTween.move(selector.gameObject, mp.transform.position, 0.75f)
            .setOnStart( () =>
            {   
                selector.TweemPause(); 
            })
            .setOnComplete( () => 
            {   
                selector.TweemResume();
                currentPoint = mp;
            });
        dataPersistence.mapPointPosition_ID = mp.mapPointID;
        dataPersistence.SaveJson();
        ShowMapName(mp);
    }

    private void ShowMapName(MapPoint mp)
    {
        currentMapName.text = mp.mapName;
    }

    public void SpawnOnMapPoint(MapPoint mp)
    {
        selector.transform.position = mp.transform.position;
    }

    public void CloseBattlePopUp()
    {
        battlePopUp.SetActive(false);
    }
    public void StartBattle()
    {
        // tem que por o offset da ordem das batalhas
        SceneManager.LoadScene(currentPoint.mapPointID + 3);
    }
}
