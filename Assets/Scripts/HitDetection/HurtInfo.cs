using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Used to pass data back to a hitbox about the interaction.
 */
public class HurtInfo
{
  public bool hitConnected;

  public HurtInfo(bool hitConnected)
  {
    this.hitConnected = hitConnected;
  }
}
