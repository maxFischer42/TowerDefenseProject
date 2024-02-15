using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public PathwayDisplay paths;

    private int entities = 0;

    private float cooldown = 3f;

    public List<SpawnPhase> phases = new List<SpawnPhase>();
    private int current_phase = 0;

    private int current_spawn = 0;
    private int target_spawn = 0;

    public List<GameObject> prefabsToSpawn = new List<GameObject>();
    private void Start()
    {
        GameManager.Instance.SetupWaves(phases.Count);
        ResetForNewPhase();
    }

    public void HandleSpawn()
    {
        if(entities > 0)
        {
            Invoke(nameof(Spawn), cooldown);
        } else if(target_spawn <= 0 && entities <= 0)
        {
            MoveOnToNextPhase();
        } else
        {
            Invoke(nameof(HandleSpawn), 1f);
        }
    }

    public void decrease(int num)
    {
        target_spawn -= num;
    }

    public void Spawn()
    {
        entities--;
        Vector2 spawnpoint = paths.waypoints[0];
        GameObject obj = Instantiate(prefabsToSpawn[phases[current_phase].id_s[current_spawn]], spawnpoint, Quaternion.identity);
        obj.GetComponent<Pather>().SetPather(paths, 1);
        current_spawn++;
        HandleSpawn();
    }

    public void MoveOnToNextPhase()
    {
        current_phase++;        
        if (current_phase > phases.Count)
        {
            return;
        }
        else
        {
            ResetForNewPhase();
        }      
    }

    public void AddEntity(int num)
    {
        target_spawn += num;
    }

    void ResetForNewPhase()
    {
        current_spawn = 0;
        if (current_phase > phases.Count - 1)
        {
            //
            // Victory!!
            print("VICTORY!!!!");
        }
        else
        {
            GameManager.Instance.IncreaseWave();
            entities = phases[current_phase].id_s.Length;
            target_spawn = entities;
            cooldown = phases[current_phase].cooldown_between;
            Invoke(nameof(Spawn), cooldown);
        }
    }
}

[System.Serializable]
public class SpawnPhase
{
    public int[] id_s;
    public float cooldown_between = 3f;
}