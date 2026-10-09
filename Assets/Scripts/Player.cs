using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private UIManager ui;
    [SerializeField] public float speed = 2;
    [SerializeField] private float JumpHeight = 2.4f;
    [SerializeField] private GameObject projectilePrefab;
    private Rigidbody2D rb;
    private Vector2 move;
    private int direction = 1;
    private Animator animator;
    private int jumpCount = 0;
    private int score = 0;
    [SerializeField] private int lives = 3;
    private Vector2 lastPosition;

    // for invinciblitly
    private float invincibilityTime = 3.0f;
    private float invincibilityCount = 0f;
    public bool isInvin = false;
    private AudioSource audio;
    private bool isPlaying = false;
    [SerializeField] private AudioClip collectClip;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        lastPosition = rb.transform.position;

        audio = GetComponent<AudioSource>();

    }

    private void Fire()
    {
        GameObject go = Instantiate(projectilePrefab, rb.transform.position, Quaternion.identity);
        Projectile pro = go.GetComponent<Projectile>();
        pro.Launch(new Vector2(direction, 0), 300);
    }

    void Update()
    {
        move = InputSystem.actions["Move"].ReadValue<Vector2>();
        if(move.x != 0)
        {
            direction = move.x < 0 ? -1 : 1;
        }

        if(InputSystem.actions["Jump"].IsPressed() && jumpCount < 2)
        {
            InputSystem.actions["Jump"].Reset();
            rb.linearVelocity = new Vector2 (0, 0);
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2 * Physics.gravity.y * JumpHeight)),
                ForceMode2D.Impulse);
            jumpCount++;
        }
        if(InputSystem.actions["Attack"].IsPressed())
        {
            InputSystem.actions["Attack"].Reset();
            Fire();
        }

        if(move.x != 0 && jumpCount == 0 && !isPlaying)
        {
            audio.Play();
            isPlaying = true;
        }

        if(isPlaying && (jumpCount > 0 || move.x == 0))
        {
            audio.Pause();
            isPlaying = false;
        }

        if(isInvin)
        {
            invincibilityCount += Time.deltaTime;
            if(invincibilityCount > invincibilityTime)
            {
                isInvin = false;
                invincibilityCount = 0;
                this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
        }
    }

    void FixedUpdate()
    {
        animator.SetInteger("Direction", direction);
        animator.SetFloat("Move", move.x);
        Vector2 position = rb.position;
        position.x += move.x * speed * Time.fixedDeltaTime;
        rb.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
        }
        else if(collision.gameObject.tag == "EnemyProjectile" && !isInvin)
        {
            rb.linearVelocity = Vector2.zero;
            PlayerHit();
        }
        else if(collision.gameObject.tag == "EnemyProjectile" && !isInvin)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        /*if(collision.gameObject.tag == "EnemyProjectile")
        {
            lives--;
            //gm.updateLives(lives);
            rb.transform.position = lastPosition;
        }*/
    }

    private void PlayerHit()
    {
        lives--;
            ui.SetLives(lives);
            rb.transform.position = lastPosition;
            if(lives == 0)
            {
                SceneManager.LoadScene("SampleScene");
            }
            isInvin = true;
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Checkpoint")
        {
            lastPosition = collision.transform.position;
            collision.gameObject.GetComponent<Checkpoint>().Hit();
        }
    }

    public void AddPresent()
    {
        if(isPlaying)
        {
            audio.Pause();
        }
        audio.PlayOneShot(collectClip);
        if(isPlaying)
        {
            audio.Play();
        }
        score++;
        /*if(score == 5)
        {
            //win condition - load a new scene or whatever
        }*/
        Debug.Log(score);
        ui.setScore(score);
    }
}