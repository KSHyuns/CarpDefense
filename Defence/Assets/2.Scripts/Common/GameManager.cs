using Cysharp.Threading.Tasks;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UGS;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform goalTarget;
    public UnitSpawner spawner;

    public Transform towersParent;
    public Transform enemisParent;


    public Scriptable_Prefab scriptable;
    

    public int GameStage = 0;

    public List<SpawnBlock> spawnBlockList = new List<SpawnBlock>();

    public List<TowerUnit> towerUnitList = new List<TowerUnit>();

    public List<EnemyUnit> enemyUnitList = new List<EnemyUnit>();

    public CancellationTokenSource source;

    /// <summary>
    /// 길이 끊어짐
    /// 적들 생성 불가 / 
    /// </summary>
    public bool FailGoal = false;
    /// <summary>
    /// 길이 끊어짐
    /// 타워 생성불가
    /// </summary>
    public bool blockUnWay = false;

    /// <summary>
    /// 파괴된 타워 갯수
    /// 타워를 다시 설치 할경우 0으로 세팅
    /// </summary>
    public int destroyCnt = 0;


    public List<EnemyUnit> overlapUnits = new List<EnemyUnit>();

    public bool overlap
    {
        get => OverLap;
        set
        { 
            OverLap = value;

            if (value == true)
            {
                overlapSpeed().Forget();
            }
        }
    
    }
    private bool OverLap;


    public Color catchColor = new Color(0, 1, 0, 0.3f);
    public Color missingColor = new Color(0, 1, 0, 1f);
    public Color catchColColor = new Color(1, 0, 0, 0.3f);
    public Color missingColColor = new Color(0, 1, 0, 0.3f);

    public TowerUnit selectTower;

    public InputProcess inputProcess;


    private void Awake()
    {
        Instance = this;

        UnityGoogleSheet.LoadAllData();


        if (source != null) source.Dispose();

        source = new();

        

    }

    private async UniTask overlapSpeed()
    {
        overlapUnits.Sort((x,y)=>x.buffer.Count.CompareTo(y.buffer.Count));


        for (int  i = 0; i < overlapUnits.Count; i++)
        {
            if (i % 2 == 0) continue;

           await overlapDelay(i);
        }
            overlapUnits.Clear();
            OverLap = false;

    }

    private async UniTask overlapDelay(int i)
    {
        overlapUnits[i].aiLerp.speed = 0.4f;

        await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.UnscaledDeltaTime, cancellationToken: source.Token);

        overlapUnits[i].aiLerp.speed = 1f;
    }


    /// <summary>
    /// 최대 10
    /// </summary>
    /// <param name="stage"></param>
    public GameData.EnemySheet EnemySheetStageLoad(int stage)
    {
        return GameData.EnemySheet.EnemySheetList[stage];
    }

   
    public async UniTask RemoveTowerLogic()
    {
        FailGoal = true;
        blockUnWay = true;

        foreach (var enemy in enemyUnitList)
        {
            enemy.aiLerp.isStopped = true;
        }

        //Time.timeScale = 0;
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f), DelayType.UnscaledDeltaTime, cancellationToken: source.Token);

        var tower = towerUnitList[UnityEngine.Random.Range(0, towerUnitList.Count - 1)];
        towerUnitList.Remove(tower);
        Destroy(tower.gameObject);

        //foreach (var tower in towerUnitList)
        //{
        //    Destroy(tower.gameObject);
        //}
        //towerUnitList.Clear();

        destroyCnt += 1;

        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f), DelayType.UnscaledDeltaTime, cancellationToken: source.Token);
        //Time.timeScale = 1;

        foreach (var enemy in enemyUnitList)
        {
            enemy.aiLerp.isStopped = false;
        }

        FailGoal = false;
        blockUnWay = false;
    }


    public void PathReSearch()
    {
        IAstarAI[] ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();
        for (int i = 0; i < ais.Length; i++)
        {
            ais[i].SearchPath();
        }
    }
    private void OnDisable()
    {
        spawnBlockList.Clear();
        towerUnitList.Clear();
        enemyUnitList.Clear();

        source.Cancel();
    }

    private void OnDestroy()
    {
        source.Cancel();
        source.Dispose();
    }


    

}
