using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float wanderIntervalTime = 3.0f;
    public float wanderSpeed = 2.0f;
    private Rigidbody2D rb2d;
    private Coroutine moveCoroutine;
    private Vector2 endPointPosition;
    private Animator anim;
    private CircleCollider2D circle;
    private Transform playerTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(WanderCoroutine());

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, endPointPosition, Color.red);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,circle.radius);
    }
    */

    IEnumerator WanderCoroutine()
    {
        while(true)
        {
            ChooseEndPoint();
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveCoroutine());
            yield return new WaitForSeconds(wanderIntervalTime);
        }
    }

    void ChooseEndPoint()
    {
        float wanderAngle = Random.Range(0, 360);
        float wanderRadius = Random.Range(2, 5);
        wanderAngle = wanderAngle * Mathf.Deg2Rad;
        endPointPosition = rb2d.position + new Vector2(Mathf.Cos(wanderAngle), Mathf.Sin(wanderAngle)) * wanderRadius;
    }

    IEnumerator MoveCoroutine()
    {
        float remainingDistance = (rb2d.position - endPointPosition).sqrMagnitude;
        while(remainingDistance > float.Epsilon)
        {
            anim.SetBool("isWalking", true);

            if(playerTransform != null)
            {
                endPointPosition = playerTransform.position;
            }

            Vector2 newPosition = Vector2.MoveTowards(rb2d.position, endPointPosition, wanderSpeed * Time.fixedDeltaTime);
            rb2d.MovePosition(newPosition);
            remainingDistance = (rb2d.position - endPointPosition).sqrMagnitude;
            yield return new WaitForFixedUpdate();
        }
        anim.SetBool("isWalking", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.gameObject.transform;
            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isWalking", false);
            playerTransform = null;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
        }
    }
}
