using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale;
    public CharacterController controller;
    public GameObject dustEffect;
    public GameObject landingEffect;

    private Vector3 moveDirection;
    public float jump2;
    private bool jump3;
    public float jump3Timer;
    public Transform cam;
    private bool isWallJumping;

    //public float jumpTime;
    //private float jumpTimeCounter;
    //private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        isWallJumping = false;
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
                Instantiate(dustEffect, transform.position, transform.rotation);
                FindObjectOfType<AudioManaging>().Play("Jump1");
                moveDirection.y = jumpForce;
                jump2 = 0;
            }

            else if (Input.GetButtonDown("Jump") && jump2 <= 0.15f && jump3 == false)
            {
                //isJumping = true;
                //jumpTimeCounter = jumpTime;
                Instantiate(dustEffect, transform.position, transform.rotation);
                FindObjectOfType<AudioManaging>().Play("Jump2");
                moveDirection.y = jumpForce + 5;
                jump2 = 0;
                jump3 = true;
            }

            else if (Input.GetButtonDown("Jump") && jump2 <= 0.15f && jump3)
            {
                //isJumping = true;
                //jumpTimeCounter = jumpTime;
                Instantiate(dustEffect, transform.position, transform.rotation);
                FindObjectOfType<AudioManaging>().Play("Jump3");
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

            if (jump2 < 0.1)
            {
                Instantiate(landingEffect, transform.position, transform.rotation);
            }

            if (jump3Timer > 0.1 && jump3Timer < 0.2)
            {
                Instantiate(landingEffect, transform.position, transform.rotation);
            }
        }

        //if (controller.transform.position.y > 2f)
        //{
        //    controller.isGrounded;
        //}

        //if (controller.transform.position.y < 1f)

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

        if (isWallJumping == false)
        {
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
            controller.Move(moveDirection * Time.deltaTime);
        }
    }

    //WALL JUMP
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.isGrounded && hit.normal.y < 0.1f)
        {
            jump2 = 0;
            jump3 = false;
            jump3Timer = 0.2f;
            if (Input.GetButtonDown("Jump"))
            {
                isWallJumping = true;
                FindObjectOfType<AudioManaging>().Play("Jump2");
                moveDirection = new Vector3(hit.normal.x * 400, jumpForce, hit.normal.z * 400);
                controller.Move(moveDirection * Time.deltaTime);
                isWallJumping = false;
            }
        }
    }
}