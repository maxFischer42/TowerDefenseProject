using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingMageTower : TowerManager
{
    public GameObject healObjToSpawn;
    public HeroPosition heroToDefend;
    public float baseSupportReach = 3f;
    public bool canSupport = true;
    public float timeBetweenSupport = 5f;

    public override void Start()
    {
        base.Start();
        pos.spawnMod = healObjToSpawn;
        OnPriorityChange();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnPriorityChange()
    {
        heroToDefend = GameManager.Instance.heroManager.GetHeroToDefend(transform.position, priority, baseSupportReach + pos.supportReachMod, pos);
        base.OnPriorityChange();
    }

    public void ResetCanSupport()
    {
        canSupport = true;
    }

    public override void DoAttack()
    {
        if (heroToDefend != null && canSupport)
        {
            GameObject v = Instantiate(pos.spawnMod, heroToDefend.transform.position, Quaternion.identity);
            heroToDefend = GameManager.Instance.heroManager.GetHeroToDefend(transform.position, priority, baseSupportReach + pos.supportReachMod, pos);
            canSupport = false;
            Invoke(nameof(ResetCanSupport), 5f);
            anim.SetTrigger("HEAL");
            base.DoAttack();
        }

        else if (range.transformsInRange.Count > 0)
        {
            Vector2 spawnPos = transform.position;
            GameObject hit = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            hit.GetComponent<ProjectileInfo>().damage = damage + transform.parent.GetComponent<HeroPosition>().damageMod;
            hit.GetComponent<ProjectileInfo>().origin = transform.parent.GetComponent<HeroPosition>();
            anim.SetTrigger("ATTACK");
            base.DoAttack();
        }
    }

    public override void HandleSupportProcess()
    {
        if (heroToDefend == null) return;
        if (heroToDefend.tower.hp < heroToDefend.tower.maxHP)
        {
            DoAttack();
        }
        base.HandleSupportProcess();
    }
}
