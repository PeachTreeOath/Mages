using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossPhase : MonoBehaviour {
    public Transform[] checkpoints;
    public MovementStrategy movementStrategy = MovementStrategy.STATIONARY;
    public FireStrategy fireStrategy = FireStrategy.ALWAYS_FIRE;
    public Barrel[] globalBarrelPrefabs;

    private Barrel[] globalBarrels;
    void Start()
    {
        globalBarrels = new Barrel[globalBarrelPrefabs.Length];
        for ( int i = 0; i < globalBarrels.Length; i++)
        {
            Barrel barrelPrefab = globalBarrelPrefabs[i];
            globalBarrels[i] = (Barrel)Instantiate(barrelPrefab, barrelPrefab.transform.position, barrelPrefab.transform.rotation);
        }
    }
    
    void OnDestroy()
    {
        foreach (Barrel barrel in globalBarrels)
        {
            Destroy(barrel.gameObject);
        }
    }

    public enum MovementStrategy
    {
        STATIONARY,
        MOVE_TO_POINT_AND_STAY,
        LERP_BETWEEN_POINTS
    }

    public enum FireStrategy
    {
        ALWAYS_FIRE,
        FIRE_AT_DESTINATION, //to be used with MovementStrategy.MOVE_TO_POINT_AND_STAY
        FIRE_BURST_AT_CHECKPOINTS,
        NEVER_FIRE
        
    }

    public Barrel[] getBarrels()
    {
        Barrel[] childBarrels = GetComponentsInChildren<Barrel>();
        Barrel[] totalBarrels = new Barrel[childBarrels.Length + globalBarrels.Length];
        childBarrels.CopyTo(totalBarrels,0);
        globalBarrels.CopyTo(totalBarrels, childBarrels.Length);
        return totalBarrels;
    }
}
