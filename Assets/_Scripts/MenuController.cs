using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour 
{
    public Button[] buttons;
   

	void Awake() 
    {

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }
	}
	
	void Update() 
    {

	}

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
}
