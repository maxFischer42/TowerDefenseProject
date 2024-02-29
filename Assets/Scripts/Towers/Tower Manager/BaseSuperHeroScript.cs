using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSuperHeroScript : TowerManager
{

    public Image downedHealthbar;
    public GameObject downedHealthbarParent;
    public bool isDowned = false;
    public GameObject hitboxToSpawn;


    public override void Start()
    {
        base.Start();
    }
    public override void ManageHealthChanges()
    {
        if (!isDowned)
        {
            if (hp <= 0)
            {
                isDowned = true;
                perform = false;
                hp = maxHP - 1;
                downedHealthbarParent.SetActive(true);

                //DestroyTower();
            }
            if (hp >= maxHP)
            {
                hp = maxHP;
                healthbarParent.SetActive(false);
            }
            else if (hp < maxHP)
            {
                healthbarParent.SetActive(true);
            }
            healthbar.rectTransform.sizeDelta = new Vector2((float)hp / (float)maxHP, 0.1f);
        }
        else if(isDowned)
        {
            if (hp <= 0)
            {
                DestroyTower();
            }
            if (hp >= maxHP)
            {
                hp = 1;
                downedHealthbarParent.SetActive(false);
                isDowned = false;
                perform = true;
            }
            else if (hp < maxHP)
            {
                downedHealthbarParent.SetActive(true);
            }
            downedHealthbar.rectTransform.sizeDelta = new Vector2((float)hp / (float)maxHP, 0.1f);
        }
    }

    public override void DoAttack()
    {
        Vector2 spawnPos = transform.position;
        GameObject hit = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        hit.GetComponent<ProjectileInfo>().damage = damage;
        hit.GetComponent<ProjectileInfo>().origin = transform.parent.GetComponent<HeroPosition>();
        base.DoAttack();
    }

    public override void SuperUpdate()
    {     
        base.SuperUpdate();
    }
}
