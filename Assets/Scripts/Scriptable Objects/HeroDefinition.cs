using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HeroDefinition : ScriptableObject
{
    public string name;
    public string description;
    public GameObject prefab;
    public Hero baseHeroInfo;
    public int cost = 5;
    public int damage = 1;
    public int mxp = 5;
    public float xpMult = 1.5f;
    public Sprite TowerIcon;
    public Sprite DescriptionIcon;
    public bool isSuper = false;
    public bool isLightningRod = false;
    public bool isSupport = false;

    public Upgrade upgradePath_1;
    public Upgrade upgradePath_2;
}
