using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private float groundDrag;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatisGround;
    [SerializeField] private bool grounded;

    [Header("Animation")]
    Animator anim;

    [SerializeField] private Transform Player;

    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    public void Update()
    {
        //ground check
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        grounded = Physics.Raycast(raycastOrigin, Vector3.down, playerHeight * 0.5f, whatisGround);
        Debug.DrawRay(raycastOrigin, Vector3.down, Color.red);
        MyInput();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }

        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        
        anim.SetFloat("yvelocity", rb.velocity.y);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = Player.forward * verticalInput + Player.right * horizontalInput;

        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            float xzVelocityMaginitude = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);
            anim.SetFloat("xvelocity", xzVelocityMaginitude);
            //Debug.Log("Grounded");
        }
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            //Debug.LogError("Warning Not Grounded");
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); ;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        anim.SetBool("isJumping", !grounded);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
