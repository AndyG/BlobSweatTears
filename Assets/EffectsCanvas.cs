using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EffectsCanvas : MonoBehaviour
{

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    public void DoWhiteFlash(float duration) {
        StopAllCoroutines();
        StartCoroutine(_DoWhiteFlash(duration));
    }

    public void PlayNoEffect() {
        StopAllCoroutines();
        animator.Play("ScreenEffectNone");
    }

    private IEnumerator _DoWhiteFlash(float duration) {
        animator.Play("WhiteFlash");
        yield return new WaitForSecondsRealtime(duration);
        PlayNoEffect();
    }
}
