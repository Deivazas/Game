using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    private bool facingLeft = true;
    private Collider2D coll;
    private Rigidbody2D rb;
    private Animator anim;
    public int health = 100;
    public float HP = 1f;
    

    public GameObject deathEffect;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 2f;

    

    public void TakeDamage (int damage)
    {
        health -= damage;
        HP -= damage/200f;
        healthBar.SetSize(HP);
        if(health <= 0 && HP <= 0f)
        {
            Die();
            Talk();

        }

    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {

        if (anim.GetBool("Walking"))
        {
            if (rb.velocity.x == 0)
            {
                anim.SetBool("Walking", false);
            }
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if(speed !=0)
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                    anim.SetBool("Walking", true);
                }
     
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                    //transform.Rotate(HealthBar(0f, 180f, 0f));
                }

                if (speed != 0)
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                    anim.SetBool("Walking", true);
                }
            }
            else
            {
                facingLeft = true;
            }

        }
    }

    private Vector3 HealthBar(float v1, float v2, float v3)
    {
        throw new NotImplementedException();
    }

    public void Talk()
    {
        Debug.Log("Nooo!");
    }
}
