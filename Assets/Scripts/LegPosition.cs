using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegPosition : MonoBehaviour
{
    [SerializeField] private Transform _legPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _legPosition.position;
    }
}
