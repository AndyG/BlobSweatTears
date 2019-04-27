using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManfredSiphonStart : ManfredStates.ManfredState0Param
{

  public override void OnMessage(string message)
  {
    if (message.Equals("DoSiphonSourceCheck"))
    {
      DoSiphonSourceCheck();
    }
  }

  public override string GetAnimation()
  {
    return "VampSiphonStart";
  }

  private void DoSiphonSourceCheck()
  {
    TransitionToSiphonActive();
  }

  private void TransitionToSiphonActive()
  {
    manfred.fsm.ChangeState(manfred.stateSiphonActive, manfred.stateSiphonActive);
  }
}
