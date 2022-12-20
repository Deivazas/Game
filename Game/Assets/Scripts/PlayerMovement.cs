using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    public GameObject deathEffect;

    public int coins = 0;
    public int health = 100;
    public int damage = 30;
    public float HP = 1f;
    private bool m_facingRight = true;

    private enum State {idle, running, jumping, falling, hurt};
    private State state = State.running;
   



    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private Text goldNum;
    [SerializeField] private float hurtforce = 7.5f;
    [SerializeField] private HealthBar healthBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

    }
    private void Update()
    {
        if(state != State.hurt)
        {
        Movement();
        }
        VelocityState();
        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            coins += 15;
            goldNum.text = coins.ToString();
        } 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            state = State.hurt;
            health -= damage;
            HP -= damage / 100f;
            healthBar.SetSize(HP);
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                rb.velocity = new Vector2(-hurtforce, rb.velocity.y);
                if (health <= 0 && HP <= 0f)
                {
                    Die();
                }
            }
            else
            {
                rb.velocity = new Vector2(hurtforce, rb.velocity.y);
                if (health <= 0 && HP <= 0f)
                {
                    Die();

                }
            }
        }
    }

    private void Movement()
    {
        float hdirection = Input.GetAxis("Horizontal");


        if (hdirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if(m_facingRight == true)
            {
                flip();
            }

        }
        else if (hdirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(4, 4);
            if(m_facingRight != true)
            {
                flip();
            }
        }


        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            state = State.jumping;
        }

    }

    void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void flip()
    {
        m_facingRight = !m_facingRight;

        transform.Rotate(0f, 180f, 0f);
    }

    private void VelocityState()
    {
        if (state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }

        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }    
     
    }

    private Vector3 HealthBar(float v1, float v2, float v3)
    {
        throw new NotImplementedException();
    }

}

