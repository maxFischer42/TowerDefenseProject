using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHealingTower : TowerManager
{

    public GameObject healObjToSpawn;
    public HeroPosition heroToDefend;
    public float baseSupportReach = 3f;

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


    public override void DoAttack()
    {
        if(heroToDefend != null)
        {
            GameObject v = Instantiate(pos.spawnMod, heroToDefend.transform.position, Quaternion.identity);
            heroToDefend = GameManager.Instance.heroManager.GetHeroToDefend(transform.position, priority, baseSupportReach + pos.supportReachMod, pos);
        }
        base.DoAttack();
    }

    public override void HandleSupportProcess()
    {
        if (heroToDefend == null) return;
        if(heroToDefend.tower.hp < heroToDefend.tower.maxHP)
        {
            DoAttack();
        }
        base.HandleSupportProcess();
    }
}
