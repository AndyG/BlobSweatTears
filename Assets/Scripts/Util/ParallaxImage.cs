using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxImage : MonoBehaviour
{

    [SerializeField]
    private float parallaxCoefficient = 1f; 

    private Transform cameraTransform;
    private float lastCameraX;
    private float lastCameraY;

    void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = cameraTransform.position.x - lastCameraX;
        float deltaY = cameraTransform.position.y - lastCameraY;

        transform.position += Vector3.right * (deltaX * parallaxCoefficient);
        transform.position += Vector3.down * (deltaY * parallaxCoefficient);

        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;
    }
}
