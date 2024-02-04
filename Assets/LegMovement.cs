using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
{
    [SerializeField] private Transform leg, target;
    [SerializeField] private float distanceDelta = 1f;
    [SerializeField] private float stepHeight = 1f, speed = 10f;

    private float lerp;
    private Vector3 newPos, currentPosition, oldPos;

    private void Awake()
    {
        transform.position = leg.position;
        newPos = target.position;
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
        if (distance > distanceDelta) {
            lerp = 0;
            newPos = target.position;
        }

        if (lerp<1) {
            Vector3 pos = Vector3.Lerp(oldPos, newPos, lerp);
            pos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            
            currentPosition = pos;
            lerp += Time.deltaTime * speed;
        }
        else {
            oldPos = newPos;
        }
    }
}