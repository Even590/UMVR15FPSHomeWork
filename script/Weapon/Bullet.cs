using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask Enemy;
    public GameObject bloodEffectPrefab;
    public GameObject bulletHolePrefab;

    public float bulletHoleLifeTime = 4f;

    [Range(0, 1)]
    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public int explosionRange;
    public int explosionForce;

    public int MaxCollisions;
    public float MaxLifeTime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physic_mat;

    GameObject Enemies;

    private void Awake()
    {
        Enemies = GameObject.FindGameObjectWithTag("Enemy");
    }

    private void Start()
    {
        Setup();
    }

    private void Update()
    {

        if (collisions > MaxCollisions) Destroy(gameObject);

        MaxLifeTime -= Time.deltaTime;

        if (MaxLifeTime <= 0) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisions++;

        if (collision.collider.CompareTag("Envirment"))
        {
            Debug.Log("你打中了地形！");
            if (bulletHolePrefab != null)
            {
                ContactPoint contact = collision.contacts[0];
                Vector3 hitPoint = contact.point;
                Quaternion hitRotation = Quaternion.LookRotation(contact.normal);

                GameObject bulletHole = Instantiate(bulletHolePrefab, hitPoint, hitRotation);
                bulletHole.transform.SetParent(collision.collider.transform);

                Destroy(bulletHole, bulletHoleLifeTime);
            }
            else
            {
                Debug.Log("未使用牆洞貼圖");
            }

            Destroy(gameObject);
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("你打中了殭屍！");
            Health enemyHealth = collision.collider.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(explosionDamage);
                Debug.Log("殭屍受到傷害:" + explosionDamage + "剩餘血量：" + enemyHealth.GetCurrentHealth());
            }
            if (enemyHealth == null)
            {
                Debug.Log("沒有找到殭屍血量系統");
            }

            if(bloodEffectPrefab != null)
            {
                Vector3 hitpoint = collision.contacts[0].point;
                Instantiate(bloodEffectPrefab, hitpoint, Quaternion.identity);
            }
            else
            {
                Debug.Log("未使用血液特效");
            }

            Destroy(gameObject);
        }
    }
    private void Setup()
    {
        physic_mat = new PhysicMaterial();
        physic_mat.bounciness = bounciness;
        physic_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physic_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        GetComponent<MeshCollider>().material = physic_mat;

        rb.useGravity = useGravity;
    }
}
