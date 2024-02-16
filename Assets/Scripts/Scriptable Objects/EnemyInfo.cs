using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyInfo : ScriptableObject
{
    public int health = 3;
    public int damage = 3;
    public int damageToBase = 2;
    public float speed = 1;
    public bool isBoss = false;
    public int prize = 1;
    public int dangerLevel = 1;
    public int xp = 1;
    public int additional_damage = 3;
    public List<EnemyPassiveActions> events = new List<EnemyPassiveActions>();
    public List<GameObject> objects = new List<GameObject>();
}

public enum condition { onSpawn, onDeath, onAction, onHealth, onNearHero, onRandomTime, none}
public enum action { spawnEntities, spawnDamage, dealAreaDamage, attackLocal, attackRange, attackAll, tryPossess, disableRange, stopAndSummon, none}

[System.Serializable]
public class EnemyPassiveActions
{
    public string name = "DefaultEventName";
    public condition condition = condition.none;
    public float condition_modifier = 0f;
    public List<PassiveActionDefinition> actions = new List<PassiveActionDefinition>();
    public bool hasCooldown = false;
    public bool isCooldown = false;
    public float cooldown = 4f;
    public float currentCooldown = 0f;
    public Vector2 timeRangeFromSpawn = Vector2.zero;
    public float chance = 2f;
    public bool isOneTime = false;
}

[System.Serializable]
public class PassiveActionDefinition
{
    public string name = "DefaultActionName";
    public action action = action.none;
    public SpawnEntitiesInfo spawnEntitiesInfo;
    public List<GameObject> spawnDamageInfo = new List<GameObject>();
    public AttackLocalInfo attackLocalInfo;
    public AttackRangeInfo attackRangeInfo;
    public AttackAllInfo attackAllInfo;
    public AttackAreaInfo attackAreaInfo;
    public float timeToPossess = 5f;
    public GameObject possessedTower;
    public float radius = 2f;
    public float timeToDisable = 3f;
    public float waitingDuration = 0f; 
}

public enum spawnType { onEntity, onSpawnpoint, none}

[System.Serializable]
public class SpawnEntitiesInfo
{
    public bool isEntity = true;
    public List<GameObject> spawnEntities = new List<GameObject>();
    public spawnType spawnType = spawnType.none;
}

[System.Serializable]
public class AttackLocalInfo
{
    public int damage;
}

[System.Serializable]
public class AttackRangeInfo
{
    public GameObject projectile;
    public float min;
    public float max;
    public priority priority = priority.high;
}

[System.Serializable]
public class AttackAllInfo
{
    public int damage;
}

[System.Serializable]
public class AttackAreaInfo
{
    public int damage;
    public float radius;
}