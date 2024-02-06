using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    private List<Transform> targetTransforms;
    private float medianYPosition;
    private Vector3 bodyPosition;
    
    void Awake()
    {
        targetTransforms = new List<Transform>();
        foreach (Transform t in GetComponentsInChildren<Transform>()) {
            if(t.name.Contains("Target")) {
                targetTransforms.Add(t);
            }
        }
    }

    
    void FixedUpdate() {
        transform.position = MeanYPosition();
    }
    
    private Vector3 MeanYPosition() {
        // Get the mean Y position of the targets
        float sum = 0;
        foreach (Transform t in targetTransforms) {
            sum += t.position.y;
        }
        medianYPosition = sum / targetTransforms.Count;
        return new Vector3(transform.position.x, transform.position.y + medianYPosition, transform.position.z);
    }


}
