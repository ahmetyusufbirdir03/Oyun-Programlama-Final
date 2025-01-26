using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject rock;
    public Transform spawnPoint;
    public bool isSpawned;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isSpawned)
        {
            Instantiate(rock, spawnPoint.position,Quaternion.identity);
            isSpawned = true;
        }
    }
}
