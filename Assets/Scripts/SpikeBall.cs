using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    [Range(0f, 360f)]
    [SerializeField] private float angleRange = 0f; 
    [Range(-180f, 180f)]
    [SerializeField] private float startAngle = 0f; 
    [SerializeField] private float radius = 2f; 
    [SerializeField] private float speed = 2f; 

    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, startAngle);
    }

    void Update()
    {
        if (angleRange == 360f)
        {
            transform.Rotate(Vector3.forward * 200f * Time.deltaTime);
        }
        else
        {
            float angle = startAngle + Mathf.Sin(Time.time * speed) * angleRange / 2f;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Vector3 startDirection = Quaternion.Euler(0, 0, startAngle - 135f) * Vector3.right * radius;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(position, position + startDirection);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(position, position + startDirection);
        Vector3 endDirection = Quaternion.Euler(0, 0, startAngle + angleRange - 135f) * Vector3.right * radius;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, position + endDirection);
        Gizmos.color = Color.yellow;
        int segments = 20;
        float angleStep = angleRange / segments;
        for (int i = 0; i < segments; i++)
        {
            float angleA = startAngle + i * angleStep - 135f;
            float angleB = startAngle + (i + 1) * angleStep - 135f;
            Vector3 pointA = Quaternion.Euler(0, 0, angleA) * Vector3.right * radius;
            Vector3 pointB = Quaternion.Euler(0, 0, angleB) * Vector3.right * radius;
            Gizmos.DrawLine(position + pointA, position + pointB);
        }
    }
}
