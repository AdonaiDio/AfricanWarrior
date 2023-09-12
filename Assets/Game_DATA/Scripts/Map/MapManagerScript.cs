using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManagerScript : MonoBehaviour
{
    public List<MapPoint> mapPoints;
    public MapSelectorScript selector;
    public MapPoint currentPoint;

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
    }
}
