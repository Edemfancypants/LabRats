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
    public bool isGrounded;

    [Header("Model Rotation Settings")]
    public MovementDirection moveDir;
    private bool canTurn;
    private float turnTime = 1f;

    [Header("Platform Settings")]
    [HideInInspector]
    public GameObject platform;
    private float StandableJumpClearTime = 1f;
    private float StandableClear = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        canTurn = true;
    }

    private void Update()
    {
        // Horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, 0f);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Rotate player model
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

        if (Input.GetKeyDown(KeyCode.L))
        {
            AddRigidbody();
        }
    }

    private void Jump()
    {
        if (rb == null)
        {
            AddRigidbody();
        }

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
        rb = gameObject.AddComponent<Rigidbody>();

        rb.angularDrag = 0.1f;
        rb.constraints = RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    public void RemoveRigidbody()
    {
        Destroy(rb);
        rb = null;
    }

    private void RotatePlayerModel(MovementDirection direction)
    {
        Quaternion targetRotation = Quaternion.identity;

        switch (direction)
        {
            case MovementDirection.right:
                moveDir = MovementDirection.right;
                targetRotation = Quaternion.LookRotation(Vector3.forward);
                break;
            case MovementDirection.left:
                moveDir = MovementDirection.left;
                targetRotation = Quaternion.LookRotation(-Vector3.forward);
                break;
            default:
                break;
        }

        if (canTurn == true)
        {
            StartCoroutine(RotatePlayerModelCoroutine(targetRotation));
        }
    }

    private IEnumerator RotatePlayerModelCoroutine(Quaternion targetRotation)
    {
        canTurn = false;

        float elapsedTime = 0f;

        while (elapsedTime < turnTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / turnTime);
            playerModel.rotation = Quaternion.Lerp(playerModel.rotation, targetRotation, t);
            yield return null;
        }

        canTurn = true;
    }
}