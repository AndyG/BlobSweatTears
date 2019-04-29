using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

[RequireComponent(typeof(AudioSource))]
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
  private bool didShrink = false;

  private AudioSource audioSource;

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
  private Transform bloodTrailSource;
  [SerializeField]
  private GameObject bloodTrailPrototype;

  [Header("Movement Sound Effects")]
  [SerializeField]
  private AudioSource movementSoundsAudioSource;
  [SerializeField]
  private AudioClip solidGroundMovementAudioClip;
  [SerializeField]
  private AudioClip wetGroundMovementAudioClip;
  [SerializeField]
  private float solidMovementSoundEffectCooldown;
  [SerializeField]
  private float wetMovementSoundEffectCooldown;

  [Header("Other Sound Effects")]
  [SerializeField]
  private AudioClip landAudioClip;

  private float curMovementSoundEffectCooldown;
  private float bloodSpeedMultiplier = 2f;

  void Start() {
    this.audioSource = GetComponent<AudioSource>();
  }

  public override void Enter(bool isLanding)
  {
    this.isLanding = isLanding;
    this.didSpawnLandingDroplets = false;
    this.didShrink = false;
    this.curMovementSoundEffectCooldown = 0f;
  }

  public override void Tick()
  {
    GroundBloodChecker.ActiveBloodInfo activeBloodInfo = slime.groundBloodChecker.IsGroundBloodActive();
    GroundChecker.CollisionInfo solidGroundCollisionInfo = slime.groundChecker.GetCollisionInfo();
    bool isOnSolidGround = solidGroundCollisionInfo.left || solidGroundCollisionInfo.right;

    if (isLanding && !didShrink) {
      if (isOnSolidGround && !activeBloodInfo.Either()) {
        slime.Shrink();
        this.didShrink = true;
      }
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

    if (horizInput != 0f) {
      float speed = activeBloodInfo.Both() ? slime.horizSpeed * bloodSpeedMultiplier : slime.horizSpeed;

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
    } else {
      slime.velocity.x = 0f;
    }

    slime.velocity.y = slime.gravity * Time.deltaTime;

    if (horizInput != 0f)
    {
      slime.FaceMovementDirection();
      AudioClip movementSoundEffect = null;
      if (activeBloodInfo.Both())
      {
        SpawnTrail(horizInput > 0);
        if (curMovementSoundEffectCooldown >= wetMovementSoundEffectCooldown) {
          movementSoundEffect = wetGroundMovementAudioClip;
        }
      } else if (isOnSolidGround) {
        if (curMovementSoundEffectCooldown >= solidMovementSoundEffectCooldown) {
          movementSoundEffect = solidGroundMovementAudioClip;
        }
      }

      if (movementSoundEffect != null) {
        movementSoundsAudioSource.PlayOneShot(movementSoundEffect);
        curMovementSoundEffectCooldown = 0f;
      }
    }

    slime.controller.Move(slime.velocity * Time.deltaTime);
    if (!slime.controller.isGrounded)
    {
      slime.fsm.ChangeState(slime.stateAirborne, slime.stateAirborne, true);
    }

    if (isLanding) {
      audioSource.PlayOneShot(landAudioClip);
    }

    isLanding = false;
    curMovementSoundEffectCooldown += Time.deltaTime;
  }

  public override string GetAnimation()
  {
    return didRunThisFrame ? "SlimeIdle" : "SlimeIdle";
  }

  private void CheckForGroundBlood()
  {
    GroundBloodChecker groundBloodChecker = slime.groundBloodChecker;
    if (groundBloodChecker.IsOnBloodlessGround())
    {
      DoShrink();
    }
  }

  private void SpawnLandingDroplets()
  {
    for (int i = 0; i < numLandingDroplets; i++)
    {
      SpawnLandingDroplet();
    }
    didSpawnLandingDroplets = true;
  }

  private void SpawnLandingDroplet()
  {
    GameObject landingDroplet = GameObject.Instantiate(landingDropletPrototype, sourceTransform.position, Quaternion.identity);
    Rigidbody2D dropletRb = landingDroplet.GetComponent<Rigidbody2D>();
    Vector2 impulse = Random.insideUnitCircle;
    impulse.y = 1;
    impulse *= force;
    dropletRb.AddForce(impulse);
  }

  private void SpawnTrail(bool facingRight)
  {
    GameObject trail = GameObject.Instantiate(bloodTrailPrototype, bloodTrailSource.position, Quaternion.identity);
    if (!facingRight)
    {
      Transform trailTransform = trail.GetComponent<Transform>();
      trailTransform.localScale = new Vector3(-1f, 1f, 1f);
    }
  }

  private void DoShrink()
  {
    slime.SpawnBloodDroplet();
  }

  private void SpawnJumpEffect()
  {
    GameObject jumpEffect = GameObject.Instantiate(this.jumpEffect, transform.position, Quaternion.identity);
    Transform jumpEffectTransform = jumpEffect.GetComponent<Transform>();
    jumpEffectTransform.localScale = new Vector3(1f, 1f, 1f) * slime.transform.localScale.y;
  }
}
