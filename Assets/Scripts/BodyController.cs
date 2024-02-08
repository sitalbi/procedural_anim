using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    
    private List<Transform> targetTransforms;
    private float medianYPosition;
    private Vector3 bodyPosition, originalBodyPosition;

    void Awake()
    {
        targetTransforms = new List<Transform>();
        foreach (Transform t in GetComponentsInChildren<Transform>()) {
            if(t.name.Contains("Target")) {
                targetTransforms.Add(t);
            }
        }
        originalBodyPosition = transform.position;
    }

    
    void FixedUpdate() {
        BodyPosition();
    }
    
    /**
     * Calculate the body position.
     * X position updated with a constant speed
     * Y position updated with the average y position of the targets
     */
    private void BodyPosition() {
        // Calculate the mean y positions of the targets
        medianYPosition = 0;
        foreach (Transform t in targetTransforms) {
            medianYPosition += t.position.y;
        }
        medianYPosition /= targetTransforms.Count;
        medianYPosition -= 0.5f; // Adjust the median pos with an arbitrary offset (might need to modify this)
        
        bodyPosition = new Vector3(transform.position.x + speed * Time.deltaTime, originalBodyPosition.y + medianYPosition, transform.position.z);
        transform.position = bodyPosition;
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision with " + collision.gameObject.name);
        if(collision.gameObject.tag.Equals("Obstacle")) {
            speed = -speed;
        }
    }
}
