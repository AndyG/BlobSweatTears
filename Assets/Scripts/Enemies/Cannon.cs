using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Cannon : MonoBehaviour
{

  private Spawner spawner;
  private Animator animator;

  private FireballPool fireballPool;

  [SerializeField]
  private float interval = 1;

  [SerializeField]
  GameObject spawnPosition;

  private float timeUntilShot;

  void Awake()
  {
    fireballPool = FindObjectOfType<FireballPool>();
  }

  void Start()
  {
    spawner = GetComponentInChildren<Spawner>();
    animator = GetComponent<Animator>();

    timeUntilShot = interval;
  }

  void Update()
  {
    timeUntilShot -= Time.deltaTime;
    if (timeUntilShot <= 0)
    {
      DoShoot();
      timeUntilShot = interval;
    }
  }

  private void DoShoot()
  {
    animator.SetTrigger("Shoot");
    Fireball fireball = fireballPool.Get();
    fireball.transform.position = this.spawnPosition.transform.position;
    fireball.transform.rotation = this.transform.rotation;
  }
}
