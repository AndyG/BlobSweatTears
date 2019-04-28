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
  GameObject landEffect;

  [SerializeField]
  private float coyoteJumpPower;
  [SerializeField]
  private float coyoteTime;

  private float currentCoyoteTime;

  private float attackCooldown = 0.05f;
  private float timeInState = 0.25f;

  private bool didRunThisFrame = false;

  // Add a little slop value to help with checks to see if we're boosted.
  private float boostedVelocitySlop = 0.05f;

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

    bool isBoosted = Mathf.Abs(slime.velocity.x) > (slime.horizSpeed + boostedVelocitySlop);

    if (isBoosted && horizInput != 0f && (Mathf.Sign(horizInput) == Mathf.Sign(slime.velocity.x))) {// && Mathf.Abs(targetVelocityX) < Mathf.Abs(slime.velocity.x)) {
      // don't need to slow him down!
    } else {
      slime.velocity.x = Mathf.SmoothDamp(
        slime.velocity.x,
        targetVelocityX,
        ref slime.velocityXSmoothing,
        slime.velocityXSmoothFactorAirborne);
    }

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
      slime.fsm.ChangeState(slime.stateGrounded, slime.stateGrounded, true);
      SpawnLandEffect();
      return;
    }
  }

  public override string GetAnimation()
  {
    if (slime.velocity.y > 0)
    {
      return "SlimeJump";
    }
    else
    {
      return "SlimeFall";
    }
  }

  private void SpawnLandEffect() {
    GameObject landEffect = GameObject.Instantiate(this.landEffect, transform.position, Quaternion.identity);
    Transform landEffectTransform = landEffect.GetComponent<Transform>();
    landEffectTransform.localScale = new Vector3(1f, 1f, 1f) * slime.transform.localScale.y;
  }
}
