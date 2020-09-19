using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningTrap : MonoBehaviour
{
    public float rotationSpeed;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        this.transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
