using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneTransitioner : MonoBehaviour
{
    private int targetSceneIndex;

    private Animator animator;

    void Start() {
        this.animator = GetComponent<Animator>();
    }

    public void RestartScene() {
        TransitionToScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TransitionToScene(int targetSceneIndex) {
        this.targetSceneIndex = targetSceneIndex;
        animator.SetTrigger("FadeToOpaque");
    }

    public void OnTransitionComplete() {
        SceneManager.LoadScene(this.targetSceneIndex);
    }
}
