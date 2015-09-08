using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{
    #region variables

    public Button[] buttons;

    #endregion

    #region Awake function

    void Awake() 
    {

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
	}

    #endregion

    #region Public functions

    public void StartGameClick()
    {
        GameController.instance.InitGame();
    }

    public void InstructionClick()
    {

    }

    public void HighScoresClick()
    {

    }

    public void AchievementsClick()
    {

    }

    public void QuitGameClick()
    {
        GameController.instance.QuitGame();
    }

    #endregion
}
