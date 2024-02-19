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

    [Header("Bullet Settings")]
    public float bulletForce = 20f;
    public float rotationSpeed = 5f;

    [Header("Gun Settings")]
    private float rotationZ = 0f;

    [Header("Grapple Settings")]
    public ConfigurableJoint grappleJoint;
    public GameObject playerGameObject;
    public Transform grappledPoint;
    public bool isGrappled;
    [HideInInspector]
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

        if (Input.GetButtonDown("Fire1") && bullet == null)
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            lineRenderer.enabled = false;
            GrappleDetach();
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

        if (isGrappled == true && grappleJoint != null && grappleJoint != null)
        {
            grappleJoint.connectedAnchor = grappledPoint.position;
        }
    }

    public void GrappleAttach()
    {
        grappleJoint = playerGameObject.AddComponent<ConfigurableJoint>();

        grappleJoint.xMotion = ConfigurableJointMotion.Limited;
        grappleJoint.yMotion = ConfigurableJointMotion.Limited;
        grappleJoint.zMotion = ConfigurableJointMotion.Locked;

        grappleJoint.autoConfigureConnectedAnchor = false;
        grappleJoint.connectedAnchor = grappledPoint.position;
    
        SoftJointLimitSpring limitSpring = new SoftJointLimitSpring();
        limitSpring.spring = 5f;
        limitSpring.damper = 1f;
        grappleJoint.linearLimitSpring = limitSpring;

        SoftJointLimit tempLimit = grappleJoint.linearLimit;
        tempLimit.limit = 5f;
        grappleJoint.linearLimit = tempLimit;

        grappleJoint.configuredInWorldSpace = true;
    }

    public void GrappleDetach()
    {
        if (grappleJoint != null)
        {
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
