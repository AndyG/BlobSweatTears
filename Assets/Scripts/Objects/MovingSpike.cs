using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineImpulseSource))]
public class MovingSpike : MonoBehaviour
{

  [Header("Points")]
  [SerializeField]
  private Transform point1;
  [SerializeField]
  private Transform point2;

  [Header("Movement")]
  [SerializeField]
  private bool isMoving;
  [SerializeField]
  private float speed;
  [SerializeField]
  private float threshold = 0.01f;
  [SerializeField]
  private float pauseTime = 1f;

  [Header("Effects")]
  [SerializeField]
  private float shakeDistanceThreshold = 20f;

  private Transform playerTransform;

  private Cinemachine.CinemachineImpulseSource impulseSource;

  private Transform nextTransform;

  private float curPauseTime = 0f;

  void Start()
  {
    this.impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
    this.nextTransform = point2;

    this.playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
  }

  // Update is called once per frame
  void Update()
  {
    if (!isMoving) {
      return;
    }

    if (curPauseTime < pauseTime)
    {
      curPauseTime += Time.deltaTime;
      return;
    }
    Vector3 positionDelta = nextTransform.position - this.transform.position;
    float distanceToNextTransform = positionDelta.magnitude;
    float naturalDistance = speed * Time.deltaTime;
    float resolvedDistance = Mathf.Min(naturalDistance, distanceToNextTransform);
    Vector3 movement = positionDelta.normalized * resolvedDistance;
    transform.Translate(movement, Space.World);

    Vector3 newPositionDelta = nextTransform.position - this.transform.position;
    if (newPositionDelta.magnitude < threshold)
    {
      nextTransform = ReferenceEquals(nextTransform, point1) ? point2 : point1;
      curPauseTime = 0f;
      if ((playerTransform.position - this.transform.position).magnitude < shakeDistanceThreshold)
      {
        DoShake();
      }
    }
  }

  private void DoShake()
  {
    impulseSource.GenerateImpulse();
  }
}
