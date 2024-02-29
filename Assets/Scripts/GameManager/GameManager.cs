using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private GameManager() {
        instance = this;
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    

    public HeroManager heroManager;

    private void Start()
    {
        if (heroManager == null) { heroManager = FindObjectOfType<HeroManager>(); }
        UpdateHealth();
        PopulateUnitMenuWithUnlocks();
    }

    private void Update()
    {
  
    }

    public bool managerIsOpen = false;
    public GameObject unitManagerObject;
    public Animator unitMenuAnimator;
    public Animator heroMenuAnimator;

    public GameObject unitStatObject;
    public Animator unitStatAnimator;
    public GameObject unitStatClose;
    public Image unitStatDescriptionImage;

    public GameObject unitPlaceObject;

    public GameObject unitBuyPrefab;
    public RectTransform panelParent;

    
    public TextMeshProUGUI currency_display;
    public TextMeshProUGUI health_display;

    public TextMeshProUGUI unit_name;
    public TextMeshProUGUI unit_level;
    public TextMeshProUGUI unit_experience;

    public int currency = 10;
    public int health = 100;

    public GameObject unitMenuButtonTemplate;
    public Scrollbar scrollObject;
    public RectTransform unitMenuRect;
    public RectTransform heroMenuRect;
    public bool[] unlockedTowers = { true, true };
    public bool[] unlockedHeroes = { true };

    public GameObject unitMenuObj;
    public GameObject heroMenuObj;

    private int currentWave;
    private int finalWave;
    public TextMeshProUGUI wave_display;

    public int prizePerWave = 30;

    private bool isSuper = false;

    private int currentMenu = 0; // 0 == none, 1 == tower, 2 == hero

    public void PopulateUnitMenuWithUnlocks()
    {
        for(int i = 0; i < unlockedTowers.Length; i++)
        {
            if (unlockedTowers[i] == false) continue;
            GameObject u = Instantiate(unitMenuButtonTemplate, unitMenuRect);
            u.GetComponent<VerifyPurchase>()._id = i;
            HeroDefinition current = heroManager.heroes[i];

            u.GetComponent<PopulateButtonInfo>().Populate(current.name, current.cost, current.TowerIcon);
        }
        float newSize = 275 * (float)unlockedTowers.Length;
        unitMenuRect.sizeDelta = new Vector2(unitMenuRect.sizeDelta.x, newSize);

        for(int i = 0; i < unlockedHeroes.Length; i++)
        {
            if (unlockedHeroes[i] == false) continue;
            GameObject u = Instantiate(unitMenuButtonTemplate, heroMenuRect);
            u.GetComponent<VerifyPurchase>()._id = i;
            u.GetComponent<VerifyPurchase>().isHero = true;
            HeroDefinition current = heroManager.super[i];
            u.GetComponent<PopulateButtonInfo>().Populate(current.name, current.cost, current.TowerIcon);
        }
        newSize = 275 * (float)unlockedHeroes.Length;
        heroMenuRect.sizeDelta = new Vector2(heroMenuRect.sizeDelta.x, newSize);

    }

    // deprecated, TODO remove
    public void OnMenuBackdropEnter()
    {
        if(!managerIsOpen)
        {
            OpenTowerMenu();
        }
        managerIsOpen = true;
       
    }

    public void DamageBase(int dmg)
    {
        health -= dmg;
        UpdateHealth();
        if(health <= 0)
        {
            // TODO, remove temp lose state
            Time.timeScale = 0;
        }
    }

    void UpdateHealth()
    {
        health_display.text = health.ToString();
    }


    public void SetupWaves(int final)
    {
        finalWave = final;
    }

    public void IncreaseWave()
    {
        if(currentWave >= finalWave)
        {
            Debug.Log("NO MORE WAVES");
            return;
        }
        currentWave++;
        currency += prizePerWave;
        UpdateCurrencyDisplay();
        if (currentWave == finalWave)
        {
            wave_display.text = "FINAL WAVE";
        }
        else
        {
            wave_display.text = "Wave " + currentWave + "/" + finalWave;
        }
    }
    public void OnUnitMenuToggle()
    {
        isSuper = false;
        if (!managerIsOpen)
        {
            OpenTowerMenu();
        }
        else if (managerIsOpen && !unitMenuObj.gameObject.activeInHierarchy)
        {
            ToggleTowerMenu(true);
        }
        else
        {
            CloseTowerMenu();
        }
    }

    public void OnHeroMenuToggle()
    {
        isSuper = true;
        if (!managerIsOpen)
        {
            OpenHeroMenu();
        }
        else if (managerIsOpen && !heroMenuObj.gameObject.activeInHierarchy)
        {
            ToggleTowerMenu(false);
        }
        else
        {
            CloseTowerMenu();
        }
    }

    public void OpenTowerMenu()
    {
        unitMenuAnimator.SetBool("isOpen", true);
        unitMenuObj.gameObject.SetActive(true);
        heroMenuObj.gameObject.SetActive(false);
        managerIsOpen = true;
    }

    public void CloseTowerMenu()
    {
        unitMenuAnimator.SetBool("isOpen", false);
        unitMenuObj.gameObject.SetActive(false); 
        managerIsOpen = false;
    }

    public void ToggleTowerMenu(bool isTower)
    {
        unitMenuObj.SetActive(isTower);
        heroMenuObj.SetActive(!isTower);
    }

    public void OpenHeroMenu()
    {
        unitMenuAnimator.SetBool("isOpen", true);
        heroMenuObj.gameObject.SetActive(true);
        unitMenuObj.gameObject.SetActive(false);
        managerIsOpen = true;
    }

    public void CloseHeroMenu()
    {
        unitMenuAnimator.SetBool("isOpen", false);
        heroMenuObj.gameObject.SetActive(false);
        managerIsOpen = false;
    }

    public void FullCloseMenu()
    {
        managerIsOpen = false;
        unitMenuAnimator.SetBool("isOpen", false);
        heroMenuObj.gameObject.SetActive(false);
        unitMenuObj.gameObject.SetActive(false);
    }

    public void PlaceUnitStart()
    {
        unitPlaceObject.SetActive(true);
        unitManagerObject.SetActive(false);
        unitStatObject.SetActive(false);
    }

    public void PlaceUnitEnd()
    {
        unitPlaceObject.SetActive(false);
        unitManagerObject.SetActive(true);
        unitStatObject.SetActive(true);
        FullCloseMenu();
    }


    public bool SelectUnitMenuOpen = false;
    private HeroPosition temporaryHeroDetail;

    public void SelectUnitOpen(HeroPosition h, bool isSuper)
    {
        unitStatClose.SetActive(true);
        unitStatAnimator.SetBool("isOpen", true);
        unitManagerObject.SetActive(false);
        SelectUnitMenuOpen = true;
        //print("test:: " + tileId);
        PopulateUnitDetailMenu(h);        
        
    }

    void PopulateUnitDetailMenu(HeroPosition h)
    {
        temporaryHeroDetail = h;
        Upgrade up1 = h.path1;
        Upgrade up2 = h.path2;
        UpdateUnitMenuDisplay();
        UpdateUpgradeDisplay();
        ChangeHeroPriority(0);
    }

    public void SelectUnitClose()
    {
        unitStatClose.SetActive(false);
        unitStatAnimator.SetBool("isOpen", false);
        unitManagerObject.SetActive(true);
        SelectUnitMenuOpen = false;
        temporaryHeroDetail = null;
    }

    public void HandleUpgradeButton(int path)
    {

        Debug.Log("Upgrade Handling...");

        //first handle the path
        float cost = Mathf.Infinity;
        if (path == 0 && temporaryHeroDetail.path1.levelRequirement > temporaryHeroDetail.level) return;
        if (path == 1 && temporaryHeroDetail.path2.levelRequirement > temporaryHeroDetail.level) return;
        if (!temporaryHeroDetail.lockPath1 && path == 0)
        {
            cost = temporaryHeroDetail.path1.cost;
        } else if(!temporaryHeroDetail.lockPath2 && path == 1)
        {
            cost = temporaryHeroDetail.path2.cost;
        }

        
        //check if it can be afforded
        if (currency <= cost)
        {
            Debug.Log("Upgrade too expensive | " + cost + " > " + currency);           
            return;
        }
        Upgrade u = null;
        //if it can be afforded, apply it to the heroposition object
        if(path == 0)
        {
            u = temporaryHeroDetail.path1;
            temporaryHeroDetail.path1 = u.descendant;     
            if(temporaryHeroDetail.path1 == null)
            {
                temporaryHeroDetail.lockPath1 = true;
            }
        } else if (path == 1)
        {
            u = temporaryHeroDetail.path2;
            temporaryHeroDetail.path2 = u.descendant;
            if (temporaryHeroDetail.path2 == null)
            {
                temporaryHeroDetail.lockPath2 = true;
            }
        }
        if (u == null)
        {
            Debug.LogError("No Upgrade defined.");
            return;
        }
        currency -= (int)cost;
        UpdateCurrencyDisplay();
        UpdateUpgradeDisplay();

        HeroPosition temp;
        temp = temporaryHeroDetail;
        //change the hero position values to use the new modified values
        

        // If this is a normal hero, only change it's values
        HandleUpgradeModifiers(u, temp, false);

        // If this is a SUPPORT upgrade, change the values of all the heroes in range.
        if(u.fieldUpgrade)
        {
            // Get all nearby valid heroes (heroes with a range <= u.radiusForUpgrades)
            List<HeroPosition> p = new List<HeroPosition>();
            Vector2 origin = temp.transform.position;
            List<HeroPosition> supportedHeroes = new List<HeroPosition>();
            foreach(HeroPosition pos in heroManager.heroList)
            {
                if (!pos.isPopulated) continue;
                Vector2 position = pos.transform.position;
                float distance = (position - origin).magnitude;
                if(distance <= u.radiusForUpgrade)
                {
                    Debug.Log("Applying Support Upgrade to " + pos);
                    HandleUpgradeModifiers(u, pos, false);
                    temp.isSupport = true;
                    supportedHeroes.Add(pos);
                }
            }
            temp.myUpgradedTowers.Add(u, supportedHeroes);
        }

        // TODO Update graphics
    }

    public void HandleUpgradeModifiers(Upgrade u, HeroPosition hero, bool isRevert)
    {
        foreach (HeroModify mod in u.modifiers)
        {
            switch (mod.type)
            {
                case 0:     // Increase Fire Rate
                    if (isRevert)
                    {
                        hero.GetComponentInChildren<TowerManager>().timeBetweenAttacks += mod.fireRate;
                        hero.firerateMod += mod.fireRate;
                    }
                    else
                    {
                        hero.GetComponentInChildren<TowerManager>().timeBetweenAttacks -= mod.fireRate;
                        hero.firerateMod -= mod.fireRate;
                        // check that firerate is now <= 0
                        if (hero.GetComponentInChildren<TowerManager>().timeBetweenAttacks < 0.05f)
                        {
                            hero.GetComponentInChildren<TowerManager>().timeBetweenAttacks = 0.05f;
                        }
                    }
                    break;
                case 1:     // Increase Range
                    if (isRevert)
                    {
                        hero.GetComponentInChildren<TowerManager>().GetComponentInChildren<TowerRange>().GetComponent<CircleCollider2D>().radius -= mod.range;
                        hero.rangeMod -= mod.range;
                    }
                    else
                    {
                        hero.GetComponentInChildren<TowerManager>().GetComponentInChildren<TowerRange>().GetComponent<CircleCollider2D>().radius += mod.range;
                        hero.rangeMod += mod.range;
                    }
                    break;
                case 2:     // Replace Hero Object
                    // replace hero object;
                    Destroy(hero.GetComponentInChildren<TowerManager>().gameObject);
                    GameObject newObj = Instantiate(mod.newSpawn, hero.transform);
                    newObj.GetComponent<TowerManager>().GetComponent<CircleCollider2D>().radius += hero.rangeMod;
                    newObj.GetComponent<TowerManager>().timeBetweenAttacks -= hero.firerateMod;
                    newObj.GetComponent<TowerManager>().damage += hero.damageMod;
                    if (newObj.GetComponent<TowerManager>().timeBetweenAttacks < 0.05f)
                    {
                        newObj.GetComponent<TowerManager>().timeBetweenAttacks = 0.05f;
                    }
                    if (!mod.newHeroHasPrefab)
                    {
                        newObj.GetComponent<TowerManager>().prefabToSpawn = hero.spawnMod;
                    }

                    break;
                case 3:     // Change Tower's Spawn
                    // check for each tower case
                    hero.GetComponentInChildren<TowerManager>().prefabToSpawn = mod.newSpawn;
                    break;
                case 4:     // Change Tower's Damage
                    if (isRevert)
                    {
                        hero.damageMod -= mod.dmg;
                        hero.GetComponentInChildren<TowerManager>().damage -= mod.dmg;
                    }
                    else
                    {
                        hero.damageMod += mod.dmg;
                        hero.GetComponentInChildren<TowerManager>().damage += mod.dmg;
                    }
                    break;
                case 5:     // Change if tower can pierce
                    hero.pierceMod = true;
                    break;
                case 6:     // Change if tower can see from support
                    if (isRevert && hero.GetComponentInChildren<TowerRange>().canSeeFromSupport)
                    {
                        hero.GetComponentInChildren<TowerRange>().canSeeFromSupport = false;
                        hero.GetComponentInChildren<TowerRange>().canSeeHidden = false;
                    } else if (hero.GetComponentInChildren<TowerRange>().canSeeHidden)
                    {
                        hero.GetComponentInChildren<TowerRange>().canSeeFromSupport = true;
                        hero.GetComponentInChildren<TowerRange>().canSeeHidden = true;
                    }
                    break;
                case 7:     // Change if tower can see hidden themselves
                    hero.GetComponentInChildren<TowerRange>().canSeeHidden = true;
                    break;
                case 8:     // change if tower can pierce through support
                    if (isRevert && hero.canPierceFromSupport)
                    {
                        hero.canPierceFromSupport = false;
                        hero.pierceMod = false;
                    }
                    else if(!hero.pierceMod)
                    {
                        hero.canPierceFromSupport = true;
                        hero.pierceMod = true;
                    }
                    break;
            }
        }
    }


    public void RevertUpgrade(HeroPosition hero, Upgrade u)
    {

    }

    private int pendingPurchase = 0;
    private List<GameObject> purchaseObjects = new List<GameObject>();
    public TextMeshProUGUI[] upgradeNames = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] upgradePrices = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] upgradeDescriptions = new TextMeshProUGUI[2];
    public TextMeshProUGUI priority_text;
    public Image[] upgradeIcons = new Image[2];
    public Sprite lockImage;

    public GameObject healingFieldPlacement;
    public GameObject healingFieldObject;

    private List<HeroPosition> tempPositionsForDisable = new List<HeroPosition>();

    public void VerifyPurchase(int id) 
    {
        // Check if the item can be bought
        int cost = heroManager.heroes[id].cost;
        if (CanAfford(cost))
        {
            pendingPurchase = id;
            PopulatePurchaseMenu();
        } else
        {
            // TODO do some effect to show its too expensive
        }

    }

    public void SetupReenable(List<HeroPosition> disabled, float timeToEnable)
    {
        tempPositionsForDisable = disabled;
        Debug.Log("Temp Heroes Disasbled Length = " + tempPositionsForDisable.Count);
        Debug.Log("Invoking ResetDisabledHeroes in GameManager");
        Invoke(nameof(ResetDisabledHeroes), timeToEnable);
    }

    void ResetDisabledHeroes()
    {
        Debug.Log("Invoke succeeded for ResetDisabledHeroes");
        foreach (HeroPosition d in tempPositionsForDisable)
        {
            Debug.Log("Enabling hero " + d.transform.GetChild(0).name);
            d.GetComponentInChildren<TowerManager>().enabled = true;
            d.GetComponentInChildren<TowerManager>().HandleIsOnDisableCooldown();
            d.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    public void HealingFieldPlace()
    {
        // get cursor position and spawn healing field
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(healingFieldObject, worldPosition, Quaternion.identity);

        HealingFieldPlacementEnd();
    }

    public void PurchaseHealingField()
    {
        int cost = 10;
        if(CanAfford(cost))
        {
            currency -= cost;
            HealingFieldPlacementStart();
        }
    }

    void HealingFieldPlacementStart()
    {
        healingFieldPlacement.SetActive(true);
        unitPlaceObject.SetActive(false);
        unitManagerObject.SetActive(false);
        unitStatObject.SetActive(false);
    }

    void HealingFieldPlacementEnd()
    {
        healingFieldPlacement.SetActive(false);
        unitPlaceObject.SetActive(false);
        unitManagerObject.SetActive(true);
        unitStatObject.SetActive(true);
    }

    public void UpdateCurrencyDisplay()
    {
        currency_display.text = "$" + currency;
    }

    public void UpdateUnitMenuDisplay()
    {
        unit_name.text = temporaryHeroDetail.hero.name;
        unit_level.text = temporaryHeroDetail.level.ToString();
        unit_experience.text = temporaryHeroDetail.xp.ToString() + "/" + temporaryHeroDetail.mxp.ToString();
        unitStatDescriptionImage.sprite = temporaryHeroDetail.hero.DescriptionIcon;
    }

    public void UpdateUpgradeDisplay()
    {

        if (temporaryHeroDetail.lockPath1)
        {
            upgradeNames[0].text = "Path Complete";
            upgradePrices[0].text = "";
            upgradeDescriptions[0].text = "";
        }
        else if(temporaryHeroDetail.path1.levelRequirement > temporaryHeroDetail.level)
        {
            upgradeNames[0].text = "Requires Level " + temporaryHeroDetail.path1.levelRequirement;
            upgradePrices[0].text = "";
            upgradeDescriptions[0].text = "";
            upgradeIcons[0].sprite = lockImage;
        } /* else if( TODO, if this upgrade has been unlocked in the skill tree) */
        else if (temporaryHeroDetail.path1 == null)
        {

        }
        else
        {
            upgradeNames[0].text = temporaryHeroDetail.path1.name;
            upgradePrices[0].text = "$" + temporaryHeroDetail.path1.cost;
            upgradeDescriptions[0].text = temporaryHeroDetail.path1.blurb;
            upgradeIcons[0].sprite = temporaryHeroDetail.path1.upgradeIcon;
        }

        if (temporaryHeroDetail.lockPath2)
        {
            upgradeNames[1].text = "Path Complete";
            upgradePrices[1].text = "";
            upgradeDescriptions[1].text = "";
        }
        else if(temporaryHeroDetail.path2 == null)
        {

        }
        else if (temporaryHeroDetail.path2.levelRequirement > temporaryHeroDetail.level)
        {
            upgradeNames[1].text = "Requires Level " + temporaryHeroDetail.path2.levelRequirement;
            upgradePrices[1].text = "";
            upgradeDescriptions[1].text = "";
            upgradeIcons[1].sprite = lockImage;
        }        
        else
        {
            upgradeNames[1].text = temporaryHeroDetail.path2.name;
            upgradePrices[1].text = "$" + temporaryHeroDetail.path2.cost;
            upgradeDescriptions[1].text = temporaryHeroDetail.path2.blurb;
            upgradeIcons[1].sprite = temporaryHeroDetail.path2.upgradeIcon;
        }

    }

    public void ChangeHeroPriority(int direction)
    {
        if(temporaryHeroDetail.isSupport)
        {
            ChangeSupportHeroPriority(direction);
            return;
        }
        target_priority p = temporaryHeroDetail.GetComponentInChildren<TowerManager>().priority;
        p += direction;
        if (p < 0) p = (target_priority)6;
        else if ((int)p > 6) p = 0;
        temporaryHeroDetail.GetComponentInChildren<TowerManager>().priority = p;

        switch(p)   
        {
            case target_priority.highHP:
                priority_text.text = "Highest Health First";
                    break;
            case target_priority.lowHP:
                priority_text.text = "Lowest Health First";
                break;
            case target_priority.slowest:
                priority_text.text = "Slowest First";
                break;
            case target_priority.fastest:
                priority_text.text = "Fastest First";
                break;
            case target_priority.danger:
                priority_text.text = "Dangerous First";
                break;
            case target_priority.first:
                priority_text.text = "First Seen";
                break;
            case target_priority.last:
                priority_text.text = "Last Seen";
                break;
        }
        temporaryHeroDetail.tower.OnPriorityChange();
    }

    public void ChangeSupportHeroPriority(int direction)
    {
        target_priority p = temporaryHeroDetail.GetComponentInChildren<TowerManager>().priority;
        support_priority sp = targetToSupport(p);

        sp += direction;
        p = supportToTarget(sp);
        temporaryHeroDetail.GetComponentInChildren<TowerManager>().priority = p;

        switch (p)
        {
            case target_priority.highHP:
                priority_text.text = "Highest Health First";
                break;
            case target_priority.lowHP:
                priority_text.text = "Lowest Health First";
                break;
            case target_priority.first:
                priority_text.text = "Closest";
                break;
            case target_priority.last:
                priority_text.text = "Furthest";
                break;
        }
        temporaryHeroDetail.tower.OnPriorityChange();
    }

    public support_priority targetToSupport(target_priority t)
    {
        switch(t)
        {
            case target_priority.highHP:
                return support_priority.highHP;                
            case target_priority.lowHP:
                return support_priority.lowHP;
            case target_priority.first:
                return support_priority.closest;
            case target_priority.last:
                return support_priority.furthest;
            default:
                return support_priority.lowHP;
        }
    }

    public target_priority supportToTarget(support_priority sp)
    {
        switch (sp)
        {
            case support_priority.highHP:
                return target_priority.highHP;
            case support_priority.lowHP:
                return target_priority.lowHP;
            case support_priority.closest:
                return target_priority.first;
            case support_priority.furthest:
                return target_priority.last;
            default:
                return target_priority.lowHP;
        }
    }

    public void IncreaseCurrency(int value)
    {
        currency += value;
        UpdateCurrencyDisplay();
    }

    public void SpendCurrency(int value)
    {
        currency -= value;
        UpdateCurrencyDisplay();
    }

    public bool CanAfford(int value)
    {
        if(currency - value <= 0) return false;
        return true;
    }

    void PopulatePurchaseMenu()
    {
        
        PlaceUnitStart();
        int id = 0;
        foreach(HeroPosition h in heroManager.heroList)
        {
            if (!h.isPopulated && !h.isPossessed)
            {
                GameObject button = Instantiate(unitBuyPrefab, panelParent);
                button.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(h.transform.position);
                button.GetComponent<HandlePuchaseCallback>().Setup(id, this);
                purchaseObjects.Add(button);
            }
            id++;
        }
    }

    public void OnPurchaseHandle(int tileId)
    {
        // Spawn hero at position
        heroManager.heroList[tileId].isPopulated = true;
        HeroDefinition d = isSuper ? heroManager.super[pendingPurchase] : heroManager.heroes[pendingPurchase];
        GameObject v = d.prefab;
        HeroPosition p = heroManager.heroList[tileId];
        GameObject instanceObject = Instantiate(v, p.transform);
        heroManager.heroList[tileId].Setup(d);
        int cost = heroManager.heroes[pendingPurchase].cost;
        SpendCurrency(cost);
        // Refresh UI
        foreach (GameObject h in purchaseObjects)
        {
            Destroy(h);
        }
        purchaseObjects.Clear();
        PlaceUnitEnd();

        Vector2 position = instanceObject.transform.position;
        // Apply any support upgrades this tower should benefit from
        foreach(HeroPosition origin in heroManager.heroList)
        {
            if (!origin.isSupport) continue;
            
            // loop through the origin's support upgrades
            foreach(Upgrade k in origin.myUpgradedTowers.Keys)
            {
                Vector2 originPos = origin.transform.position;
                float distance = (position - originPos).magnitude;
                if(distance <= k.radiusForUpgrade)
                {
                    HandleUpgradeModifiers(k, p, false);
                    origin.myUpgradedTowers[k].Add(p);
                }
            }
        }
    }


}

[System.Serializable]
public class intBool
{
    public int _int = 0;
    public bool _bool = false;
}
