using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManfredWallCling : ManfredStates.ManfredState1Param<bool>
{

  private bool isWallOnLeft;

  [SerializeField]
  private Vector2 leapVelocity;

  [SerializeField]
  private Vector2 jumpVelocity;

  [SerializeField]
  private Vector2 hopVelocity;

  [SerializeField]
  private Vector2 climbVelocity;

  [SerializeField]
  private float holdAwayToHopTime;

  [SerializeField]
  private float lockMovementAfterJumpTime;

  [SerializeField]
  private GameObject wallJumpEffect;

  private float currentHoldAwayTime;

  public override void Enter(bool isWallOnLeft)
  {
    this.isWallOnLeft = isWallOnLeft;
    this.currentHoldAwayTime = 0f;
  }

  public override void Tick()
  {
    int directionRelativeToWall = GetDirectionRelativeToWall();
    if (directionRelativeToWall == 1)
    {
      currentHoldAwayTime += Time.deltaTime;
    }

    if (currentHoldAwayTime > holdAwayToHopTime)
    {
      DoHop();
      return;
    }

    if (manfred.playerInput.GetDidPressJumpBuffered())
    {
      if (directionRelativeToWall == 1)
      {
        DoLeap();
      }
      else if (directionRelativeToWall == -1)
      {
        DoWallClimb();
      }
      else
      {
        DoJump();
      }
    }
  }

  public override string GetAnimation()
  {
    return "VampWallCling";
  }

  public override bool IsFacingDefaultDirection()
  {
    return isWallOnLeft;
  }

  public override bool OverridesFacingDirection() => true;

  private void LeaveWall(Vector2 targetVelocity)
  {
    Vector2 velocity = targetVelocity;
    if (!isWallOnLeft)
    {
      velocity.x = velocity.x * -1;
    }

    manfred.velocity = velocity;
    manfred.lockAirborneMovementTime = lockMovementAfterJumpTime;
    manfred.fsm.ChangeState(manfred.stateAirborne, manfred.stateAirborne, false);
    SpawnEffect();
  }

  private void SpawnEffect() {
    GameObject effect = GameObject.Instantiate(wallJumpEffect, manfred.jumpEffectTransform.position, Quaternion.identity);
    if (!isWallOnLeft) {
      effect.transform.localScale = new Vector3(-1, 1, 1);
    }
  }

  private void DoJump()
  {
    LeaveWall(jumpVelocity);
  }

  private void DoLeap()
  {
    Debug.Log("doing leap");
    LeaveWall(leapVelocity);
  }

  private void DoWallClimb()
  {
    LeaveWall(climbVelocity);
  }

  private void DoHop()
  {
    LeaveWall(hopVelocity);
  }

  // 1 if pressing away from wall, -1 if pressing toward wall, 0 otherwise.
  private int GetDirectionRelativeToWall()
  {
    float horizInput = manfred.playerInput.GetHorizInput();
    if ((isWallOnLeft && horizInput > 0) || (!isWallOnLeft && horizInput < 0))
    {
      return 1;
    }
    else if ((isWallOnLeft && horizInput < 0) || (!isWallOnLeft && horizInput > 0))
    {
      return -1;
    }

    return 0;
  }
}
