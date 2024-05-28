using EasyTransition;
using UnityEngine;
using UnityEngine.Events;

public interface IGameState
{
    public void Handle(StateMachine machine);
}

public class StateContext
{ 
    public IGameState currentState;

    public StateMachine machine;
    public StateContext(StateMachine _machine)
    { 
        machine = _machine;
    }

   
    public void Transition(IGameState _currentState , UnityAction unityAction = null)
    {
        currentState = _currentState;
        currentState.Handle(machine);
        unityAction?.Invoke();
    }
}
