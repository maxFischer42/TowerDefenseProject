using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectCallback : MonoBehaviour
{
    private HeroPosition posInfo;

    private void Start()
    {
        posInfo = transform.parent.parent.parent.GetComponent<HeroPosition>();  
    }
    public void OnHeroSelctCallback()
    {
        int id = posInfo.tileId;
        GameManager.Instance.SelectUnitOpen(posInfo, posInfo.hero.isSuper);
    }
}
