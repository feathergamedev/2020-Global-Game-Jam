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
    private TrailRenderer m_sprintEffect;

    [SerializeField]
    private Transform m_groundDetectTransform;

    [SerializeField]
    private LayerMask m_layerGround;

    [SerializeField]
    private Vector2 m_sprintForce;

    [SerializeField]
    private float m_sprintDuration;

    [SerializeField]
    private Rigidbody2D m_rigid;

    [SerializeField]
    private SpriteRenderer m_renderer;

    [SerializeField]
    private Rigidbody2D Temp_camera;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<SpriteRenderer>();

        EventEmitter.Add(GameEvent.Action, (value) =>
        {
            RegisterInputEvent((value as ActionEvent).Value);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        m_initMoveSpeed = m_moveSpeed;
    }

    private void OnDestroy()
    {
        EventEmitter.Remove(GameEvent.Action, (value) =>
        {
            RegisterInputEvent((value as ActionEvent).Value);
        });
    }

    // Update is called once per frame
    void Update()
    {

        m_isOnGround = GroundCheck();

        #region CheatInput

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

        if (Input.GetButtonDown("Sprint"))
        {
            Sprint();
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            m_moveSpeed = m_initMoveSpeed;
        }

        #endregion
    }

    private void RegisterInputEvent(ActionType e)
    {
        if ((e & ActionType.Jump) == ActionType.Jump)
        {
            Jump();
        }

        if ((e & ActionType.Left) == ActionType.Left)
        {
            MoveLeft();
        }

        if ((e & ActionType.Right) == ActionType.Right)
        {
            MoveRight();
        }
    }

    private bool GroundCheck()
    {
        var DetectSize = m_groundDetectTransform.localScale;
        return Physics2D.OverlapBox(m_groundDetectTransform.position, DetectSize, 0, m_layerGround);
    }

    public void Stop()
    {
        m_rigid.velocity = Vector2.zero;
    }

    public void MoveLeft()
    {
        //Fix: Stange Bug
        if (m_rigid == null)
        {
            Debug.LogFormat("not found.");
            return;
        }

        var curVelocityY = m_rigid.velocity.y;
        m_rigid.velocity = new Vector2(-m_moveSpeed, curVelocityY);
        transform.rotation = Quaternion.Euler(0, 180, 0);
        m_isFacingRight = false;
    }

    public void MoveRight()
    {
        //Fix: Stange Bug
        if (m_rigid == null)
        {
            Debug.LogFormat("not found.");
            return;
        }

        var curVelocityY = m_rigid.velocity.y;
        m_rigid.velocity = new Vector2(m_moveSpeed, curVelocityY);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        m_isFacingRight = true;
    }

    public void Sprint()
    {
        var sprintForce = m_sprintForce * ((m_isFacingRight==true) ? 1 : -1);
        Debug.Log(sprintForce);
        StartCoroutine(SprintPerform(sprintForce));        
    }

    private IEnumerator SprintPerform(Vector2 force)
    {
        m_sprintEffect.emitting = true;

        m_rigid.velocity = force;
/*
        var time = m_sprintDuration * 60f;

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

        DOTween.To(() => curX, x => curX = x, 0, m_sprintDuration);
        DOTween.To(() => curY, y => curX = y, 0, m_sprintDuration);

        DOVirtual.DelayedCall(m_sprintDuration, () => { isPerforming = false; });

        for(int i=0; i<m_sprintDuration*60f; i++)
        {
            Debug.LogFormat("X:{0}, Y:{1}", curX, curY);
            newVelocity = new Vector2(curX, curY);
            Debug.LogFormat("NewVelocity is {0}", newVelocity);
            m_rigid.velocity = newVelocity;
            yield return new WaitForSeconds(m_sprintDuration / 60f);
        }        

        m_sprintEffect.emitting = false;
        

        yield return null;
    }

    public void Jump()
    {
        if (!m_isOnGround)
            return;

        m_rigid.velocity += new Vector2(0, m_jumpForce);
    }
}
