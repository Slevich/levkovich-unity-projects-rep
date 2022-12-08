using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Damage which will be taken to characters")]
    [SerializeField] private float damage;

    [Header("GameObjact with animation of destroy of the bullet")]
    [SerializeField] private GameObject destroyFX;

    //Время жизни пули, чтобы та не летела бесконечно.
    private float lifeTimer;

    //Заспаунена ли пуля.
    private bool spawned;

    private void Awake()
    {
        lifeTimer = 1.5f;
        spawned = true;
    }

    private void Update()
    {
        if (spawned)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                SpawnObject();
                Destroy(gameObject);
            }
        }
    }

    private void SpawnObject()
    {
        Instantiate(destroyFX, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damageable"))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            SpawnObject();
            Destroy(gameObject);
        }
    }
}
