using Cysharp.Threading.Tasks;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class GameManager : SerializedMonoBehaviour
{
    public static GameManager Instance;

    [Title("���۰� ��")]
    public Transform goalTarget;
    public UnitSpawner spawner;

    [Title("Ǯ�� Parent")]
    public Transform towersParent;
    public Transform enemisParent;
    public Transform sliderParent;
    public Transform storeRoomParent;

    [Title("��ũ���ͺ�")]
    public Scriptable_Prefab scriptable;
    public BungBBangResources resources;
    [Title("�Է½�ũ��Ʈ")]
    public InputProcess inputProcess;

    [Title("���ӽ�����Ʈ�ӽ�")]
    public StateMachine machine;

    [TabGroup("List")] public List<SpawnBlock> spawnBlockList = new List<SpawnBlock>();
    [TabGroup("List")] public Dictionary<Point, SpawnBlock> spawnBlockDic = new Dictionary<Point, SpawnBlock>();

    [TabGroup("List")] public List<ITowerUnit> towerUnitList = new List<ITowerUnit>();

    [TabGroup("List")] public List<IEnemyUnit> enemyUnitList = new List<IEnemyUnit>();

    [TabGroup("List")] public List<IBullet> bulletList = new List<IBullet>();


    [TabGroup("Tower Alpha")] public Color catchColor = new Color(1, 1, 1, 0.3f);
    [TabGroup("Tower Alpha")] public Color missingColor = Color.white;

    [TabGroup("Text")] public TextMeshProUGUI gameHpText;
    [TabGroup("Text")] public TextMeshProUGUI enemyCntText;
    [TabGroup("Text")] public TextMeshProUGUI stageText;
    [TabGroup("Text")] public TextMeshProUGUI DoughText;


    [System.NonSerialized] public CancellationTokenSource source;

    public int GameStage = 0;
    /// <summary>
    /// ���� ������
    /// ���� ���� �Ұ� / 
    /// </summary>
    public bool FailGoal = false;
    /// <summary>
    /// ���� ������
    /// Ÿ�� �����Ұ�
    /// </summary>
    public bool blockUnWay = false;

    /// <summary>
    /// �ı��� Ÿ�� ����
    /// Ÿ���� �ٽ� ��ġ �Ұ�� 0���� ����
    /// </summary>
    public int destroyCnt = 0;

    [System.NonSerialized] public List<EnemyUnit> overlapUnits = new List<EnemyUnit>();

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

    [Title("�����")]
    public TowerUnit selectTower;
    public BungData bungData;


    [SerializeField] private float Gamelife;
    public float GameLife
    {
        get => Gamelife;
        set
        {
            Gamelife = value;
            gameHpText.text = $"HP : {Gamelife}";
            if (Gamelife <= 0)
            {
                machine.GameOver();
            }

        }
    }

    public Button SoundBtn;
    private bool bgmOn = true;

    private int doughCnt;
    public int DoughCnt
    { 
        get => doughCnt;
        set 
        {
            doughCnt = value;
            DoughText.text = $"{doughCnt}";
        }
    }


    

    private void Awake()
    {
        Instance = this;

        if (source != null) source.Dispose();

        source = new();

        GameLife = 5;

        var data = GameDataBase.Instance.equipBungBBangList[UnityEngine.Random.Range(0, GameDataBase.Instance.equipBungBBangList.Count)];
        Image image = Instantiate(scriptable.IconPrefabs, storeRoomParent);
        bungStoreRoom.Add(new IconData(data.IconSprite, image, data));

        SoundMGR.Instance.SoundPlay(audioName.Game);
    }

    public void SoundOn()
    { 
        bgmOn = !bgmOn;
        if (SoundBtn.TryGetComponent(out Image img))
        {
            if (bgmOn)
            {
                img.sprite = resources.BGM_on;
                SoundMGR.Instance.SoundAllPlay();
                SoundMGR.Instance.SoundPlay(audioName.music_on);
            }
            else
            {
                img.sprite = resources.BGM_off;
                SoundMGR.Instance.SoundAllStop();
            }
        }
    }



    private async UniTask overlapSpeed()
    {
        overlapUnits.Sort((x, y) => x.buffer.Count.CompareTo(y.buffer.Count));


        for (int i = 0; i < overlapUnits.Count; i++)
        {
            if (i % 2 == 0) continue;

            await overlapDelay(i);
        }
        overlapUnits.Clear();
        overlap = false;

    }

    private async UniTask overlapDelay(int i)
    {
        overlapUnits[i].state = EnemyUnit.moveState.move;

        overlapUnits[i].aiLerp.speed -= 0.1f;

        await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.UnscaledDeltaTime, cancellationToken: source.Token);

        overlapUnits[i].aiLerp.speed = spawner.moveSpeed;
    }

    public async UniTask RemoveTowerLogic()
    {
        FailGoal = true;
        blockUnWay = true;

        foreach (var enemy in enemyUnitList)
        {
            enemy.isStopped(true);
            enemy.idle();
        }

        if (towerUnitList.Count > 0)
        { 
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f), DelayType.UnscaledDeltaTime, cancellationToken: source.Token);
            var tower = towerUnitList[UnityEngine.Random.Range(0, towerUnitList.Count - 1)];
            towerUnitList.Remove(tower);
            tower.MyDestroy();
            PathReSearch();
            destroyCnt += 1;
        }

        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f), DelayType.UnscaledDeltaTime, cancellationToken: source.Token);

        foreach (var enemy in enemyUnitList)
        {
            (enemy as EnemyUnit).state = EnemyUnit.moveState.move;
            enemy.isStopped(false);
        }

        FailGoal = false;
        blockUnWay = false;

    }


    public void PathReSearch()
    {
        IAstarAI[] ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();
        ais.ToList().ForEach(a => a.SearchPath());
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


    public class IconData
    {
        public Sprite Icon;
        public Image render;
        public BungData bungdata;

        public IconData(Sprite icon, Image render, BungData bungdata) 
        {
            this.Icon = icon;
            this.render = render;
            this.bungdata = bungdata;

            render.sprite = icon;
        }
    }

    /// <summary>
    /// �÷����� Ÿ�� ����� ����Ʈ
    /// </summary>
    public List<IconData> bungStoreRoom = new List<IconData>();


    // ��ư ���� 10�� ¥��

    private int vanzuc = 10;
    public void AddRandomTower()
    {
        SoundMGR.Instance.SoundPlay(audioName.reward_btn);

        if (bungStoreRoom.Count <= 4)  
        {
            if (DoughCnt <= 0)
            {
                DoughCnt = 0;
                GameDataBase.Instance.logManager.Show($"������ {vanzuc}�� �ʿ��մϴ�").Forget();
                return;
            }
            else
            {
                if (DoughCnt >= vanzuc)
                {
                    DoughCnt -= vanzuc;
                    vanzuc += 10;
                }
                else
                {
                    GameDataBase.Instance.logManager.Show($"������ {vanzuc}�� �ʿ��մϴ�").Forget();
                    return;
                }
            }

            var data = GameDataBase.Instance.equipBungBBangList[UnityEngine.Random.Range(0, GameDataBase.Instance.equipBungBBangList.Count)];
            Image image = Instantiate(scriptable.IconPrefabs, storeRoomParent);
            bungStoreRoom.Add(new IconData(data.IconSprite , image , data));
            //����� �̹��� ����
        }
        else
        {
            GameDataBase.Instance.logManager.Show("Ÿ�� ����� ���� ä�������ϴ� ����� ��켼��").Forget();
        }
    }

    public void SpawnTower(SpawnBlock block)
    {
       destroyCnt = 0;
        var obj = Instantiate(scriptable.towerUnit, towersParent);
        var bungIconData = bungStoreRoom[0];
       bungStoreRoom.RemoveAt(0);
       bungData = bungIconData.bungdata;

        Destroy(bungIconData.render.gameObject);


        obj.bungBBangSetting(GameManager.Instance.bungData);
        obj.RangeLound();
        obj.transform.position = block.transform.position - Vector3.forward;
        Instance.towerUnitList.Add(obj);
    }

    private void Update()
    {
        foreach (var tower in towerUnitList)
        {
            var towerUnit = (tower as TowerUnit);
            towerUnit.attaker.ManagedUpdate();
            towerUnit.Attacked();
        }

        if (bulletList.Count > 0)
        {
            for (int i = 0; i < bulletList.Count; i++)
            {
                bulletList[i].OnUpdate();
            }
        }
    }

}
