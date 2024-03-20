using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

[System.Serializable]
public class bungData
{ 
    public Sprite mySprite;
    public string name;
    public float stkpoint;
    public float stkspeed;
    public float stkrange;
    public float ability;
    public string description;

    public bungData()
    { }

    public bungData(Sprite _sprite, string _name, float _stkpoint, float _stkspeed, float _stkrange, float _ability ,string _deesciption)
    { 
        mySprite = _sprite;
        name = _name;
        stkpoint = _stkpoint;
        stkspeed = _stkspeed;
        stkrange = _stkrange;
        ability = _ability;
        description = _deesciption;
    }


}

public class TowerDataBase : Singleton<TowerDataBase>
{

    public BungBBangResources bungBBangResources;

    /// <summary>
    /// ¿¸√º ∫ÿªß µ•¿Ã≈Õ ±∏º∫
    /// </summary>
    public List<bungData> bungBBangAllDataList = new List<bungData>();


    /// <summary>
    /// ªÃæ∆º≠ º“¡ˆ«œ∞Ì¿÷¥¬ ∫ÿªß∏ÆΩ∫∆Æ
    /// </summary>
    public List<bungData> equipBungBBangList = new List<bungData>();

    public override void Awake()
    {
        base.Awake();

        UnityGoogleSheet.LoadAllData();

        for (int i = 0; i < GameData.TowerSheet.TowerSheetList.Count; i++)
        {
            var sheetData = GameData.TowerSheet.TowerSheetList[i];
            var data = new bungData(bungBBangResources.bungBBangSprites[i] ,
                                    sheetData.name,
                                    sheetData.atkPoint,
                                    sheetData.atkSpeed,
                                    sheetData.atkRange ,
                                    sheetData.ability,
                                    sheetData.Description);

            bungBBangAllDataList.Add(data);
        }

        
    
    }
}
