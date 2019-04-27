using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBat : MonoBehaviour
{

    [SerializeField]
    private float speed;

    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private Material flashMaterial;

    [SerializeField]
    private float flashTimeSecs;

    private Vector3 velocity = Vector3.zero; 

    private Vector3 targetPosition;

    private SpriteRenderer renderer;
    private Material defaultSpriteMaterial;

    public void SetTargetPosition(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }

    public void SetVelocity(Vector3 velocity) {
        this.velocity = velocity;
    }

    void Start() {
        Animator animator = GetComponentInChildren<Animator>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        float randomOffset = Random.Range(0f, 1f);
        animator.SetFloat("Offset", randomOffset);
        this.defaultSpriteMaterial = renderer.material;
        StartCoroutine(Flash());
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardTarget();
    }

    private void MoveTowardTarget()
    {
        Vector3 direction = ((Vector3)targetPosition - transform.position).normalized;
        velocity = velocity + (direction * speed);
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);
        transform.Translate(velocity * Time.deltaTime);
    }

    private IEnumerator Flash() {
        renderer.material = flashMaterial;
        yield return new WaitForSeconds(flashTimeSecs);
        renderer.material = defaultSpriteMaterial;
    }
}
