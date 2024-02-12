using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("GameObject Settings")]
    public Transform playerModel;
    public Transform groundCheck;
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;

    [Header("Platform Settings")]
    public GameObject platform;
    float StandableJumpClearTime = 1f;
    float StandableClear = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        if (horizontalInput > 0)
        {
            RotatePlayerModel(Vector3.forward);
        }
        else if (horizontalInput < 0)
        {
            RotatePlayerModel(-Vector3.forward);
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
                    platform = OutHit.collider.gameObject;
                }
            }
        }
        else
        {
            DetachFromPlatform();
        }
    }

    private void Jump()
    {
        DetachFromPlatform();
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void DetachFromPlatform()
    {
        if (platform)
        {  
            StandableClear = Time.time + StandableJumpClearTime;
            platform.GetComponent<PlatformLogic>().PlatformStand(gameObject, false);
            platform = null; 
        }
    }

    private void RotatePlayerModel(Vector3 lookDirection)
    {
        Quaternion quaternion = Quaternion.LookRotation(lookDirection);
        playerModel.rotation = Quaternion.Lerp(playerModel.rotation, quaternion, Time.deltaTime * moveSpeed);
    }
}