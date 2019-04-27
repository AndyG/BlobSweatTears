using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour, TrashEnemy.Listener
{

    private Animator flameAnimator;

    [SerializeField]
    private TrashEnemy trashEnemy;

    void Start() {
        this.flameAnimator = GetComponentInChildren<Animator>();
        trashEnemy.AddListener(this);
    }

    public void Light() {
        flameAnimator.SetTrigger("Light");
    }

    public void OnDeath() {
        StartCoroutine(DelayedLight());
    }
    
    private IEnumerator DelayedLight() {
        yield return new WaitForSeconds(1.5f);
        Light();
    }
}
