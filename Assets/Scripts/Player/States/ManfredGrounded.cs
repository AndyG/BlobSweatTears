using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class ManfredGrounded : ManfredStates.ManfredState0Param
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
    didRunThisFrame = false;
    timeInState += Time.deltaTime;

    if (manfred.playerInput.GetDidPressSiphon())
    {
      manfred.velocity.x = 0;
      manfred.fsm.ChangeState(manfred.stateSiphonStart, manfred.stateSiphonStart);
    }

    if (manfred.playerInput.GetDidPressJumpBuffered())
    {
      SpawnJumpEffect();
      manfred.velocity.y = jumpPower;
      manfred.fsm.ChangeState(manfred.stateAirborne, manfred.stateAirborne, false);
      return;
    }

    if (enableCrouch && manfred.playerInput.GetHorizInput() == 0f && manfred.playerInput.GetVerticalInput() < 0)
    {
      manfred.velocity.x = 0;
      manfred.fsm.ChangeState(manfred.stateCrouch, manfred.stateCrouch);
      return;
    }

    if (timeInState >= attackCooldown && manfred.playerInput.GetDidPressAttack())
    {
      manfred.fsm.ChangeState(manfred.stateAttack1, manfred.stateAttack1);
      return;
    }

    if (manfred.playerInput.GetDidPressParry())
    {
      manfred.fsm.ChangeState(manfred.stateUseCard, manfred.stateUseCard);
      return;
    }

    float horizInput = manfred.playerInput.GetHorizInput();

    float targetVelocityX = horizInput * manfred.horizSpeed;
    manfred.velocity.x = Mathf.SmoothDamp(
      manfred.velocity.x,
      targetVelocityX,
      ref manfred.velocityXSmoothing,
      manfred.velocityXSmoothFactorGrounded);

    if (targetVelocityX != 0f)
    {
      didRunThisFrame = true;
    }

    manfred.velocity.y = manfred.gravity * Time.deltaTime;

    if (horizInput != 0f)
    {
      manfred.FaceMovementDirection();
    }
    manfred.controller.Move(manfred.velocity * Time.deltaTime);

    if (!manfred.controller.isGrounded)
    {
      manfred.fsm.ChangeState(manfred.stateAirborne, manfred.stateAirborne, true);
    }
  }

  public override string GetAnimation()
  {
    return didRunThisFrame ? "VampRun" : "VampIdle";
  }

  private void SpawnJumpEffect() {
    GameObject effect = GameObject.Instantiate(jumpEffect, manfred.jumpEffectTransform.position, Quaternion.identity);
  }
}
