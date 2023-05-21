using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class MoveControl : MonoBehaviour
{// Essentials
    [SerializeField] Transform cam;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    CharacterController controller;

    Animator anim;

    // Movement
    Vector2 movement;
    bool sprinting;

    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    float trueSpeed;

    // Jumping

    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;

    

    bool isGrounded;
    Vector3 velocity;
    void Start()
    {
        trueSpeed = walkSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(transform.position, 0.1f, 1);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            trueSpeed = sprintSpeed;
            sprinting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            trueSpeed = walkSpeed;
            sprinting = false;
        }
        anim.transform.localPosition = Vector3.zero;
        anim.transform.localEulerAngles = Vector3.zero;
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * trueSpeed * Time.deltaTime);
            anim.SetInteger("state", 1);
            
        }
        else
        {
            anim.SetInteger("state", 0);
        } 
        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2f * gravity);
        }
        if(velocity.y > 0)
        {
            anim.SetInteger("state", 2);
        }

        if (velocity.y > -20)
        {
            velocity.y += (gravity * 15) * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.E)) { anim.SetInteger("state", 3); }
        controller.Move(velocity * Time.deltaTime);
    }

}
