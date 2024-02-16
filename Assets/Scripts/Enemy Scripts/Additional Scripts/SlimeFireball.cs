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
        List<int> populatedPositions = new List<int>();
        foreach (HeroPosition p in GameObject.FindObjectsByType<HeroPosition>(FindObjectsSortMode.InstanceID))
        {
            if(p.isPopulated && !p.isPossessed)
            {
                populatedPositions.Add(p.tileId);
            }
        }

        int count = populatedPositions.Count;
        int rnd = Random.Range(0, count);

        if (count == 0) Destroy(gameObject);

        Vector2 pos = GameManager.Instance.heroManager.heroList[rnd].transform.position;
        Vector2 direction = (pos - (Vector2)transform.position).normalized;
        directionOnSpawn = direction;
        rb = GetComponent<Rigidbody2D>();
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "HERO_CHARACTER")
        {
            collision.GetComponent<TowerManager>().UpdateHealth(damage);
        }
        if (isPierce && collision.tag == "HERO_CHARACTER") {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = directionOnSpawn * speed;
        rb.velocity = velocity;
    }


}
