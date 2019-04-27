using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hurtable
{
  HurtInfo OnHit(HitInfo hitInfo);
}
