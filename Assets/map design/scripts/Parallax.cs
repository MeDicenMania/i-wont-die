using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxEffect;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private float width;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(deltaMovement.x * parallaxEffect, 0f, 0f);

        lastCameraPosition = cameraTransform.position;

        float distanceFromCamera = cameraTransform.position.x - transform.position.x;

        if (Mathf.Abs(distanceFromCamera) >= width)
        {
            var movement = distanceFromCamera > 0 ? width * 2f : -width * 2f;
            transform.position = new Vector3(transform.position.x + movement, transform.position.y, 0f);
        }
    }

}
