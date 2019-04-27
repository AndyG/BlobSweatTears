using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager
{
  private Animator animator;
  private AnimationProvider animationProvider;

  public AnimationManager(Animator animator, AnimationProvider animationProvider)
  {
    this.animator = animator;
    this.animationProvider = animationProvider;
  }

  public void Update()
  {
    string animation = animationProvider.GetAnimation();
    if (!animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals(animation))
    {
      animator.Play(animation);
    }
  }

  public interface AnimationProvider
  {
    string GetAnimation();
  }
}
