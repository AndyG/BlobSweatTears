using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineImpulseSource))]
public class ManfredSiphonActive : ManfredStates.ManfredState0Param
{

  [SerializeField]
  private float siphonRadius = 3f;
  [SerializeField]
  private LayerMask siphonLayerMask;
  [SerializeField]
  private float attractForce = 0.1f;
  [SerializeField]
  private float collectDistanceRadius = 0.1f;

  private AudioSource audioSource;

  private Cinemachine.CinemachineImpulseSource impulseSource;

  void Start()
  {
    this.impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
    this.audioSource = GetComponent<AudioSource>();
  }

  public override void Enter() {
    audioSource.Play();
  }

  public override void Exit() {
    audioSource.Stop();
  }

  public override void Tick()
  {
    if (!manfred.playerInput.GetIsHoldingSiphon())
    {
      TransitionToSiphonRecovery();
      return;
    }

    Collider2D[] colliders = Physics2D.OverlapCircleAll(manfred.siphonSinkTransform.position, siphonRadius, siphonLayerMask);
    for (int i = 0; i < colliders.Length; i++)
    {
      Collider2D collider = colliders[i];
      SiphonSource source = collider.GetComponentInParent<SiphonSource>();
      if (source != null)
      {
        source.OnSiphoned(manfred.siphonSinkTransform.position, attractForce);

        SiphonDroplet droplet = source as SiphonDroplet;
        if (droplet != null && ShouldCollectDroplet(droplet))
        {
          CollectDroplet(droplet);
          Debug.Log("generating impulse");
          impulseSource.GenerateImpulse();
        }
      }
    }
  }

  public override string GetAnimation()
  {
    return "VampSiphonActive";
  }

  private bool ShouldCollectDroplet(SiphonDroplet droplet)
  {
    float distance = (droplet.transform.position - manfred.siphonSinkTransform.position).magnitude;
    return distance < collectDistanceRadius;
  }

  private void CollectDroplet(SiphonDroplet droplet)
  {
    droplet.OnCollected();
    manfred.cardManager.AddPercentToCard(droplet.GetPercentContained(), droplet.givesJudgment);
  }

  private void TransitionToSiphonRecovery()
  {
    // This is a hack and should be done smarter by storing the currently attracted droplets and notifying them without finding them again.
    Collider2D[] colliders = Physics2D.OverlapCircleAll(manfred.siphonSinkTransform.position, siphonRadius * 2, siphonLayerMask);
    for (int i = 0; i < colliders.Length; i++)
    {
      Collider2D collider = colliders[i];
      SiphonSource source = collider.GetComponentInParent<SiphonSource>();
      if (source != null)
      {
        source.OnSiphonStopped();
      }
    }

    manfred.fsm.ChangeState(manfred.stateSiphonRecovery, manfred.stateSiphonRecovery);
  }
}
