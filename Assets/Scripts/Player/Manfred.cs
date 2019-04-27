using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HitboxManager))]
public class Manfred : MonoBehaviour, AnimationManager.AnimationProvider, Hurtable
{

  [SerializeField]
  private AudioClip deathSoundEffect;

  public Animator animator;
  public AnimationManager animationManager;
  public PlayerInput playerInput;
  public CharacterController2D controller;
  public GroundTypeDetector spikeDetector;

  [System.NonSerialized]
  public AudioSource effectsAudioSource;

  [System.NonSerialized]
  public CardManager cardManager;

  [System.NonSerialized]
  public HitboxManager hitboxManager;

  [System.NonSerialized]
  public SceneTransitioner sceneTransitioner;

  [System.NonSerialized]
  public EffectsCanvas effectsCanvas;

  public EndCanvas endCanvas;

  public float gravity;
  public Vector2 velocity = new Vector2(0f, 0f);
  public float horizSpeed = 0.5f;
  public float minJumpVelocity = 5;
  public float velocityXSmoothFactorGrounded = 0.2f;
  public float velocityXSmoothFactorAirborne = 0.4f;

  public bool isFacingRight = true;

  // smoothing function
  public float velocityXSmoothing = 0f;

  public Transform siphonSinkTransform;
  public Transform loverEffectSourceTransform;
  public Transform jumpEffectTransform;

  public FSM<Manfred, ManfredStates.IManfredState> fsm;

  [Header("States")]
  public ManfredAttack1 stateAttack1;
  public ManfredAttack2 stateAttack2;
  public ManfredAttack3 stateAttack3;
  public ManfredGrounded stateGrounded;
  public ManfredCrouch stateCrouch;
  public ManfredAirborne stateAirborne;
  public ManfredParryStance stateParryStance;
  public ManfredParryAction stateParryAction;
  public ManfredSiphonStart stateSiphonStart;
  public ManfredSiphonRecovery stateSiphonRecovery;
  public ManfredSiphonActive stateSiphonActive;
  public ManfredDead stateDead;
  public ManfredUseCard stateUseCard;
  public ManfredWallCling stateWallCling;

  // hack! get rid of this!
  public float lockAirborneMovementTime;

  void Awake()
  {
    playerInput.Awake();
  }

  void Start()
  {
    animator = GetComponent<Animator>();
    controller = GetComponent<CharacterController2D>();
    hitboxManager = GetComponent<HitboxManager>();
    effectsAudioSource = GetComponent<AudioSource>();
    sceneTransitioner = (SceneTransitioner)FindObjectOfType(typeof(SceneTransitioner));
    effectsCanvas = (EffectsCanvas)FindObjectOfType(typeof(EffectsCanvas));

    cardManager = (CardManager)FindObjectOfType(typeof(CardManager));

    animationManager = new AnimationManager(animator, this);

    fsm = new FSM<Manfred, ManfredStates.IManfredState>(this);

    fsm.ChangeState(stateAirborne, stateAirborne, false);
  }

  void Update()
  {
    lockAirborneMovementTime -= Time.deltaTime;
    playerInput.Update();
    fsm.TickCurrentState();
    animationManager.Update();

    if (!fsm.currentState.IsDead() && spikeDetector.IsTouching())
    {
      DieAndRestartScene();
    }

    transform.localScale = new Vector3(IsFacingDefaultDirection() ? 1f : -1f, 1f, 1f);
  }

  public void OnMessage(string message)
  {
    if (fsm.currentState != null)
    {
      fsm.currentState.OnMessage(message);
    }
  }

  public HurtInfo OnHit(HitInfo hitInfo)
  {
    HurtInfo hurtInfo = fsm.currentState.OnHit(hitInfo);
    if (hurtInfo.hitConnected)
    {
      DieAndRestartScene();
    }

    return hurtInfo;
  }

  public string GetAnimation()
  {
    return fsm.currentState.GetAnimation();
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

  public void EndGame()
  {
    Die();
    StartCoroutine(DelayedShowEndScreen());
  }

  private void Die()
  {
    if (!fsm.currentState.IsDead())
    {
      effectsAudioSource.PlayOneShot(deathSoundEffect);
    }
    fsm.ChangeState(stateDead, stateDead);
  }

  private void DieAndRestartScene()
  {
    Die();
    StartCoroutine(DelayedRestartScene());
  }

  private IEnumerator DelayedRestartScene()
  {
    yield return new WaitForSeconds(3f);
    sceneTransitioner.RestartScene();
  }

  private IEnumerator DelayedShowEndScreen()
  {
    yield return new WaitForSeconds(3f);
    endCanvas.FadeIn();
  }
}
