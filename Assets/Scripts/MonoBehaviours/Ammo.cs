using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float ammoSpeed = 8.0f;
    public float maxAmmoLifeTime = 5.0f;
    [HideInInspector]
    public float lifeWeight = 1.0f;
    private float ammoLifeTime;
    private float baseTime = 0.0f;
    private Item ammoItem;
    public GameObject fireParticlePrefab;
    public GameObject waterParticlePrefab;
    private AudioSource aSource;

    private void Awake()
    {
        aSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        baseTime = 0.0f;
        ammoLifeTime = maxAmmoLifeTime * lifeWeight;
        aSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        ammoItem = gameObject.GetComponent<Pickup>().item;
        ammoLifeTime = maxAmmoLifeTime * lifeWeight;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * ammoSpeed * Time.deltaTime);
        baseTime += Time.deltaTime;
        if(baseTime >= ammoLifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(collision is BoxCollider2D)
            {
                if(ammoItem.itemType == Item.ItemType.FIREBALL)
                {
                    if(fireParticlePrefab != null)
                    {
                        Instantiate(fireParticlePrefab, transform.position, Quaternion.identity);
                    }
                }
                else if(ammoItem.itemType == Item.ItemType.WATERBALL)
                {
                    if (waterParticlePrefab != null)
                    {
                        Instantiate(waterParticlePrefab, transform.position, Quaternion.identity);
                    }
                }
                Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
                StartCoroutine(enemyScript.TakeDamage(ammoItem.amount, 0.0f));
                gameObject.SetActive(false);
            }
        }
    }
}
