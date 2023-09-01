using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon : MonoBehaviour
{
    //public GameObject ammoObjectPrefab;
    public const int ammoCount = 3;
    public GameObject[] ammoObjectPrefabs = new GameObject[ammoCount];
    static List<GameObject> ammoPool;
    public int poolSize = 5;
    private Player player;

    private bool isFiring = false;
    [HideInInspector]
    public bool beCanWalk = true;
    private Animator anim;
    private float angleDir;

    private void Awake()
    {
        ammoPool = new List<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoObjectPrefabs[0]);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Player>();
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                isFiring = true;
                beCanWalk = false;
                FireAmmo();
            }
        }

        UpdateAnimState();
    }

    private void FireAmmo()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - new Vector2(transform.position.x, transform.position.y);
        angleDir = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angleDir - 90, Vector3.forward);

        player.staminaPoints--;
        if(player.staminaPoints <= 10.0f)
        {
            player.staminaPoints = 10.0f;
        }
        SpawnAmmo(transform.position, rotation);
    }

    GameObject SpawnAmmo(Vector3 location, Quaternion rotation)
    {
        foreach(GameObject ammo in ammoPool)
        {
            if(ammo.activeSelf == false)
            {
                ammo.transform.position = location;
                ammo.transform.rotation = rotation;
                Ammo ammoScript = ammo.GetComponent<Ammo>();
                ammoScript.lifeWeight = player.staminaPoints / player.maxStaminaPoints;
                ammo.SetActive(true);
                return ammo;
            }
        }
        return null;
    }

    public void ChangeAmmoPool(Item ammoItem)
    {
        int index = 0;
        for(int i = 0; i < ammoCount; i++)
        {
            Item ammoScript = ammoObjectPrefabs[i].GetComponent<Pickup>().item;
            if(ammoScript.itemType == ammoItem.itemType)
            {
                index = i;
                break;
            }
        }
        for(int i = 0; i < poolSize; i++)
        {
            Destroy(ammoPool[i]);
        }
        ammoPool.Clear();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoObjectPrefabs[index]);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    void UpdateAnimState()
    {
        if(isFiring)
        {
            anim.SetBool("isFiring", true);

            if(angleDir <= 45.0f && angleDir >= -45.0f)
            {
                anim.SetFloat("fireXDir", 1.0f);
                anim.SetFloat("fireYDir", 0.0f);
                anim.SetFloat("xDir", 1.0f);
                anim.SetFloat("yDir", 0.0f);
            }
            else if(angleDir < -45.0f && angleDir >= -135.0f)
            {
                anim.SetFloat("fireXDir", 0.0f);
                anim.SetFloat("fireYDir", -1.0f);
                anim.SetFloat("xDir", 0.0f);
                anim.SetFloat("yDir", -1.0f);
            }
            else if (angleDir > 45.0f && angleDir <= 135.0f)
            {
                anim.SetFloat("fireXDir", 0.0f);
                anim.SetFloat("fireYDir", 1.0f);
                anim.SetFloat("xDir", 0.0f);
                anim.SetFloat("yDir", 1.0f);
            }
            else
            {
                anim.SetFloat("fireXDir", -1.0f);
                anim.SetFloat("fireYDir", 0.0f);
                anim.SetFloat("xDir", -1.0f);
                anim.SetFloat("yDir", 0.0f);
            }

            isFiring = false;

            StartCoroutine(SetCanWalk());
        }
        else
        {
            anim.SetBool("isFiring", false);
        }
    }

    IEnumerator SetCanWalk()
    {
        yield return new WaitForSeconds(0.3f);
        beCanWalk = true;
    }
}
