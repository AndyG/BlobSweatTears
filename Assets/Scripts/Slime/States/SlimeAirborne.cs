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

  [Header("AirPlatform")]
  [SerializeField]
  private GameObject airPlatformPrototype;

  [Header("WallCling")]
  [SerializeField]
  private BoxCollider2D wallClingChecker;
  [SerializeField]
  private LayerMask wallLayerMask;
  [SerializeField]
  private float wallCheckDistance;

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

    if (slime.lockAirborneMovementTime <= 0f)
    {
      if (isBoosted && horizInput != 0f && (Mathf.Sign(horizInput) == Mathf.Sign(slime.velocity.x)))
      {
        // don't need to slow him down!
      }
      else
      {
        slime.velocity.x = Mathf.SmoothDamp(
          slime.velocity.x,
          targetVelocityX,
          ref slime.velocityXSmoothing,
          slime.velocityXSmoothFactorAirborne);
      }
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

    if (slime.playerInput.GetDidPressAttack())
    {
      SpawnAirPlatform();
    }

    if (IsCollidingWithWall(horizInput))
    {
      slime.velocity.x = 0f;
      slime.velocity.y = 0f;
      bool isWallOnLeft = horizInput < 0;
      slime.fsm.ChangeState(slime.stateWallCling, slime.stateWallCling, isWallOnLeft);
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

  private void SpawnLandEffect()
  {
    GameObject landEffect = GameObject.Instantiate(this.landEffect, transform.position, Quaternion.identity);
    Transform landEffectTransform = landEffect.GetComponent<Transform>();
    landEffectTransform.localScale = new Vector3(1f, 1f, 1f) * slime.transform.localScale.y;
  }

  private void SpawnAirPlatform()
  {
    GameObject.Instantiate(airPlatformPrototype, transform.position, Quaternion.identity);
    slime.Shrink();
  }

  private bool IsCollidingWithWall(float horizInput)
  {
    if (horizInput == 0)
    {
      return false;
    }

    float top = wallClingChecker.bounds.max.y;
    float bottom = wallClingChecker.bounds.min.y;
    float side = (horizInput > 0) ? wallClingChecker.bounds.max.x : wallClingChecker.bounds.min.x;

    Vector2 topOrigin = new Vector2(side, top);
    Vector2 rayDirection = (horizInput > 0) ? Vector2.right : Vector2.left;

    RaycastHit2D topHit = Physics2D.Raycast(topOrigin, rayDirection, wallCheckDistance, wallLayerMask);
    Debug.DrawRay(topOrigin, rayDirection * wallCheckDistance, Color.blue);
    if (!topHit)
    {
      // Debug.Log("top missed");
      return false;
    }

    Vector2 bottomOrigin = new Vector2(side, bottom);
    RaycastHit2D bottomHit = Physics2D.Raycast(bottomOrigin, rayDirection, wallCheckDistance, wallLayerMask);
    Debug.DrawRay(bottomOrigin, rayDirection * wallCheckDistance, Color.blue);
    return bottomHit;
  }
}
