using UnityEngine;
using System.Collections;

public class BossPhase : MonoBehaviour {
    public Transform[] checkpoints;
    public MovementStrategy movementStrategy = MovementStrategy.STATIONARY;
    public FireStrategy fireStrategy = FireStrategy.ALWAYS_FIRE;

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
}
