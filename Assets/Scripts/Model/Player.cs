using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
{
    #region Public Fields
    public string idPlayer;// este id deve ser igual ao que esta cadastrado no inputsystem. para este jogo o player1 está como _P1 e o player2 como _P2. colocar isso no campo público da unity.

    public float jumpPower = 1000f;
    public float maxJumpTime = 0.50f;
    public float raycastMaxDistance = 10f;
    public float walkSpeed = 500f;

    #endregion Public Fields

    #region Private Fields

    private const int OBSTACLE_LAYER = 9;

    private Animator anim;

    private Rigidbody2D body;

    private bool jumpStarted = false;

    private float jumpTimeElapsed = 0f;

    private float originOffset = 0.5f;

    #endregion Private Fields

    #region Public Properties

    /// <summary>
    /// Tells whether or not the player is current on top of a valid platform
    /// </summary>
    public bool ContactWithPlatformTop
    {
        get
        {
            return true;
        }
    }

    /// <summary>
    /// Whether or not the character is currently jumping
    /// </summary>
    public bool Jumping
    {
        get
        {
            if (jumpStarted && Input.GetButton("Jump"+ idPlayer) && jumpTimeElapsed < maxJumpTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool JumpReady
    {
        get
        {
            if (!Jumping && ContactWithPlatformTop && !jumpStarted)
                return true;
            else
                return false;
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Raycasts out from the player and returns the targets hit.
    /// </summary>
    /// <param name="direction"></param>
    public RaycastHit2D CheckRaycast(Vector2 direction)
    {
        float directionOriginOffset = originOffset * (direction.x > 0 ? 1 : -1);

        Vector2 startingPosition = new Vector2(transform.position.x + directionOriginOffset, transform.position.y);

        return Physics2D.Raycast(startingPosition, direction, raycastMaxDistance);
    }

    #endregion Public Methods

    #region Private Methods

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        MovementAndAnimationUpdate();
        RaycastCheckUpdate();
    }

    /// <summary>
    /// Controls player input and movement for left / right axis movement, jumping, and corresponding
    /// animations in the animator.
    /// </summary>
    private void MovementAndAnimationUpdate()
    {
        // If jump is available, is in contact with a platform, and jump button down. Start jumping
        if (JumpReady && Input.GetButtonDown("Jump"+ idPlayer))
        {
            jumpStarted = true;
        }

        float yJump = Jumping ? 1 : 0;

        // Increment timer if jumping
        if (Jumping)
        {
            jumpTimeElapsed += Time.deltaTime;
        }

        // If Z velocity hits 0, it
        Vector2 direction = new Vector2(
            Input.GetAxisRaw("Horizontal"+ idPlayer),
            yJump);

        bool isWalking = false;

        if ((direction.x > 0 || direction.y > 0) ||
            (direction.x < 0 || direction.y < 0))
        {
            isWalking = true;

            // Add in gravity

            // Stop jump if the time has elapsed past maxJumpTime

            // There is some input body.MovePosition((Vector2)gameObject.transform.position +
            // direction * speed * Time.deltaTime);
            Vector2 addForce = new Vector2(direction.x * walkSpeed * Time.deltaTime,
                                           direction.y * jumpPower * Time.deltaTime);
            body.AddForce(addForce);

            // Set the animator parameters
            anim.SetFloat("X", direction.x);
            anim.SetFloat("Y", direction.y);
        }

        anim.SetBool("isWalking", isWalking);
    }

    /// <summary>
    /// Checks for platforms collisions with 2d rigid bodies
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When player collides with an obstacle on top, it would stop and then it can jump again
        Debug.Log("Collision layer: " + collision.gameObject.layer);
        if (collision.gameObject.layer == OBSTACLE_LAYER)
        {
            // Check if collision happens on top of the game object
            jumpStarted = false;
            jumpTimeElapsed = 0;
        }
    }

    /// <summary>
    /// Checks for user input to create raycasts on update and proceeds to make those raycast checks
    /// if the button is down
    /// </summary>
    /// <returns>True if raycheck is made this frame, false otherwise</returns>
    private bool RaycastCheckUpdate()
    {
        // Raycast button pressed
        if (Input.GetButtonDown("Fire1"+idPlayer))
        {
            // Launch a raycast in the forward direction from where the player is facing.
            Vector2 direction = new Vector2(1, 0);

            // If facing left, negative direction
            if (anim.GetFloat("X") < 0)
                direction *= -1;

            // First target hit
            RaycastHit2D hit = CheckRaycast(direction);

            if (hit.collider)
            {
                Debug.Log("Hit the collidable object " + hit.collider.name);

                Debug.DrawRay(transform.position, hit.point, Color.red, 3f);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion Private Methods
}
