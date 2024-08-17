using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform attackAnchor;
    [SerializeField] Collider2D attackArea;
    [SerializeField] LayerMask mask;

    ContactFilter2D filter;

    [Header("Base Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float cooldownDuration;
    [SerializeField] int damage;

    float lastAttack = 0;

    private void Start()
    {
        filter = new ContactFilter2D();
        filter.SetLayerMask(mask);
        filter.useTriggers = true;
    }

    public void Initialize(int score)
    {
        Debug.Log("Initializing player with " + score + " power!");
    }

    void Update()
    {
        Movement();
        Attacking();
    }

    void Movement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(inputX, inputY) * moveSpeed;
    }

    void Attacking()
    {
        // Point collider towards mouse
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        attackAnchor.up = mousePos - (Vector2)transform.position;

        if (Time.time < lastAttack + cooldownDuration) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            lastAttack = Time.time;

            // Check overlap on collider
            List<Collider2D> results = new List<Collider2D>();
            attackArea.OverlapCollider(filter, results);
            if (results.Count > 0)
            {
                foreach (Collider2D result in results)
                {
                    if (result.transform.CompareTag("Enemy"))
                    {
                        result.GetComponent<Enemy>().TakeDamage(damage);
                    }
                }
            }

            Debug.Log("attack");
        }
    }
}
