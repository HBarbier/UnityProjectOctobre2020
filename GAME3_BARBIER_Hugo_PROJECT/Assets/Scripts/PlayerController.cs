using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale;
    public CharacterController controller;

    private Vector3 moveDirection;
    public float jump2;
    private bool jump3;
    public float jump3Timer;
    public Transform cam;

    //public float jumpTime;
    //private float jumpTimeCounter;
    //private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1) * moveSpeed;
        moveDirection.y = yStore;
        controller.transform.rotation = cam.rotation;

        if (Input.GetKey(KeyCode.Q))
        {
            controller.transform.rotation = cam.rotation * Quaternion.Euler(0f, 0f, 5f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            controller.transform.rotation = cam.rotation * Quaternion.Euler(0f, 0f, -5f);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            controller.transform.rotation = cam.rotation * Quaternion.Euler(5f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            controller.transform.rotation = cam.rotation * Quaternion.Euler(-5f, 0f, 0f);
        }

        //JUMP
        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            jump2 += Time.deltaTime;
            
            if (Input.GetButtonDown("Jump") && jump2 > 0.15f)
            {
                //isJumping = true;
                //jumpTimeCounter = jumpTime;
                moveDirection.y = jumpForce;
                jump2 = 0;
            }

            else if (Input.GetButtonDown("Jump") && jump2 <= 0.15f && jump3 == false)
            {
                //isJumping = true;
                //jumpTimeCounter = jumpTime;
                moveDirection.y = jumpForce + 5;
                jump2 = 0;
                jump3 = true;
            }

            else if (Input.GetButtonDown("Jump") && jump2 <= 0.15f && jump3)
            {
                //isJumping = true;
                //jumpTimeCounter = jumpTime;
                moveDirection.y = jumpForce + 11;
                jump2 = 0;
                jump3 = false;
                jump3Timer = 0.2f;
            }

            if (jump3)
            {
                jump3Timer -= Time.deltaTime;

                if (jump3Timer <= 0)
                {
                    jump3 = false;
                    jump3Timer = 0.2f;
                }
            }
        }

        //if (isJumping && Input.GetKey(KeyCode.Space))
        //{
        //    if (jumpTimeCounter > 0)
        //    {
        //        moveDirection.y = jumpForce;
        //        jumpTimeCounter -= Time.deltaTime;
        //    }

        //    else if (jumpTimeCounter < 0)
        //    {
        //        isJumping = false;
        //    }
        //}

        //if (Input.GetButtonUp("Jump"))
        //{
        //    isJumping = false;
        //}

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
        print(jump3Timer);
    }

    //WALL JUMP
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.isGrounded && hit.normal.y < 0.1f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
    }
}