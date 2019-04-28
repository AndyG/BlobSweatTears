using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

public class SlimeGrounded : SlimeStates.SlimeState1Param<bool>
{

  [SerializeField]
  private float jumpPower;

  [SerializeField]
  private bool enableCrouch;

  [SerializeField]
  GameObject jumpEffect;

  [SerializeField]
  private int shrinkRate;

  private float shrinkCooldown;

  private bool didRunThisFrame = false;

  [SerializeField]
  private LayerMask groundBloodLayerMask;

  private bool isLanding = false;
  private bool didSpawnLandingDroplets;

  [Header("Landing")]
  [SerializeField]
  private GameObject landingDropletPrototype;
  [SerializeField]
  private int numLandingDroplets;
  [SerializeField]
  private float force;
  [SerializeField]
  private Transform sourceTransform;

  [Header("Blood Running")]
  [SerializeField]
  private float horizSpeedOnBlood = 40f;
  [SerializeField]
  private Transform bloodTrailSource;
  [SerializeField]
  private GameObject bloodTrailPrototype;

  public override void Enter(bool isLanding) {
    this.isLanding = isLanding;
    this.didSpawnLandingDroplets = false;
  }

  public override void Tick()
  {
    if (isLanding && !didSpawnLandingDroplets) {
      // SpawnLandingDroplets();
    }

    CheckForGroundBlood();

    didRunThisFrame = false;

    if (slime.playerInput.GetDidPressJumpBuffered())
    {
      slime.velocity.y = jumpPower;
      slime.fsm.ChangeState(slime.stateAirborne, slime.stateAirborne, false);
      SpawnJumpEffect();
      return;
    }

    float horizInput = slime.playerInput.GetHorizInput();

    bool isOnBlood = slime.groundBloodChecker.IsGroundBloodActive();
    float speed = isOnBlood ? horizSpeedOnBlood : slime.horizSpeed;

    float targetVelocityX = horizInput * speed;
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
      if (isOnBlood) {
        SpawnTrail(horizInput > 0);
      }
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

  private void CheckForGroundBlood() {
    GroundBloodChecker groundBloodChecker = slime.groundBloodChecker;
    if (groundBloodChecker.IsOnBloodlessGround()) {
      DoShrink();
    }
  }

  private void SpawnLandingDroplets() {
    for (int i = 0; i < numLandingDroplets; i++) {
      SpawnLandingDroplet();
    }
    didSpawnLandingDroplets = true;
  }

  private void SpawnLandingDroplet() {
    GameObject landingDroplet = GameObject.Instantiate(landingDropletPrototype, sourceTransform.position, Quaternion.identity);
    Rigidbody2D dropletRb = landingDroplet.GetComponent<Rigidbody2D>();
    Vector2 impulse = Random.insideUnitCircle;
    impulse.y = 1;
    impulse *= force;
    dropletRb.AddForce(impulse);
  }

  private void SpawnTrail(bool facingRight) {
    GameObject trail = GameObject.Instantiate(bloodTrailPrototype, bloodTrailSource.position, Quaternion.identity);
    if (!facingRight) {
      Transform trailTransform = trail.GetComponent<Transform>();
      trailTransform.localScale = new Vector3(-1f, 1f, 1f) * slime.transform.localScale.y;
    }
  }

  private void DoShrink()
  {
    slime.Shrink(shrinkRate);
  }

  private void SpawnJumpEffect() {
    GameObject jumpEffect = GameObject.Instantiate(this.jumpEffect, transform.position, Quaternion.identity);
    Transform jumpEffectTransform = jumpEffect.GetComponent<Transform>();
    jumpEffectTransform.localScale = new Vector3(1f, 1f, 1f) * slime.transform.localScale.y;
  }
}
