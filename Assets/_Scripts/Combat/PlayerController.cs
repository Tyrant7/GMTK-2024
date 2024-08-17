using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] Rigidbody2D rb;

    [Header("Base Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float cooldownDuration;

    float lastAttack = 0;

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
        if (Time.time < lastAttack + cooldownDuration) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            lastAttack = Time.time;
            Debug.Log("attack");
        }
    }
}
