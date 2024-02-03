using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
{
    [SerializeField] private Transform leg, target;
    [SerializeField] private float distanceDelta = 1f;

    private void Awake() {
        transform.position = leg.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();   
    }
    
    private void CheckDistance() {
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance > distanceDelta) {
            transform.position = target.position;
        }
    }
}
