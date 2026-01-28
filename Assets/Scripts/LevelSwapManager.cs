using System;
using UnityEngine;

public enum WorldState
{
    Comedy,
    Tragedy
}
public class LevelSwapManager : BroadCasterClass
{
    public static event Action<WorldState> OnLevelChanged;
    public static event Action<WorldState> SwapCheckOutcomeSuccessful;
    
    private WorldState _currentState = WorldState.Comedy;
    private WorldState _nextState;
    private float swapCooldown = 0.5f;
    private float swapTimer;

    // Update is called once per frame
    void Update()
    {
        swapTimer -= Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.E) && swapTimer <= 0)
        {
            swapTimer = swapCooldown;
            switch (_currentState)
            {
                case WorldState.Comedy:
                    Debug.Log("New State is Tragedy");
                    _nextState = WorldState.Tragedy;
                    break;
                case WorldState.Tragedy:
                    Debug.Log("New State is Comedy");
                    _nextState = WorldState.Comedy;
                    break;
            }
            OnLevelChanged?.Invoke(_nextState);
            //NotifyObservers(_currentState);
        }
    }
    
    public void LevelSwitchSuccessful(bool result)
    {
        
        if (result)
        {
            _currentState = _nextState;
            SwapCheckOutcomeSuccessful?.Invoke(_currentState);
            
        }
    }
}
