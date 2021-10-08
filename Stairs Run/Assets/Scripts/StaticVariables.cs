using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariables : MonoBehaviour
{
    public static bool _isGameStarted, _isGameEnded;
    public static int _stairsInBackpackCounter;

    private void Awake()
    {
        _isGameStarted = false;
        _isGameEnded = false;
        _stairsInBackpackCounter = 0;
    }
}
