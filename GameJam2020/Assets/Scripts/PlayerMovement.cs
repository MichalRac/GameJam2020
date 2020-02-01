﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public  float MoveSpeed = 10f;
    public float MaxMoveSpeed = 5.0f;
    public float Acceleration = 10f;
    public float JumpForce = 10f;
    public float targetVelocityX;
    public float targetVelocityY;
    public float horizontal;
    public float vertical;
    public bool isGrounded;
    //private float maxTargetVelocityY = 5.5f;

    private Rigidbody2D myRigidbody;
    private float magnitude;
    private Vector2 targetVelocity;
    private Vector2 velocityChange;
    private Vector2 velocityNorm;
    private CapsuleCollider2D myCollider;
    private float playerHalfHeight;
    private float playerHalfWidth;
    private float threshold = 0.1f;
    private float jumpTimeThreshold = 0.3f; //how often can you jump?
    private float currentJumpTime;
    private bool doubleJumped;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CapsuleCollider2D>();
        playerHalfHeight = myCollider.size.y /2f;
        playerHalfWidth = myCollider.size.x / 2f;
    }

    void Update()
    {
        CheckGrounded();

        var dt = Time.deltaTime;
        var horizontal = Input.GetAxis("Horizontal");
        //var vertical = Input.GetAxis("Vertical");
        var jump = Input.GetButtonDown("Jump");

        targetVelocityX = IncrementTowards(targetVelocityX, MoveSpeed *  horizontal, Acceleration, dt);
        currentJumpTime += dt;
        if (isGrounded && jump || !isGrounded && jump && !doubleJumped)
        {
            //if (currentJumpTime > jumpTimeThreshold)
            {
                currentJumpTime = 0f;
                myRigidbody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

                if(!isGrounded)
                    doubleJumped = true;
            }
        }

        targetVelocityY = myRigidbody.velocity.y;
        targetVelocity = targetVelocityX * Vector2.right + targetVelocityY * Vector2.up;
        myRigidbody.velocity = targetVelocity;

    }


    private float IncrementTowards(float currSpeed, float targetSpeed, float acc, float deltaTime)
    {
        if (currSpeed == targetSpeed)
            return currSpeed;
        else
        {
            float dir = Mathf.Sign(targetSpeed - currSpeed);
            currSpeed += acc * deltaTime * dir;
            return (dir == Mathf.Sign(targetSpeed - currSpeed)) ? currSpeed : targetSpeed;
        }
    }

    

    private void CheckGrounded()
    {
        var rayDist = 0.05f;
        isGrounded = false;
        var playerFeetPos = transform.position - new Vector3(0f, playerHalfHeight, 0f);
        var playerFeetPosLeftSide = playerFeetPos - new Vector3(-playerHalfWidth, 0f, 0f);
        var playerFeetPosRightSide = playerFeetPos - new Vector3(playerHalfWidth, 0f, 0f);

        var filter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("Default"),
            useLayerMask = true
        };
        var results = new List<RaycastHit2D>();
        Physics2D.Raycast(playerFeetPos, Vector2.down, filter, results, rayDist);
        CheckResults(results);
        Physics2D.Raycast(playerFeetPosLeftSide, Vector2.down, filter, results, rayDist);
        CheckResults(results);
        Physics2D.Raycast(playerFeetPosRightSide, Vector2.down, filter, results, rayDist);
        CheckResults(results);
        Debug.DrawLine(playerFeetPos, playerFeetPos + Vector3.down * rayDist);
        Debug.DrawLine(playerFeetPosLeftSide, playerFeetPosLeftSide + Vector3.down * rayDist);
        Debug.DrawLine(playerFeetPosRightSide, playerFeetPosRightSide + Vector3.down * rayDist);
    }

    private void CheckResults(List<RaycastHit2D> results)
    {
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].distance <= threshold)
            {
                isGrounded = true;
                doubleJumped = false;
                break;
            }
        }
    }
}
