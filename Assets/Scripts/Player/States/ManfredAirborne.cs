using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class ManfredAirborne : ManfredStates.ManfredState1Param<bool>
{

  [SerializeField]
  private GameObject landEffect;

  [SerializeField]
  private float coyoteJumpPower;
  [SerializeField]
  private float coyoteTime;

  private float currentCoyoteTime;

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

    if (currentCoyoteTime <= coyoteTime && manfred.playerInput.GetDidPressJumpBuffered())
    {
      manfred.velocity.y = coyoteJumpPower;
    }

    if (manfred.velocity.y > manfred.minJumpVelocity && manfred.playerInput.GetDidReleaseJump())
    {
      manfred.velocity.y = manfred.minJumpVelocity;
    }

    float horizInput = manfred.playerInput.GetHorizInput();
    if (manfred.lockAirborneMovementTime <= 0f)
    {
      // move
      float targetVelocityX = horizInput * manfred.horizSpeed;
      manfred.velocity.x = Mathf.SmoothDamp(
        manfred.velocity.x,
        targetVelocityX,
        ref manfred.velocityXSmoothing,
        manfred.velocityXSmoothFactorAirborne);

    }

    if (manfred.velocity.x != 0f)
    {
      manfred.FaceMovementDirection();
    }

    manfred.velocity.y += manfred.gravity * Time.deltaTime;
    manfred.controller.Move(manfred.velocity * Time.deltaTime);

    if (manfred.playerInput.GetDidPressParry())
    {
      manfred.fsm.ChangeState(manfred.stateUseCard, manfred.stateUseCard);
      return;
    }

    if (manfred.playerInput.GetDidPressSiphon())
    {
      manfred.fsm.ChangeState(manfred.stateSiphonStart, manfred.stateSiphonStart);
      return;
    }

    // check if hit ground
    CharacterController2D.CharacterCollisionState2D collisions = manfred.controller.collisionState;
    if (collisions.below)
    {
      manfred.velocity.y = 0f;
      SpawnLandEffect();
      manfred.fsm.ChangeState(manfred.stateGrounded, manfred.stateGrounded);
      return;
    }

    // check if hit ground
    if (collisions.left && horizInput < 0 || collisions.right && horizInput > 0)
    {
      manfred.velocity.x = 0f;
      manfred.velocity.y = 0f;
      bool isWallOnLeft = collisions.left;
      manfred.fsm.ChangeState(manfred.stateWallCling, manfred.stateWallCling, isWallOnLeft);
      return;
    }
  }

  private void SpawnLandEffect()
  {
    GameObject effect = GameObject.Instantiate(landEffect, manfred.jumpEffectTransform.position, Quaternion.identity);
  }

  public override string GetAnimation()
  {
    if (manfred.velocity.y > 0)
    {
      return "VampAirborneUp";
    }
    else
    {
      return "VampAirborneDown";
    }
  }
}
