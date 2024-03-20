using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SceneChanger : Singleton<SceneChanger>
{

    public TransitionSettings[] transition;
    public float startDelay;
    public override void Awake()
    {
        base.Awake();
    }


    public void LoadScene(string SceneName , UnityAction action = null)
    {
        TransitionManager.Instance().Transition(SceneName, transition[Random.Range(0, transition.Length)], startDelay);
        action?.Invoke();
    }

    public void LoadScene(int SceneNumber , UnityAction action = null)
    {
        TransitionManager.Instance().Transition(SceneNumber, transition[Random.Range(0, transition.Length)], startDelay);
        action?.Invoke();
    }


    

}
