using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenCase : MonoBehaviour
{

    [SerializeField]
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Eliminated()
    {
        var player = transform.Find("Player_Art");
        
        if (player != null)
        {
            player.parent = null;
        }

        m_animator.SetBool("Hurt", true);
    }
}
