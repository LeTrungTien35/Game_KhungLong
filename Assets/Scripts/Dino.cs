using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D m_rb;
    Animator m_amin;
    bool m_isDead;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_amin = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_isDead) return;
        Flip();
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -9f, 9f), transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (m_isDead && m_rb)
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
        if (m_isDead) return;
        MoveHandle();     
    }
    void MoveHandle()
    {
        if(GamePadsController.Ins.CanMoveLeft)
        {
            if (m_rb)
                m_rb.velocity = new Vector2(-1f, m_rb.velocity.y) * moveSpeed;

            if (m_amin)
                m_amin.SetBool("Run", true);
        }else if(GamePadsController.Ins.CanMoveRight)
        {
            if (m_rb)
                m_rb.velocity = new Vector2(1f, m_rb.velocity.y) * moveSpeed;

            if (m_amin)
                m_amin.SetBool("Run", true);
        }
        else
        {
            if (m_rb)
                m_rb.velocity = new Vector2(0f, m_rb.velocity.y);

            if (m_amin)
                m_amin.SetBool("Run", false);
        }
    }    

    void Flip()
    {
        if(GamePadsController.Ins.CanMoveLeft)
        {
            if(transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1,
                    transform.localScale.y, transform.localScale.z);
            }
        }else if(GamePadsController.Ins.CanMoveRight)
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1,
                    transform.localScale.y, transform.localScale.z);
            }
        }
    }

     void Die()
    {
        m_isDead = true;
        if (m_amin)
            m_amin.SetTrigger("Dead");
        if (m_rb)
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Rock"))
        {
            if (m_isDead) return;

            Stone stone = col.gameObject.GetComponent<Stone>();

            if(stone && !stone.IsGround)
            {
                Die();
                GameManager.Ins.IsGameover = true;
                GameGUI.Ins.ShowGameover(true);
                AudioController.Ins.PlaySound(AudioController.Ins.loseSound);
                AudioController.Ins.PlaySound(AudioController.Ins.landSound);
            }
        }
    }
}
