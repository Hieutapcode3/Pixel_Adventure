using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundEffect : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f; // Tốc độ cuộn

    private Tilemap tilemap;
    private Vector3 startPosition;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        startPosition = transform.position;
    }

    void Update()
    {
        // Di chuyển Tilemap theo hướng (1,1)
        transform.position = new Vector3(
            transform.position.x + scrollSpeed * Time.deltaTime,
            transform.position.y + scrollSpeed * Time.deltaTime,
            transform.position.z
        );

        // Nếu Tilemap di chuyển quá xa, đặt lại vị trí ban đầu
        if (Vector3.Distance(transform.position, startPosition) >= tilemap.cellBounds.size.x)
        {
            transform.position = startPosition;
        }
    }
}
