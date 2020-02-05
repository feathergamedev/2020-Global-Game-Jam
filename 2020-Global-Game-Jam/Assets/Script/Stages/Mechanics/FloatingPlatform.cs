using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject m_platform;

    [SerializeField]
    private BoxCollider2D m_collider;

    [SerializeField]
    private Transform m_minBoundary, m_maxBoundary;

    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private Vector3 m_moveVector;

    private float m_curDirectionSign = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_moveVector = (m_maxBoundary.position - m_minBoundary.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_collider.bounds.Contains(m_minBoundary.position))
        {
            m_curDirectionSign = 1;
        }
        else if (m_collider.bounds.Contains(m_maxBoundary.position))
        {
            m_curDirectionSign = -1;
        }

    }

    private void FixedUpdate()
    {
        m_platform.transform.position += m_moveVector * m_moveSpeed * m_curDirectionSign * Time.fixedDeltaTime;
    }
}
