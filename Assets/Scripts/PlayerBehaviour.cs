using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;
    public Transform groundOrigin;
    public float groundRadius;
    public LayerMask groundLayerMask;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        checkIfGrounded();
    }

    private void checkIfGrounded()
    {
        RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);

        isGrounded = (hit) ? true : false;
    }

    private void Move()
    {
        if (isGrounded)
        {
            float deltaTime = Time.deltaTime;

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            float jump = Input.GetAxisRaw("Jump");

            //Check for flip
            if(x != 0)
            {
                x = flipAnimation(x);
            }
            

            //Touch Input
            Vector2 worldTouch = new Vector2();
            foreach (var touch in Input.touches)
            {
                worldTouch = Camera.main.ScreenToWorldPoint(touch.position);
            }

            float horizontalMoveForce = x * horizontalForce;
            float jumpMoveForce = jump * verticalForce;

            float mass = rb.mass * rb.gravityScale;

            rb.AddForce(new Vector2(horizontalMoveForce, jumpMoveForce) * mass);
            rb.velocity *= 0.99f;
        }
    }

    private float flipAnimation(float x)
    {

        //Depending on direction scale across x-axis either 1 or -1
        x = (x > 0) ? 1 : -1;

        transform.localScale = new Vector3(x, 1.0f);
        return x;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
    }
}
