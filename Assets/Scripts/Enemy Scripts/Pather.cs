using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pather : MonoBehaviour
{
    private PathwayDisplay path;
    private bool isActive = false;
    private Vector2 currentTarget = Vector2.zero;
    private int targetIndex = -1;
    int finalIndex = -1;

    private EnemyManager enemy;

    public float arriveDistance = 0.1f;
    public float moveDistancePerFrame = 0.05f;

    public float speedMod = 0.0f;

    public bool arrivedAtPossessed = false;

    private void Start()
    {
        enemy = GetComponent<EnemyManager>();    
    }

    public Vector2 getTarget()
    {
        return currentTarget;
    }

    public void ModSpeed(float mod)
    {
        speedMod += mod;
    }

    public void SetPather(PathwayDisplay npath, int startingPath)
    {
        path = npath;
        isActive = true;
        targetIndex = startingPath;
        currentTarget = path.waypoints[targetIndex];
        finalIndex = path.waypointCount - 1;
    }

    public int getTargetIndex()
    {
        return targetIndex;
    }

    public void Update()
    {
        if (!isActive) return;
        if(enemy.isTryPossess)
        {
            PossessUpdate();
            return;
        }
        Vector2 cPos = (Vector2)transform.position;

        if(Arrived()) 
        {
            if(targetIndex == finalIndex)
            {
                FindFirstObjectByType<HomeManager>().ModifyHealth(1, true);
                GameManager.Instance.DamageBase(GetComponent<EnemyManager>().enemy.damageToBase);
                FindFirstObjectByType<EntitySpawner>().decrease(1);
                enemy.HandleDeathPossession();
                Destroy(gameObject);
                return;
            }
            currentTarget = path.GetNextWaypoint(targetIndex);
            targetIndex++;
        } else
        {
            // Move towards waypoint
            Vector2 dir = currentTarget - cPos;
            dir = dir.normalized * moveDistancePerFrame;
            if (enemy == null || enemy.getEnemy() == null) return; 
            dir *= (enemy.getEnemy().speed + speedMod);
            transform.position += (Vector3)dir;
        }
        
    }

    public void PossessUpdate()
    {
        

        if (ArrivePossess())
        {
            arrivedAtPossessed = true;
        }
        else
        {            
            Vector2 cPos = (Vector2)transform.position;
            Vector2 dir = (Vector2)enemy.heroToPossess.transform.position - cPos;
            dir = dir.normalized * moveDistancePerFrame;
            if (enemy == null || enemy.getEnemy() == null) return;
            dir *= enemy.getEnemy().speed;
            transform.position += (Vector3)dir;
        }
    }

    public bool ArrivePossess()
    {
        Vector2 cPos = (Vector2)transform.position;
        Vector2 distance = (Vector2)enemy.heroToPossess.transform.position - cPos;
        float m = Mathf.Abs(distance.magnitude);
        if (m > arriveDistance) return false;
        return true;
    }

    public bool Arrived()
    {
        // Generate magnitude of distance between waypoint and NPC
        Vector2 distance = currentTarget - (Vector2)transform.position;
        float m = Mathf.Abs(distance.magnitude);
        if (m > arriveDistance) return false;
        return true;
    }
}
