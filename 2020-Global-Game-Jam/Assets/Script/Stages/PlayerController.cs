using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Settings;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private bool m_isOnGround;

    [SerializeField]
    private bool m_isFacingRight;

    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private float m_sprintSpeedMultiplier;

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
    private BoxCollider2D m_collider;

    [SerializeField]
    private Transform m_attackTransform;

    [SerializeField]
    private Animator m_catAnimator, m_weaponAnimator;

    [SerializeField]
    private GameObject m_curTouchingPlatform;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();

        EventEmitter.Add(GameEvent.Action, HandleOnAction);
        EventEmitter.Add(GameEvent.Killed, ElectricKill);
        EventEmitter.Add(GameEvent.StageClear , RequestStageClear);

    }

    // Start is called before the first frame update
    void Start()
    {
        m_initMoveSpeed = m_moveSpeed;
    }

    private void OnDestroy()
    {
        EventEmitter.Remove(GameEvent.Action, HandleOnAction);
        EventEmitter.Remove(GameEvent.Killed, ElectricKill);
        EventEmitter.Remove(GameEvent.StageClear, RequestStageClear);
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

    private void HandleOnAction(IEvent @event)
    {
        RegisterInputEvent((@event as ActionEvent).Value);
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

        if ((e & ActionType.Sprint) == ActionType.Sprint)
        {
            SpeedUp();
            m_catAnimator.SetBool("Dash", true);
        }
        else
        {
            BackToNormalSpeed();
            m_catAnimator.SetBool("Dash", false);
        }


        if ((e & ActionType.Hit) == ActionType.Hit)
        {
            RequestAttack();
        }
    }

    private bool GroundCheck()
    {
        var DetectSize = m_groundDetectTransform.localScale;
        var platform = Physics2D.OverlapBox(m_groundDetectTransform.position, DetectSize, 0, m_layerGround);



        if (platform == null)
        {
            m_curTouchingPlatform = null;
            return false;
        }

        m_curTouchingPlatform = platform.gameObject;
        transform.SetParent(m_curTouchingPlatform.transform);
        return true;
    }

    public void StopWalking()
    {
        m_rigid.velocity -= new Vector2(m_rigid.velocity.x, 0);
        m_catAnimator.SetBool("Walk", false);
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

    public void SpeedUp()
    {
        m_moveSpeed = m_initMoveSpeed * m_sprintSpeedMultiplier;
    }

    public void BackToNormalSpeed()
    {
        m_moveSpeed = m_initMoveSpeed;
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

    public void ElectricKill(IEvent @event)
    {
        m_catAnimator.SetTrigger("Die");
        m_collider.enabled = false;
        m_rigid.bodyType = RigidbodyType2D.Kinematic;

        StartCoroutine(DiePerform());
    }

    private IEnumerator DiePerform()
    {
        yield return new WaitForSeconds(1.05f);
        EventEmitter.Emit(GameEvent.Restart);
    }

    public void RequestStageClear(IEvent @event)
    {
        m_catAnimator.SetTrigger("Win");
        m_collider.enabled = false;
        m_rigid.bodyType = RigidbodyType2D.Kinematic;

        StartCoroutine(StageClearPerform());
    }

    private IEnumerator StageClearPerform()
    {
        yield return new WaitForSeconds(1.0f);

        // Fade out;

        EventEmitter.Emit(GameEvent.Complete);

        yield return null;

    }
}
