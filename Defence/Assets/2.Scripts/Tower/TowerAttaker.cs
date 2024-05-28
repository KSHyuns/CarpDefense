using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerAttaker : MonoBehaviour , IAttaker
{
    // Start is called before the first frame update

    private ITowerUnit tower;

    private TowerUnit towerData;

    public float myAtk
    {
        get 
        {
            towerData = tower as TowerUnit;
            return towerData.bungData.stkpoint; 
        }
    }

    public float myAtkRange
    {
        get
        {
            towerData = tower as TowerUnit;
            return towerData.bungData.stkrange;
        }
    }

    public GameObject target;

    private void Awake()
    {
        tower = GetComponent<ITowerUnit>();
    }

    public void ManagedUpdate()
    {
        //들려있다면 
        if (tower.DragOn()) return;

        var colls = Physics2D.OverlapCircleAll(transform.position, (myAtkRange) , 1 << LayerMask.NameToLayer("Enemy"));
       // print($"검출 : {colls.Length}");
        if (colls.Length >= 2)
        {
            //가장 가까운 붕빵이를 검출 해야된다. 계산 ㄱ
            target = Common.FindNearestObject(colls.ToList() , transform.position);
        }
        else if (colls.Length > 0 && colls.Length <= 1)
        {
            target = colls[0].gameObject;
        }
        else
        {
            target = null;
            return;
        }
    }

    public GameObject getEnemy() => target;

    public void SetEnemy(GameObject obj)
    {
        target = obj;
    }
  
}
