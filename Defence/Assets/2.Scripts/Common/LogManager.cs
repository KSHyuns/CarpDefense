using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager
{
    public TextMeshProUGUI logText;
    public CanvasGroup logParent;

    private bool isShow = false;
    public LogManager() { }
    public LogManager(CanvasGroup _logParent ,TextMeshProUGUI _logText )
    { 
        logText = _logText;
        logParent = _logParent;
        logParent.alpha = 0;
    }
    Sequence myseq;
    public async UniTask Show(string str)
    {
      //  if (str == logText.text && isShow) return;
        
      //  isShow = true;

        myseq?.Rewind(); //Ãß°¡
        myseq = DOTween.Sequence();
        myseq.OnStart(() => 
        {
            logText.text = str;
            logParent.alpha = 0;
        });
        myseq.Append(DOTween.To(() => logParent.alpha, x => logParent.alpha = x, 1, 1));
        myseq.AppendInterval(1f);
        myseq.Append(DOTween.To(() => logParent.alpha, x => logParent.alpha = x, 0, 1)).OnComplete(() => 
        {
         //   isShow = false;
        });


      

        await UniTask.Yield();
    }

    public static void print(string str) => print(str);

}
