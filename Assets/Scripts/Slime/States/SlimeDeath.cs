using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class SlimeDeath : SlimeStates.SlimeState0Param
{

  [SerializeField]
  private float timeToSpendDying = 3f;

  private float timeInState = 0f;
  private bool hasReportedDeath;

  public override void Enter()
  {
    this.timeInState = 0f;
    this.hasReportedDeath = false;
  }

  public override void Tick()
  {
    this.timeInState += Time.deltaTime;
    if (this.timeInState >= this.timeToSpendDying && !hasReportedDeath)
    {
      slime.OnDeath();
      this.hasReportedDeath = true;
    }
  }

  public override string GetAnimation()
  {
    return "SlimeDeath";
  }
}
