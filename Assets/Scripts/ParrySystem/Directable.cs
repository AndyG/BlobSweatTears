using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Directable
{
    Vector2 GetDirection();
    void DirectToward(Vector2 direction);
}
