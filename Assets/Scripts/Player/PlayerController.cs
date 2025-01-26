using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float backSpeed;

    [Header("Ground Checker")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.35f;

    [Header("Booleans")]
    [SerializeField] private bool isGrounded;
    [SerializeField] public bool isAboutToLand;
    [SerializeField] private bool canFire;

    [Header("Fire")]
    [SerializeField] private GameObject shuriken;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float fireCooldown;

    [Header("Health")]
    [SerializeField] public GameObject healthParent;

    [Header("Coin")]
    [SerializeField] public TMP_Text coinText;
    [SerializeField] private float coinIncreaseSpeed = 0.1f;

    [Header("Bonus")]
    [SerializeField] private bool canDoubleJump = false; 
    [SerializeField] public bool isJumpBonusActive = false;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip shurikenSound;
    [SerializeField] private AudioClip coinCollectSound;
    [SerializeField] private AudioClip chestOpenSound;
    [SerializeField] private AudioClip trapSound;
    [SerializeField] private AudioClip healthSound;

    Rigidbody2D rb;
    Animator anim;
    AudioSource audioSource;
    Timer timer;
    UIManager uiManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        timer = GameObject.FindWithTag("Timer").GetComponent<Timer>();
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();

        canFire = true;
        if (!PlayerPrefs.HasKey("Life"))
        {
            PlayerPrefs.SetInt("Life", 3);
        }

        

    }

    private void Update()
    {
        Move();
        Jump();
        Fire();
        HealthManager();
        AdjustRotationToSlope();

        coinText.text = PlayerPrefs.GetInt("Coin").ToString("00");

    }

    private void Move()
    {
        moveSpeed = Input.GetKey(KeyCode.LeftShift) ? 6 : 3;

        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        anim.SetBool("Move", isMoving);

        if (isMoving)
        {
            anim.speed = Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1f;
        }

    }

    private void PerformJump(float slopeAngle)
    {
        if (slopeAngle == 0)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        else
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * 1.2f);

        anim.SetBool("JumpUp", true);
        audioSource.PlayOneShot(jumpSound); // JUMP SESİ
        anim.SetBool("JumpDown", false);
    }


    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isAboutToLand = !isGrounded && rb.velocity.y < 0 && Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius * 5f, groundLayer);

        anim.SetBool("JumpUp", !isGrounded && rb.velocity.y > 0);
        anim.SetBool("JumpDown", isAboutToLand);

        Vector2 rayDirection = Vector2.down + (Vector2.right * rb.velocity.x * 0.1f);
        float rayDistance = groundCheckRadius * 5f;

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, rayDirection, rayDistance, groundLayer);
        float slopeAngle = Vector2.Angle(Vector2.up, hit.normal); 

        if (isGrounded)
        {
            anim.SetBool("JumpDown", false);
            canDoubleJump = true; 
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded) 
            {
                PerformJump(slopeAngle);
            }
            else if (isJumpBonusActive && canDoubleJump) 
            {
                PerformJump(0); 
                canDoubleJump = false; 
            }
        }
    }


    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire)
        {
            canFire = false;

            GameObject shrukienObj = Instantiate(shuriken, firePoint.transform.position, Quaternion.identity);
            audioSource.PlayOneShot(shurikenSound); // shuriken sesi
            shrukienObj.GetComponent<Shuriken>().moveInput = transform.eulerAngles.y == 180 ? -1 : 1;

            Invoke(nameof(ResetCanFire), fireCooldown);
        }
    }

    private void ResetCanFire()
    {
        canFire = true;
    }

    private void HealthManager()
    {
        Transform[] healthIcons = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            healthIcons[i] = healthParent.transform.GetChild(i);
        }

        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].gameObject.SetActive(i < PlayerPrefs.GetInt("Life"));
        }

        if (PlayerPrefs.GetInt("Life") <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PlayerPrefs.SetInt("Coin", 0);
            timer.ResetTimer();
            SceneManager.LoadScene(0);
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 1);
            audioSource.PlayOneShot(coinCollectSound); // para toplama sesi
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("ChestBlue"))
        {
            Animator chestAnim = collision.gameObject.GetComponent<Animator>();
            Animator textAnim = collision.gameObject.transform.GetChild(0).GetComponent<Animator>();

            textAnim.SetTrigger("Open");
            chestAnim.SetTrigger("Open");
            audioSource.PlayOneShot(chestOpenSound); // para sandığı sesi 

            StartCoroutine(IncrementCoins(20));
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (collision.gameObject.CompareTag("ChestBrown"))
        {
            Animator chestAnim = collision.gameObject.GetComponent<Animator>();
            Animator textAnim = collision.gameObject.transform.GetChild(0).GetComponent<Animator>();

            chestAnim.SetTrigger("Open");
            textAnim.SetTrigger("Open");
            audioSource.PlayOneShot(chestOpenSound); // double jump sandığı alma sesi 

            ActivateJumpBonus();
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (collision.gameObject.CompareTag("HealthPot"))
        {
            audioSource.PlayOneShot(healthSound); // can alma sesi
            if (PlayerPrefs.GetInt("Life") < 3) 
            {
                PlayerPrefs.SetInt("Life", PlayerPrefs.GetInt("Life") + 1);
                
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("EndLevel"))
        {
            print(timer.GetTimerValue());
            SceneManager.LoadScene(2);
        }

        if (collision.gameObject.CompareTag("EndGame"))
        {
            uiManager.EndGame();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            PlayerPrefs.SetInt("Life", PlayerPrefs.GetInt("Life") - 1);
            audioSource.PlayOneShot(trapSound); // trap'e basınca can azalma sesi 
            StartCoroutine(MoveBackSmoothly());
        }
    }

    private IEnumerator MoveBackSmoothly()
    {
        float targetX = transform.position.x - 1f;
        float startX = transform.position.x;
        float elapsedTime = 0f;

        while (elapsedTime < backSpeed)
        {
            float newX = Mathf.Lerp(startX, targetX, elapsedTime / backSpeed);
            rb.MovePosition(new Vector2(newX, transform.position.y));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(new Vector2(targetX, transform.position.y));
    }


    private void AdjustRotationToSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius * 3f, groundLayer);

        if (hit.collider != null)
        {
            Vector2 groundNormal = hit.normal;
            float slopeAngle = Vector2.SignedAngle(Vector2.up, groundNormal);

            if (rb.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, slopeAngle);
            }
            else if (rb.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, -slopeAngle);
            }
        }

    }

    private IEnumerator IncrementCoins(int amount)
    {
        int coinsAdded = 0;
        while (coinsAdded < amount)
        {
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 1);
            coinsAdded++;
            yield return new WaitForSeconds(coinIncreaseSpeed);
        }
    }

    public void ActivateJumpBonus()
    {
        isJumpBonusActive = true;
        Invoke(nameof(JumpBonusDisable), 10f);
    }
        
    private void JumpBonusDisable()
    {
        isJumpBonusActive = false;

    }

}
