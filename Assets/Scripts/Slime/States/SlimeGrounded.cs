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

  private float attackCooldown = 0.05f;
  private float timeInState = 0.25f;

  private bool didRunThisFrame = false;

  public override void Enter()
  {
    timeInState = 0f;
  }

  public override void Tick()
  {
    Debug.Log("tick");
    didRunThisFrame = false;
    timeInState += Time.deltaTime;

    if (slime.playerInput.GetDidPressJumpBuffered())
    {
      slime.velocity.y = jumpPower;
      slime.fsm.ChangeState(slime.stateAirborne, slime.stateAirborne, false);
      return;
    }

    float horizInput = slime.playerInput.GetHorizInput();

    float targetVelocityX = horizInput * slime.horizSpeed;
    Debug.Log("target velocity x: " + targetVelocityX);
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
  }

  public override string GetAnimation()
  {
    return didRunThisFrame ? "SlimeIdle" : "SlimeIdle";
  }
}
