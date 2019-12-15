using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
    #region Variables
    // Manage text
    [SerializeField]
    TextMeshProUGUI titleTextComponent;
    [SerializeField]
    TextMeshProUGUI textComponent;
    [SerializeField]
    TextMeshProUGUI authorTextComponent;

    // Manage State
    [SerializeField]
    State startingState;
    [SerializeField]
    State achievementState;
    private State state;

    // Manage Achievements
    private List<State> currentFailedStates = new List<State>();
    private List<string> currentWinStates   = new List<string>();
    private List<string> allAchievements    = new List<string>(){"Ghost Inmate", "Convict Allies", "Presumed Dead",
                                                                 "International Rescue Operative", "The Betrayed"};
    private int totalFail;
    private bool checkFail; // For checking to run only one time in Update

    // Manage background color
    [SerializeField]
    Image backgroundImage;
    private Color32 backgroundColor;

    // Manage timer
    [SerializeField]
    Slider timeSlider;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        state = startingState;

        titleTextComponent.text = state.GetStateTitle();
        textComponent.text      = state.GetStateStory(); 
    }

    // Update is called once per frame
    void Update()
    {
        ManageStates();
        ManageAchievements();
        ManageBackgroundColor();
        ManageTimer();
    }

    #region StateManager
    private void ManageStates()
    {
        var nextStates = state.GetNextStates();

        for (int index = 0; index < nextStates.Length; index++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                ResetTimer();
                state     = nextStates[index];
                checkFail = false;
            }
        }

        titleTextComponent.text = state.GetStateTitle();
        if (state != achievementState)
        {
            textComponent.text = state.GetStateStory();
        }     
        if (state == startingState)
        {
            authorTextComponent.gameObject.SetActive(true);
        }
        else
        {
            authorTextComponent.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Achievements
    private void ManageAchievements()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == startingState)
            {
                state = achievementState;
                SetTextForAchievements();
            }
        }
    }

    private void SetTextForAchievements()
    {
        textComponent.text  = "Unique Fail Count: " + currentFailedStates.Count.ToString() + "\n";
        textComponent.text += "Total Fail Count: "  + totalFail.ToString() + "\n\n";
        textComponent.text += "Achievements:\n";

        for (int i = 0; i < allAchievements.Count; i++)
        {
            if (currentWinStates.Contains(allAchievements[i]))
            {
                textComponent.text += "- " + allAchievements[i] + "\n";
            }
            else
            {
                textComponent.text += "<color=#" + ColorUtility.ToHtmlStringRGB(new Color(0.5f, 0.5f, 0.5f, 1f)) + ">- " + allAchievements[i] + " (Unlocked)" + "</color>\n";
            }
        }

        if (currentFailedStates.Count >= 60)
        {
            textComponent.text += "- King of Fails (Find all 60 unique fails)\n";
        }
        else
        {
            textComponent.text += "<color=#" + ColorUtility.ToHtmlStringRGB(new Color(0.5f, 0.5f, 0.5f, 1f)) + ">- King of Fails (Find all 60 unique fails) (Unlocked)</color>\n";
        }

        textComponent.text += "\nPress \"1\" to back to menu.";
    }
    #endregion

    #region BackgroundColor
    private void ManageBackgroundColor()
    {
        if (state.isWin)
        {
            backgroundColor = new Color32(0, 77, 4, 255);

            string titleText = state.GetStateTitle();
            string subTitleText = titleText.Substring(titleText.IndexOf("(") + 1);
            subTitleText = subTitleText.Substring(0, subTitleText.Length - 1);

            if (!currentWinStates.Contains(subTitleText))
            {
                currentWinStates.Add(subTitleText);
            }
        }
        else if (state.isFailed)
        {
            backgroundColor = new Color32(192, 39, 35, 255);

            if (!currentFailedStates.Contains(state))
            {
                currentFailedStates.Add(state);
            }

            if (!checkFail)
            {
                totalFail += 1;
                checkFail  = true;
            }         
        }
        else
        {
            backgroundColor = new Color32(77, 66, 0, 255);
        }

        backgroundImage.color = backgroundColor;
    }
    #endregion

    #region Timer
    private void ManageTimer()
    {
        if (state.hasTimer)
        {          
            timeSlider.gameObject.SetActive(true);
            RunTimer();  
        }
        else
        {
            ResetTimer();
        }
    }

    private void RunTimer()
    {     
        timeSlider.value -= (timeSlider.maxValue / state.GetTimeValue()) * Time.deltaTime;

        if (timeSlider.value == 0f)
        {
            state = state.GetTimedUpState();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timeSlider.gameObject.SetActive(false);
        timeSlider.value = timeSlider.maxValue;
    }
    #endregion
}
