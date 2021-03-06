using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Repair.Infrastructures.Events;
using Repair.Infrastructures.Settings;
using System;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

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

    private bool m_isAttacking;
    private bool m_isJumping;
    private bool m_isInitialized;
    private long m_startJumpAt;

    [SerializeField]
    private Animator m_catAnimator, m_weaponAnimator;

    [SerializeField]
    private GameObject m_curTouchingPlatform;

    private void Awake()
    {
        instance = this;

        m_rigid = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();

        EventEmitter.Add(GameEvent.Action, HandleOnAction);
        EventEmitter.Add(GameEvent.Killed, ElectricKill);
        EventEmitter.Add(GameEvent.StageClear, RequestStageClear);

    }

    // Start is called before the first frame update
    void Start()
    {
        m_initMoveSpeed = m_moveSpeed;
        m_rigid.bodyType = RigidbodyType2D.Dynamic;
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
        if (!m_isInitialized)
        {
            m_isOnGround = GroundCheck();
            if (m_isOnGround)
            {
                m_isInitialized = true;
            }
        }

        if (m_isInitialized && m_isJumping && (DateTime.UtcNow.Ticks - m_startJumpAt > 1000000))
        {
            m_isOnGround = GroundCheck();
            if (m_isOnGround)
            {
                m_isJumping = false;
                m_rigid.velocity -= new Vector2(0, m_rigid.velocity.y);
            }
        }

        m_catAnimator.SetBool("OnGround", m_isOnGround);

#if UNITY_EDITOR        
        if (Input.GetKey(KeyCode.J))
        {
            MoveLeft();
        }
        else if (Input.GetKey(KeyCode.L))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RequestAttack();
        }

        if (Input.GetKey(KeyCode.I))
        {
            Debug.Log($"Press I");
            Jump();
        }

        if (Input.GetKey(KeyCode.F))
        {
            SpeedUp();
        }
#endif

        if (m_catAnimator.GetBool("Walk") == false)
        {
            m_rigid.velocity = new Vector2(0f, m_rigid.velocity.y);
        }

    }

    private void HandleOnAction(IEvent @event)
    {
        RegisterInputEvent((@event as ActionEvent).Value);
    }

    private void RegisterInputEvent(ActionType e)
    {
        if ((e & ActionType.Left) == ActionType.Left
            && (e & ActionType.Right) == ActionType.Right)
        {
            Debug.Log("Left and Right at the same time. Do nothing.");
        }
        else if ((e & ActionType.Left) == ActionType.Left)
        {
            MoveLeft();
        }
        else if ((e & ActionType.Right) == ActionType.Right)
        {
            MoveRight();
        }
        else
        {
            m_catAnimator.SetBool("Walk", false);
        }

        if ((e & ActionType.Sprint) == ActionType.Sprint)
        {
            SpeedUp();
        }
        else
        {
            BackToNormalSpeed();
        }

        if ((e & ActionType.NewJump) == ActionType.NewJump)
        {
            Debug.Log("Jump!!");
            Jump();
        }

        if ((e & ActionType.Hit) == ActionType.Hit)
        {
            RequestAttack();
        }
    }

    public void Initialize(Vector3 initPos)
    {
        m_rigid.velocity = Vector2.zero;
        transform.position = initPos;
    }

    private bool GroundCheck()
    {
        var DetectSize = m_groundDetectTransform.localScale;
        var platform = Physics2D.OverlapBox(m_groundDetectTransform.position, DetectSize, 0, m_layerGround);

        if (platform == null)
        {
            m_curTouchingPlatform = null;
            transform.SetParent(null);
            return false;
        }

        m_curTouchingPlatform = platform.gameObject;
        transform.SetParent(m_curTouchingPlatform.transform);

        return true;
    }

    public void StopWalking()
    {
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
        m_catAnimator.SetBool("Dash", true);
    }

    public void BackToNormalSpeed()
    {
        m_moveSpeed = m_initMoveSpeed;
        m_catAnimator.SetBool("Dash", false);
    }

    public void RequestAttack()
    {
        if (m_isAttacking == true)
            return;

        m_weaponAnimator.SetTrigger("Attack");

        m_isAttacking = true;

        EventEmitter.Emit(GameEvent.PlaySound, new SoundEvent(SoundType.swing_sword, 6));
    }

    public void AttackDetect()
    {
        var radius = m_attackTransform.GetComponent<CircleCollider2D>().radius;

        var result = Physics2D.OverlapCircleAll(m_attackTransform.position, radius);

        if (result.Length == 0)
            return;

        for (int i = 0; i < result.Length; i++)
        {
            var woodCase = result[i].GetComponent<WoodenCase>();

            if (woodCase != null)
            {
                woodCase.Eliminated();
            }
        }

        m_isAttacking = false;
    }

    public void Jump()
    {
        if (!m_isOnGround || m_isJumping)
        {
            Debug.Log("Not on ground!");
            return;
        }

        transform.SetParent(null);
        m_startJumpAt = DateTime.UtcNow.Ticks;

        m_isOnGround = false;
        m_isJumping = true;
        m_rigid.velocity += new Vector2(0, m_jumpForce);

        m_catAnimator.SetTrigger("Jump");

        EventEmitter.Emit(GameEvent.PlaySound, new SoundEvent(SoundType.CatJump, 8));
    }

    public void ElectricKill(IEvent @event)
    {
        Debug.LogWarning("ElectricKill");
        transform.SetParent(null);
        m_catAnimator.SetTrigger("Die");
        m_collider.enabled = false;
        m_rigid.velocity = Vector2.zero;
        m_rigid.bodyType = RigidbodyType2D.Static;

        EventEmitter.Emit(GameEvent.PlaySound, new SoundEvent(SoundType.cat_screaming, 7));

        StartCoroutine(DiePerform());
    }

    private IEnumerator DiePerform()
    {
        yield return new WaitForSeconds(1.6f);
        EventEmitter.Emit(GameEvent.Restart);
    }

    public void RequestStageClear(IEvent @event)
    {
        Debug.LogWarning("RequestStageClear");
        var finishPoint = GameObject.FindWithTag("FinishPoint");
        m_collider.enabled = false;
        m_rigid.bodyType = RigidbodyType2D.Static;
        var oldPosY = transform.position.y;
        m_rigid.velocity = Vector2.zero;

        transform.position = new Vector3(finishPoint.transform.position.x - 0.92f, oldPosY);

        m_catAnimator.SetTrigger("Win");

        EventEmitter.Emit(GameEvent.PlaySound, new SoundEvent(SoundType.Clown_Horn_Squeak, 9));

        StartCoroutine(StageClearPerform());
    }

    private IEnumerator StageClearPerform()
    {
        yield return new WaitForSeconds(1.0f);
        EventEmitter.Emit(GameEvent.Complete);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "InstantKill":
                EventEmitter.Emit(GameEvent.Killed);
                break;
        }
    }
}
