using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
   public StateContext stateContext;

    public IGameState runState, waitState, gameOverState;

    public ECurStare curStare = ECurStare.WAIT;

    public TextMeshProUGUI CountText;

    public int Timer = 5;

    public GoalDisolveController disolveController;

    public int EnemyMaxCont;

    [SerializeField]  private int enemyCurCont;
    public int EnemyCurCont
    { 
        get => enemyCurCont;
        set 
        {
            enemyCurCont = value;

             //enemyCurCont = value > 0 ? value : 0;
            GameManager.Instance.enemyCntText.text = $"{enemyCurCont} / {EnemyMaxCont}";
            if (enemyCurCont >= EnemyMaxCont && EnemyMaxCont != 0 && GameManager.Instance.GameLife > 0) WaitStart();
        }
    }


    private void Start()
    {
        stateContext = new StateContext(this);

        runState = gameObject.AddComponent<StateRun>();
        waitState = gameObject.AddComponent<StateWait>();
        gameOverState = gameObject.AddComponent<StateGameOver>();

        Debug.Log("Start");

        disolveController.On().Forget();

        WaitStart();
    }

    public void RunStart()
    {
        Debug.Log("Run");
        curStare = ECurStare.RUN;
        stateContext.Transition(runState);
    }

    public void WaitStart()
    {
        Debug.Log("Wait");
        curStare = ECurStare.WAIT;
        stateContext.Transition(waitState);
    }

    public void GameOver()
    {
        curStare = ECurStare.GameOver;
        stateContext.Transition(gameOverState);
    }

    private async UniTask RunUpdate()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(10), DelayType.UnscaledDeltaTime);

        while (true)
        {
            await UniTask.WaitUntil(() => curStare != ECurStare.RUN);

            if (EnemyCurCont == EnemyMaxCont)
            {
             //   WaitStart();
            }

        }
    }
}



