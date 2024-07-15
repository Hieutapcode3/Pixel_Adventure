using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 300f; 
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
