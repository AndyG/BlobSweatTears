using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class Target : MonoBehaviour
{

    [SerializeField]
    private float oscillateYRange = 2f;

    [SerializeField]
    private float oscillateSpeed = 1f;

    private float oscillateOffset;
    private float origPositionY;

    private bool isDestroyed = false;

    private Animator animator;

    void Start() {
        this.origPositionY = transform.position.y;
        this.oscillateOffset = Random.Range(0f, 2 * Mathf.PI);
        this.animator = GetComponent<Animator>();
    }

    void Update()
    {
        float positionDelta = Mathf.Sin((Time.time + oscillateOffset) * oscillateSpeed) * oscillateYRange;
        transform.position = new Vector3(transform.position.x, origPositionY + positionDelta, transform.position.z);
    }

    public void Explode() {
        if (!isDestroyed) {
            isDestroyed = true;
            animator.SetBool("IsExploding", true);
        }
    }

    void OnExplosionComplete() {
        Destroy(this.transform.gameObject);
    }
}
