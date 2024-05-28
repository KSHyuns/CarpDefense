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
        //����ִٸ� 
        if (tower.DragOn()) return;

        var colls = Physics2D.OverlapCircleAll(transform.position, (myAtkRange) , 1 << LayerMask.NameToLayer("Enemy"));
       // print($"���� : {colls.Length}");
        if (colls.Length >= 2)
        {
            //���� ����� �ػ��̸� ���� �ؾߵȴ�. ��� ��
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
