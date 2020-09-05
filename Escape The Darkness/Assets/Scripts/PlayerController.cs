using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Finite State Machine
    public enum State { IDLE, WALK, RUN, JUMP, FALL, SHOT, HURT, CLIMB, CROUCH, CROUCH_SHOT }
    public State state;
    #endregion

    //[SerializeField] private FieldOfView fieldOfView;

    #region Components
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    private SpriteRenderer sr;
    private Transform tf;
    #endregion

    #region Immortal
    private bool isImmortal = false;
    private float immortalTime = 3f;
    private Color c;
    #endregion

    #region Ladder Fields
    [SerializeField] private float climbSpeed = 3f;
    //[SerializeField] private PlatformEffector2D pe = default;
    [SerializeField] private LayerMask whatIsLadderTop = default;

    private bool isOnLadder = false;
    private float naturalGravity;

    //private Ladder ladder = GetComponent<Ladder>();

    //Encapsulated Fields
    private Ladder ladder;
    //public Ladder ladder;

    private bool canClimb = false;
    private bool bottomLadder = false;
    private bool topLadder = false;
    #endregion

    #region Jump Fields   
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float airDragMultiplier = 0.95f;
    [SerializeField] private float variableJumpHeightMultiplier = 0.5f;
    [SerializeField] private int amountOfJumps = 1;
    [SerializeField] private float jumpTimerSet = 0.15f;

    private float jumpTimer;
    private int amountOfJumpsLeft;
    private bool canJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    #endregion

    #region Dash and Move Fields
    [SerializeField] private Transform groundCheck = default;
    [SerializeField] private float dashTime = default;
    [SerializeField] private float dashSpeed = default;
    [SerializeField] private float distanceBetweenImages = default;
    [SerializeField] private float dashCoolDown = default;
    [SerializeField] private float groundCheckRadius = default;
    [SerializeField] private LayerMask whatIsGround = default;
    [SerializeField] private float runSpeed = 5f;

    private float lastDash = 100f;
    private float dashTimeLeft;
    private float lastDashImageXpos;
    private bool isDashing = false;
    private bool canMove = true;
    private bool canFlip = true;
    private bool isFacingRight = true;
    private bool isGrounded;
    private float horMovementDirection;
    private float verMovementDirection;
    private int facingDirection;
    #endregion

    #region Effects Fields
    [SerializeField] private GameObject jumpDustCloud = default;
    [SerializeField] private GameObject jumpEffect = default;
    [SerializeField] private GameObject impactFX = default;
    [SerializeField] private GameObject hitParticle = default;
    [SerializeField] private Transform spawnPoint = default;
    [SerializeField] private GameObject deathEffect = default;

    [SerializeField] private ParticleSystem deathParticleSystem = default;
    #endregion

    #region Getter_Setters
    public bool CanClimb { get => canClimb; set => canClimb = value; }
    public bool BottomLadder { get => bottomLadder; set => bottomLadder = value; }
    public bool TopLadder { get => topLadder; set => topLadder = value; }
    public Ladder Ladder { get => ladder; set => ladder = value; }
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();
        naturalGravity = rb.gravityScale;
        c = sr.material.color;
        state = State.IDLE;
        amountOfJumpsLeft = amountOfJumps;

        //ladder = GetComponent<Ladder>();
    }

    private void Update()
    {
        ApplyMovement();

        CheckMovementDirection();
        //CheckJump();
        CheckIfCanJump();
        CheckInput();
        //CheckDash();
        //CheckKnockback();
        AnimationState();
        SetAnimation();
    }

    private void ApplyMovement()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(runSpeed * horMovementDirection, rb.velocity.y);
            //fieldOfView.SetAimDirection(rb.velocity);
            //fieldOfView.SetOrigin(transform.position);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && horMovementDirection < 0)
        {
            facingDirection = -1;
            if (canFlip)
            {
                isFacingRight = !isFacingRight;
                sr.flipX = true;
            }
        }
        else if (!isFacingRight && horMovementDirection > 0)
        {
            facingDirection = 1;
            if (canFlip)
            {
                isFacingRight = !isFacingRight;
                sr.flipX = false;
            }
        }
    }

    private void CheckIfCanJump()
    {
        if (coll.IsTouchingLayers(whatIsGround) && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    private void SetAnimation()
    {
        anim.SetInteger("state", (int)state);
    }

    private void CheckInput()
    {
        horMovementDirection = Input.GetAxisRaw("Horizontal");
        verMovementDirection = Input.GetAxisRaw("Vertical");

        if (CanClimb && Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            state = State.CLIMB;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(Ladder.transform.position.x, rb.position.y);//Ladder
            rb.gravityScale = 0f;
        }
        if (Input.GetButtonDown("Jump") && !isOnLadder)
        {
            state = State.FALL;

            if (coll.IsTouchingLayers(whatIsGround) || (amountOfJumpsLeft > 0))
            {
                Jump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
        if (Input.GetButton("Crouch"))
        {
            state = State.CROUCH;
        }
        //if (Input.GetButtonDown("Dash"))
        //{
        //    if (Time.time >= (lastDash + dashCoolDown))
        //    {
        //        AttemptToDash();
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    if (Time.time >= (lastDash + dashCoolDown))
        //    {
        //        AttemptToDash();
        //    }
        //}
    }

    //private void AttemptToDash()
    //{
    //    isDashing = true;
    //    dashTimeLeft = dashTime;
    //    lastDash = Time.time;
    //    PlayerAfterImagePool.Instance.GetFromPool();
    //    lastDashImageXpos = transform.position.x;
    //    SoundManager.PlaySound("Sprint");
    //}

    //private void CheckDash()
    //{
    //    if (isDashing)
    //    {
    //        if (dashTimeLeft > 0)
    //        {
    //            canMove = false;
    //            canFlip = false;
    //            rb.velocity = new Vector2(dashSpeed * facingDirection, 0);
    //            dashTimeLeft -= Time.deltaTime;
    //            if (Mathf.Abs(transform.position.x - lastDashImageXpos) > distanceBetweenImages)
    //            {
    //                PlayerAfterImagePool.Instance.GetFromPool();
    //                lastDashImageXpos = transform.position.x;
    //            }
    //        }
    //        if (dashTimeLeft <= 0)
    //        {
    //            isDashing = false;
    //            canMove = true;
    //            canFlip = true;
    //        }
    //    }
    //}

    private void AnimationState()
    {
        if (state == State.CLIMB)
        {
            Climb();
        }


        //else if (state == State.CROUCH)
        //{
        //    state == State.CROUCH;
        //}

        else if (state == State.JUMP)
        {
            if (rb.velocity.y < Mathf.Epsilon)
            {
                state = State.FALL;
            }
        }
        else if (state == State.FALL)
        {
            if (coll.IsTouchingLayers(whatIsGround))
            {
                state = State.IDLE;
                Instantiate(jumpDustCloud, groundCheck.transform.position, jumpDustCloud.transform.rotation);
                SoundManager.PlaySound("Land");
            }
            else if (coll.IsTouchingLayers(whatIsLadderTop))
            {
                state = State.IDLE;
            }
        }
        else if (state == State.HURT)
        {
            if (Mathf.Abs(rb.velocity.x) < Mathf.Epsilon)
            {
                state = State.IDLE;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon)
        {
            state = State.RUN;
        }
        else
        {
            state = State.IDLE;
        }
    }

    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            state = State.JUMP;            
            SoundManager.PlaySound("Jump");
            Instantiate(jumpEffect, groundCheck.transform.position, jumpEffect.transform.rotation);
        }
    }

    private void Bounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.JUMP;
    }

    private void Climb()
    {
        

        isOnLadder = true;
        verMovementDirection = Input.GetAxis("Vertical");
        //Climbing up
        if (verMovementDirection > 0.1f && !TopLadder)
        {
            //pe.rotationalOffset = 0f; 
            ladder.PESwitcherOff();

            rb.velocity = new Vector2(0f, verMovementDirection * climbSpeed);
            anim.speed = 1f;
        }
        //Climbing down
        else if (verMovementDirection < -0.1f && !BottomLadder)
        {
            //pe.rotationalOffset = 180f;
            ladder.PESwitcherOn();

            rb.velocity = new Vector2(0f, verMovementDirection * climbSpeed);
            anim.speed = 1f;
        }
        //Reach bottom or top of ladder
        else if (BottomLadder || TopLadder)
        {
            //pe.rotationalOffset = 0f;
            ladder.PESwitcherOff();

            state = State.IDLE;
            isOnLadder = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = naturalGravity;
            anim.speed = 1f;
        }
        //Jump off the ladder
        else if (Input.GetButtonDown("Jump"))
        {
            isOnLadder = false;
            rb.constraints = RigidbodyConstraints2D.None;
            CanClimb = false;
            anim.speed = 1f;
            Jump();
            rb.gravityScale = naturalGravity;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            return;
        }
        //Still
        else
        {
            anim.speed = 0f;
            rb.velocity = Vector2.zero;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //SoundManager.PlaySound("Squashed");
            deathParticleSystem.Play();
            //Instantiate(jumpEffect, groundCheck.transform.position, jumpEffect.transform.rotation
            //Instantiate(deathParticleSystem, GetPosition(), Quaternion.Euler(-90, 0, 0));
            //this.gameObject.SetActive(false);
            coll.enabled = false;
            sr.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY| RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        sr.enabled = true;
        coll.enabled = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    #region Animation Events
    private void FootSteps1SFX()
    {
        Debug.Log("Feet1");
        SoundManager.PlaySound("Footstep1");
    }

    private void FootSteps2SFX()
    {
        SoundManager.PlaySound("Footstep2");
    }

    private void ClimbSFX()
    {
        SoundManager.PlaySound("Climb");
    }

    private void EndHurt()
    {
        state = State.IDLE;
    }
    #endregion
}
