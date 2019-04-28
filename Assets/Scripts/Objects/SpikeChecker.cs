using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SpikeChecker : MonoBehaviour
{

  private BoxCollider2D _collider;

  [SerializeField]
  private LayerMask spikeLayerMask;

  void Start()
  {
    this._collider = GetComponent<BoxCollider2D>();
  }

  public bool IsOnSpikes()
  {
    return Physics2D.OverlapBox(_collider.bounds.center, _collider.bounds.size / 2, 0f, spikeLayerMask) != null;
  }
}
