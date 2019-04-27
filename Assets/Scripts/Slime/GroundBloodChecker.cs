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

    public bool IsGroundBloodActive() {
        Vector2 position = transform.position;
        Vector2 bottomLeftOrigin = new Vector2(position.x - bloodCheckRadius, position.y + skinWidth);
        Vector2 bottomRightOrigin = new Vector2(position.x + bloodCheckRadius, position.y + skinWidth);

        // cast downward and see if there is no blood beneath us.
        RaycastHit2D bloodLeft = Physics2D.Raycast(bottomLeftOrigin, Vector2.down, rayDepth, groundBloodLayerMask);
        RaycastHit2D bloodRight = Physics2D.Raycast(bottomRightOrigin, Vector2.down, rayDepth, groundBloodLayerMask);
        
        // need both sides.
        if (!bloodLeft || !bloodRight) {
            return false;
        }

        GroundBlood groundBloodLeft = bloodLeft.transform.GetComponent<GroundBlood>();
        GroundBlood groundBloodRight = bloodRight.transform.GetComponent<GroundBlood>();

        return groundBloodLeft.isFinishedSpawning && groundBloodRight.isFinishedSpawning;
    }
}
