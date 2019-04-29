using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prime31;


public class Slime : MonoBehaviour, AnimationManager.AnimationProvider
{

  [System.NonSerialized]
  public Animator animator;

  [Header("GroundCheck")]
  public GroundChecker groundChecker;

  [Header("Health")]
  [SerializeField]
  private int health;

  [Header("Input")]
  [SerializeField]
  public PlayerInput playerInput;

  [System.NonSerialized]
  public CharacterController2D controller;

  [Header("Stats")]
  public float gravity;
  public float horizSpeed = 0.5f;
  public float minJumpVelocity = 5;
  public float velocityXSmoothFactorGrounded = 0.2f;
  public float velocityXSmoothFactorAirborne = 0.4f;

  [System.NonSerialized]
  public bool isFacingRight = true;

  // smoothing function
  [System.NonSerialized]
  public float velocityXSmoothing = 0f;

  public Vector2 velocity = new Vector2(0f, 0f);

  public FSM<Slime, SlimeStates.ISlimeState> fsm;

  private AnimationManager animationManager;

  [Header("States")]
  public SlimeGrounded stateGrounded;
  public SlimeAirborne stateAirborne;
  public SlimeWallCling stateWallCling;
  public SlimeVictory stateVictory;
  public SlimeDeath stateDeath;

  [Header("Blood")]
  public GroundBloodChecker groundBloodChecker;
  [SerializeField]
  private GameObject groundBloodPrototype;

  [Header("Goal")]
  [SerializeField]
  private LayerMask goalpostLayerMask;

  [Header("Collectibles")]
  [SerializeField]
  private LayerMask collectibleLayerMask;

  [Header("WallJump")]
  public float lockAirborneMovementTime;

  private BoxCollider2D goalpostChecker;

  [Header("Spikes")]
  [SerializeField]
  private SpikeChecker spikeChecker;

  private bool didWin = false;
  private bool didDie = false;

  private SceneTransitioner sceneTransitioner;

  void Awake()
  {
    playerInput.Awake();
  }

  void Start()
  {
    animator = GetComponent<Animator>();
    controller = GetComponent<CharacterController2D>();
    animationManager = new AnimationManager(animator, this);
    goalpostChecker = GameObject.FindGameObjectWithTag("GoalpostChecker").GetComponent<BoxCollider2D>();
    sceneTransitioner = GameObject.FindGameObjectWithTag("SceneTransitioner").GetComponent<SceneTransitioner>();

    fsm = new FSM<Slime, SlimeStates.ISlimeState>(this);

    fsm.ChangeState(stateGrounded, stateGrounded, false);
  }

  // Update is called once per frame
  void Update()
  {
    lockAirborneMovementTime -= Time.deltaTime;
    playerInput.Update();
    fsm.TickCurrentState();
    animationManager.Update();
    CheckForCollectibles();
    UpdateScale();
    if (!didWin && !didDie)
    {
      CheckForWin();
    }

    if (!didWin && !didDie) {
      CheckForSpikes();
    }
  }

  public bool IsFacingDefaultDirection()
  {
    if (fsm.currentState.OverridesFacingDirection())
    {
      return fsm.currentState.IsFacingDefaultDirection();
    }
    else
    {
      return isFacingRight;
    }
  }

  public void FaceMovementDirection()
  {
    isFacingRight = velocity.x >= 0f;
  }

  public string GetAnimation()
  {
    return fsm.currentState.GetAnimation();
  }

  public void Shrink()
  {
    if (didWin || didDie) {
      return;
    }

    this.health--;
    health = Mathf.Min(health, 7);
    if (health <= 0)
    {
      Die();
    }
    else
    {
      UpdateScale();
    }
  }

  public void OnVictoryCompleted()
  {
    sceneTransitioner.TransitionToScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void OnDeath()
  {
    sceneTransitioner.TransitionToScene(SceneManager.GetActiveScene().buildIndex);
  }

  public void SpawnBloodDroplet()
  {
    Vector3 position = transform.position;
    // position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), position.z);

    GameObject blood = GameObject.Instantiate(groundBloodPrototype, position, Quaternion.identity);
  }

  private void UpdateScale()
  {
    int xSign = IsFacingDefaultDirection() ? 1 : -1;

    float scale = GetScale();

    this.transform.localScale = new Vector3(
      scale * xSign,
      scale,
      1f
    );

    controller.recalculateDistanceBetweenRays();
  }

  private void CheckForCollectibles()
  {
    Collider2D collectible = Physics2D.OverlapBox(goalpostChecker.bounds.center, goalpostChecker.bounds.size / 2, 0f, collectibleLayerMask);
    if (collectible != null)
    {
      Absorb(1);
      GameObject.Destroy(collectible.transform.gameObject);
    }
  }

  private void CheckForWin()
  {
    Collider2D goalpost = Physics2D.OverlapBox(goalpostChecker.bounds.center, goalpostChecker.bounds.size / 2, 0f, goalpostLayerMask);
    if (goalpost != null)
    {
      fsm.ChangeState(stateVictory, stateVictory);
      didWin = true;
    }
  }

  private void CheckForSpikes()
  {
    if (spikeChecker.IsOnSpikes())
    {
      Die();
    }
  }

  private void Absorb(int amount)
  {
    this.health += amount;
    health = Mathf.Min(health, 7);
    UpdateScale();
  }

  private void Die()
  {
    if (!didDie)
    {
      this.didDie = true;
      fsm.ChangeState(stateDeath, stateDeath);
    }
  }

  private float GetScale()
  {
    // hack
    if (didDie)
    {
      return 1.5f;
    }

    // hack 2
    if (didWin && health <= 1)
    {
      return 1f;
    }

    if (health >= 7)
    {
      return 3.5f;
    }
    if (health == 6)
    {
      return 3f;
    }
    if (health == 5)
    {
      return 2.5f;
    }
    if (health == 4)
    {
      return 2f;
    }
    else if (health == 3)
    {
      return 1.5f;
    }
    else if (health == 2)
    {
      return 1f;
    }
    else
    {
      return 0.5f;
    }
  }
}
