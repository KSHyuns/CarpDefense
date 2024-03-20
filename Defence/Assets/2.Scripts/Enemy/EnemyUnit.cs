using Cysharp.Threading.Tasks;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyUnit : MonoBehaviour
{
    public IObjectPool<EnemyUnit> Ipool;

    public AILerp aiLerp;
    public AIDestinationSetter setter;

    private EnemyHealth enemyHealth;

    private bool isFinal;


    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        setter.target = GameManager.Instance.goalTarget;

        //GameManager.Instance.PathReSearch();
       
        

        isFinal = false;
       // Taskupdate().Forget();
    }

    public List<Vector3> buffer = new List<Vector3>();
    public bool stal;
    async UniTask Taskupdate()
    {
        await UniTask.Yield();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), DelayType.UnscaledDeltaTime, cancellationToken: GameManager.Instance.source.Token);

        while (true)
        {
            await UniTask.WaitUntil(()=> isFinal == false , cancellationToken : GameManager.Instance.source.Token);

            

            if (setter.ai.remainingDistance <= 0 && setter.ai.reachedDestination )
            {
             //   print("도착? " +setter.ai.hasPath);
              //  Debug.Log("도착");
              //  Release();
               // stal = false;
             //   return;

            }
            else
            {
                setter.ai.GetRemainingPath(buffer, out stal);
                //if (buffer.Count > 2   && buffer[buffer.Count-1] != GameManager.Instance.goalTarget.position && !setter.ai.reachedEndOfPath)
                //{

                //    if (GameManager.Instance.blockUnWay == false)
                //    {
                //        GameManager.Instance.FailGoal = true;
                //        print("막혀있다");
                //        isFinal = true;

                //        Time.timeScale = 0;

                //        GameManager.Instance.blockUnWay = true;


                //        TowerUnit towers  = GameManager.Instance.towerUnitList[UnityEngine.Random.Range(0, GameManager.Instance.towerUnitList.Count)];
                //        GameManager.Instance.towerUnitList.Remove(towers);
                //        Destroy(towers.gameObject);

                //        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), DelayType.UnscaledDeltaTime, cancellationToken: GameManager.Instance.source.Token);



                //        isFinal = false;
                //        GameManager.Instance.blockUnWay = false;
                //        GameManager.Instance.FailGoal = false;
                //        Time.timeScale = 1;
                //    }
                //    else return;

                //}



            }
            await UniTask.Yield(cancellationToken: GameManager.Instance.source.Token);
        }

    }


    private void Update()
    {
       

        if (setter.ai.reachedDestination && setter.ai.remainingDistance <= 0)
        {
            Release();
        }
        else
        {
            if (buffer.Count > 2 && buffer.Last() != GameManager.Instance.goalTarget.position)
            {
                if (GameManager.Instance.FailGoal && GameManager.Instance.blockUnWay /*|| GameManager.Instance.destroyCnt >= 1*/) return;
                {
                    print("꽉 막혀서 못감");

                    //타워 지움
                    GameManager.Instance.RemoveTowerLogic().Forget();
                }
                
            }
        }


        var coll2DAll = Physics2D.OverlapCircleAll(transform.position, 0.6f, 1 << LayerMask.NameToLayer("Enemy"));

        if (coll2DAll.Length >= 2 && !GameManager.Instance.overlap)
        {
            Debug.Log("호출");

            if (!GameManager.Instance.overlapUnits.Contains(coll2DAll[0].GetComponent<EnemyUnit>()))
                GameManager.Instance.overlapUnits.Add(coll2DAll[0].GetComponent<EnemyUnit>());

            if (!GameManager.Instance.overlapUnits.Contains(coll2DAll[1].GetComponent<EnemyUnit>()))
                GameManager.Instance.overlapUnits.Add(coll2DAll[1].GetComponent<EnemyUnit>());


            GameManager.Instance.overlap = true;
        }


        

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.7f);
    }

    private async UniTask alphaTowerPath()
    {
        if (GameManager.Instance.overlap == true) return;

      //  GameManager.Instance.overlap = true;
        var alphaTower = Instantiate(GameManager.Instance.scriptable.towerUnit);
        alphaTower.transform.position = GameManager.Instance.overlapUnits[0].transform.position;
        setter.ai.SearchPath();
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        Destroy(alphaTower.gameObject);
      //  GameManager.Instance.overlap = false;


    }


    private async UniTask rayStopNGo()
    {
        aiLerp.canMove = false;

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        aiLerp.canMove = true;
    }



    private void LateUpdate()
    {
        setter.ai.GetRemainingPath(buffer, out stal);
        setter.ai.SearchPath();
    }


    private void OnDisable()
    {
        transform.position = Vector3.one * 999;
    }

    public void Release()
    {
        GameManager.Instance.enemyUnitList.Remove(this);
        Ipool.Release(this);
    }




}
