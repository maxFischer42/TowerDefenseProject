using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeroManager : MonoBehaviour
{

    public List<HeroPosition> heroList = new List<HeroPosition>();
    public Transform positionParent;

    public List<HeroDefinition> heroes = new List<HeroDefinition>();
    public List<HeroDefinition> super = new List<HeroDefinition>();

    public void Start()
    {
        for(int i = 0; i < positionParent.childCount; i++)
        {
            heroList.Add(positionParent.GetChild(i).GetComponent<HeroPosition>());
        }
    }

    public void KillHero(int ind)
    {
        Destroy(heroList[ind].transform.GetChild(0).gameObject);
        heroList[ind].isPopulated = false;
    }

    public void DamageHero(int ind, int dmg)
    {
        heroList[ind].transform.GetChild(0).GetComponent<TowerManager>().UpdateHealth(dmg);
    }

    public void DealDamageToNear(Vector2 pos, int damageNum)
    {
        // TODO remove hardcoded "close" parameter (2f) and replace with calculation for object's size + skin width
        intBool heroNear = getNearbyHero(pos, 2f);
        Debug.Log("Attempting to damage " + heroNear._int + "   ::  " + heroNear._bool);
        if (heroNear._bool == true)
        {
            DamageHero(heroNear._int, damageNum);
        }
    }

    public void DealDamageToAll(int damageNum)
    {
        foreach(var hero in heroList)
        {
            DamageHero(hero.tileId, damageNum);
        }
    }

    public void DealDamageToArea(Vector2 pos, int damageNum, float radius)
    {
        foreach(var hero in heroList)
        {
            float len = (pos - (Vector2)hero.transform.position).magnitude;
            if (len <= radius) {
                DamageHero(hero.tileId, damageNum);
            }
        }
    }

    public intBool priorityHeroWithinBounds(Vector2 pos, float min, float max, priority priority)
    {
        intBool isHeroInRange = new intBool();
        Dictionary<int, float> heroDistances = new Dictionary<int, float>();
        foreach(var hero in heroList)
        {
            if (!hero.isPopulated) continue;
            float magn = (pos - (Vector2)hero.transform.position).magnitude;
            if(magn >= min && magn <= max) { heroDistances.Add(hero.tileId, magn); }
        }

        int ind = priority == priority.high ? 999999 : -1;
        foreach(var candidate in heroDistances)
        {
            if (priority == priority.high)
            {
                if (candidate.Value > ind) ind = candidate.Key;
            } else if(priority == priority.low)
            {
                if (candidate.Value < ind) ind = candidate.Key;
            }
        }

        isHeroInRange._bool = ind == -1 ? false : true;
        isHeroInRange._int = ind;
        return isHeroInRange;
    }

    public intBool getNearbyHero(Vector2 position, float radius)
    {
        intBool hero = new intBool();
        hero._bool = false;
        // return the index of the current hero
        float range = 99999f;
        foreach(var h in heroList)
        {
            if (!h.isPopulated) continue;
            float dis = (position - (Vector2)h.transform.position).magnitude;
            if(dis <= radius)
            {
                print(dis + "  " + radius);
                range = radius;
                hero._bool = h.isPopulated;
                hero._int = h.tileId;
            }
        }
        return hero;
    }

}

public enum priority { high, low }

[System.Serializable]
public class Hero
{
    public int hp;
    public int mhp;
    public int index;
}