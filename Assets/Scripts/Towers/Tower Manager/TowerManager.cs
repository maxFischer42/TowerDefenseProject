using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public int maxHP = 20;
    [HideInInspector] public int hp;

    private bool attackCooldown = false;
    public float timeBetweenAttacks = 1f;
    private float currentCooldownTime;

    public HeroDefinition myHero;

    public GameObject prefabToSpawn;

    public TowerRange range;

    public bool isBeingPossessed = false;

    public int damage;

    public bool perform = true;

    public target_priority priority = target_priority.first;

    public Image healthbar;
    public GameObject healthbarParent;

    public bool isSuper = false;

    public Image possess_fill;
    public GameObject possess_parent;

    public bool isDead = false;

    public bool isOnDisableCooldown = false;
    public Animator anim;
    public HeroPosition pos;


    public Transform subObject;

    public virtual void ApplySubObjectAction()
    {
        if (subObject == null) return;
        subObject.GetComponent<Animator>().runtimeAnimatorController = pos.subAnimMod;
    }

    public virtual void Start()
    {
        hp = maxHP;
        damage = myHero.damage;
        pos = transform.parent.GetComponent<HeroPosition>();
        range = GetComponentInChildren<TowerRange>();
        range.SetRadius(pos.rangeMod, true);
        anim = GetComponentInChildren<Animator>();
        FaceTarget();
    }

    public void HandleIsOnDisableCooldown()
    {
        Invoke(nameof(EndCooldown), 5f);
    }

    public void EndCooldown()
    {
        isOnDisableCooldown = false;
    }

    public void UpdateHealth(int _change)
    {        
        hp -= _change;
       
    }

    public virtual void ManageHealthChanges()
    {
        if(hp <= 0)
        {
            isDead = true;
            //DestroyTower();
        }
        if (hp >= maxHP)
        {
            hp = maxHP;
            healthbarParent.SetActive(false);
        } else if(hp < maxHP)
        {
            healthbarParent.SetActive(true);
        }
        healthbar.rectTransform.sizeDelta = new Vector2((float)hp / (float)maxHP, 0.1f);
    }

    public void FaceTarget()
    {
        if (range.currentTarget == null) return;
        Transform t = range.currentTarget;
        float distance = t.position.x - transform.position.x;
        if(distance > 0)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        } else if(distance < 0)
        {

            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
    }

    public void DestroyTower()
    {
        transform.parent.GetComponent<HeroPosition>().isPopulated = false;
        transform.parent.GetComponent<HeroPosition>().OnDeath();
        Destroy(transform.parent.gameObject);
    }

    void PossessTower()
    {
        transform.parent.GetComponent<HeroPosition>().isPossessed = true;
        Destroy(this.gameObject);
    }


    public virtual void Update()
    {
        if(isSuper)
        {
            SuperUpdate();
            return;
        }

        if(!isBeingPossessed && isDead)
        {
            DestroyTower();
        } else if(isBeingPossessed && isDead)
        {

        }
        ManageHealthChanges();
        if (isBeingPossessed) return;
        if (possess_parent.gameObject.activeInHierarchy) possess_parent.gameObject.SetActive(false);
        if (attackCooldown)
        {
            HandleAttackCooldown();
        } else if((!pos.isSupport && range.currentTarget != null))
        {
            //Debug.Log("Performing Hero Action");
            DoAttack();
        } else if(pos.isSupport)
        {
            HandleSupportProcess();
        }
    }

    public virtual void HandleSupportProcess()
    {

    }

    public virtual void SuperUpdate()
    {
        if (!isBeingPossessed && isDead)
        {
            DestroyTower();
        }
        else if (isBeingPossessed && isDead)
        {

        }
        ManageHealthChanges();
        if (isBeingPossessed) return;
        if (!perform) return;
        if (possess_parent.gameObject.activeInHierarchy) possess_parent.gameObject.SetActive(false);
        if (attackCooldown)
        {
            HandleAttackCooldown();
        }
        else if (range.currentTarget != null)
        {
            //Debug.Log("Performing Hero Action");
            DoAttack();
        }
    }

    public virtual void DoAttack()
    {
        FaceTarget();
        attackCooldown = true;
        currentCooldownTime = timeBetweenAttacks;
    }

    public void HandleAttackCooldown()
    {
        currentCooldownTime -= Time.deltaTime;
        if(currentCooldownTime <= 0)
        {
            attackCooldown = false;
        }
    }

    public void DoPossession()
    {
        isBeingPossessed = true;
    }

    public void EndPossession()
    {
        isBeingPossessed = false;
    }

    public virtual void OnPriorityChange()
    {

    }

}


public enum target_priority { highHP, lowHP, slowest, fastest, danger, first, last}
public enum support_priority { highHP, lowHP, closest, furthest}