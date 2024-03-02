using UnityEngine;

public class GrappleGunLogic : MonoBehaviour
{
    public static GrappleGunLogic instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Component References")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    private GameObject bullet;
    private GameObject playerGameObject;

    [Header("Bullet Settings")]
    private float bulletForce = 20f;
    private float rotationSpeed = 5f;

    [Header("Gun Settings")]
    private float rotationZ = 0f;

    [Header("Grapple Settings")]
    private ConfigurableJoint grappleJoint;
    [HideInInspector]
    public Transform grappledPoint;
    [HideInInspector]
    public bool isGrappled;

    [Header("Line Settings")]
    public LineRenderer lineRenderer;

    private void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        rotationZ += mouseY * rotationSpeed;
        rotationZ = Mathf.Clamp(rotationZ, -60f, 60f);

        transform.localRotation = Quaternion.Euler(0, 0f, rotationZ);

        if (Input.GetButtonDown("Fire2") || Input.GetButtonDown("Right Shoulder") && bullet == null && isGrappled != true)
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire3") || Input.GetButtonDown("Left Shoulder"))
        {
            lineRenderer.enabled = false;
            Destroy(bullet);
            GrappleDetach();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && grappleJoint != null)
        {
            if (grappleJoint.linearLimit.limit < 5f)
            {
                ChangeGrappleRange(0.1f);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && grappleJoint != null)
        {
            if (grappleJoint.linearLimit.limit > .5f)
            {
                ChangeGrappleRange(-0.1f);
            }
        }
    }

    private void LateUpdate()
    {
        if (bullet != null)
        {
            grappledPoint = null;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, bullet.transform.position);
        }
        else if (grappledPoint != null)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, grappledPoint.position);
        }

        if (isGrappled == true && grappleJoint != null)
        {
            grappleJoint.connectedAnchor = grappledPoint.position;
        }
    }

    public void GrappleAttach(GrapplePointProperties grappleProperties)
    {
        if (PlayerController.instance.platform != null)
        {
            PlayerController.instance.DetachFromPlatform();
        }

        float grappleSpring = grappleProperties.properties.spring;
        float grappleDamping = grappleProperties.properties.damping;
        float grappleRange = grappleProperties.properties.range;

        grappleJoint = playerGameObject.AddComponent<ConfigurableJoint>();

        grappleJoint.xMotion = ConfigurableJointMotion.Limited;
        grappleJoint.yMotion = ConfigurableJointMotion.Limited;
        grappleJoint.zMotion = ConfigurableJointMotion.Locked;

        grappleJoint.autoConfigureConnectedAnchor = false;
        grappleJoint.connectedAnchor = grappledPoint.position;
    
        SoftJointLimitSpring limitSpring = new SoftJointLimitSpring();
        limitSpring.spring = grappleSpring;
        limitSpring.damper = grappleDamping;
        grappleJoint.linearLimitSpring = limitSpring;

        SoftJointLimit tempLimit = grappleJoint.linearLimit;
        tempLimit.limit = grappleRange;
        grappleJoint.linearLimit = tempLimit;

        grappleJoint.configuredInWorldSpace = true;
    }

    public void ChangeGrappleRange(float rangeFloat)
    {
        SoftJointLimit tempLimit = grappleJoint.linearLimit;
        tempLimit.limit += rangeFloat;
        grappleJoint.linearLimit = tempLimit;
    }

    public void GrappleDetach()
    {
        if (grappleJoint != null)
        {
            isGrappled = false;

            Destroy(grappledPoint.gameObject);

            grappleJoint = playerGameObject.GetComponent<ConfigurableJoint>();
            Destroy(grappleJoint);
        }
    }

    private void Shoot()
    {
        bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            lineRenderer.enabled = true;
            rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
        }
    }
}
