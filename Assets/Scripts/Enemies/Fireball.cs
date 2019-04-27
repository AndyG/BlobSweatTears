using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitboxManager))]
[RequireComponent(typeof(SpriteRenderer))]
public class Fireball : MonoBehaviour
{

  [SerializeField]
  private bool debug = false;

  private HitboxManager hitboxManager;
  private SpriteRenderer spriteRenderer;

  [SerializeField]
  private float velocity;

  [SerializeField]
  private GameObject parrySpawnObjectPrototype;

  [SerializeField]
  private float timeToLive = 50f;

  private static Vector3 invertedLocalScale = new Vector3(-1f, 1f, 1f);

  private FireballPool fireballPool;

  private float timeAlive = 0f;

  void OnEnable()
  {
    this.timeAlive = 0f;
  }

  void Awake()
  {
    hitboxManager = GetComponent<HitboxManager>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    fireballPool = FindObjectOfType<FireballPool>();
  }

  void Update()
  {
    timeAlive += Time.deltaTime;
    if (timeAlive > timeToLive)
    {
      fireballPool.ReturnObject(this);
      return;
    }

    transform.Translate(-transform.right * velocity * Time.deltaTime, Space.World);

    List<Hurtable> hurtables = hitboxManager.GetOverlappedHurtables();
    if (hurtables.Count > 0)
    {
      Hurtable hurtable = hurtables[0];
      HurtInfo hurtInfo = hurtable.OnHit(new HitInfo(this.transform.position, this.parrySpawnObjectPrototype));
      if (hurtInfo.hitConnected)
      {
        fireballPool.ReturnObject(this);
      }
    }

    transform.localScale = velocity > 0 ? Vector3.one : invertedLocalScale;
  }
}
