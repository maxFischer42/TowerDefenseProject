using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlasherTower : TowerManager
{
    public GameObject hitboxToSpawn;
    public GameObject projectileToShoot;
    public float shootSpeed = 1f;


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
        anim.SetTrigger("ATTACK");
        Vector2 spawnPos = transform.position;
        GameObject hit = Instantiate(hitboxToSpawn, spawnPos, Quaternion.identity);
        hit.GetComponent<ProjectileInfo>().damage = damage + transform.parent.GetComponent<HeroPosition>().damageMod;
        hit.GetComponent<ProjectileInfo>().origin = transform.parent.GetComponent<HeroPosition>();
        if (pos.altAttackUnlocked) DoAltAttack();
        base.DoAttack();
    }

    public void DoAltAttack()
    {
        Vector3 targetPos = range.currentTarget.position;
        Vector3 direction = targetPos - transform.position;
        direction.Normalize();
        direction *= shootSpeed;
        float rotAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject proj = Instantiate(projectileToShoot, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = direction;
        proj.GetComponent<ProjectileInfo>().origin = transform.parent.GetComponent<HeroPosition>();
        proj.GetComponent<ProjectileInfo>().damage += damage;
        proj.transform.rotation = Quaternion.AngleAxis(rotAngle, new Vector3(0, 0, 1));
        if (transform.parent.GetComponent<HeroPosition>().pierceMod)
        {
            proj.GetComponent<DieOnTimer>().dieOnCollision = false;
            proj.GetComponent<DieOnTimer>().pierceCount += pos.numPierceMod;
        }


    }
}