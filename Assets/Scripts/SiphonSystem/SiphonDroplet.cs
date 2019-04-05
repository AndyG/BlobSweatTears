﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class SiphonDroplet : MonoBehaviour, SiphonSource
{

  private Rigidbody2D rb;
  private Animator animator;
  private Collider2D collectCollider;

  private bool IsBeingCollected;

  [SerializeField]
  private int percentContained = 25;
  [SerializeField]
  private float maxVelocity = 25;
  [SerializeField]
  private float stopLerpFactor = 0.1f;

  private Vector3 velocity = Vector3.zero;

  private bool isStopping = false;

  private bool isSiphonEnabled = true;

  [SerializeField]
  private float siphonDisableTime = 0.6f;

  private float timeSinceSiphonDisabled = 0.3f;

  public void Awake()
  {
    this.rb = GetComponent<Rigidbody2D>();
    this.animator = GetComponent<Animator>();
    this.collectCollider = GetComponent<Collider2D>();
  }

  public void SetInitialVelocity(Vector3 velocity) {
    this.velocity = velocity;
  }

  void Update() {
    if (!isSiphonEnabled) {
      timeSinceSiphonDisabled += Time.deltaTime;
      if (timeSinceSiphonDisabled > siphonDisableTime) {
        isSiphonEnabled = true;
      }
    }

    if (isStopping) {
      this.velocity = Vector3.Lerp(velocity, Vector3.zero, stopLerpFactor);
      if (this.velocity.magnitude < 0.05f) {
        this.velocity = Vector3.zero;
        isStopping = false;
      } else {
        transform.Translate(velocity * Time.deltaTime);
      }
    }
  }
  
  public void OnSiphoned(Vector3 siphonPosition, float siphonForce) {
    if (isSiphonEnabled) {
      AttractToward(siphonPosition, siphonForce);
    }
  }

  public void OnSiphonStopped() {
    OnAttractionStopped();
  }

  private void AttractToward(Vector2 targetPosition, float attractForce)
  {
    isStopping = false;
    Vector3 direction = ((Vector3)targetPosition - transform.position).normalized;
    velocity = velocity + (direction * attractForce);
    velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
    transform.Translate(velocity * Time.deltaTime);
  }

  public void OnAttractionStopped() {
    isStopping = true;
  }

  public void OnCollected() {
    animator.SetBool("IsBeingCollected", true);
    collectCollider.enabled = false;
  }

  public void FinishCollect() {
    GameObject.Destroy(this.gameObject);
  }

  public int GetPercentContained() => percentContained;

  public void DisableSiphoning() {
    isSiphonEnabled = false;
    timeSinceSiphonDisabled = 0f;
    OnAttractionStopped();
  }
}
