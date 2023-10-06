using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapPoint : MonoBehaviour
{
    //Cada ponto no mapa está localizado de algum lado deste MapPoint
    //public Dictionary<MapPoint, Direction> pontosAdjacente;
    public string mapName;
    public List<MapPointDictionary> pontosAdjacentes;
    public int mapPointID;
    private Image image;
    public Image lineImage;

    private DataPersistenceManager dataPersistence;
    private void Awake()
    {
        image = GetComponent<Image>();
        dataPersistence = FindObjectOfType<DataPersistenceManager>();

    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => dataPersistence.initialized);

        if (dataPersistence.unlockedMapPoint_ID_List.Contains(mapPointID))
        {
            image.color = new Color(0.4941176f, 0.9471262f, 1f);
            if (lineImage != null)
            {
                lineImage.color = new Color(0,0,0,1);
            }
        }
        else
        {
            image.color = new Color(0.9433962f, 0.3426486f, 0.3426486f);
            if (lineImage != null)
            {
                lineImage.color = new Color(0, 0, 0, 0);
            }
        }
    }
}

[Serializable]
public class MapPointDictionary
{
    public MapPoint pontoNoMapa;
    public Direction[] direção;
}

public enum Direction {
    Esquerda, Direita, Cima, Baixo
}
