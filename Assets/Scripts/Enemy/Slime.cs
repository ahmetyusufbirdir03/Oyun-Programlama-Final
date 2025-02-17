using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject slimeBomb;
    [SerializeField] private Transform spawnPoint;

    [Header("Booleans")]
    [SerializeField] private bool isAttacking = false;

    [Header("Health")]
    [SerializeField] public float health;
    [SerializeField] private Image healthBar;

    Animator anim;


    AudioSource audioSource;
    public AudioClip slimeSoundClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();

        health = 100;
        if (audioSource == null)
        {
            Debug.LogError("Ses Dosyası Bulunamadı");
        }
        else{
            audioSource.clip = slimeSoundClip;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if(player.transform.position.x > transform.position.x)
            {
                transform.rotation = new Quaternion(0, 0, 0, 1);
            }
            else
            {
                transform.rotation = new Quaternion(0, 180, 0, 1);
            }


            if (distance < 12f && !isAttacking)
            {
                StartCoroutine(SpawnSlimeBombs());
            }
        }

        if (health <= 0)
        {
            anim.SetTrigger("Dead");
            Destroy(this.gameObject, 1.5f);
        }

        healthBar.fillAmount = health / 100f;
    }

    private IEnumerator SpawnSlimeBombs()
    {
        isAttacking = true;

        while (Vector3.Distance(transform.position, player.transform.position) < 12f)
        {
            anim.SetTrigger("Attack");
            GameObject slimeObj = Instantiate(slimeBomb, spawnPoint.position, Quaternion.Euler(0, 180, 0));
            audioSource.Play(); // slime atıs sesi
            slimeObj.GetComponent<SlimeBall>().moveInput = transform.eulerAngles.y != 0 ? -1 : 1;
            yield return new WaitForSeconds(2f);
        }

        isAttacking = false;
    }
}
