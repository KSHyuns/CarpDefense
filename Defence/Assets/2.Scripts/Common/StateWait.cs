using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class StateWait : MonoBehaviour , IGameState
{
    private StateMachine stateMachine;
    public void Handle(StateMachine machine)
    {
        if (stateMachine == null)
            stateMachine = machine;

        Transition().Forget();
    }

    private async UniTask Transition()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.UnscaledDeltaTime, cancellationToken: GameManager.Instance.source.Token);


        for (int i= stateMachine.Timer; i >= 0 ; i--) 
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1) , DelayType.UnscaledDeltaTime , cancellationToken : GameManager.Instance.source.Token);
            TextSizeUp(stateMachine.CountText, i.ToString());
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1), DelayType.UnscaledDeltaTime, cancellationToken: GameManager.Instance.source.Token);

        stateMachine.CountText.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
        {
            stateMachine.EnemyCurCont = 0;
            GameManager.Instance.GameStage++;
            GameManager.Instance.stageText.text = $"STAGE {GameManager.Instance.GameStage}";
            stateMachine.RunStart();
        });

    }


    private void TextSizeUp(TextMeshProUGUI _countText , string str)
    {
        _countText.transform.localScale = Vector3.zero;
        _countText .text = str;
        _countText.transform.DOScale(Vector3.one, 0.2f);
    }


}
