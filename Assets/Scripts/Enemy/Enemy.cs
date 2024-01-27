using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AIDestinationSetter aIDestinationSetter;
    
    private void Start()
    {
        aIDestinationSetter.target = PlayerController.Instance.transform;
    }
}
