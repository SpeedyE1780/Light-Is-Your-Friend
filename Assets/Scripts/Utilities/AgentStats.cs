using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AgentStats")]
public class AgentStats : ScriptableObject
{
    public string AnimatorTrigger;
    public float MovementSpeed;
    public float AngularSpeed;
    public bool IsStopped;

#if UNITY_EDITOR

    private void OnValidate()
    {
        MovementSpeed = MovementSpeed <= 0 ? 0 : MovementSpeed;
        AngularSpeed = AngularSpeed <= 0 ? 0 : AngularSpeed;
    }

#endif
}