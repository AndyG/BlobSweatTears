using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GroundTypeDetector : MonoBehaviour
{

  [SerializeField]
  private LayerMask layerMask;

  private BoxCollider2D boxCollider;

  void Start()
  {
    this.boxCollider = GetComponent<BoxCollider2D>();
  }

  public bool IsTouching()
  {
    return boxCollider.IsTouchingLayers(layerMask);
  }
}
