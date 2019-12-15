using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "State", order = 1)]
public class State : ScriptableObject
{
    [SerializeField]
    string titleText;

    [TextArea(10, 14)] [SerializeField]
    string storyText;

    [SerializeField]
    State[] nextStates;

    [SerializeField]
    public bool isWin;

    [SerializeField]
    public bool isFailed;

    [SerializeField]
    public bool hasTimer;
    [SerializeField]
    float timeValue;
    [SerializeField]
    State timedUpState;

    public string GetStateTitle()
    {
        return titleText;
    }

    public string GetStateStory()
    {
        return storyText;
    }

    public State[] GetNextStates()
    {
        return nextStates;
    }

    public State GetTimedUpState()
    {
        return timedUpState;
    }

    public float GetTimeValue()
    {
        if (hasTimer)
            return timeValue;
        else
            return 0f;
    }
}
