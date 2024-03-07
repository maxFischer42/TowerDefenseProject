using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class BasicShooterTower : TowerManager
{
    public GameObject projectileToShoot;
    public float shootSpeed = 1f;

    public Transform bowObjectTransform;
    public Animator bowObjectAnimator;



    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }

    public virtual void ApplySubObjectAction()
    {
        subObject.GetComponent<Animator>().runtimeAnimatorController = pos.subAnimMod;
    }

    public override void DoAttack() {
        
        Vector3 targetPos = range.currentTarget.position;
        Vector3 direction = targetPos - transform.position;
        direction.Normalize();
        direction *= shootSpeed;
        

        // Apply bow transforms and animations
        bowObjectAnimator.SetTrigger("FIRE");

        float rotAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bowObjectTransform.rotation = Quaternion.AngleAxis(rotAngle, new Vector3(0, 0, 1));
        rot = rotAngle;
        dir = direction;
        Invoke(nameof(DelayedAttack), 0.15f);
 
        base.DoAttack();
    }

    private Vector2 dir;
    private float rot;

    public void DelayedAttack()
    {
        GameObject proj = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = dir;
        proj.GetComponent<ProjectileInfo>().origin = transform.parent.GetComponent<HeroPosition>();
        proj.GetComponent<ProjectileInfo>().damage += damage;
        proj.transform.rotation = Quaternion.AngleAxis(rot, new Vector3(0, 0, 1));
        if (transform.parent.GetComponent<HeroPosition>().pierceMod)
        {
            proj.GetComponent<DieOnTimer>().dieOnCollision = false;
            proj.GetComponent<DieOnTimer>().pierceCount += pos.numPierceMod;
        }
    }
}
