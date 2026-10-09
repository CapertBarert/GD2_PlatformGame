using UnityEngine;

public class RobotBevahiour : MonoBehaviour
{
    [SerializeField] private float walkTime;
    [SerializeField] private float idleTime;
    private float elapsedTime;
    private int direction = 1;
    private bool isWalking = false;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private float FireTimer = 0.5f;
    [SerializeField] private GameObject projectile;
    private float fireCountdown;

    private AudioSource audio;
    private int strength;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetInteger("Direction", direction);
        animator.SetFloat("Move", 0);
        audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        if(isWalking && elapsedTime < walkTime)
        {
            Vector2 position = rb.transform.position;
            position.x += direction * Time.deltaTime;
            rb.transform.position = position;
        }
        else if(!isWalking && elapsedTime > idleTime)
        {
            isWalking = true;
            elapsedTime = 0;
            direction *= -1;
            audio.Play();
            animator.SetInteger("Direction", direction);
            animator.SetFloat("Move", direction);
        }
        else if(isWalking && elapsedTime > walkTime)
        {
            isWalking = false;
            elapsedTime = 0;
            animator.SetFloat("Move", 0);
            audio.Pause();
        }
        elapsedTime += Time.deltaTime;

        //if(is)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            rb.linearVelocity = new Vector2(0, 0);
            hit();
        }
    }

    private void hit()
    {
        //if (!isInvincible)
        //{
            strength--;

            if(strength <= 0)
            {
                Destroy(this.gameObject);
                return;
            }

            //isInvincible = true;
            //this.GetComponent<spriteRenderer>().color = new Color(1,1,1,0.5f);
        //}
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, new Vector2(direction, 0), 5f, LayerMask.GetMask("Player"));
        if (hit.collider !=null)
        {
            if(hit.collider.GetComponent<Player>()!=null)
            {
                Fire();
            }
        }
        fireCountdown += Time.fixedDeltaTime;
    }

    private void Fire()
    {
        if (fireCountdown > FireTimer)
        {
            GameObject go = Instantiate(projectile, rb.transform.position, Quaternion.identity);
            Projectile pro = go.GetComponent<Projectile>();
            pro.Launch(new Vector2(direction, 0), 300);
            fireCountdown = 0;
        }
    }
}
