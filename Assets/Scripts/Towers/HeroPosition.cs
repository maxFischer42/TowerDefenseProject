using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
    public int numPierceMod = 0;
    public float supportReachMod = 0f;

    public GameObject gainXpEffect;
    public GameObject levelUpEffect;

    public bool isPossessed = false;

    public TowerManager tower;

    public bool isLightningRod = false;

    public bool isSupport = false;

    public bool canPierceFromSupport = false;

    public List<HeroPosition> listOfSupports = new List<HeroPosition>();

    private Color myColor;

    public bool canAttack = true;

    // Archive of what nearby towers have recieved what support upgrades
    public Dictionary<Upgrade, List<HeroPosition>> myUpgradedTowers = new Dictionary<Upgrade, List<HeroPosition>>();

    public void Setup(HeroDefinition h)
    {
        path1 = h.upgradePath_1;
        path2 = h.upgradePath_2;
        level = 1;
        xp = 0;
        hero = h;
        mxp = h.mxp;
        sellprice = h.cost / 2;
        mxp_multiplier = h.xpMult;
        isLightningRod = h.isLightningRod;
        isSupport = h.isSupport;
        listOfSupports.Clear();
        tower = GetComponentInChildren<TowerManager>();
    }

    public bool isDisabled = false;

    public void DisableForTime(float time)
    {
        if (!tower) return;
        if (tower.enabled == false || isDisabled) return;
        myColor = GetComponentInChildren<SpriteRenderer>().color;
        tower.enabled = false;
        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        isDisabled = true;
        Invoke(nameof(ReEnableAfterTime), time);
    }

    public void ReEnableAfterTime()
    {
        isDisabled = false;
        Debug.Log("Enabling hero " + transform.GetChild(0).name);
        tower.enabled = true;
        tower.HandleIsOnDisableCooldown();
        GetComponentInChildren<SpriteRenderer>().color = myColor;
    }

    public bool HasSupport(HeroPosition p)
    {
        foreach(HeroPosition h in listOfSupports)
        {
            if(h == p)
            {
                return true;
            }
        }
        return false;
    }
    public void AddSupport(HeroPosition p)
    {
        listOfSupports.Add(p);
    }


    public void TryAddSupport(HeroPosition p)
    {
        if (!HasSupport(p) && p != this)
        {
            AddSupport(p);
        }
    }

    public void GainXP(int _xp, int elim)
    {
        //if (!isPopulated) return;
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
        if(!isSupport)
        {
            foreach(HeroPosition s in listOfSupports)
            {
                // all units supporting this unit will recieve half of the xp this unit recieved (rounded up)
                s.GainXP(Mathf.CeilToInt((float)_xp / 2), 1);
            }
        }

    }

    public void OnDeath()
    {
        if(isSupport)
        {
            GameManager.Instance.heroManager.RemoveSupportOnDeath(this);
        }
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
