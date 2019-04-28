using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class SlimeVictory : SlimeStates.SlimeState0Param
{

  [SerializeField]
  private float timeToSpendCelebrating = 2f;

  private float timeInState = 0f;
  private bool hasReportedVictory;

  public override void Enter() {
    this.timeInState = 0f;
    this.hasReportedVictory = false;
  }

  public override void Tick()
  {
    this.timeInState += Time.deltaTime;
    if (this.timeInState >= this.timeToSpendCelebrating && !hasReportedVictory) {
      slime.OnVictoryCompleted();
      this.hasReportedVictory = true;
    }
  }

  public override string GetAnimation() {
    return "SlimeVictory";
  }
}
