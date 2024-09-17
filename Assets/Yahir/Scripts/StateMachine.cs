using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public interface IState
{
    void OnEnter();
    void Execute();
    void OnExit();
    void CheckEnterConditions();
}
public class StateMachine<T>
{
    public T Owner;
    public IState CurrentState;

    public void Update()
    {
        CurrentState.Execute();
        CurrentState.CheckEnterConditions();
    }
}