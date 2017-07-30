using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{

    CharacterController _controller;
    CharacterController controller { get { if (!_controller) _controller = GetComponent<CharacterController>(); return _controller; } }

    void Start()
    {

    }

    public float walkSpeed = 6;
    public float runSpeed = 10;
    public float strafeSpeed = 5;
    public float gravity = 20;
    public float jumpHeight = 2;
    public bool canJump = true;
    private bool isRunning = false;
    private bool isGrounded = false;

    void Update()
    {

        bool running = Input.GetKey(KeyCode.LeftShift);

        float forwardAndBackSpeed = running ? runSpeed : walkSpeed;

        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = Vector3.ClampMagnitude(targetVelocity, 1);
        targetVelocity *= forwardAndBackSpeed;

        targetVelocity += Physics.gravity;

        controller.Move(transform.TransformDirection(targetVelocity * Time.deltaTime));
    }
}
