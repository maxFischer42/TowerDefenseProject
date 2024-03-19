using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlimeFireball : MonoBehaviour
{

    public float speed;
    public bool isPierce = false;
    public int damage = 2;

    private Vector2 directionOnSpawn;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        List<HeroPosition> p = GameManager.Instance.heroManager.heroList;

        int count = p.Count;
        if (count == 0)
        {
            Destroy(gameObject);
            return;
        }
        int rnd = Random.Range(0, count);
        // Check if hero still exists
        if(GameManager.Instance.heroManager.heroList[rnd] == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 pos = GameManager.Instance.heroManager.heroList[rnd].transform.position;
        Vector2 direction = (pos - (Vector2)transform.position).normalized;
        directionOnSpawn = direction;       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "HERO_CHARACTER")
        {
            collision.GetComponent<TowerManager>().UpdateHealth(damage);
        }
        if (!isPierce && collision.tag == "HERO_CHARACTER") {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = directionOnSpawn * speed;
        rb.velocity = velocity;

        // Check if no heroes exist
        int count = GameManager.Instance.heroManager.heroList.Count;
        if (count == 0) Destroy(gameObject);
    }


}
