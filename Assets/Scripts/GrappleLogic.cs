using UnityEngine;

public class GrappleLogic : MonoBehaviour
{

    public static GrappleLogic instance;

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
    public Vector3 grappledPoint;
    public bool isGrappled;
    [HideInInspector]
    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        rotationZ += mouseY * rotationSpeed;
        rotationZ = Mathf.Clamp(rotationZ, -60f, 60f);

        transform.localRotation = Quaternion.Euler(0, 0f, rotationZ);

        if (Input.GetButtonDown("Fire1") && bullet == null)
        {
            Shoot();
        }

        if (bullet != null)
        {
            grappledPoint = Vector3.zero;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, bullet.transform.position);
        }
        else if (grappledPoint != Vector3.zero)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, grappledPoint);
        }
    }

    void Shoot()
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
