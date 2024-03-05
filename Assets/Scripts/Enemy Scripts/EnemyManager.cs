using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public EnemyInfo enemy = null;
    private EnemyInfo myEnemy = null;

    public bool hasSpawned = false;
    public bool hasDied = false;

    private int maxHealth;
    private int health;

    private Vector2 spawnpoint;

    public GameObject healthCanvas;
    private Image healthBar;

    private List<bool> cooldowns = new List<bool>();
    private List<float> cooldown_lengths = new List<float>();

    private HeroPosition lastHurtBy = null;
    private List<HeroPosition> hurtList = new List<HeroPosition>();

    public bool isTryPossess = false;
    public float possessProgress = 0.0f;
    private float possess_length;
    public HeroPosition heroToPossess;
    private GameObject objectToReplace;

    public bool hasMinions = false;
    public List<GameObject> minions = new List<GameObject>();
    private List<HeroPosition> minionPositions = new List<HeroPosition>();

    private List<HeroPosition> disabledHeroes = new List<HeroPosition>();

    private Image possess_fill_temp;

    private float timeSinceBirth = 0.0f;

    private List<bool> hasDoneEvent = new List<bool>();
    private SpawnEntitiesInfo tempSpawnList;

    private float tempPossessInfo;

    public bool enemyIsHidden = false;

    private bool lockDeathEvent = false;

    void Start()
    {
        if(enemy == null)
        {
            Debug.LogError("Enemy's definition is NULL");
            Destroy(gameObject);
            return;
        }
        myEnemy = Instantiate(enemy);
        health = maxHealth = myEnemy.health;

        for (int i = 0; i <  myEnemy.events.Count; i++)
        {
            cooldowns.Add(false);
            cooldown_lengths.Add(0f);
            hasDoneEvent.Add(false);
        }
        healthBar = healthCanvas.transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    public void HideEnemy()
    {
        enemyIsHidden = false;
    }

    public void RevealEnemy()
    {
        enemyIsHidden = true;
    }

    public EnemyInfo getEnemy()
    {
        return myEnemy;
    }

    public int GetHealth()
    {
        return health;
    }

    private void Update()
    {
        if(enemyIsHidden)
        {
            GetComponent<Animator>().enabled = false;
        } else
        {
            GetComponent<Animator>().enabled = true;
        }
        timeSinceBirth += Time.deltaTime;
        HandleDeath();
        HandleHealth();        
        if(isTryPossess)
        {
            PossessUpdate();
            return;
        }
        HandleEvents();
    }

    void PossessUpdate()
    {
        if (!GetComponent<Pather>().arrivedAtPossessed) return;
        if(heroToPossess.isPossessed)
        {
            isTryPossess = false;
            return;
        }
        if (!heroToPossess.isPossessed && heroToPossess && heroToPossess.transform.GetChild(0) && !heroToPossess.transform.GetChild(0).GetComponent<TowerManager>().possess_parent.activeInHierarchy)
        {
            heroToPossess.transform.GetChild(0).GetComponent<TowerManager>().possess_parent.SetActive(true);
        }
        heroToPossess.tower.isBeingPossessed = true;
        possess_fill_temp.transform.parent.gameObject.SetActive(true);
        possessProgress -= Time.deltaTime;
        possess_fill_temp.fillAmount = 1 - (possessProgress / possess_length);
        if(possessProgress <= 0.0f)
        {
            if(heroToPossess.tower.isDead)
            {
                hasMinions = true;
                Debug.Log("HERO HAS BECOME POSSESSED");
                Destroy(heroToPossess.transform.GetChild(0).gameObject);
                // Get random object from enemy to spawn
                int ind = Random.Range(0, myEnemy.objects.Count);
                GameObject m = Instantiate(myEnemy.objects[ind], heroToPossess.transform);
                isTryPossess = false;
                heroToPossess.isPossessed = true;
                minions.Add(m);
                minionPositions.Add(heroToPossess);
                heroToPossess = null;
            } else
            {
                heroToPossess.tower.UpdateHealth(myEnemy.additional_damage);
                possessProgress = tempPossessInfo;
                possess_fill_temp.fillAmount = 1f;
            }
            
        }   
    }

    public void HandleDeathPossession()
    {
        foreach(GameObject m in minions)
        {
            Destroy(m);
        }

        foreach(HeroPosition h in minionPositions)
        {
            h.isPossessed = false;
            h.isPopulated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PLAYER_PROJECTILE")
        {
            Damage(collision.GetComponent<ProjectileInfo>());
        }
    }

    private void Damage(ProjectileInfo proj)
    {
        health -= proj.damage;
        lastHurtBy = proj.origin;
        bool exists = false;
        foreach(HeroPosition p in hurtList)
        {
            if(p == lastHurtBy)
            {
                exists = true;
                break;
            }
        }
        if(!exists)
        {
            hurtList.Add(lastHurtBy);
        }
    }
    private void HandleHealth()
    {
        if (health <= 0)
        {
            hasDied = true;
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
            healthCanvas.SetActive(false);
        }
        else if(health < maxHealth)
        {
            healthCanvas.SetActive(true);
            float recalc = (float)health / (float)maxHealth;
            healthBar.rectTransform.sizeDelta = new Vector2(recalc, 0.1f);
        } 
    }
    private void HandleDeath()
    {
        if(hasDied) {
            GameManager.Instance.IncreaseCurrency(enemy.prize);            
            foreach(HeroPosition p in hurtList)
            {
                if (p == null)
                {
                    Debug.Log("A hero who hurt enemy \"" + this.name + "\" no longer exists.");
                    continue;
                }
                if(p == lastHurtBy)
                {
                    lastHurtBy.GainXP(myEnemy.xp * 2, 1);
                } else
                {
                    p.GainXP(myEnemy.xp, 1);
                }
            }
            HandleDeathPossession();
            GameObject.FindFirstObjectByType<EntitySpawner>().decrease(1);
            Destroy(gameObject);
        }
    }

    #region event handling

    void HandleEvents()
    {
        int index = 0;
        foreach(EnemyPassiveActions _event in myEnemy.events)
        {
            //Debug.Log("Handling Event " + _event.name);
            if (_event.hasCooldown && cooldowns[index])
            {
                cooldown_lengths[index] -= Time.deltaTime;
                if (cooldown_lengths[index] < 0)
                {
                    cooldowns[index] = false;
                }
                continue;
            }
            // First get event type
            switch (_event.condition)
            {
                case condition.none:
                    continue;
                case condition.onSpawn:
                    HandleEvent_OnSpawn(_event);
                    break;
                case condition.onDeath:
                    HandleEvent_OnDeath(_event);
                    break;
                case condition.onAction:
                    HandleEvent_OnAction(_event);
                    break;
                case condition.onHealth:
                    HandleEvent_OnHealth(_event);
                    break;
                case condition.onNearHero:
                    HandleEvent_OnNearHero(_event);
                    break;
                case condition.onRandomTime:
                    HandleEvent_OnRandomTime(_event, index);
                    break;
                default:
                    break;
            }

            if(_event.hasCooldown)
            {
                cooldowns[index] = true;
                cooldown_lengths[index] = _event.cooldown;
            }

            index++;
        }
        if(!hasSpawned)
        {
            hasSpawned = true;
        }
    }

    void HandleEvent_OnSpawn(EnemyPassiveActions _event)
    {
        if(hasSpawned == true)
        {
            return;
        }
        // Perform actions here
        HandleAction(_event);
    }

    void HandleEvent_OnDeath(EnemyPassiveActions _event)
    {
        if(!hasDied)
        {
            return;
        }
        if(lockDeathEvent)
        {
            return;
        }
        lockDeathEvent = true;
        Debug.Log("DeathEventProcessing...");
        // Perform actions here
        HandleAction(_event);
    }

    void HandleEvent_OnAction(EnemyPassiveActions _event)
    {

    }

    void HandleEvent_OnHealth(EnemyPassiveActions _event)
    {
        // compare health values
        if(health <= _event.condition_modifier)
        {
            // Perform actions here
        }
    }

    void HandleEvent_OnNearHero(EnemyPassiveActions _event)
    {
        intBool nearbyHero = GameManager.Instance.heroManager.getNearbyHero(transform.position, _event.condition_modifier);
        Debug.Log("NerbyheroResult: " + nearbyHero._int + "   :   " + nearbyHero._bool);
        if(nearbyHero._bool == true)
        {
            Debug.Log("OnNearHero");
           
            // Perform actions here
            HandleAction(_event);            
        }
    }

    void HandleEvent_OnRandomTime(EnemyPassiveActions _event, int index)
    {
        //Check if one time event; if already happened continue
        //If not, proceed and mark event
        if (hasDoneEvent[index] == true && _event.isOneTime)
        {
            return;
        }
        

        float min = _event.timeRangeFromSpawn.x;
        float max = _event.timeRangeFromSpawn.y;
        if(timeSinceBirth > min && timeSinceBirth < max)
        {
            float rnd = Random.Range(0, 100);
            if(rnd < _event.chance)
            {
                if (_event.isOneTime)
                {
                    hasDoneEvent[index] = true;
                }
                Debug.Log("OnRandomTime event has succeeded!");
                HandleAction(_event);
            }
        }
    }
    #endregion

    #region action handling

    void HandleAction(EnemyPassiveActions _event)
    {

        Debug.Log("Event \"" + _event.name + "\" has succeeded, handling actions...");
        foreach(PassiveActionDefinition _action in _event.actions)
        {
            switch(_action.action)
            {
                case action.none:
                    continue;
                case action.spawnEntities:
                    HandleAction_SpawnEntities(_action);
                    break;
                case action.attackLocal:
                    HandleAction_AttackLocal(_action);
                    break;
                case action.attackAll:
                    HandleAction_AttackAll(_action);
                    break;
                case action.dealAreaDamage:
                    HandleAction_DealAreaDamage(_action);
                    break;
                case action.spawnDamage:
                    HandleAction_SpawnDamage(_action);
                    break;
                case action.attackRange:
                    HandleAction_AttackRange(_action);
                    break;
                case action.tryPossess:
                    HandleAction_TryPossess(_action);
                    break;
                case action.disableRange:
                    HandleAction_DisableRange(_action);
                    break;
                case action.stopAndSummon:
                    HandleAction_StopAndSummon(_action);
                    break;
                default:
                    break;
            }
        }
    }

    void HandleAction_SpawnEntities(PassiveActionDefinition _action)
    {
        //print("handling spawnentity action");
        SpawnEntitiesInfo info = _action.spawnEntitiesInfo;
        if(info == null)
        {
            Debug.LogError("No SpawnEntitiesInfo object was delivered!");
            return;
        }
        HandleSpawnEntities(info);

    }

    void HandleAction_AttackLocal(PassiveActionDefinition _action)
    {
        Debug.Log("Attacking nearby heroes...");    
        GameManager.Instance.heroManager.DealDamageToNear(transform.position, _action.attackLocalInfo.damage);
    }

    void HandleAction_AttackAll(PassiveActionDefinition _action)
    {
        GameManager.Instance.heroManager.DealDamageToAll(_action.attackAllInfo.damage);
    }

    void HandleAction_DealAreaDamage(PassiveActionDefinition _action)
    {
        GameManager.Instance.heroManager.DealDamageToArea(transform.position, _action.attackAreaInfo.damage, _action.attackAreaInfo.radius);
    }

    void HandleAction_SpawnDamage(PassiveActionDefinition _action)
    {
        foreach(GameObject obj in _action.spawnDamageInfo)
        {
            GameObject ob = Instantiate(obj, transform.position, Quaternion.identity);
            ob.transform.parent = null;
        }
    }

    void HandleAction_AttackRange(PassiveActionDefinition _action)
    {
        intBool heroInRange = GameManager.Instance.heroManager.priorityHeroWithinBounds(transform.position, _action.attackRangeInfo.min, _action.attackRangeInfo.max, _action.attackRangeInfo.priority);
        // TODO
        // get object we are targeting
        // get direction to object
        // Spawn object
        // apply velocity and damage to projectile
    }

    void HandleAction_TryPossess(PassiveActionDefinition _action)
    {
        GetComponent<Pather>().arrivedAtPossessed = false;
        intBool hero = GameManager.Instance.heroManager.getNearbyHero(transform.position, _action.radius);
        //if (hero._bool == false) return;
        if (GameManager.Instance.heroManager.heroList[hero._int].isPossessed) return;
        if (GameManager.Instance.heroManager.heroList[hero._int].transform.childCount == 0) return; // somehow passed the first check, recheck with transform children
        isTryPossess = true;
        possessProgress = _action.timeToPossess;
        possess_length = possessProgress;
        tempPossessInfo = possessProgress;
        objectToReplace = _action.possessedTower;
        heroToPossess = GameManager.Instance.heroManager.heroList[hero._int];
        possess_fill_temp = heroToPossess.transform.GetChild(0).GetComponent<TowerManager>().possess_fill;
    }

    void HandleAction_DisableRange(PassiveActionDefinition _action)
    {
        Vector2 startPos = transform.position;
        disabledHeroes.Clear();
        foreach(HeroPosition h in GameManager.Instance.heroManager.heroList)
        {
            if (h.isPossessed || !h.isPopulated) continue;
            Vector2 heroPos = h.transform.position;
            Vector2 distance = startPos - heroPos;
            float magnitude = distance.magnitude;
            if(_action.radius >= magnitude)
            {
                disabledHeroes.Add(h);
            }
        }
        foreach(HeroPosition d in disabledHeroes)
        {
            GameManager.Instance.SetupReenable(d, _action.timeToDisable);
        }        
    }

    void HandleAction_StopAndSummon(PassiveActionDefinition _action)
    {
        GetComponent<Pather>().enabled = false;
        tempSpawnList = _action.spawnEntitiesInfo;
        Invoke(nameof(SpawnEntityOnTimer), _action.waitingDuration);
    }

    

    #endregion



    public void SpawnEntityOnTimer()
    {
        GetComponent<Pather>().enabled = true;
        HandleSpawnEntities(tempSpawnList);
    }

    public void HandleSpawnEntities(SpawnEntitiesInfo info)
    {
        spawnType spawnType = info.spawnType;
        Vector2 spawnPos = Vector2.zero;
        switch (spawnType)
        {
            case spawnType.none:
                break;
            case spawnType.onSpawnpoint:
                spawnPos = spawnpoint;
                break;
            case spawnType.onEntity:
                //print("foo");
                spawnPos = transform.position;
                break;
            default:
                break;
        }
        float offsetX = 0;
        float offsetY = 0;
        float mult = 0f;
        Vector2 directionToSpawn = Vector2.zero;
        Debug.Log("Started Spawn Entities loop");
        foreach (GameObject child in info.spawnEntities)
        {
            //Debug.Log("Spawned child of Enemy on Action");
            spawnPos += new Vector2(offsetX * (mult), offsetY * (mult));
            mult += 0.2f;
            GameObject newObj = Instantiate(child, spawnPos, Quaternion.identity);
            newObj.transform.parent = null;
            if (info.isEntity) GameObject.FindFirstObjectByType<EntitySpawner>().AddEntity(1);
            if (info.isEntity) newObj.GetComponent<Pather>().SetPather(GameObject.FindObjectOfType<PathwayDisplay>(), GetComponent<Pather>().getTargetIndex());
            if (directionToSpawn == Vector2.zero && info.isEntity)
            {
                directionToSpawn = (spawnPos - newObj.GetComponent<Pather>().getTarget()).normalized;
                offsetX = directionToSpawn.x;
                offsetY = directionToSpawn.y;
            }
        }
        Debug.Log("Ended Spawn Entities loop");
    }

}

