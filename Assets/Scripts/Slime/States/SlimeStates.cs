using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStates
{

  public interface ISlimeState : FSMState<Slime>
  {
    void OnMessage(string message);
    bool IsDead();
    HurtInfo OnHit(HitInfo hitInfo);
    bool OverridesFacingDirection();
    bool IsFacingDefaultDirection();
    string GetAnimation();
  }

  public abstract class SlimeState0Param : MonoBehaviour, ISlimeState, Enterable0Param
  {
    protected Slime slime;

    public void BindContext(Slime slime)
    {
      this.slime = slime;
    }

    // FSM methods
    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void Exit() { }
    public virtual void OnMessage(string message) { }

    // SlimeState methods
    public virtual bool IsDead() => IsDeadDefaultImpl();
    public virtual HurtInfo OnHit(HitInfo hitInfo) => OnHitDefaultImpl(hitInfo);
    public virtual bool OverridesFacingDirection() => OverridesFacingDirectionDefaultImpl();
    public virtual bool IsFacingDefaultDirection() => IsFacingDefaultDirectionDefaultImpl();
    public virtual string GetAnimation() => GetAnimationDefaultImpl();
  }

  public abstract class SlimeState1Param<T0> : MonoBehaviour, ISlimeState, Enterable1Param<T0>
  {
    protected Slime slime;

    public void BindContext(Slime slime)
    {
      this.slime = slime;
    }

    // FSM methods
    public virtual void Enter(T0 param0) { }
    public virtual void Tick() { }
    public virtual void Exit() { }
    public virtual void OnMessage(string message) { }

    // SlimeState methods
    public virtual bool IsDead() => IsDeadDefaultImpl();
    public virtual HurtInfo OnHit(HitInfo hitInfo) => OnHitDefaultImpl(hitInfo);
    public virtual bool OverridesFacingDirection() => OverridesFacingDirectionDefaultImpl();
    public virtual bool IsFacingDefaultDirection() => IsFacingDefaultDirectionDefaultImpl();
    public virtual string GetAnimation() => GetAnimationDefaultImpl();
  }

  public abstract class SlimeState2Params<T0, T1> : MonoBehaviour, ISlimeState, Enterable2Param<T0, T1>
  {
    protected Slime slime;

    public void BindContext(Slime slime)
    {
      this.slime = slime;
    }

    // FSM methods
    public virtual void Enter(T0 param0, T1 param1) { }
    public virtual void Tick() { }
    public virtual void Exit() { }
    public virtual void OnMessage(string message) { }

    // SlimeState methods
    public virtual bool IsDead() => IsDeadDefaultImpl();
    public virtual HurtInfo OnHit(HitInfo hitInfo) => OnHitDefaultImpl(hitInfo);
    public virtual bool OverridesFacingDirection() => OverridesFacingDirectionDefaultImpl();
    public virtual bool IsFacingDefaultDirection() => IsFacingDefaultDirectionDefaultImpl();
    public virtual string GetAnimation() => GetAnimationDefaultImpl();
  }

  public abstract class SlimeState3Params<T0, T1, T2> : ScriptableObject, ISlimeState, Enterable3Param<T0, T1, T2>
  {
    protected Slime slime;

    public void BindContext(Slime slime)
    {
      this.slime = slime;
    }

    // FSM methods
    public virtual void Enter(T0 param0, T1 param1, T2 param2) { }
    public virtual void Tick() { }
    public virtual void Exit() { }
    public virtual void OnMessage(string message) { }

    // SlimeState methods
    public virtual bool IsDead() => IsDeadDefaultImpl();
    public virtual HurtInfo OnHit(HitInfo hitInfo) => OnHitDefaultImpl(hitInfo);
    public virtual bool OverridesFacingDirection() => OverridesFacingDirectionDefaultImpl();
    public virtual bool IsFacingDefaultDirection() => IsFacingDefaultDirectionDefaultImpl();
    public virtual string GetAnimation() => GetAnimationDefaultImpl();
  }

  private static HurtInfo OnHitDefaultImpl(HitInfo hitInfo)
  {
    return new HurtInfo(true);
  }

  private static bool IsDeadDefaultImpl() => false;
  private static bool OverridesFacingDirectionDefaultImpl() => false;
  private static bool IsFacingDefaultDirectionDefaultImpl() => true;
  private static string GetAnimationDefaultImpl() => "SlimeIdle";
}
