using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    private float moveDirection;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float jumpForce = 200f;
    [SerializeField] private Transform leftFoot, rightFoot, rightHand, leftHand;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float raycastDistance = 0.25f;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private ParticleSystem jumpParticleSystem;

    [SerializeField] private ParticleSystem doubleJumpParticleSystem;

    bool canMove = true;

    private bool isOnWallBool;
    private AudioSource audioSource;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;

    private bool canDoubleJump = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        jump.action.started += Jump;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.action.ReadValue<float>();

        isOnWall();

        anim.SetFloat("MoveSpeed",Mathf.Abs (rgbd.linearVelocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.linearVelocity.y);
        anim.SetBool("IsGrounded", CheckIsGrounded());
       
       if (moveDirection < 0f)
       {
            FlipSprite(true);
       }

       if (moveDirection > 0f)
       {
            FlipSprite(false);
       }

        if (isOnWallBool && !CheckIsGrounded() && moveDirection != 0)
        {

            float newSpeed = Mathf.MoveTowards(rgbd.linearVelocity.y, 0, 40 * Time.fixedDeltaTime);
            rgbd.linearVelocity = new Vector2(rgbd.linearVelocity.x, newSpeed);
            
           // rgbd.mass = 0.7f;
            rgbd.gravityScale = 1f;
        }
        else {
            //rgbd.mass = 1f;
            rgbd.gravityScale = 2;
                };

        }

    private void FixedUpdate()
    {
        if(!canMove)
        {
            return;
        }
        //rgbd.linearVelocity = new Vector2(moveDirection * moveSpeed * Time.deltaTime, rgbd.linearVelocity.y);

        //Targetspeed blir 0 om man inte håller på någon knapp, och 1* maxSpeed om man gör det
        float targetSpeed = moveDirection * maxSpeed;

        float newSpeed = Mathf.MoveTowards(rgbd.linearVelocity.x,targetSpeed, acceleration * Time.fixedDeltaTime);

        rgbd.linearVelocity = new Vector2(newSpeed,rgbd.linearVelocity.y);
    }

    private void OnDisable()
    {
        jump.action.started -= Jump;
    }

    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (CheckIsGrounded() == true)
        {

            rgbd.linearVelocity = new Vector2(rgbd.linearVelocityX, 0);

            rgbd.AddForce(new Vector2(0, jumpForce));
            jumpParticleSystem.Play();
            int randomJumpSound = Random.Range(0, jumpSounds.Length);
            audioSource.PlayOneShot(jumpSounds[randomJumpSound]);
        } else if (isOnWallBool== true && moveDirection!= 0) {

            rgbd.AddForce(new Vector2(jumpForce, jumpForce*0.75f));
            jumpParticleSystem.Play();
            int randomJumpSound = Random.Range(0, jumpSounds.Length);
            audioSource.PlayOneShot(jumpSounds[randomJumpSound]);
        }
        else if (canDoubleJump == true)
        {
            //resettar ens momentum i Y-led så man inte kan stacka massa momentum
            rgbd.linearVelocity = new Vector2(rgbd.linearVelocityX, 0);

            rgbd.AddForce(new Vector2(0, jumpForce));
            doubleJumpParticleSystem.Play();
            int randomJumpSound = Random.Range(0, jumpSounds.Length);
            audioSource.PlayOneShot(jumpSounds[randomJumpSound]);
            canDoubleJump = false;
        }
    }
    private void isOnWall()
    {
        RaycastHit2D leftHandHit = Physics2D.Raycast(leftHand.position, Vector2.left, 0.15f, whatIsGround);
        RaycastHit2D rightHandHit = Physics2D.Raycast(rightHand.position, Vector2.right, 0.15f, whatIsGround);
        //print("Är i isOnWall");
        if (leftHandHit.collider != null && leftHandHit || rightHandHit.collider != null && rightHandHit)
        {
            //print("Händer tar på nåt");
            isOnWallBool = true;
        }
        else
        {
            //print("Händer tar inte på nåt");
            isOnWallBool = false;

        }
    }
    private bool CheckIsGrounded()

    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, raycastDistance, whatIsGround);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFoot.position, Vector2.down, raycastDistance, whatIsGround);

        if (leftHit.collider != null && leftHit || rightHit.collider != null && rightHit)
        {

            EnableDoubleJump();
            return true;
        }
        else
        {
            return false;
        }

    }

    public void TakeKnockback(float knockbackForce, float upwardsForce)
    {
        canMove = false;
        rgbd.AddForce(new Vector2 (knockbackForce, upwardsForce));
        Invoke(nameof(CanMoveAgain), 0.25f);
    }

    private void CanMoveAgain()
    {
        canMove = true;
    }

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }

}
