using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManagerScript : MonoBehaviour
{
    public List<MapPoint> mapPoints;
    public MapSelectorScript selector;
    public MapPoint currentPoint;

    private DataPersistenceManager dataPersistence;
    
    public TMP_Text currentMapName;

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
        //checa se é um adjacente se sim pode andar pra lá
        foreach  (MapPointDictionary pointDictionary in currentPoint.pontosAdjacentes)
        {
            if (pointDictionary.pontoNoMapa == mapPoint)
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
}
