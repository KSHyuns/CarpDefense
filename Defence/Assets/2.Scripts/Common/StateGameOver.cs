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

        //���� ����

        GameManager.Instance.FailGoal = true;
        
        foreach (var enemy in GameManager.Instance.enemyUnitList)
        {
            enemy.isStopped(true);
            (enemy as EnemyUnit).isFinal = true;
        }
        // ���â �г� ����
        GameDataBase.Instance.saveData.gold += GameManager.Instance.DoughCnt;

        //SendMessage �� ���̱⶧���� ���� ���� �ȵ� 
        GameManager.Instance.goalTarget.SendMessageUpwards("SpriteSet", SendMessageOptions.RequireReceiver);
        
        machine.disolveController.Off().Forget();
        
        //���̵�
        Ending().Forget();

    }

    private async UniTask Ending()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(5), DelayType.UnscaledDeltaTime);
        SceneChanger.Instance.MainScene();

    }



}
