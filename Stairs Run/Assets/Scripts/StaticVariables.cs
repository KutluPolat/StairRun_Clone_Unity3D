using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariables : MonoBehaviour
{
    public static bool _isGameStarted, _isGameEnded;
    public static BackpackClass backpack;
    public static StairClass stairSpawnManager;
    private void Awake()
    {
        _isGameStarted = false;
        _isGameEnded = false;
    }
    private void Start()
    {
        backpack = new BackpackClass();
        stairSpawnManager = new StairClass();
    }
}
