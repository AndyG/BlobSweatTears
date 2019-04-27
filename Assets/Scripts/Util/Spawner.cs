using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 Can spawn an object.
 */
public interface Spawner
{
  void Spawn(Vector3 position, Quaternion rotation);
}
