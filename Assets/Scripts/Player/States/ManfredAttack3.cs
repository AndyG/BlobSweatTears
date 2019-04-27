using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManfredAttack3 : ManfredStates.ManfredState0Param
{

  [SerializeField]
  private AudioClip soundEffect;

  private bool didLandAttack = false;

  public override void Enter() {
    didLandAttack = false;
  }

  public override void Tick() {
    List<Hurtable> hurtables = manfred.hitboxManager.GetOverlappedHurtables();
    if (hurtables.Count > 0 && !didLandAttack) {
      HitInfo hitInfo = new HitInfo(manfred.transform.position, true);
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
    if (message.Equals("EndAttack3"))
    {
      manfred.fsm.ChangeState(manfred.stateGrounded, manfred.stateGrounded);
    }
  }

  public override string GetAnimation()
  {
    return "VampAttack3";
  }
}
