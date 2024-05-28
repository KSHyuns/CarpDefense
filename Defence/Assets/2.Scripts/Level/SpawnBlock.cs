using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct Point
{
    public int x;
    public int y;

    public Point(int _x, int _y)
    {
        x = _x; y = _y;
    }
}

public class SpawnBlock : MonoBehaviour
{

    public Point points;
    public void OnMouseDown()
    {
        if (GameManager.Instance.blockUnWay) return;
       // Debug.Log("Down");

    }


}
