using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManfredAttack1 : ManfredStates.ManfredState0Param
{

  [SerializeField]
  private AudioClip soundEffect;

  [SerializeField]
  private bool lockoutMashers;

  private bool allowNextAttack = false;
  private bool lockout = false;
  private bool didLandAttack = false;


  public override void Enter()
  {
    allowNextAttack = false;
    lockout = false;
    didLandAttack = false;
  }

  public override void Tick()
  {
    if (manfred.playerInput.GetDidPressAttack())
    {
      if (!allowNextAttack)
      {
        if (lockoutMashers) {
          lockout = true;
        }
      }
      else if (!lockout)
      {
        manfred.fsm.ChangeState(manfred.stateAttack2, manfred.stateAttack2);
      }
    }

    List<Hurtable> hurtables = manfred.hitboxManager.GetOverlappedHurtables();
    if (hurtables.Count > 0 && !didLandAttack) {
      HitInfo hitInfo = new HitInfo(manfred.transform.position, false);
      foreach(Hurtable hurtable in hurtables) {
        HurtInfo hurtInfo = hurtable.OnHit(hitInfo);
        didLandAttack = didLandAttack || hurtInfo.hitConnected;
        if (hurtInfo.hitConnected) {
          Debug.Log("landed attack!");
        }
      }

      if (didLandAttack) {
        TimeManagerSingleton.Instance.DoDramaticPause(0.2f);
        manfred.effectsAudioSource.PlayOneShot(soundEffect);
      }
    }
  }

  public override void OnMessage(string message)
  {
    if (message.Equals("EndAttack1"))
    {
      manfred.fsm.ChangeState(manfred.stateGrounded, manfred.stateGrounded);
      return;
    }

    if (message.Equals("StartWindow"))
    {
      allowNextAttack = true;
    }
  }

  public override string GetAnimation()
  {
    return "VampAttack1";
  }
}
