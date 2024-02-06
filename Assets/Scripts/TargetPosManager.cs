using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosManager : MonoBehaviour
{
    [SerializeField] private Transform node;

    private bool xDirection;
    
    
    void Start()
    {
        xDirection = node.position.x > transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculateYPosition();
        CheckXDirection();
    }

    private void CheckXDirection() {
        // if the node is in front of this x, inverse the position x
        if (node.position.x >= transform.position.x && !xDirection) {
            transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            xDirection = true;
        } else if (node.position.x < transform.position.x && xDirection) {
            transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            xDirection = false;
        }
    }

    private void CalculateYPosition() {
        // Use a downward raycast to find the ground (ground tag) and set the target position to the closest hit point (hits[hits.Length - 1])
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.parent.position, Vector3.down, 10f, LayerMask.GetMask("Ground"));
        transform.position = new Vector3(transform.position.x, hits[0].point.y, transform.position.z);
        
        // Draw the raycast for debugging purposes
        Debug.DrawRay(transform.parent.position, Vector3.down * 10f, Color.red);
    }
}