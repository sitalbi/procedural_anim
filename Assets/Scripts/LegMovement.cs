using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distanceDelta = 1f;
    [SerializeField] private float stepHeight = 1f, speed = 5f;
    [SerializeField] private LegMovement oppositeLeg;

    [NonSerialized] public bool isMoving;
    private float lerp;
    private Vector3 newPos, currentPosition, oldPos;

    private void Awake()
    {
        newPos = target.position;
        transform.position = newPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = currentPosition;
        CheckDistance();
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(newPos, target.position);
        if (distance > distanceDelta && !oppositeLeg.isMoving) {
            lerp = 0;
            newPos = target.position;
            
        }

        if (lerp<1) {
            isMoving = true;
            Vector3 pos = Vector3.Lerp(oldPos, newPos, lerp);
            pos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            
            currentPosition = pos;
            lerp += Time.deltaTime * speed;
        }
        else {
            oldPos = newPos;
            isMoving = false;
        }
    }
}