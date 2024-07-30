using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 viewDir = player.position - transform.position;
        viewDir.y = 0;
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(inputDir != Vector3.zero )
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
