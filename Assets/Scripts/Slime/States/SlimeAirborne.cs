using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class SlimeAirborne : SlimeStates.SlimeState1Param<bool>
{

  [SerializeField]
  private float jumpPower;

  [SerializeField]
  private bool enableCrouch;

  [SerializeField]
  GameObject jumpEffect;

  [SerializeField]
  private float coyoteJumpPower;
  [SerializeField]
  private float coyoteTime;

  private float currentCoyoteTime;

  private float attackCooldown = 0.05f;
  private float timeInState = 0.25f;

  private bool didRunThisFrame = false;

  public override void Enter(bool allowCoyoteTime)
  {
    if (allowCoyoteTime)
    {
      currentCoyoteTime = 0f;
    }
    else
    {
      currentCoyoteTime = coyoteTime + 1;
    }
  }

  public override void Tick()
  {
    currentCoyoteTime += Time.deltaTime;

    if (currentCoyoteTime <= coyoteTime && slime.playerInput.GetDidPressJumpBuffered())
    {
      slime.velocity.y = coyoteJumpPower;
    }

    if (slime.velocity.y > slime.minJumpVelocity && slime.playerInput.GetDidReleaseJump())
    {
      slime.velocity.y = slime.minJumpVelocity;
    }

    float horizInput = slime.playerInput.GetHorizInput();
    // move
    float targetVelocityX = horizInput * slime.horizSpeed;
    slime.velocity.x = Mathf.SmoothDamp(
      slime.velocity.x,
      targetVelocityX,
      ref slime.velocityXSmoothing,
      slime.velocityXSmoothFactorAirborne);

    if (slime.velocity.x != 0f)
    {
      slime.FaceMovementDirection();
    }

    slime.velocity.y += slime.gravity * Time.deltaTime;
    slime.controller.Move(slime.velocity * Time.deltaTime);

    // check if hit ground
    CharacterController2D.CharacterCollisionState2D collisions = slime.controller.collisionState;
    if (collisions.below)
    {
      slime.velocity.y = 0f;
      slime.fsm.ChangeState(slime.stateGrounded, slime.stateGrounded);
      return;
    }
  }

  public override string GetAnimation()
  {
    if (slime.velocity.y > 0)
    {
      return "SlimeIdle";
    }
    else
    {
      return "SlimeIdle";
    }
  }
}
