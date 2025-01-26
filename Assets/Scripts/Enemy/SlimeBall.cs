using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBall : MonoBehaviour
{
    public float moveSpeed;
    public float moveInput;

    GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player.transform.position.x > transform.position.x)
        {
            transform.rotation = new Quaternion(0, 0, 0, 1);
        }
        else
        {
            transform.rotation = new Quaternion(0, 180, 0, 1);
        }
    }
    private void Update()
    {
        transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0, 0);
        Destroy(this.gameObject, 2f);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Life", PlayerPrefs.GetInt("Life") - 1);
            Destroy(this.gameObject);
        }
    }
}
