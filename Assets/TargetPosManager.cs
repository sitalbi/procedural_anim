using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPosManager : MonoBehaviour
{
    [SerializeField] private Transform node;

    private bool xDirection, zDirection;
    
    
    void Start()
    {
        xDirection = node.position.x > transform.position.x;
        zDirection = node.position.z > transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckXDirection();
        //CheckZDirection();
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
    
    private void CheckZDirection() {
        // if the node is in front of this z, inverse the position z
        if (node.position.z >= transform.position.z && !zDirection) {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -transform.localPosition.z);
            zDirection = true;
        } else if (node.position.z < transform.position.z && zDirection) {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -transform.localPosition.z);
            zDirection = false;
        }
    }
}
