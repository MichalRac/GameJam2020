using System.Collections;
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

    private Rigidbody2D myRigidbody;
    private float magnitude;
    private Vector2 targetVelocity;
    private Vector2 velocityChange;
    private Vector2 velocityNorm;
    private CapsuleCollider2D myCollider;
    private float playerHalfHeight;
    private float threshold = 0.1f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CapsuleCollider2D>();
        playerHalfHeight = myCollider.size.y /2f;
    }

    void Update()
    {
        CheckGrounded();

        var dt = Time.deltaTime;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        targetVelocityX = IncrementTowards(targetVelocityX, MoveSpeed *  horizontal, Acceleration, dt);

        //targetVelocityY = IncrementTowards(targetVelocityY, MoveSpeed * vertical, Acceleration, dt);
        //if (isGrounded && vertical > 0f)
        //    targetVelocityY = JumpForce;
        //else
        //    targetVelocityY = myRigidbody.velocity.y;

        
        //velocityChange = (targetVelocity - myRigidbody.velocity);
        //velocityNorm = velocityChange;
        //velocityNorm.Normalize();
        //magnitude = Mathf.Clamp(velocityChange.magnitude, -MaxMoveSpeed, MaxMoveSpeed);
        //velocityChange = velocityNorm * magnitude;

        if (isGrounded && vertical > 0f)
            myRigidbody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

        targetVelocityY = myRigidbody.velocity.y;

        targetVelocity = targetVelocityX * Vector2.right + targetVelocityY * Vector2.up;
        //myRigidbody.AddForce(velocityChange, ForceMode2D.Force);
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
        var rayDist = 0.1f;
        isGrounded = false;
        var playerFeetPos = transform.position - new Vector3(0f, playerHalfHeight, 0f);
        var filter = new ContactFilter2D
        {
            layerMask = LayerMask.GetMask("Default"),
            useLayerMask = true
        };
        var results = new List<RaycastHit2D>();
        var hit = Physics2D.Raycast(playerFeetPos, Vector2.down, filter, results, rayDist);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].distance <= threshold)
                isGrounded = true;
        }

        //Debug.DrawLine(playerFeetPos, playerFeetPos + Vector3.down * rayDist);

    }
}
