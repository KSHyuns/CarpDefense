using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShopProcess : MonoBehaviour
{


    private void Awake()
    {
        SoundMGR.Instance.SoundPlay(audioName.Shop);
    }
    public void price(int addDaia)
    {
        GameDataBase.Instance.logManager.Show($"보석 {addDaia}개 구매했습니다").Forget();
        GameDataBase.Instance.saveData.daia += addDaia;

    //    DOTween.To(() => GameDataBase.Instance.saveData.daia, x => GameDataBase.Instance.saveData.daia = x, GameDataBase.Instance.saveData.daia + addDaia, 0.7f);

    }


    public void purchase3900()
    {
        price(150);
#if UNITY_EDITOR
#else
       // IAP.Instance.Prodect3900();
#endif
    }
    public void purchase6900()
    {
        price(500);
#if UNITY_EDITOR 
#else
       // IAP.Instance.Prodect6900();
#endif
    }
    public void purchase10900()
    {
        price(1000);
#if UNITY_EDITOR
#else
       // IAP.Instance.Prodect10900();
#endif
    }
    public void purchase29900()
    {
        price(1500);
#if UNITY_EDITOR
#else
        //IAP.Instance.Prodect29900();
#endif
    }
}
