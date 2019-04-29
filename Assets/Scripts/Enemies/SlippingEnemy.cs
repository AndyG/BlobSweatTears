using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CharacterController2D))]
public class SlippingEnemy : MonoBehaviour, AnimationManager.AnimationProvider
{

  private AnimationManager animationManager;
  private CharacterController2D characterController;
  private Animator animator;
  private GroundBloodChecker groundBloodChecker;
  private GroundChecker groundChecker;

  [Header("Death")]
  [SerializeField]
  private Transform deathPickupSpawnPoint;
  [SerializeField]
  private GameObject deathPickupPrototype;

  [SerializeField]
  private float groundSpeed;

  [SerializeField]
  private State state;

  // Start is called before the first frame update
  void Start()
  {
    this.animator = GetComponent<Animator>();
    this.characterController = GetComponent<CharacterController2D>();
    this.animationManager = new AnimationManager(animator, this);
    this.groundBloodChecker = GetComponentInChildren<GroundBloodChecker>();
    this.groundChecker = GetComponentInChildren<GroundChecker>();
    this.state = State.PATROLLING;
    FaceMovementDirection();
  }

  // Update is called once per frame
  void Update()
  {
    if (state == State.PATROLLING)
    {
      DoPatrol();
    }

    animationManager.Update();
  }

  public void OnSlipFinished()
  {
    this.state = State.SLIPPED;
    GameObject.Instantiate(deathPickupPrototype, this.deathPickupSpawnPoint.position, Quaternion.identity);
  }

  public string GetAnimation()
  {
    if (state == State.PATROLLING)
    {
      return "walk";
    }
    else if (state == State.SLIPPING)
    {
      return "slip";
    }
    else
    {
      return "EyeballEnemy_dead";
    }
  }

  private void DoPatrol()
  {
    Vector3 velocity = new Vector3(groundSpeed, 0f, 0f) * Time.deltaTime;
    characterController.Move(new Vector3(groundSpeed, 0f, 0f));
    if (groundBloodChecker.IsGroundBloodActive().Both())
    {
      this.state = State.SLIPPING;
      return;
    }

    // check if walking into wall.
    if (groundSpeed > 0 && characterController.collisionState.right || groundSpeed < 0 && characterController.collisionState.left)
    {
      TurnAround();
      return;
    }

    // check if walked off cliff.
    GroundChecker.CollisionInfo collisionInfo = groundChecker.GetCollisionInfo();
    if (groundSpeed < 0 && !collisionInfo.left)
    {
      TurnAround();
    }
    else if (groundSpeed > 0 && !collisionInfo.right)
    {
      TurnAround();
    }
  }

  private void TurnAround()
  {
    this.groundSpeed *= -1;
    FaceMovementDirection();
  }

  private void FaceMovementDirection()
  {
    if (groundSpeed >= 0)
    {
      this.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    else
    {
      this.transform.localScale = new Vector3(-1f, 1f, 1f);
    }
  }

  enum State
  {
    PATROLLING,
    SLIPPING,
    SLIPPED
  }
}
