using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundChecker : MonoBehaviour
{

  private BoxCollider2D _collider;

  [SerializeField]
  private float skinWidth = 0.02f;

  [SerializeField]
  private LayerMask groundLayerMask;

  private float rayDepth;

  void Start()
  {
    this._collider = GetComponent<BoxCollider2D>();
    this.rayDepth = skinWidth * 2;
  }

  public CollisionInfo GetCollisionInfo()
  {
    Vector2 bottomLeftOrigin = new Vector2(_collider.bounds.min.x, _collider.bounds.min.y + skinWidth);
    Vector2 bottomRightOrigin = new Vector2(_collider.bounds.max.x, _collider.bounds.min.y + skinWidth);

    // cast downward and see if there is no blood beneath us.
    RaycastHit2D groundLeft = Physics2D.Raycast(bottomLeftOrigin, Vector2.down, rayDepth, groundLayerMask);
    RaycastHit2D groundRight = Physics2D.Raycast(bottomRightOrigin, Vector2.down, rayDepth, groundLayerMask);

    CollisionInfo info = new CollisionInfo();
    info.left = groundLeft;
    info.right = groundRight;

    return info;
  }

  public struct CollisionInfo
  {
    public bool left;
    public bool right;
  }
}
