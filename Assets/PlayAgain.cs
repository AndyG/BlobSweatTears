using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAgain : MonoBehaviour
{

    [Header("Input")]
    [SerializeField]
    private PlayerInput playerInput;

    [Header("Transition")]
    [SerializeField]
    private SceneTransitioner sceneTransitioner;

    private bool isTransitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        this.isTransitioning = false;
        playerInput.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTransitioning) {
            return;
        }

        if (playerInput.GetDidPressJump()) {
            this.sceneTransitioner.TransitionToScene(0);
            this.isTransitioning = true;
        }
    }
}
