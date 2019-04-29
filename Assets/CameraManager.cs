using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera playerCamera;

    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera goalpostCamera;

    [SerializeField]
    private float timeToLookAtGoalpost;

    private bool didStartLookingAtPlayer;
    private float curTimeLookingAtGoalpost;

    // Start is called before the first frame update
    void Start()
    {
        this.curTimeLookingAtGoalpost = 0f;
        this.didStartLookingAtPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (didStartLookingAtPlayer) {
            return;
        }

        this.curTimeLookingAtGoalpost += Time.deltaTime;
        if (this.curTimeLookingAtGoalpost >= timeToLookAtGoalpost) {
            this.didStartLookingAtPlayer = true;
            playerCamera.enabled = true;
            goalpostCamera.enabled = false;
        }
    }
}
