using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.InputSystem;

public enum GroundType
{
    None,
    Soft,
    Hard
}

public class CharacterController2D : MonoBehaviour
{
    readonly Vector3 flippedScale = new Vector3(-1, 1, 1);
    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    [Header("Character")]
    [SerializeField] Animator animator = null;
    [SerializeField] Transform puppet = null;
    [SerializeField] CharacterAudio audioPlayer = null;

    [Header("Tail")]
    [SerializeField] Transform tailAnchor = null;
    [SerializeField] Rigidbody2D tailRigidbody = null;

    [Header("Equipment")]
    [SerializeField] Transform handAnchor = null;
    [SerializeField] SpriteLibrary spriteLibrary = null;

    [Header("Movement")]
    [SerializeField] float acceleration = 0.0f;
    [SerializeField] float maxSpeed = 0.0f;
    [SerializeField] float jumpForce = 0.0f;
    [SerializeField] float minFlipSpeed = 0.1f;
    [SerializeField] float jumpGravityScale = 1.0f;
    [SerializeField] float fallGravityScale = 1.0f;
    [SerializeField] float groundedGravityScale = 1.0f;
    [SerializeField] bool resetSpeedOnLand = false;

    private Rigidbody2D controllerRigidbody;
    private Collider2D controllerCollider;
    private LayerMask softGroundMask;
    private LayerMask hardGroundMask;

    private Vector2 movementInput;
    private bool jumpInput;

    private Vector2 prevVelocity;
    private GroundType groundType;
    private bool isFlipped;
    private bool isJumping;
    private bool isFalling;

    private int animatorGroundedBool;
    private int animatorRunningSpeed;
    private int animatorJumpTrigger;

    ReplayInputs replayInputs;

    [SerializeField]
    bool useAutomation = false;

    public bool CanMove { get; set; }

    void Start()
    {
        controllerRigidbody = GetComponent<Rigidbody2D>();
        controllerCollider = GetComponent<Collider2D>();
        softGroundMask = LayerMask.GetMask("Ground Soft");
        hardGroundMask = LayerMask.GetMask("Ground Hard");

        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");
        animatorJumpTrigger = Animator.StringToHash("Jump");

        CanMove = true;

        replayInputs = GetComponent<ReplayInputs>();
    }

    void Update()
    {
        if (useAutomation)
        {
            InputState replayState = replayInputs.inputState;
            
            float moveHorizontal = 0.0f;

            if (replayState.Left)
            {
                //Debug.Log("Move left");
                moveHorizontal = -1.0f;
            }
            
            else if (replayState.Right)
                moveHorizontal = 1.0f;

            movementInput = new Vector2(moveHorizontal, 0);

            // Jumping input
            if (!isJumping && replayState.JumpDown)
                jumpInput = true;
        }
        else
        {
            var keyboard = Keyboard.current;

            if (!CanMove || keyboard == null)
            {
                Debug.Log("No keyboard");
                return;
            }
            

            // Horizontal movement
            float moveHorizontal = 0.0f;

            if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
            {
                //Debug.Log("Move left");
                moveHorizontal = -1.0f;
            }
            
            else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
                moveHorizontal = 1.0f;

            movementInput = new Vector2(moveHorizontal, 0);

            // Jumping input
            if (!isJumping && keyboard.spaceKey.wasPressedThisFrame)
                jumpInput = true;
        }
        
    }

    void FixedUpdate()
    {
        UpdateGrounding();
        UpdateVelocity();
        UpdateDirection();
        UpdateJump();
        UpdateTailPose();
        UpdateGravityScale();

        prevVelocity = controllerRigidbody.velocity;
    }

    private void UpdateGrounding()
    {
        // Use character collider to check if touching ground layers
        if (controllerCollider.IsTouchingLayers(softGroundMask))
            groundType = GroundType.Soft;
        else if (controllerCollider.IsTouchingLayers(hardGroundMask))
            groundType = GroundType.Hard;
        else
            groundType = GroundType.None;

        // Update animator
        animator.SetBool(animatorGroundedBool, groundType != GroundType.None);
    }

    private void UpdateVelocity()
    {
        Vector2 velocity = controllerRigidbody.velocity;

        // Apply acceleration directly as we'll want to clamp
        // prior to assigning back to the body.
        velocity += movementInput * acceleration * Time.fixedDeltaTime;

        // We've consumed the movement, reset it.
        movementInput = Vector2.zero;

        // Clamp horizontal speed.
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

        // Assign back to the body.
        controllerRigidbody.velocity = velocity;

        // Update animator running speed
        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) / maxSpeed;
        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);

        // Play audio
        audioPlayer.PlaySteps(groundType, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
        // Set falling flag
        if (isJumping && controllerRigidbody.velocity.y < 0)
            isFalling = true;

        // Jump
        if (jumpInput && groundType != GroundType.None)
        {
            // Jump using impulse force
            controllerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            // Set animator
            animator.SetTrigger(animatorJumpTrigger);

            // We've consumed the jump, reset it.
            jumpInput = false;

            // Set jumping flag
            isJumping = true;

            // Play audio
            audioPlayer.PlayJump();
        }

        // Landed
        else if (isJumping && isFalling && groundType != GroundType.None)
        {
            // Since collision with ground stops rigidbody, reset velocity
            if (resetSpeedOnLand)
            {
                prevVelocity.y = controllerRigidbody.velocity.y;
                controllerRigidbody.velocity = prevVelocity;
            }

            // Reset jumping flags
            isJumping = false;
            isFalling = false;

            // Play audio
            audioPlayer.PlayLanding(groundType);
        }
    }

    private void UpdateDirection()
    {
        // Use scale to flip character depending on direction
        if (controllerRigidbody.velocity.x > minFlipSpeed && isFlipped)
        {
            isFlipped = false;
            puppet.localScale = Vector3.one;
        }
        else if (controllerRigidbody.velocity.x < -minFlipSpeed && !isFlipped)
        {
            isFlipped = true;
            puppet.localScale = flippedScale;
        }
    }

    private void UpdateTailPose()
    {
        // Calculate the extrapolated target position of the tail anchor.
        Vector2 targetPosition = tailAnchor.position;
        targetPosition += controllerRigidbody.velocity * Time.fixedDeltaTime;

        tailRigidbody.MovePosition(targetPosition);
        if (isFlipped)
            tailRigidbody.SetRotation(tailAnchor.rotation * flippedRotation);
        else
            tailRigidbody.SetRotation(tailAnchor.rotation);
    }

    private void UpdateGravityScale()
    {
        // Use grounded gravity scale by default.
        var gravityScale = groundedGravityScale;

        if (groundType == GroundType.None)
        {
            // If not grounded then set the gravity scale according to upwards (jump) or downwards (falling) motion.
            gravityScale = controllerRigidbody.velocity.y > 0.0f ? jumpGravityScale : fallGravityScale;           
        }

        controllerRigidbody.gravityScale = gravityScale;
    }

    public void GrabItem(Transform item)
    {
        // Attach item to hand
        item.SetParent(handAnchor, false);
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;
    }

    public void SwapSprites(SpriteLibraryAsset spriteLibraryAsset)
    {
        spriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
    }
}
