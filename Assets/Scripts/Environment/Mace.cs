using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour
{
    public float rotationSpeed = 3;
    public float maxRotationAngle = 90f;
    public float knockbackForce = 10f;
    private float timeCounter = 0f;

    AudioSource audioSource;
    public AudioClip maceDamageSound;

    private void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timeCounter += Time.deltaTime * rotationSpeed;
        float currentAngle = Mathf.Sin(timeCounter) * maxRotationAngle;
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("Life", PlayerPrefs.GetInt("Life") - 1);
            audioSource.PlayOneShot(maceDamageSound);

            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
