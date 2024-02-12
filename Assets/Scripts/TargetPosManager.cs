using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosManager : MonoBehaviour
{
    [SerializeField] private Transform node, body;

    private bool xDirection;
    private float nodeX, targetX;
    
    
    void Awake()
    {
        nodeX = Vector3.Dot(node.position, body.right);
        targetX = Vector3.Dot(transform.position, body.right);
        xDirection = nodeX > targetX;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckXDirection();
        CalculateYPosition();
    }

    private void CheckXDirection() {
        // Calculate the position of the node and target relative to the body's right axis
        nodeX = Vector3.Dot(node.position, body.right);
        targetX = Vector3.Dot(transform.position, body.right);

        // Compare the positions on the body's right axis
        if (nodeX > targetX && !xDirection) {
            // Mirror the local position along the body's right axis
            transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            xDirection = true;
        } else if (nodeX < targetX && xDirection) {
            // Mirror the local position along the body's right axis
            transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            xDirection = false;
        }
    }


    private void CalculateYPosition() {
        // Use a downward raycast to find the ground (ground tag) and set the target position to the closest hit point (hits[hits.Length - 1])
        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        Physics.Raycast(rayOrigin, Vector3.down, out hit, 10f, LayerMask.GetMask("Ground"));
        transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        
        // Draw the raycast for debugging purposes
        Debug.DrawRay(rayOrigin, Vector3.down * 10f, Color.red);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}