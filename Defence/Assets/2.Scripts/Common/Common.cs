using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BungData
{
    public Sprite mySprite;
    public Sprite IconSprite;
    public string name;
    public float stkpoint;
    public float stkspeed;
    public float stkrange;
    public float ability;
    public string description;

    public BungData()
    { }

    public BungData(Sprite _sprite, Sprite icon ,string _name, float _stkpoint, float _stkspeed, float _stkrange, float _ability, string _deesciption)
    {
        mySprite = _sprite;
        IconSprite = icon;
        name = _name;
        stkpoint = _stkpoint;
        stkspeed = _stkspeed;
        stkrange = _stkrange;
        ability = _ability;
        description = _deesciption;
    }


}

[System.Serializable]
public struct bungDataShowConponent
{
    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI stkPointText; 
    public TextMeshProUGUI stkSpeedText;
    public TextMeshProUGUI stkRangeText;
    public TextMeshProUGUI descriptionText;
}


public enum ECurStare { RUN, WAIT , GameOver }
public class Common
{
    public static GameObject FindNearestObject(List<Collider2D> someList , Vector3 tr)
    {
        // 오브젝트들의 리스트
        List<Collider2D> objList = someList;

        // LINQ를 이용해 가장 가까운 적을
        var neareastObject = objList
            .OrderBy(obj =>
            {
                return Vector3.Distance(tr, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject.gameObject;
    }
}

