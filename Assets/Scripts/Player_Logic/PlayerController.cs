using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    public enum MovementDirection
    {
        right,
        left
    }

    [Header("GameObject Settings")]
    public Transform playerModel;
    public Transform groundCheck;
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float inertia = 0.9f;
    private Vector3 moveDirection = Vector3.zero;
    public bool isFlipped;
    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isPaused;

    [Header("Model Rotation Settings")]
    public bool lookRotationLock;
    public Transform lookRotationPoint;
    public float lookRotationSpeed;
    public MovementDirection moveDir;
    private bool canTurn;
    private float turnTime = 1f;

    [Header("Platform Settings")]
    [HideInInspector]
    public GameObject platform;
    private float StandableJumpClearTime = 1f;
    private float StandableClear = 0f;

    [Header("Grapple Gun Settings")]
    public GrappleGunLogic grappleGun;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canTurn = true;

        grappleGun = gameObject.GetComponentInChildren<GrappleGunLogic>();
    }

    private void Update()
    {
        //Match local pause state to global pause state
        if (PauseCheck.instance != null)
        {
            isPaused = PauseCheck.instance.isPaused;
        }

        if (isPaused == false)
        {
            // Horizontal movement input
            float horizontalInput = Input.GetAxis("Horizontal");
            moveDirection.x = horizontalInput * moveSpeed * Time.deltaTime;

            // Apply inertia
            moveDirection *= inertia;

            // Move the player
            transform.Translate(moveDirection);

            // Jumping
            if (Input.GetButtonDown("Jump") && isGrounded || Input.GetButtonDown("Cross") && isGrounded)
            {
                Jump();
            }

            // Set player model direction
            if (horizontalInput > 0 && canTurn == true)
            {
                if (moveDir != MovementDirection.right)
                {
                    RotatePlayerModel(MovementDirection.right);
                }
            }
            else if (horizontalInput < 0 && canTurn == true)
            {
                if (moveDir != MovementDirection.left)
                {
                    RotatePlayerModel(MovementDirection.left);
                }
            }

            // Rotate player towards the lookRotation point 
            if (lookRotationLock == true)
            {
                //Find the direction of the point
                Vector3 targetDirection = lookRotationPoint.position - playerModel.position;
                targetDirection.y = 0f;

                // Swap x and z components to handle the X-axis as forward
                Vector3 forwardDirection = new Vector3(-targetDirection.z, 0f, targetDirection.x);

                //Rotate the PlayerModel towards the point
                Quaternion lookRotation = Quaternion.LookRotation(forwardDirection);
                playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, lookRotation, lookRotationSpeed * Time.deltaTime);
            }

            // Check if grounded
            if (Physics.Raycast(groundCheck.position, Vector3.down, 0.1f, LayerMask.GetMask("Ground")) || Physics.Raycast(groundCheck.position, Vector3.down, 0.1f, LayerMask.GetMask("Platform")))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            //Check for platform
            RaycastHit OutHit;
            if (Physics.Raycast(groundCheck.position, Vector3.down, out OutHit, 0.1f, LayerMask.GetMask("Platform")))
            {
                if (Time.time > StandableClear)
                {
                    PlatformLogic HitStandable = OutHit.collider.gameObject.GetComponent<PlatformLogic>();
                    if (HitStandable)
                    {
                        HitStandable.PlatformStand(gameObject, true);

                        if (HitStandable.type == PlatformLogic.PlatformType.MoveableVertical)
                        {
                            RemoveRigidbody();
                        }

                        platform = OutHit.collider.gameObject;
                    }
                }
            }
            else
            {
                DetachFromPlatform();
            }
        }
    }

    private void Jump()
    {
        AddRigidbody();
        DetachFromPlatform();
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void DetachFromPlatform()
    {

        if (rb == null)
        {
            AddRigidbody();
        }

        if (platform)
        {
            StandableClear = Time.time + StandableJumpClearTime;
            platform.GetComponent<PlatformLogic>().PlatformStand(gameObject, false);
            platform = null;
        }
    }

    public void AddRigidbody()
    {
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();

            rb.angularDrag = 0.1f;
            rb.constraints = RigidbodyConstraints.FreezePositionZ |
                             RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationY |
                             RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void RemoveRigidbody()
    {
        Destroy(rb);
        rb = null;
    }

    public void RotatePlayerModel(MovementDirection direction)
    {
        Quaternion targetRotation = Quaternion.identity;

        switch (direction)
        {
            case MovementDirection.right:
                moveDir = MovementDirection.right;
                if (isFlipped != true)
                {
                    targetRotation = Quaternion.LookRotation(Vector3.forward);

                }
                else
                {
                    targetRotation = Quaternion.LookRotation(-Vector3.forward);
                }
                break;
            case MovementDirection.left:
                moveDir = MovementDirection.left;
                if (isFlipped != true)
                {
                    targetRotation = Quaternion.LookRotation(-Vector3.forward);
                }
                else
                {
                    targetRotation = Quaternion.LookRotation(Vector3.forward);
                }
                break;
            default:
                break;
        }

        if (canTurn == true && lookRotationLock == false)
        {
            StartCoroutine(RotatePlayerModelCoroutine(targetRotation));
        }
    }

    private IEnumerator RotatePlayerModelCoroutine(Quaternion targetRotation)
    {
        canTurn = false;

        float elapsedTime = 0f;
        Quaternion startRotation = playerModel.rotation;

        while (elapsedTime < turnTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / turnTime);
            playerModel.rotation = Quaternion.Lerp(startRotation, targetRotation, 3f * t);
            yield return null;
        }

        canTurn = true;
    }
}