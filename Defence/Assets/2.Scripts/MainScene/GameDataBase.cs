using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using TMPro;
using UGS;
using UnityEngine;

public class GameDataBase : Singleton<GameDataBase>
{
    public LogManager logManager;
    public BungBBangResources bungBBangResources;

    /// <summary>
    /// ¿¸√º ∫ÿªß µ•¿Ã≈Õ ±∏º∫
    /// </summary>
    public List<BungData> AllbungBBangList = new List<BungData>();

    /// <summary>
    /// ¿Â¬¯«— ∫ÿªß∏ÆΩ∫∆Æ
    /// </summary>
    public List<BungData> equipBungBBangList = new List<BungData>();
 
    public TextMeshProUGUI logText;
    public CanvasGroup logParent;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI daiaText;

    public SaveData saveData;
    public CancellationTokenSource source;

    public Rito.WeightedRandomPicker<BungData> wrPicker;

    public override void Awake()
    {
        base.Awake();
        saveData = new SaveData(daiaText, goldText);
        
        logManager = new LogManager(logParent, logText);

        UnityGoogleSheet.LoadAllData();


        for (int i = 0; i < GameData.TowerSheet.TowerSheetList.Count; i++)
        {
            var sheetData = GameData.TowerSheet.TowerSheetList[i];
            var data = new BungData(bungBBangResources.bungBBangSprites[i],
                                    bungBBangResources.bungBBangIconSprites[i],
                                    sheetData.name,
                                    sheetData.atkPoint,
                                    sheetData.atkSpeed,
                                    sheetData.atkRange,
                                    sheetData.ability,
                                    sheetData.Description);

            AllbungBBangList.Add(data);
        }



        if(saveData.haveBungBBangList.Count <= 0)  saveData.haveBungBBangList.Add(AllbungBBangList[0]);

        wrPicker = new Rito.WeightedRandomPicker<BungData>();

        wrPicker.Add
            (
              (AllbungBBangList[1], AllbungBBangList[1].ability),
              (AllbungBBangList[2], AllbungBBangList[2].ability),
              (AllbungBBangList[3], AllbungBBangList[3].ability),
              (AllbungBBangList[4], AllbungBBangList[4].ability),
              (AllbungBBangList[5], AllbungBBangList[5].ability),
              (AllbungBBangList[6], AllbungBBangList[6].ability),
              (AllbungBBangList[7], AllbungBBangList[7].ability),
              (AllbungBBangList[8], AllbungBBangList[8].ability),
              (AllbungBBangList[9], AllbungBBangList[9].ability),
              (AllbungBBangList[10], AllbungBBangList[10].ability)
            );



    }
    private void OnEnable()
    {
        if (source != null) source.Cancel();
        source = new();
    }

    private void OnDisable()
    {
    //    source.Cancel();
    }

    private void OnDestroy()
    {
   //     source.Cancel();
    //    source.Dispose();
    }


    [Button]
    public void Add()
    {
        daiaText.text = saveData.daia.ToString();
        logManager.Show($"∫∏ºÆ¿Ã {saveData.daia}∑Œ ∫Ø∞Êµ«æ˙Ω¿¥œ¥Ÿ.").Forget();
    }

    #region Shake
    [Button]
    public async UniTask Shake(Transform target, float duration, float magnitude)
    {
        float timer = 0;
        var pos = target.localPosition;
        while (timer <= duration)
        {
            target.localPosition = Random.insideUnitSphere * magnitude + pos;

            timer += Time.deltaTime;
            await UniTask.Yield();
        }
        target.localPosition = pos;

    }
    #endregion
}

[System.Serializable]    
public class SaveData
{
    //// ∆œ
    //public int beans;
    //// π›¡◊
    //public int Dough;

    private TextMeshProUGUI daiaText;
    private TextMeshProUGUI goldText;

    public SaveData(TextMeshProUGUI _daiaText, TextMeshProUGUI _goldText)
    {
        this.daiaText = _daiaText;
        this.goldText = _goldText;

    }

    [SerializeField] private int Daia;
    // ∫∏ºÆ
    public int daia
    {
        get => Daia;
        set 
        {
            Daia = value;
            daiaText.text = $"{value}";
        }
    }

    [SerializeField] private int Gold;
    // ∞ÒµÂ
    public int gold
    { 
        get => Gold;
        set 
        {
            Gold = value;
            goldText.text = $"{Gold}";
        }
    }

  


    /// <summary>
    /// º“¡ˆ«œ∞Ì ¿÷¥¬ ∫ÿªß ∏ÆΩ∫∆Æ
    /// </summary>
    public List<BungData> haveBungBBangList = new List<BungData>();
}
