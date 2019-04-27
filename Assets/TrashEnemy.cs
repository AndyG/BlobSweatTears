using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HitboxManager))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Cinemachine.CinemachineImpulseSource))]
public class TrashEnemy : MonoBehaviour, AnimationManager.AnimationProvider, CharacterDetector.Listener, SiphonSource, Hurtable, EnemyManager.Listener
{

  private Animator animator;
  private HitboxManager hitboxManager;
  private SpriteRenderer spriteRenderer;
  private Cinemachine.CinemachineImpulseSource impulseSource;

  [SerializeField]
  private CharacterDetector characterDetector;

  [SerializeField]
  private State state = State.MOUND;

  [Header("Combat")]
  [SerializeField]
  private float timeTilAttack = 1f;
  [SerializeField]
  private float dizzyTime = 2f;
  [SerializeField]
  private Material hitFlashMaterial;

  [Header("Siphon")]
  [SerializeField]
  private Transform siphonSourcePosition;
  [SerializeField]
  private float spawnDropletCooldown = 0.1f;
  [SerializeField]
  private GameObject dropletPrototype;
  [SerializeField]
  private int health = 60;

  [Header("AI")]
  [SerializeField]
  private bool isBoss;
  private int bossRetreatThreshold;
  private bool hasBossRetreated;
  private bool isRumbling;

  private EnemyManager enemyManager;

  private AnimationManager animationManager;

  private float timeSinceSpawnedDroplet;
  private HashSet<SiphonDroplet> droplets = new HashSet<SiphonDroplet>();
  private float timeInState = 0f;

  private Material defaultSpriteMaterial;

  private bool didLandHit = false;

  private HashSet<Listener> listeners;

  // Start is called before the first frame update
  void Start()
  {
    this.animator = GetComponent<Animator>();
    this.hitboxManager = GetComponent<HitboxManager>();
    this.spriteRenderer = GetComponent<SpriteRenderer>();
    this.impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();

    this.animationManager = new AnimationManager(animator, this);
    characterDetector.AddListener(this);

    this.defaultSpriteMaterial = spriteRenderer.material;
    this.bossRetreatThreshold = this.health / 2;

    enemyManager = (EnemyManager)FindObjectOfType(typeof(EnemyManager));
    if (!isBoss)
    {
      enemyManager.IncrementTrashEnemies();
    }
    else
    {
      enemyManager.SetListener(this);
    }
  }

  // Update is called once per frame
  void Update()
  {
    animationManager.Update();
    timeSinceSpawnedDroplet += Time.deltaTime;
    timeInState += Time.deltaTime;

    if ((state == State.ATTACK || state == State.DOUBLE_ATTACK) && !didLandHit)
    {
      List<Hurtable> hurtables = hitboxManager.GetOverlappedHurtables();
      if (hurtables.Count > 0)
      {
        HitInfo hitInfo = new HitInfo(transform.position, null);
        foreach (Hurtable hurtable in hurtables)
        {
          if (hurtable.OnHit(hitInfo).hitConnected)
          {
            didLandHit = true;
            break;
          }
        }
      }
      return;
    }

    if (state == State.IDLE && timeInState >= timeTilAttack)
    {
      if (isBoss && hasBossRetreated)
      {
        ChangeState(State.DOUBLE_ATTACK);
      }
      else
      {
        ChangeState(State.ATTACK);
      }
      return;
    }

    if (state == State.DIZZY && timeInState >= dizzyTime)
    {
      ChangeState(State.RECOVERING);
      return;
    }
  }

  public void AddListener(Listener listener) {
      if (listeners == null) {
          listeners = new HashSet<Listener>();
      }

      listeners.Add(listener);
  }

  public void RemoveListener(Listener listener) {
      listeners.Remove(listener);
  }

  public void OnSiphoned(Vector3 siphonPosition, float siphonForce)
  {
    if (state == State.STUNNED)
    {
      ChangeState(State.PAINED);
    }
    else if (state == State.PAINED)
    {
      if (timeSinceSpawnedDroplet > spawnDropletCooldown)
      {
        health -= 1;
        SiphonDroplet droplet = GameObject.Instantiate(dropletPrototype, siphonSourcePosition.position, Quaternion.identity).GetComponent<SiphonDroplet>();
        droplets.Add(droplet);
        droplet.SetInitialVelocity(Random.insideUnitCircle * 5);
        timeSinceSpawnedDroplet = 0f;

        if (hasBossRetreated)
        {
          droplet.givesJudgment = true;
        }

        if (health <= bossRetreatThreshold / 2 && isBoss && !hasBossRetreated)
        {
          ChangeState(State.RETREATING);
          NotifyDropletsStopSiphoning();
          hasBossRetreated = true;
        }
      }
    }

    if (health <= 0 && state != State.DYING)
    {
      NotifyDropletsStopSiphoning();
      ChangeState(State.DYING);
      if (!isBoss) {
          enemyManager.DecrementTrashEnemies();
      }
      if (listeners != null) {
        foreach (Listener listener in listeners) {
            listener.OnDeath();
        }
      }
    }
  }

  public void OnSiphonStopped()
  {
    if (state == State.PAINED)
    {
      ChangeToDefaultState();
    }
  }

  public void OnPlayerEntered()
  {
    // Debug.Log("state: " + state);
    if (state == State.MOUND && (!isBoss || enemyManager.IsBossUnlocked()))
    {
      ChangeState(State.EMERGING);
    }
  }

  public void OnPlayerExited()
  {
    if (state == State.IDLE)
    {
      ChangeState(State.RETREATING);
    }
  }

  public void OnFinishRetreating()
  {
    if (characterDetector.isPlayerDetected)
    {
      ChangeState(State.EMERGING);
    }
    else
    {
      ChangeState(State.MOUND);
    }
  }

  public void OnFinishEmerging()
  {
    ChangeToDefaultState();
  }

  public void OnFinishAttacking()
  {
    ChangeToDefaultState();
  }

  public void OnFinishDying()
  {
    if (!isBoss)
    {
    }
  }

  public void OnFinishRecovering()
  {
    ChangeToDefaultState();
  }

  public void CheckIfLandedHit()
  {
    if (!didLandHit)
    {
      ChangeState(State.DIZZY);
    }
  }

  public void Impulse()
  {
    if (isBoss)
    {
      impulseSource.GenerateImpulse();
    }
  }

  public HurtInfo OnHit(HitInfo hitInfo)
  {
    if (state == State.DIZZY)
    {
      if (hitInfo.isHardHit)
      {
        ChangeState(State.STUNNED);
      }
      StopAllCoroutines();
      StartCoroutine(HitFlash());
      impulseSource.GenerateImpulse();
      return new HurtInfo(true);
    }
    else
    {
      return new HurtInfo(false);
    }
  }

  public void OnDeathUsed()
  {
    if (this.state != State.DYING && (!isBoss || enemyManager.IsBossUnlocked()))
    {
      ChangeState(State.STUNNED);
    }
  }

  public string GetAnimation()
  {
    switch (state)
    {
      case State.MOUND:
        if (isBoss)
        {
          return "TrashEnemyRumbling";
        }
        else
        {
          return "TrashEnemyMound";
        }
      case State.EMERGING:
        return "TrashEnemyEmerging";
      case State.IDLE:
        return "TrashEnemyIdle";
      case State.RETREATING:
        return "TrashEnemyRetreating";
      case State.PAINED:
        return "TrashEnemyPained";
      case State.DYING:
        return "TrashEnemyDying";
      case State.ATTACK:
        return "TrashEnemyAttack";
      case State.DOUBLE_ATTACK:
        return "TrashEnemyDoubleAttack";
      case State.DIZZY:
        return "TrashEnemyDizzy";
      case State.STUNNED:
        return "TrashEnemyStunned";
      case State.RECOVERING:
        return "TrashEnemyRecovering";
    }

    return "TrashEnemyMound";
  }

  public void OnBossUnlocked()
  {
    this.isRumbling = true;
  }

  private void ChangeState(State state)
  {
    if (state != this.state)
    {
      timeInState = 0f;
      this.state = state;

      if (state == State.ATTACK || state == State.DOUBLE_ATTACK)
      {
        didLandHit = false;
      }
    }
  }

  private void ChangeToDefaultState()
  {
    if (characterDetector.isPlayerDetected)
    {
      ChangeState(State.IDLE);
    }
    else
    {
      ChangeState(State.RETREATING);
    }
  }

  private void NotifyDropletsStopSiphoning()
  {
    foreach (SiphonDroplet droplet in droplets)
    {
      droplet.DisableSiphoning();
    }
    droplets.Clear();
  }

  private IEnumerator HitFlash()
  {
    spriteRenderer.material = hitFlashMaterial;
    yield return new WaitForSeconds(0.1f);
    spriteRenderer.material = defaultSpriteMaterial;
  }

  private enum State
  {
    MOUND,
    RUMBLING,
    EMERGING,
    IDLE,
    RETREATING,
    PAINED,
    DYING,
    ATTACK,
    DOUBLE_ATTACK,
    DIZZY,
    STUNNED,
    RECOVERING
  }

  public interface Listener {
      void OnDeath();
  }
}