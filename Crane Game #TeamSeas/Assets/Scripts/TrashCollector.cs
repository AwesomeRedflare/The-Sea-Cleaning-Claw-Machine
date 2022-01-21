using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollector : MonoBehaviour
{
    public TrashSpawner trashSpawner;

    [HideInInspector]
    public int trashDestroyed;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("trash"))
        {
            FindObjectOfType<AudioManager>().Play("trash");
            Destroy(col.gameObject);
            trashDestroyed++;

            if (trashDestroyed == trashSpawner.trashAmount || trashDestroyed == trashSpawner.maxTrash)
            {
                Debug.Log("you win");

                FindObjectOfType<GameManager>().WinScreen();
            }
        }
    }
}
