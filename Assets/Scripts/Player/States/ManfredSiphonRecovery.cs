using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManfredSiphonRecovery : ManfredStates.ManfredState0Param
{

  public override void OnMessage(string message)
  {
    if (message.Equals("Complete"))
    {
      manfred.fsm.ChangeState(manfred.stateGrounded, manfred.stateGrounded);
    }
  }

  public override string GetAnimation()
  {
    return "VampSiphonRecovery";
  }
}
