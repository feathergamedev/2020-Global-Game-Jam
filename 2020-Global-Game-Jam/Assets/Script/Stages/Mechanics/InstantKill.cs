using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Repair.Infrastructures.Events;

public class InstantKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventEmitter.Emit(GameEvent.Killed);
        }
    }
}
