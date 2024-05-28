using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyUnit : MonoBehaviour, IEnemyUnit
{
    public IObjectPool<EnemyUnit> Ipool;
    public AILerp aiLerp;
    public AIDestinationSetter setter;
   public EnemyHealth enemyHealth;
   [System.NonSerialized] public SpriteRenderer spriteRenderer;
    public Collider2D col2d;

    public bool isFinal = false;


    [SerializeField] private float health;

    public ITowerUnit attakedtower;

    public enum moveState {idle , move , death }

    public moveState state = moveState.idle;

    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            enemyHealth.healthSlider.value = health;
            if (health <= 0  && isFinal == false)
            {
                isFinal = true;
                isStopped(true);
                colliderOn(false);
                ++GameManager.Instance.machine.EnemyCurCont;
               
                health = 0;
                EnemyRelease().Forget();
            }
        }
    }

    

    private async UniTask EnemyRelease()
    {
        death();
        SoundMGR.Instance.SoundPlay(audioName.death);
        for(int i=0; i< 3; i++) 
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            spriteRenderer.color = new Color(1, 1, 1, 0);
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }

        Release();
    }

    private void Awake()
    {
        spriteRenderer??=GetComponentInChildren<SpriteRenderer>();
     //   setter.target = GameManager.Instance.goalTarget;
        isFinal = false;
       
    }

    public List<Vector3> buffer = new List<Vector3>();
    public bool stal;


    public void colliderOn(bool t)
    {
        col2d.enabled =t;
    }
    private void OnDisable()
    {
     //   transform.position = Vector3.one * 999;
    }

    public void Release()
    {
        isFinal = true;
        enemyHealth.Release();
        Ipool.Release(this);
        GameManager.Instance.enemyUnitList.Remove(this);
    }

    #region Interface
    public void OnUpdate()
    {
        if (setter.ai.reachedDestination && setter.ai.remainingDistance <= 0f )
        {
            GameManager.Instance.GameLife--;
            GameManager.Instance.machine.EnemyCurCont++;
            //도착후 골드 동상에 효과 주기

            GameDataBase.Instance.Shake(GameManager.Instance.goalTarget,0.2f, 0.1f).Forget();
            //동상에 균열 주고싶음 
            //살짝 흔들림 
            Release();
        }
        else
        {
            

            int last = buffer.Count - 1;

            Vector3 t1 = (GameManager.Instance.spawnBlockDic[new Point(2, GameManager.Instance.spawner.level.y - 1)].transform.position);
            Vector3 t2 =(GameManager.Instance.spawnBlockDic[new Point(3, GameManager.Instance.spawner.level.y - 1)].transform.position);
            Debug.Log($"t1 = {t1}");
            Debug.Log($"t2 = {t2}");

            if (buffer.Count > 2 && (buffer[last]  != t1  && buffer[last] != t2))
            {
                if (GameManager.Instance.FailGoal && GameManager.Instance.blockUnWay /*|| GameManager.Instance.destroyCnt >= 1*/) return;
                {
                    print("꽉 막혀서 못감");
                    //타워 지움
                 
                        GameManager.Instance.RemoveTowerLogic().Forget();
                }
            }
        }
    }
    public void OnSerchPath()
    {
        setter.ai.SearchPath();
    }
    public void OnbufferPath()
    {
        setter.ai.GetRemainingPath(buffer, out stal);
    }
    public void overlapCircleLogic()
    {
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
    public void isStopped(bool value)
    {
        aiLerp.isStopped = value;
        
    }
    public void sliderPositionUpdate()
    {
        enemyHealth.transform.position = transform.position - Vector3.up * 0.5f;
    }
    #endregion

    public void FixedUpdate()
    {
        OnSerchPath();
        OnbufferPath();
        overlapCircleLogic();
        OnUpdate();
        sliderPositionUpdate();
        move();
    }

    public void idle()
    {
        state = moveState.idle;
        spriteRenderer.sprite = GameManager.Instance.resources.enemyIdle;
    }

    private float movedelay = 0;

    private int animCnt   = 1;
    public void move()
    {
        if (state != moveState.move) return;

        if ( (movedelay += Time.deltaTime) >= 0.4f)
        {
            spriteRenderer.sprite = GameManager.Instance.resources.enemyMove[animCnt++ % GameManager.Instance.resources.enemyMove.Length    ];
            movedelay = 0;
        }
    }

    public void death()
    {
        state = moveState.death;
        spriteRenderer.sprite = GameManager.Instance.resources.enemyDeath;
    }
}
