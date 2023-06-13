using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.parent.CompareTag("Player"))
        {
            GameManager.level.curIndex++;
            Destroy(gameObject);
        }
    }
}
