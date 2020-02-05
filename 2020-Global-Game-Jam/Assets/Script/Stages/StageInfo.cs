using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Repair.Infrastructures.Events;

public class StageInfo : MonoBehaviour
{
    [SerializeField]
    private Transform m_initPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("Player").transform.position = m_initPlayerPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
