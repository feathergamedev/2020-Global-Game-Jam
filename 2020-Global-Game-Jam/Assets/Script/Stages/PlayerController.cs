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
    private Transform m_attackTransform;

    [SerializeField]
    private Animator m_catAnimator, m_weaponAnimator;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();

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

        if (m_isOnGround)
        {
            m_rigid.velocity -= new Vector2(0, m_rigid.velocity.y);
        }
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
        else  if ((e & ActionType.Right) == ActionType.Right)
        {
            MoveRight();
        }
        else
        {
            StopWalking();
        }

        if ((e & ActionType.Hit) == ActionType.Hit)
        {
            RequestAttack();
        }
    }

    private bool GroundCheck()
    {
        var DetectSize = m_groundDetectTransform.localScale;
        return Physics2D.OverlapBox(m_groundDetectTransform.position, DetectSize, 0, m_layerGround);
    }

    public void StopWalking()
    {
        m_rigid.velocity -= new Vector2(m_rigid.velocity.x, 0);
    }

    public void MoveLeft()
    {
        m_catAnimator.SetBool("Walk", true);

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
        m_catAnimator.SetBool("Walk", true);

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

    public void RequestAttack()
    {
        m_weaponAnimator.SetTrigger("Attack");
    }

    public void AttackDetect()
    {
        Debug.Log("Attack!");

        var radius = m_attackTransform.GetComponent<CircleCollider2D>().radius;

        var result = Physics2D.OverlapCircleAll(m_attackTransform.position, radius);

        if (result.Length == 0)
            return;

        for(int i=0; i<result.Length; i++)
        {
            var woodCase = result[i].GetComponent<WoodenCase>();

            if (woodCase != null)
            {
                woodCase.Eliminated();
            }
        }
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
