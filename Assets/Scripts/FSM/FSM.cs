using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<C, S> where S : FSMState<C>
{

  private C context;

  public FSM(C context)
  {
    this.context = context;
  }

  //Current state being handled in this FSM
  public S currentState { private set; get; }

  bool ChangeStateBase(S _newState)
  {
    // Debug.Log("change state base: " + _newState);
    //Exit the current state
    if (currentState != null)
    {
      currentState.Exit();
    }

    //Change to the new state
    currentState = _newState;

    if (_newState != null)
    {
      _newState.BindContext(context);
      return true;
    }
    return false;
  }

  public void ChangeState(S _newState, Enterable0Param enterable)
  {
    if (ChangeStateBase(_newState)) enterable.Enter();
  }

  public void ChangeState<T0>(S _newState, Enterable1Param<T0> enterable, T0 p0)
  {
    if (ChangeStateBase(_newState)) enterable.Enter(p0);
  }

  public void ChangeState<T0, T1>(S _newState, Enterable2Param<T0, T1> enterable, T0 p0, T1 p1)
  {
    if (ChangeStateBase(_newState)) enterable.Enter(p0, p1);
  }

  public void ChangeState<T0, T1, T2>(S _newState, Enterable3Param<T0, T1, T2> enterable, T0 p0, T1 p1, T2 p2)
  {
    if (ChangeStateBase(_newState)) enterable.Enter(p0, p1, p2);
  }

  public void TickCurrentState()
  {
    if (currentState != null)
    {
      currentState.Tick();
    }
  }
}
