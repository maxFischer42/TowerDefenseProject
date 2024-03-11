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
    public Transform bowTipTransform;
    public Transform artOrigin;
    private Vector2 bowOrigin;
    bool isAiming = false;

    private Vector3 direction;

    public override void Start()
    {
        bowOrigin = artOrigin.position;
        base.Start();

    }
    public override void Update()
    {
        if(range.transformsInRange.Count > 0)
        {
            Vector3 targetPos = range.currentTarget.position;
            direction = targetPos - bowTipTransform.position;
            direction.Normalize();
            direction *= shootSpeed;
            float rotAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //bowObjectTransform.transform.rotation= Quaternion.AngleAxis(rotAngle, new Vector3(0, 0, 1));
            Vector2 offSet = bowOrigin + (Vector2)direction * 2f;
            bowObjectTransform.transform.position = offSet;
            anim.SetBool("AIM", true);
        } else
        {
            anim.SetBool("AIM", false);
            bowObjectTransform.transform.position = bowOrigin;
            //bowObjectTransform.transform.rotation = Quaternion.identity;
        }
        base.Update();
    }

    public virtual void ApplySubObjectAction()
    {
        subObject.GetComponentInChildren<Animator>().runtimeAnimatorController = pos.subAnimMod;
    }

    public override void DoAttack() {
       

        

        // Apply bow transforms and animations
        bowObjectAnimator.SetTrigger("FIRE");

        float rotAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //bowObjectTransform.rotation = Quaternion.AngleAxis(rotAngle, new Vector3(0, 0, 1));
        //Vector2 offSet = bowOrigin + (Vector2)direction * 0.2f;
        //bowObjectTransform.transform.position = offSet;
        rot = rotAngle;
        dir = direction;

        Invoke(nameof(DelayedAttack), 0.15f);
 
        base.DoAttack();
    }

    private Vector2 dir;
    private float rot;

    public void DelayedAttack()
    {
        GameObject proj = Instantiate(prefabToSpawn, bowTipTransform.position, Quaternion.identity);
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
