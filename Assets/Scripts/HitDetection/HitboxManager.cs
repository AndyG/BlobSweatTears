using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
  [SerializeField]
  private LayerMask layerMask;

  [SerializeField]
  private List<RectHitbox> hitboxes;

  public List<Hurtable> GetOverlappedHurtables()
  {
    List<Hurtable> overlappedHurtables = new List<Hurtable>();
    foreach (RectHitbox hitbox in hitboxes)
    {
      if (hitbox != null && hitbox.IsHitboxActive()) {
        Collider2D[] colliders = hitbox.GetColliders(layerMask);
        foreach (Collider2D collider in colliders)
        {
          Hurtable hurtable = collider.GetComponentInParent<Hurtable>();
          if (hurtable != null)
          {
            overlappedHurtables.Add(hurtable);
          }
        }
      }
    }

    return overlappedHurtables;
  }
}
