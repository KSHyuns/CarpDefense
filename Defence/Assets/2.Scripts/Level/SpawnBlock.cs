using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    public void OnMouseDown()
    {
        if (GameManager.Instance.blockUnWay) return;
        Debug.Log("Down");
       
        {
            GameManager.Instance.destroyCnt = 0;
            var obj = Instantiate(GameManager.Instance.scriptable.towerUnit, GameManager.Instance.towersParent);
            obj.transform.position = transform.position;
            GameManager.Instance.towerUnitList.Add(obj);
            GameManager.Instance.inputProcess.ItowerUnitList.Add(obj);
        }
        //   GameManager.Instance.PathReSearch();


    }


}
