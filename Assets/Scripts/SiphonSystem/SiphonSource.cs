using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SiphonSource
{
    void OnSiphoned(Vector3 siphonPosition, float siphonForce);
    void OnSiphonStopped();
}
