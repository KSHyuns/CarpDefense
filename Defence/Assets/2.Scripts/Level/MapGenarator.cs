using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class MapGenarator : MonoBehaviour
{
    public int x = 6;
    public int y = 9;


    public Point goalPoint;
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
                var block = Instantiate(GameManager.Instance.scriptable.spownBlockPrefabs, transform);
                block.points = new Point(i,j);

                float xPos = vertical ? (x / 2) -0.5f : (x / 2);
                float yPos = horizon ? (y / 2) - 0.5f : (y / 2); 


                block.transform.localPosition = new Vector3(-xPos + i, yPos - j);

                GameManager.Instance.spawnBlockList.Add(block);
                GameManager.Instance.spawnBlockDic.Add(block.points, block);
            }
        }

        goalPoint = new Point(3, y-1);
        GameManager.Instance.goalTarget.transform.position = GameManager.Instance.spawnBlockDic[goalPoint].transform.position;


      //  GameManager.Instance.spawner.transform.position =  GameManager.Instance.spawnBlockList.First().transform.position;
      //   GameManager.Instance.goalTarget.transform.position = GameManager.Instance.spawnBlockList.Last().transform.position;


    }

   


}
