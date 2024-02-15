using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroPosition : MonoBehaviour
{
    public bool isPopulated = false;
    public HeroDefinition hero;
    public int tileId;
    public int xp;
    public int mxp;
    public float mxp_multiplier = 1.5f;
    public int level = 1;
    public int elims = 0;

    public int sellprice;

    public int hp = 5;

    public Upgrade path1;
    public Upgrade path2;

    public bool lockPath1 = false;
    public bool lockPath2 = false;

    public float firerateMod;
    public float rangeMod;
    public GameObject spawnMod;
    public int damageMod;
    public bool pierceMod = false;

    public GameObject gainXpEffect;
    public GameObject levelUpEffect;

    public bool isPossessed = false;

    public TowerManager tower;

    public bool isLightningRod = false;

    public bool isSupport = false;

    public bool canPierceFromSupport = false;

    // Archive of what nearby towers have recieved what support upgrades
    public Dictionary<Upgrade, List<HeroPosition>> myUpgradedTowers = new Dictionary<Upgrade, List<HeroPosition>>();

    public void Setup(HeroDefinition h)
    {
        path1 = h.upgradePath_1;
        path2 = h.upgradePath_2;
        xp = 0;
        hero = h;
        mxp = h.mxp;
        sellprice = h.cost / 2;
        mxp_multiplier = h.xpMult;
        isLightningRod = h.isLightningRod;
        tower = GetComponentInChildren<TowerManager>();
    }

    public void GainXP(int _xp, int elim)
    {
        if (!isPopulated) return;
        xp += _xp;
        
        if (xp > mxp)
        {
            while (xp >= mxp)
            {
                xp -= mxp;
                mxp = (int)(mxp * mxp_multiplier);
                // level up
                level++;
            }
            SpawnParticles(levelUpEffect);
        } else
        {
            SpawnParticles(gainXpEffect);
        }

    }

    public void OnDeath()
    {
        foreach(Upgrade k in myUpgradedTowers.Keys)
        {
            foreach (HeroPosition h in myUpgradedTowers[k])
            {
                GameManager.Instance.HandleUpgradeModifiers(k, h, true);
            }
        }
    }

    void SpawnParticles(GameObject prefab)
    {
        GameObject g = Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(g, 2f);
    }

}
