using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Repair.Infrastructures.Events;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private bool m_isOnGround;

    [SerializeField]
    private bool m_isFacingRight;

    [SerializeField]
    private float m_moveSpeed;

    private float m_initMoveSpeed;

    [SerializeField]
    private float m_jumpForce;

    [SerializeField]
    private TrailRenderer m_dashEffect;

    [SerializeField]
    private Transform m_groundDetectTransform;

    [SerializeField]
    private LayerMask m_layerGround;

    [SerializeField]
    private Vector2 m_dashForce;

    [SerializeField]
    private float m_dashDuration;

    [SerializeField]
    private Rigidbody2D m_rigid;

    [SerializeField]
    private SpriteRenderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_initMoveSpeed = m_moveSpeed;

        /*
        EventEmitter.Add(GameEvent.Action, (value) =>
        {

        });

        MoveLeft();
        MoveRight();
        Jump();
        */
    }

    // Update is called once per frame
    void Update()
    {

        m_isOnGround = GroundCheck();

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

        if (Input.GetButtonUp("Horizontal"))
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
            Dash();
        }
        else if (Input.GetButtonUp("Dash"))
        {
            m_moveSpeed = m_initMoveSpeed;
        }
    }

    private bool GroundCheck()
    {
        var DetectSize = m_groundDetectTransform.localScale;
        return Physics2D.OverlapBox(m_groundDetectTransform.position, DetectSize, 0, m_layerGround);
    }

    public void MoveLeft()
    {
        m_rigid.velocity = new Vector2(-m_moveSpeed, m_rigid.velocity.y);
        m_isFacingRight = false;
        m_renderer.flipX = true;
    }

    public void MoveRight()
    {
        m_rigid.velocity = new Vector2(m_moveSpeed, m_rigid.velocity.y);
        m_isFacingRight = true;
        m_renderer.flipX = false;
    }

    public void Dash()
    {
        var dashForce = m_dashForce * ((m_isFacingRight==true) ? 1 : -1);
        Debug.Log(dashForce);
        StartCoroutine(DashPerform(dashForce));        
    }

    private IEnumerator DashPerform(Vector2 force)
    {
        m_dashEffect.emitting = true;

        m_rigid.velocity = force;
/*
        var time = m_dashDuration * 60f;

        for(int i=0; i<time; i++)
        {
            var newVelocity = force - force / time;
            m_rigid.velocity = newVelocity;
        }
*/
        
        var newVelocity = force;
        var curX = force.x;
        var curY = force.y;
        var isPerforming = true;

        DOTween.To(() => curX, x => curX = x, 0, m_dashDuration);
        DOTween.To(() => curY, y => curX = y, 0, m_dashDuration);

        DOVirtual.DelayedCall(m_dashDuration, () => { isPerforming = false; });

        for(int i=0; i<m_dashDuration*60f; i++)
        {
            Debug.LogFormat("X:{0}, Y:{1}", curX, curY);
            newVelocity = new Vector2(curX, curY);
            Debug.LogFormat("NewVelocity is {0}", newVelocity);
            m_rigid.velocity = newVelocity;
            yield return new WaitForSeconds(m_dashDuration / 60f);
        }        

        m_dashEffect.emitting = false;
        

        yield return null;
    }

    public void Jump()
    {
        if (!m_isOnGround)
            return;

        m_rigid.velocity += new Vector2(0, m_jumpForce);
    }
}
