using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;
using Cysharp.Threading.Tasks;
using System;


public class UnitSpawner : MonoBehaviour
{

    public ObjectPool<EnemyUnit> pool;

   

    private void Awake()
    {
        pool = new ObjectPool<EnemyUnit>
        (
            () =>
            {
                var enemy = Instantiate(GameManager.Instance.scriptable.enemyPrefabs , GameManager.Instance.enemisParent);
                enemy.Ipool = pool;
                return enemy;
            },
            (enemy) => 
            {
                enemy.transform.position = transform.position;
                enemy.gameObject.SetActive(true); 
            },
            (enemy) => { enemy.gameObject.SetActive(false); },
            (enemy) => { Destroy(enemy.gameObject); },
            maxSize: 20
        );

        
        
        uni().Forget();
    }

   
    private async UniTask uni()
    {
        GameManager.Instance.FailGoal = true;

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        GameManager.Instance.FailGoal = false;


        while (true)
        {
            await UniTask.WaitUntil(() => GameManager.Instance.FailGoal == false , cancellationToken : GameManager.Instance.source.Token );
            var enemy = pool.Get();
            GameManager.Instance.enemyUnitList.Add(enemy);
        //    print(pool.CountAll);
            await UniTask.Delay(TimeSpan.FromSeconds(2.5f) , DelayType.UnscaledDeltaTime, cancellationToken: GameManager.Instance.source.Token);
        }
    }
    

  


   





}
