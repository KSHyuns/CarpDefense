using Cysharp.Threading.Tasks;
using EasyTransition;
using UnityEngine;

public class StateGameOver : MonoBehaviour , IGameState
{
    private StateMachine machine;

    public void Handle(StateMachine _machine)
    {
        if (machine == null) 
            machine = _machine;

        GameDataBase.Instance.logManager.Show("Life Zero GameOver").Forget();

        //게임 종료

        GameManager.Instance.FailGoal = true;
        
        foreach (var enemy in GameManager.Instance.enemyUnitList)
        {
            enemy.isStopped(true);
            (enemy as EnemyUnit).isFinal = true;
        }
        // 결과창 패널 오픈
        GameDataBase.Instance.saveData.gold += GameManager.Instance.DoughCnt;

        //SendMessage 는 무겁기때문에 자주 쓰면 안됨 
        GameManager.Instance.goalTarget.SendMessageUpwards("SpriteSet", SendMessageOptions.RequireReceiver);
        
        machine.disolveController.Off().Forget();
        
        //씬이동
        Ending().Forget();

    }

    private async UniTask Ending()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(5), DelayType.UnscaledDeltaTime);
        SceneChanger.Instance.MainScene();

    }



}
