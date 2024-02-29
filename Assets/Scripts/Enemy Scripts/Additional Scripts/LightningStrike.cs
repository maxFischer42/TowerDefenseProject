using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public int chainStrength = 5;
    public float disableLength = 2f;
    public GameObject childToSpawn;

    private void Start()
    {
        transform.localScale *= chainStrength;
        Debug.Log("Scale: " + transform.localScale + " || Strength: " + chainStrength);
    }
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
        if (chainStrength <= 0) return;
        if(collision.tag == "HERO_CHARACTER")
        {
            if (collision.transform.GetComponentInParent<HeroPosition>().isLightningRod)
            {
                // todo add lightning capture effect
                return;
            }
            if(collision.GetComponent<TowerManager>().enabled)
            {
                HeroPosition h = collision.transform.parent.GetComponent<HeroPosition>();
                if (h.isDisabled) return;
                GameManager.Instance.SetupReenable(h, disableLength);
                GameObject strike = Instantiate(childToSpawn, collision.transform.position, Quaternion.identity);
                strike.GetComponent<LightningStrike>().chainStrength = chainStrength - 1;
            }
        }
    }
}
