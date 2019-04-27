using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Mirror : MonoBehaviour
{

    [SerializeField]
    private MirrorType mirrorType;

    private Animator animator;

    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    public void OnHit(Directable other) {
        animator.SetBool("IsBouncing", true);
        Vector2 otherDirection = other.GetDirection();
        Vector2 newDirection;
        if (mirrorType == MirrorType.HORIZONTAL) {
            // flip the x direction
            newDirection = new Vector2(otherDirection.x * -1, otherDirection.y);
        } else {
            // flip the y direction
            newDirection = new Vector2(otherDirection.x, otherDirection.y * -1);
        }
        other.DirectToward(newDirection);
    }

    void OnBounceComplete() {
        animator.SetBool("IsBouncing", false);
    }

    private enum MirrorType {
        HORIZONTAL,
        VERTICAL
    }
}
