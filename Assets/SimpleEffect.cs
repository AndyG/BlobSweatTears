using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEffect : MonoBehaviour
{
    public void OnEffectCompleted() {
        Destroy(this.transform.gameObject);
    }
}
