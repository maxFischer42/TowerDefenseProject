using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShooterTower : TowerManager
{
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
    public override void DoAttack() {
        
        Vector3 targetPos = range.currentTarget.position;
        Vector3 direction = targetPos - transform.position;
        direction.Normalize();
        direction *= shootSpeed;
        GameObject proj = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = direction;
        proj.GetComponent<ProjectileInfo>().origin = transform.parent.GetComponent<HeroPosition>();
        proj.GetComponent<ProjectileInfo>().damage = damage;
        if(transform.parent.GetComponent<HeroPosition>().pierceMod)
        {
            proj.GetComponent<DieOnTimer>().dieOnCollision = false;
        }
        base.DoAttack();
    }
}
