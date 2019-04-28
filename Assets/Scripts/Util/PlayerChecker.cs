using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerChecker : MonoBehaviour
{

    [SerializeField]
    private LayerMask playerLayerMask;

    private BoxCollider2D _collider;

    void Start() {
        this._collider = GetComponent<BoxCollider2D>();
    }

    public bool IsPlayerPresent() {
        Vector2 position = transform.position;
        Collider2D playerCollider = Physics2D.OverlapBox(_collider.bounds.center, _collider.bounds.size / 2, 0f, playerLayerMask);

        return playerCollider != null;
    }
}
