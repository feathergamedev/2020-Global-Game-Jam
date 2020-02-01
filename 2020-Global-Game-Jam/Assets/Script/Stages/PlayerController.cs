using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private bool m_isOnGround;

    [SerializeField]
    private float m_moveSpeed;

    private float m_initMoveSpeed;

    [SerializeField]
    private float m_jumpForce;

    private Vector2 m_curVelocity;

    [SerializeField]
    private TrailRenderer m_dashEffect;

    [SerializeField]
    private Transform m_groundDetectTransform;

    [SerializeField]
    private Vector2 m_groundDetectSize;

    [SerializeField]
    private LayerMask m_layerGround;

    [SerializeField]
    private float m_dashSpeedUpRate;

    [SerializeField]
    private Rigidbody2D m_rigid;

    [SerializeField]
    private SpriteRenderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_initMoveSpeed = m_moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        m_isOnGround = Physics2D.OverlapBox(m_groundDetectTransform.position, m_groundDetectSize, 0, m_layerGround);

        if (m_isOnGround)
        {
            m_rigid.velocity = new Vector2(m_rigid.velocity.x, 0);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            MoveRight();
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            MoveLeft();
        }
        else
        {
            m_rigid.velocity = new Vector2(0, m_rigid.velocity.y);
        }

        if (Input.GetButton("Jump"))
        {
            Jump();
            m_isOnGround = false;
        }

        if (Input.GetButtonDown("Dash"))
        {
            m_dashEffect.emitting = true;
            Dash();
        }
        else if (Input.GetButtonUp("Dash"))
        {
            m_moveSpeed = m_initMoveSpeed;
            m_dashEffect.emitting = false;
        }
    }

    public void MoveLeft()
    {
        m_rigid.velocity = new Vector2(-m_moveSpeed, m_rigid.velocity.y);
        m_renderer.flipX = true;
    }

    public void MoveRight()
    {
        m_rigid.velocity = new Vector2(m_moveSpeed, m_rigid.velocity.y);
        m_renderer.flipX = false;
    }

    public void Dash()
    {
        m_moveSpeed *= m_dashSpeedUpRate;
    }

    public void Jump()
    {
        if (!m_isOnGround)
            return;

        m_rigid.velocity += new Vector2(0, m_jumpForce);
    }
}
