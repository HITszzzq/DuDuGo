using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    private Vector2 movDir = new Vector2();

    private Animator anim;

    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }
    private void MoveCharacter()
    {
        movDir.x = Input.GetAxisRaw("Horizontal");
        movDir.y = Input.GetAxisRaw("Vertical");
        movDir.Normalize();

        if(!weapon.beCanWalk)
        {
            movDir.x = 0.0f;
            movDir.y = 0.0f;
        }

        //transform.position += new Vector3(movDir.x, movDir.y, 0.0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movDir * moveSpeed * Time.deltaTime);

        //Debug.Log("XXXXXX" + movDir.ToString());
    }

    private void UpdateState()
    {
        if(Mathf.Approximately(movDir.x, 0.0f) && Mathf.Approximately(movDir.y, 0.0f))
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetFloat("xDir", movDir.x);
            anim.SetFloat("yDir", movDir.y);
            anim.SetBool("isWalking", true);
        }
    }
}
