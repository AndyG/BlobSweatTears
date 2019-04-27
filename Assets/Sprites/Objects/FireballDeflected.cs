using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class FireballDeflected : MonoBehaviour, Directable
{

  [SerializeField]
  private float speed;

  private Vector2 direction;

  [SerializeField]
  private float interactionCooldown = 0.1f;

  private float currentInteractionCooldown;
  private Rigidbody2D rb2d;

  void Start() {
    if (direction == null) {
      DirectToward(transform.right);
    }

    currentInteractionCooldown = interactionCooldown;
    rb2d = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    currentInteractionCooldown += Time.deltaTime;
  }

  void FixedUpdate() {
      Vector3 targetPosition = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, transform.position.z);
      Vector3 smoothedNewPosition = Vector3.MoveTowards(transform.position, targetPosition, Time.fixedDeltaTime * speed);
      rb2d.MovePosition(smoothedNewPosition);
  }

  public void DirectToward(Vector2 direction) {
    this.direction = direction.normalized;
  }

  public Vector2 GetDirection() {
    return direction;
  }

  void OnCollisionEnter2D() {
    if (currentInteractionCooldown < interactionCooldown) {
      return;
    }
    Poof();
    currentInteractionCooldown = 0f;
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (currentInteractionCooldown < interactionCooldown) {
      return;
    }
    bool handleHit = DidHitProjectileDestroyer(other) || DidHitTarget(other) || DidHitMirror(other);
    if (handleHit) {
      currentInteractionCooldown = 0f;
    }
  }

  private bool DidHitProjectileDestroyer(Collider2D other) {
    ProjectileDestroyer destroyer = other.GetComponent<ProjectileDestroyer>();
    if (destroyer != null) {
      Poof();
      return true;
    } 

    return false;
  }

  private bool DidHitTarget(Collider2D other) {
    Target target = other.transform.gameObject.GetComponent<Target>();
    if (target != null) {
      target.Explode();
      Poof();
      return true;
    }
    return false;
  }

  private void Poof() {
    Destroy(this.transform.gameObject);
  }

  private bool DidHitMirror(Collider2D other) {
    Mirror mirror = other.transform.gameObject.GetComponent<Mirror>();
    if (mirror != null) {
      mirror.OnHit(this);
      return true;
    }
    return false;
  }
}
