using UnityEngine;
using System.Collections;

public class BossPhase : MonoBehaviour {
    public Transform[] checkpoints;
    public MovementStrategy movementStrategy = MovementStrategy.STATIONARY;

    public enum MovementStrategy
    {
        STATIONARY,
        MOVE_TO_POINT_AND_STAY,
        LERP_BETWEEN_POINTS
    }
}
