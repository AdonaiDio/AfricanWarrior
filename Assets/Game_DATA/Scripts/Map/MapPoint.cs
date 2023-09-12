using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapPoint : MonoBehaviour
{
    //Cada ponto no mapa est� localizado de algum lado deste MapPoint
    //public Dictionary<MapPoint, Direction> pontosAdjacente;
    public List<MapPointDictionary> pontosAdjacentes;
}

[Serializable]
public class MapPointDictionary
{
    public MapPoint pontoNoMapa;
    public Direction[] dire��o;
}

public enum Direction {
    Esquerda, Direita, Cima, Baixo
}
