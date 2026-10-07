using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Player player;
    private float floatTime = 1;
    private float timePassed = 0;
    private int direction = 1;

    void Update()
    {
        Vector2 position = transform.position;
        position.y += direction * Time.deltaTime;
        transform.position = position;
        timePassed += Time.deltaTime;
        if(timePassed > floatTime)
        {
            timePassed = 0;
            direction *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.AddPresent();
            Destroy(this.gameObject);
        }
    }
}
