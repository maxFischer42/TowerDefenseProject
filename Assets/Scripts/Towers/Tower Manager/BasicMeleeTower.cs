using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMeleeTower : TowerManager
{
    public GameObject hitboxToSpawn;


    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void DoAttack()
    {

        /*Vector3 targetPos = range.currentTarget.position;
        Vector3 direction = targetPos - transform.position;
        direction.Normalize();
        direction *= shootSpeed;
        GameObject proj = Instantiate(projectileToShoot, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = direction;*/

        Vector2 spawnPos = transform.position;     
        GameObject hit = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        hit.GetComponent<ProjectileInfo>().damage = damage;
        hit.GetComponent<ProjectileInfo>().origin = transform.parent.GetComponent<HeroPosition>();
        base.DoAttack();
    }
}
