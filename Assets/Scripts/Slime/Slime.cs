using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prime31;


public class Slime : MonoBehaviour, AnimationManager.AnimationProvider
{

  [System.NonSerialized]
  public Animator animator;

  [Header("Health")]
  [SerializeField]
  private int health;

  [SerializeField]
  private float deathScaleThreshold = 0.2f;

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

  void Awake()
  {
    playerInput.Awake();
  }

  void Start()
  {
    animator = GetComponent<Animator>();
    controller = GetComponent<CharacterController2D>();
    animationManager = new AnimationManager(animator, this);

    fsm = new FSM<Slime, SlimeStates.ISlimeState>(this);

    fsm.ChangeState(stateAirborne, stateAirborne, false);
  }

  // Update is called once per frame
  void Update()
  {
    playerInput.Update();
    fsm.TickCurrentState();
    UpdateScale();
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

  public void Shrink(int amount)
  {
    Debug.Log("shrink");
    SpawnBloodDroplet();
    this.health -= amount;
    UpdateScale();
  }

  private void SpawnBloodDroplet()
  {

  }

  private void UpdateScale()
  {
    int xSign = IsFacingDefaultDirection() ? 1 : -1;
    float scale = health / 100f;

    if (scale < deathScaleThreshold)
    {
      //todo -- better death
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    this.transform.localScale = new Vector3(
      scale * xSign,
      scale,
      1f
    );

    controller.primeRaycastOrigins();
  }
}
