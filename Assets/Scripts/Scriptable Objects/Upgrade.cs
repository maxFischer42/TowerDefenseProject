using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public bool isLockout = false;
    public List<HeroModify> modifiers = new List<HeroModify>();
    public string name;
    public string blurb;
    public int cost;
    public bool isFinal;
    public Upgrade descendant;
    public int levelRequirement = 0;
    public Sprite upgradeIcon;
    public bool fieldUpgrade = false;
    public float radiusForUpgrade = 2f;
}

[System.Serializable]
public class HeroModify
{
    public int type = 0;
    public float fireRate = 0.0f;
    public float range = 0.0f;
    public HeroDefinition newHero;
    public bool newHeroHasPrefab;
    public GameObject newSpawn;
    public GameObject newPrefab;
    public int dmg = 0;
    public RuntimeAnimatorController newSubAnimation;
    public int pierceNum = 0;
    public int blessedBladeCount = 0;
    public Sprite newWeaponSprite;
    public Vector2 weaponSpriteOffset;
}