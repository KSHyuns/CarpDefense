using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenarator : MonoBehaviour
{
    public Scriptable_Prefab scriptable;

    public int x = 6;
    public int y = 9;

    private void Awake()
    {
        Generator();
    }


    private void Generator()
    {
        bool vertical = x % 2 == 0 ? true : false;
        bool horizon  = y % 2 == 0 ? true : false;

        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                var block = Instantiate(scriptable.spownBlockPrefabs, transform);


                float xPos = vertical ? (x / 2) -0.5f : (x / 2);
                float yPos = horizon ? (y / 2) - 0.5f : (y / 2); 


                block.transform.localPosition = new Vector3(-xPos + i, yPos - j);

                GameManager.Instance.spawnBlockList.Add(block);
            }
        }


        GameManager.Instance.spawner.transform.position =  GameManager.Instance.spawnBlockList.First().transform.position;
        GameManager.Instance.goalTarget.transform.position = new Vector3(0.5f, -4f);


    }

   


}
