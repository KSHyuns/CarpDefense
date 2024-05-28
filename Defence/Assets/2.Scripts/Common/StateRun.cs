using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class StateRun : MonoBehaviour , IGameState
{
    private StateMachine stateMachine;

    public void Handle(StateMachine machine)
    {
        if (stateMachine == null)
            stateMachine = machine;

        int maxUnit = GameData.EnemySheet.EnemySheetList[GameManager.Instance.GameStage].bungCnt;
        stateMachine.EnemyMaxCont = maxUnit;
        stateMachine.EnemyCurCont = 0;
        GameManager.Instance.spawner.StageGo(maxUnit, null).Forget();
        DoughUpdatae().Forget();
    }



    async UniTask DoughUpdatae()
    {
        while (stateMachine.curStare == ECurStare.RUN)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
            ++GameManager.Instance.DoughCnt;
        }
    }
   




}
