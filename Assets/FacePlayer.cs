using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    [SerializeField]
    private Transform target;

    private bool isFacingLeft = true;

    // Update is called once per frame
    void Update()
    {
        if (target != null) {
            if (isFacingLeft && this.transform.position.x <= target.transform.position.x) {
                this.transform.localScale = new Vector3(-1f, 1f, 1f);
                this.isFacingLeft = false;
            } else if (!isFacingLeft && this.transform.position.x > target.transform.position.x) {
                this.transform.localScale = new Vector3(1f, 1f, 1f);
                this.isFacingLeft = true;
            }
        }
    }
}
