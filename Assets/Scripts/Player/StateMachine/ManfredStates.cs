﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManfredStates
{

  public interface IManfredState : FSMState<Manfred>
  {
    void OnMessage(string message);
    HurtInfo OnHit(HitInfo hitInfo);
    bool OverridesFacingDirection();
    bool IsFacingDefaultDirection();
    string GetAnimation();
  }

  public abstract class ManfredState0Param : ScriptableObject, IManfredState, Enterable0Param
  {
    protected Manfred manfred;

    public void BindContext(Manfred manfred)
    {
      this.manfred = manfred;
    }

    // FSM methods
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void OnMessage(string message) { }

    // ManfredState methods
    public virtual HurtInfo OnHit(HitInfo hitInfo) => OnHitDefaultImpl(hitInfo);
    public virtual bool OverridesFacingDirection() => OverridesFacingDirectionDefaultImpl();
    public virtual bool IsFacingDefaultDirection() => IsFacingDefaultDirectionDefaultImpl();
    public virtual string GetAnimation() => GetAnimationDefaultImpl();
  }

  public abstract class ManfredState1Param<T0> : ScriptableObject, IManfredState, Enterable1Param<T0>
  {
    protected Manfred manfred;

    public void BindContext(Manfred manfred)
    {
      this.manfred = manfred;
    }

    // FSM methods
    public virtual void Enter(T0 param0) { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void OnMessage(string message) { }

    // ManfredState methods
    public virtual HurtInfo OnHit(HitInfo hitInfo) => OnHitDefaultImpl(hitInfo);
    public virtual bool OverridesFacingDirection() => OverridesFacingDirectionDefaultImpl();
    public virtual bool IsFacingDefaultDirection() => IsFacingDefaultDirectionDefaultImpl();
    public virtual string GetAnimation() => GetAnimationDefaultImpl();
  }

  public abstract class ManfredState2Params<T0, T1> : ScriptableObject, IManfredState, Enterable2Param<T0, T1>
  {
    protected Manfred manfred;

    public void BindContext(Manfred manfred)
    {
      this.manfred = manfred;
    }

    // FSM methods
    public virtual void Enter(T0 param0, T1 param1) { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void OnMessage(string message) { }

    // ManfredState methods
    public virtual HurtInfo OnHit(HitInfo hitInfo) => OnHitDefaultImpl(hitInfo);
    public virtual bool OverridesFacingDirection() => OverridesFacingDirectionDefaultImpl();
    public virtual bool IsFacingDefaultDirection() => IsFacingDefaultDirectionDefaultImpl();
    public virtual string GetAnimation() => GetAnimationDefaultImpl();
  }

  public abstract class ManfredState3Params<T0, T1, T2> : ScriptableObject, IManfredState, Enterable3Param<T0, T1, T2>
  {
    protected Manfred manfred;

    public void BindContext(Manfred manfred)
    {
      this.manfred = manfred;
    }

    // FSM methods
    public virtual void Enter(T0 param0, T1 param1, T2 param2) { }
    public virtual void Update() { }
    public virtual void Exit() { }
    public virtual void OnMessage(string message) { }

    // ManfredState methods
    public virtual HurtInfo OnHit(HitInfo hitInfo) => OnHitDefaultImpl(hitInfo);
    public virtual bool OverridesFacingDirection() => OverridesFacingDirectionDefaultImpl();
    public virtual bool IsFacingDefaultDirection() => IsFacingDefaultDirectionDefaultImpl();
    public virtual string GetAnimation() => GetAnimationDefaultImpl();
  }

  private static HurtInfo OnHitDefaultImpl(HitInfo hitInfo)
  {
    TimeManagerSingleton.Instance.DoDramaticPause(0.2f);
    return new HurtInfo(true);
  }

  private static bool OverridesFacingDirectionDefaultImpl() => false;
  private static bool IsFacingDefaultDirectionDefaultImpl() => true;
  private static string GetAnimationDefaultImpl() => "ManfredIdle";
}
