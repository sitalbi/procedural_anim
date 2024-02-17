using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{

    [SerializeField] private float offset = 0.5f;
    [SerializeField] private float speed = 4f;
    
    private List<Transform> targetTransforms;
    private float medianYPosition, rotationAngle = 0f;
    private Vector3 bodyPosition, originalBodyPosition;

    void Awake()
    {
        targetTransforms = new List<Transform>();
        foreach (Transform t in GetComponentsInChildren<Transform>()) {
            if(t.CompareTag("Target")) {
                targetTransforms.Add(t);
                Debug.Log(t.name);
            }
        }
        originalBodyPosition = transform.position;
    }

    
    void FixedUpdate() {
        BodyPosition();
        BodyRotation();
    }
    
    /**
     * Calculate the body position.
     * X,Z position updated with a constant speed
     * Y position updated with the average y position of the targets
     */
    private void BodyPosition() {
        // Calculate the mean y positions of the targets
        medianYPosition = 0;
        foreach (Transform t in targetTransforms) {
            medianYPosition += t.position.y;
        }
        medianYPosition /= targetTransforms.Count;
        medianYPosition -= offset; // Adjust the median pos with an arbitrary offset (might need to modify this)

        // Calculate the movement direction based on transform.right
        Vector3 movementDirection = transform.right * speed * Time.deltaTime;
    
        // Update the body position
        bodyPosition = transform.position + movementDirection;
        bodyPosition.y = originalBodyPosition.y + medianYPosition; // Keep the y-position same as the original
        transform.position = bodyPosition;
    }


    private void BodyRotation() {
        // Calculate the difference between the highest and lowest y position of the targets 
        // and the sign of the difference (positive or negative) whether the max is on the front or back of the body
        Vector3 maxY = Vector3.negativeInfinity;
        Vector3 minY = Vector3.positiveInfinity;
        foreach (Transform t in targetTransforms) {
            if(t.position.y > maxY.y) maxY = t.position;
            if(t.position.y < minY.y) minY = t.position;
        }
        float yDiff = maxY.y - minY.y;
        float sign = maxY.x - minY.x > 0 ? 1 : -1;
        
        // Rotate the body based on the difference
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, yDiff * sign * 20);
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag.Equals("Obstacle")) {
            speed = -speed;
        }
    }

    private void OnDrawGizmos() {
        // Draw a raycast in front of the body
        Debug.DrawRay(transform.position, transform.right * 5f, Color.red);
    }
}
