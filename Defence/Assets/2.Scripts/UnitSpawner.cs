using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class UnitSpawner : MonoBehaviour
{
    public ObjectPool<EnemyUnit> pool;

    public ObjectPool<EnemyHealth> healthPool;

    public ObjectPool<Bullet> bulletPool;

    private float defalutSpeed = 0.5f;

    private float addSpeed;

    public float moveSpeed;

    public Transform[] spawnPositions;

    public MapGenarator level;
    private void Awake()
    {
        pool = new ObjectPool<EnemyUnit>
        (
            () =>
            {
                var enemy = Instantiate(GameManager.Instance.scriptable.enemyPrefabs, GameManager.Instance.enemisParent);
                enemy.Ipool = pool;
                return enemy;
            },
            (enemy) =>
            {
                enemy.transform.position = GameManager.Instance.spawnBlockDic[new Point(Random.Range(0, 6), 0)].transform.position;

                enemy.gameObject.SetActive(true);
            },
            (enemy) => { enemy.gameObject.SetActive(false); },
            (enemy) => { Destroy(enemy.gameObject); },
            maxSize: 20
        );

        healthPool = new ObjectPool<EnemyHealth>
        (
            () =>
            {
                var health = Instantiate(GameManager.Instance.scriptable.enemyHealthSliderPrefabs, GameManager.Instance.sliderParent);
                health.Ipool = healthPool;
                return health;
            },
            (health) =>
            {
                health.transform.position = Vector3.one * 999;
                health.gameObject.SetActive(true);
            },
           (health) => { health.gameObject.SetActive(false); },
           (health) => { Destroy(health.gameObject); },
           maxSize: 20
        );

        bulletPool = new ObjectPool<Bullet>
        (
            () => 
            {
                var bullet = Instantiate(GameManager.Instance.scriptable.bulletPrefabs);
                bullet.Ipool = bulletPool;
                return bullet;
            },
            (bullet) => { bullet.gameObject.SetActive(true); },
            (bullet) => { bullet.gameObject.SetActive(false); },
            (bullet) => { Destroy(bullet.gameObject); }
        );

      //  uni().Forget();
    }

   
    public async UniTask StageGo( int maxCnt , UnityAction unityAction )
    {
        //GameManager.Instance.FailGoal = true;

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        //GameManager.Instance.FailGoal = false;

        addSpeed =  GameData.EnemySheet.EnemySheetList[GameManager.Instance.GameStage].addSpeed;
        defalutSpeed += addSpeed;
        moveSpeed = defalutSpeed;  

        for (int i = 0; i < maxCnt; i++)
        {
            await UniTask.WaitUntil(() => GameManager.Instance.FailGoal == false);

            var enemy = pool.Get();
            Point goalPoint = new Point(2 + Random.Range(0,2) ,level.y -1);
            enemy.setter.target = GameManager.Instance.spawnBlockDic[goalPoint].transform;
            (enemy as EnemyUnit).isFinal = false;
            (enemy as EnemyUnit).spriteRenderer.color = new Color(1, 1, 1, 1);
            enemy.colliderOn(true);

            enemy.isStopped(false);
            enemy.aiLerp.speed = moveSpeed;
            enemy.state = EnemyUnit.moveState.move;
            GameManager.Instance.enemyUnitList.Add(enemy);

            var health = healthPool.Get();
            enemy.enemyHealth = health;

            float maxhp =  GameData.EnemySheet.EnemySheetList[GameManager.Instance.GameStage].hp;
            enemy.enemyHealth.healthSlider.maxValue = maxhp;
            enemy.Health = maxhp;

            await UniTask.Delay(TimeSpan.FromSeconds(2.5f), DelayType.UnscaledDeltaTime, cancellationToken: GameManager.Instance.source.Token);
        }

        unityAction?.Invoke();

        
    }

}
