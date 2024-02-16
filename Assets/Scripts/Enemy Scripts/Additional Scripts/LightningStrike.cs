using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public int chainStrength = 5;
    public float disableLength = 2f;
    public GameObject childToSpawn;
    // Start is called before the first frame update
    void Update()
    {
        if (chainStrength <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.tag == "HERO_CHARACTER") ;
            d.GetComponentInChildren<TowerManager>().enabled = false;
            d.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
        GameManager.Instance.SetupReenable(disabledHeroes, _action.timeToDisable);*/

        if(collision.tag == "HERO_CHARACTER")
        {
            if (collision.transform.GetComponentInParent<HeroPosition>().isLightningRod)
            {
                // todo add lightning capture effect
                return;
            }
            if(collision.GetComponent<TowerManager>().enabled)
            {
                if (collision.GetComponent<TowerManager>().isOnDisableCooldown) return;
                collision.GetComponent<TowerManager>().enabled = false;
                collision.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                List<HeroPosition> h = new List<HeroPosition>();
                h.Add(collision.transform.parent.GetComponent<HeroPosition>());
                GameManager.Instance.SetupReenable(h, disableLength);
                GameObject strike = Instantiate(childToSpawn, collision.transform.position, Quaternion.identity);
                strike.GetComponent<LightningStrike>().chainStrength -= 1;
            }
        }
    }
}
