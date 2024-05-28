using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDisolveController : MonoBehaviour
{
    [SerializeField] private Material GoalMat;

    private float timer = 0;

    private void Awake()
    {
        GoalMat.SetFloat("_SplitValue", 0);
    }

    public async UniTask On()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(4));

        timer = 0;
        while (timer < 1f)
        {
            await UniTask.Yield();
            GoalMat.SetFloat("_SplitValue", timer);
            timer += Time.deltaTime;
        }
    }


    public async UniTask Off()
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(2));

        timer = 1f;
        while (timer > 0)
        {
            await UniTask.Yield();
            GoalMat.SetFloat("_SplitValue", timer);
            timer -= Time.deltaTime;
        }
    }


    public void SpriteSet()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = GameManager.Instance.scriptable.GameOverGoalTarget;
    }


}
