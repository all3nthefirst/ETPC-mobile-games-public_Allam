using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // 
    //    if (collision.collider.CompareTag("Box"))
    //    {
    //        if(collision.contacts[0].normal.y < -0.5f)
    //        {
    //            // El angulo de colision es vertical, sino no morimos todavia
    //            Debug.Log("A box has hit the enemy, the enemy is dead." + collision.contacts[0].normal.y);
    //            Destroy(gameObject);
    //        }
    //    }

    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        // El angulo de colision es vertical, sino no morimos todavia
    //        Debug.Log("The player has hit the enemy, the enemy is dead.");
    //        Destroy(gameObject);
    //    }

    //}
}
