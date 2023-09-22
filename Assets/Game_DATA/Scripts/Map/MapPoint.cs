using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapPoint : MonoBehaviour
{
    //Cada ponto no mapa está localizado de algum lado deste MapPoint
    //public Dictionary<MapPoint, Direction> pontosAdjacente;
    public string mapName;
    public List<MapPointDictionary> pontosAdjacentes;
    public int mapPointID;
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
