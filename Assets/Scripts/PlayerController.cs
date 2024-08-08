using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;
    private bool facingRight = true;

    [SerializeField] Transform spawnPoint;

    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioClip jumpSound;

    private bool dead = false;

    // movement variables
    [SerializeField] Rigidbody2D rbody; 
    private float horizInput;
    private float moveSpeed = 200.0f;  // 4.5 * 100 since we're using Rigidbody physics to move

    // jump variables
    private float jumpHeight = 2.0f;
    private float jumpTime = 0.75f;
    private float initialJumpVelocity;

    private int jumpMax = 2;
    private int jumpsAvailable = 0;

    // dash variables
    private float dashSpeed = 10f;
    //private float dashDuration = 0.5f;
    private float dashCooldown = 1.0f;
    private bool canDash = true;
    private bool isDashing = false;

    private bool isGrounded = false;
    [SerializeField] private Transform groundCheckPoint; // point at base of player
    [SerializeField] private LayerMask groundLayerMask; // ground layer
    private float groundCheckRadius = 0.3f;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.SPIKE_HIT, OnSpikeHit);
        Messenger.AddListener(GameEvent.FELL_OFF, OnFellOff);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.SPIKE_HIT, OnSpikeHit);
        Messenger.RemoveListener(GameEvent.FELL_OFF, OnFellOff);
    }

    // Start is called before the first frame update
    void Start()
    {
        // given a desired jumpHeight and jumpTime, calculate gravity (same formulas as 3D)
        float timeToApex = jumpTime / 2.0f;
        float gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        rbody.gravityScale = gravity / Physics2D.gravity.y;

        // calculate jump velocity (upward motion)
        initialJumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);      
    }

    // Update is called once per frame
    void Update()
    {
        // read (and store) horizontal input
        horizInput = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayerMask) && rbody.velocity.y < 0.01;
        
        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
            jumpsAvailable = jumpMax;
        }

        if (Input.GetButtonDown("Jump") && jumpsAvailable > 0)
        {
            anim.SetTrigger("jump");
            audioSrc.PlayOneShot(jumpSound);
            Jump();
            jumpsAvailable--;
        }

        bool isRunning = horizInput > 0.01 || horizInput < -0.01;

        if (isRunning)
        {
            anim.SetBool("isRunning", true);
        } else
        {
            anim.SetBool("isRunning", false);
        }

        if ((!facingRight && horizInput > 0.01) || (facingRight && horizInput < -0.01))
        {
            Flip();
        }

        if (Input.GetButtonDown("Dash") && canDash == true)
        {
            anim.SetTrigger("dash");
            Dash();
            Debug.Log("Dash Button Down");
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
            // We're moving via Rigidbody physics (setting velocity directly) so we need to use FixedUpdate
            float xVel = horizInput * moveSpeed * Time.deltaTime;   // determine x velocity
            rbody.velocity = new Vector2(xVel, rbody.velocity.y);   // set new x velocity, maintain existing y velocity
        
    }

    void Jump()
    {
        rbody.velocity = new Vector2(rbody.velocity.x, initialJumpVelocity);
    }

    void Dash()
    {
        Debug.Log("Dash() called");

        // Determine dash direction based on current facing direction
        float dashDirection = facingRight ? 1.0f : -1.0f;

        // Calculate dash velocity
        Vector2 dashVelocity = new Vector2(dashDirection * dashSpeed, rbody.velocity.y);

        // Apply dash velocity
        rbody.velocity = dashVelocity;

        // Set dashing flag
        isDashing = true;

        // Start dash cooldown coroutine
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        Debug.Log("DashCooldown() called");
        // Disable dashing for dashCooldown duration
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        isDashing = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
    }

    void Flip()
    {
        facingRight = !facingRight;
        // rotate 180 deg on Y axis (yaw)
        transform.Rotate(Vector3.up * 180);
    }

    private void OnSpikeHit()
    {
        if (dead == false)
        {
            dead = true;
            Die();
        }
    }

    private void OnFellOff()
    {
        if (dead == false)
        {
            dead = true;
            Die();
            Messenger.Broadcast("TAKE_LIFE");
        }
        
    }

    private void Die()
    {
        anim.SetTrigger("dead");
        this.enabled = false;
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        this.transform.position = spawnPoint.position;
        this.enabled = true;
        anim.ResetTrigger("dead");
        anim.Play("Idle");
        dead = false;
        
        


    }
}
