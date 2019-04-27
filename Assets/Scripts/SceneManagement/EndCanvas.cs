using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EndCanvas : MonoBehaviour
{
  private bool isShowing = false;

  [SerializeField]
  private PlayerInput playerInput;

  [SerializeField]
  private SceneTransitioner sceneTransitioner;

  private Animator animator;

  void Start()
  {
    this.animator = GetComponent<Animator>();
  }

  void Update()
  {
    if (isShowing && playerInput.GetDidPressJump())
    {
      sceneTransitioner.RestartScene();
    }
  }

  public void FadeIn()
  {
    animator.SetTrigger("FadeIn");
  }

  public void OnFadeInFinished()
  {
    isShowing = true;
  }
}
