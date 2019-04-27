using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManfredDead : ManfredStates.ManfredState0Param
{

  private HashSet<DeathBat> bats = new HashSet<DeathBat>();

  [Header("Bats")]
  [SerializeField]
  private float timeBeforeBatsDisperse;
  [SerializeField]
  private GameObject batPrototype;
  [SerializeField]
  private int batCount;
  [SerializeField]
  private float batInitialSpeed;
  [SerializeField]
  private float batTargetVariance;

  private float timeInState = 0f;

  private Cinemachine.CinemachineImpulseSource impulseSource;

  void Start()
  {
    this.impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
  }

  public override void Enter()
  {
    timeInState = 0f;
    bats.Clear();
    SpawnBats();
    impulseSource.GenerateImpulse();
  }

  public override void Tick()
  {
    timeInState += Time.deltaTime;
  }

  public override string GetAnimation()
  {
    return "ManfredInvisible";
  }

  public override HurtInfo OnHit(HitInfo hitInfo)
  {
    return new HurtInfo(false);
  }

  public override bool IsDead()
  {
    return true;
  }

  private void SpawnBats()
  {
    for (int i = 0; i < batCount; i++)
    {
      DeathBat bat = GameObject.Instantiate(batPrototype, manfred.siphonSinkTransform.position, Quaternion.identity).GetComponent<DeathBat>();
      bats.Add(bat);
      bat.SetVelocity(Random.insideUnitCircle * batInitialSpeed);

      Vector3 randomTargetOffset = Random.insideUnitCircle * batTargetVariance;
      bat.SetTargetPosition(manfred.siphonSinkTransform.position + randomTargetOffset);

      float randomFactor = Random.Range(0, 1f);
      bool isBigBat = randomFactor > 0.5f;
      if (isBigBat)
      {
        bat.transform.localScale = bat.transform.localScale * 1.2f;
      }
    }
  }
}
