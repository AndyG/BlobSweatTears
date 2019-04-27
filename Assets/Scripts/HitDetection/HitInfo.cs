using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Used to pass data to a hurtbox about the interaction.
 */
public class HitInfo
{
  public Vector3 position;
  public GameObject parrySpawnObjectPrototype;
  public bool isHardHit;

  public HitInfo(Vector3 position, GameObject parrySpawnObjectPrototype)
  {
    this.position = position;
    this.parrySpawnObjectPrototype = parrySpawnObjectPrototype;
    this.isHardHit = false;
  }

  public HitInfo(Vector3 position, bool isHardHit)
  {
    this.position = position;
    this.parrySpawnObjectPrototype = null;
    this.isHardHit = isHardHit;
  }
}
