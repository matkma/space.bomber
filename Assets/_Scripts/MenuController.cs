using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;

public class MenuController : MonoBehaviour
{
    #region variables

    public Button[] buttons;
    public GameObject mainMenu;
    public GameObject instruction;
    public GameObject[] instrunctionPages;

    private Button nextButton;
    private Button prevButton;

    private Animator menuAnimator;
    private Animator instructionAnimator;
    private int page = 1;

    #endregion

    #region Awake function

    void Awake() 
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = true;
        }

        instruction.SetActive(true);

        nextButton = GameObject.Find("Next").GetComponent<Button>();
        prevButton = GameObject.Find("Prev").GetComponent<Button>();

        menuAnimator = mainMenu.GetComponent<Animator>();
        instructionAnimator = instruction.GetComponent<Animator>();

        prevButton.enabled = false;
	}

    #endregion

    #region Public functions

    public void StartGameClick()
    {
        GameController.instance.InitGame();
    }

    public void InstructionClick()
    {
        menuAnimator.SetTrigger("forward");
        instructionAnimator.SetTrigger("forward");
    }

    public void HighScoresClick()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }

    public void AchievementsClick()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    public void QuitGameClick()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();

        GameController.instance.QuitGame();
    }

    public void GotItClick()
    {
        instructionAnimator.SetTrigger("back");
        menuAnimator.SetTrigger("back");

        prevButton.enabled = false;
        nextButton.enabled = true;

        Invoke("ResetPages", 0.15f);
    }

    public void NextClick()
    {
        if (page < instrunctionPages.Length)
        {
            instrunctionPages[page - 1].SetActive(false);
            page += 1;
            instrunctionPages[page - 1].SetActive(true);

            prevButton.enabled = true;

            if (page == instrunctionPages.Length)
                nextButton.enabled = false;
        }
    }

    public void PrevClick()
    {
        if (page > 1)
        {
            instrunctionPages[page - 1].SetActive(false);
            page -= 1;
            instrunctionPages[page - 1].SetActive(true);

            nextButton.enabled = true;

            if (page == 1)
                prevButton.enabled = false;
        }
    }

    #endregion

    #region Private functions

    void ResetPages()
    {
        page = 1;

        foreach(GameObject current in instrunctionPages)
        {
            current.SetActive(false);
        }

        instrunctionPages[0].SetActive(true);
    }

    #endregion
}
