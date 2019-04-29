using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBloodChecker : MonoBehaviour
{

    [SerializeField]
    private float rayDepth = 0.1f;

    [SerializeField]
    private float skinWidth = 0.02f;

    [SerializeField]
    private LayerMask groundBloodLayerMask;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private float groundCheckRadius = 0.5f;
    [SerializeField]
    private float bloodCheckRadius = 0.5f;

    public bool IsOnBloodlessGround() {
        Vector2 position = transform.position;
        Vector2 bottomLeftOrigin = new Vector2(position.x - bloodCheckRadius, position.y + skinWidth);
        Vector2 bottomRightOrigin = new Vector2(position.x + bloodCheckRadius, position.y + skinWidth);

        // cast downward and see if there is no blood beneath us.
        RaycastHit2D bloodLeft = Physics2D.Raycast(bottomLeftOrigin, Vector2.down, rayDepth, groundBloodLayerMask);
        RaycastHit2D bloodRight = Physics2D.Raycast(bottomRightOrigin, Vector2.down, rayDepth, groundBloodLayerMask);
        
        if (bloodLeft || bloodRight) {
            return false;
        }

        bottomLeftOrigin.x = position.x - groundCheckRadius;
        bottomRightOrigin.x = position.x + groundCheckRadius;

        // cast downward and see if there is ground beneath us.
        RaycastHit2D groundLeft = Physics2D.Raycast(bottomLeftOrigin, Vector2.down, rayDepth, groundLayerMask);
        RaycastHit2D groundRight = Physics2D.Raycast(bottomRightOrigin, Vector2.down, rayDepth, groundLayerMask);

        return groundLeft && groundRight;
    }

    public ActiveBloodInfo IsGroundBloodActive() {
        Vector2 position = transform.position;
        Vector2 bottomLeftOrigin = new Vector2(position.x - bloodCheckRadius, position.y + skinWidth);
        Vector2 bottomRightOrigin = new Vector2(position.x + bloodCheckRadius, position.y + skinWidth);

        // cast downward and see if there is no blood beneath us.
        RaycastHit2D bloodLeft = Physics2D.Raycast(bottomLeftOrigin, Vector2.down, rayDepth, groundBloodLayerMask);
        RaycastHit2D bloodRight = Physics2D.Raycast(bottomRightOrigin, Vector2.down, rayDepth, groundBloodLayerMask);

        Debug.DrawRay(bottomLeftOrigin, Vector2.down * rayDepth, Color.blue, 1f);
        Debug.DrawRay(bottomRightOrigin, Vector2.down * rayDepth, Color.blue, 1f);

        ActiveBloodInfo info = new ActiveBloodInfo();

        if (bloodLeft) {
            GroundBlood groundBloodLeft = bloodLeft.transform.GetComponent<GroundBlood>();
            info.left = groundBloodLeft.isFinishedSpawning;
        }

        if (bloodRight) {
            GroundBlood groundBloodRight = bloodRight.transform.GetComponent<GroundBlood>();
            info.right = groundBloodRight.isFinishedSpawning;
        }

        return info;
    }

    public struct ActiveBloodInfo {
        public bool left;
        public bool right;

        public bool Either() {
            return left || right;
        }

        public bool Both() {
            return left && right;
        }
    }
}
