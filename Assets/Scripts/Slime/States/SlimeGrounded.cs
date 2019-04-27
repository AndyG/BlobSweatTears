using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class SlimeGrounded : SlimeStates.SlimeState0Param
{

  [SerializeField]
  private float jumpPower;

  [SerializeField]
  private bool enableCrouch;

  [SerializeField]
  GameObject jumpEffect;

  [SerializeField]
  private float shrinkRate;

  private float shrinkCooldown;

  private bool didRunThisFrame = false;

  public override void Enter()
  {
    ResetShrinkCooldown();
  }

  public override void Tick()
  {
    didRunThisFrame = false;

    if (slime.playerInput.GetDidPressJumpBuffered())
    {
      slime.velocity.y = jumpPower;
      slime.fsm.ChangeState(slime.stateAirborne, slime.stateAirborne, false);
      return;
    }

    float horizInput = slime.playerInput.GetHorizInput();

    float targetVelocityX = horizInput * slime.horizSpeed;
    slime.velocity.x = Mathf.SmoothDamp(
      slime.velocity.x,
      targetVelocityX,
      ref slime.velocityXSmoothing,
      slime.velocityXSmoothFactorGrounded);

    if (targetVelocityX != 0f)
    {
      didRunThisFrame = true;
    }

    slime.velocity.y = slime.gravity * Time.deltaTime;

    if (horizInput != 0f)
    {
      slime.FaceMovementDirection();
    }
    slime.controller.Move(slime.velocity * Time.deltaTime);
    if (!slime.controller.isGrounded)
    {
      slime.fsm.ChangeState(slime.stateAirborne, slime.stateAirborne, true);
    }

    if (didRunThisFrame)
    {
      shrinkCooldown -= Time.deltaTime;
      if (shrinkCooldown <= 0f)
      {
        DoShrink();
      }
    }
  }

  public override string GetAnimation()
  {
    return didRunThisFrame ? "SlimeIdle" : "SlimeIdle";
  }

  private void DoShrink()
  {
    slime.Shrink(1);
    ResetShrinkCooldown();
  }

  private void ResetShrinkCooldown()
  {
    shrinkCooldown = 1 / shrinkRate;
  }
}
