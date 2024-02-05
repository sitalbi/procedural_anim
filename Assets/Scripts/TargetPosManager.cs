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
}