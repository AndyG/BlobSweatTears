using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlood : MonoBehaviour
{

    public bool isFinishedSpawning = false;

    public void FinishSpawning() {
        this.isFinishedSpawning = true;
    }
}
