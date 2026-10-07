using UnityEngine;
using UnityEngine.InputSystem;

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
    private int lives = 3;
    private Vector2 lastPosition;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        lastPosition = rb.transform.position;
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
            /*GameObject sb = Instantiate(projectile, rb.position, Quaternion.identity);
            Projectile pro = sb.GetComponent<Projectile>();
            pro.Launch(new Vector2(direction, 0), 300);*/
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
        
        if(collision.gameObject.tag == "EnemyProjectile")
        {
            lives--;
            //gm.updateLives(lives);
            rb.transform.position = lastPosition;
        }
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
        score++;
        Debug.Log(score);
        ui.setScore(score);
    }
}