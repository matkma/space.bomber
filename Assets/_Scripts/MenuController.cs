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
    public GameObject highScores;
    public Text bestScore;
    public GameObject[] instrunctionPages;

    private Button nextButton;
    private Button prevButton;
    private GameObject buttonsPanel;

    private Animator menuAnimator;
    private Animator instructionAnimator;
    private Animator highScoresAnimator;
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
        highScores.SetActive(true);

        nextButton = GameObject.Find("Next").GetComponent<Button>();
        prevButton = GameObject.Find("Prev").GetComponent<Button>();

        menuAnimator = mainMenu.GetComponent<Animator>();
        instructionAnimator = instruction.GetComponent<Animator>();
        highScoresAnimator = highScores.GetComponent<Animator>();

        prevButton.enabled = false;
	}

    #endregion

    #region Update function

    void Update()
    {
        SetLogInButton();
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
        bestScore.text = GameController.instance.highScore.ToString();
        menuAnimator.SetTrigger("forward");
        highScoresAnimator.SetTrigger("forward");
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
        GameController.instance.QuitGame();
    }

    public void LogInClick()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(SpaceBomber.GPGSIds.leaderboard_high_scores);
        }
        else
        {
            Social.localUser.Authenticate((bool success) => { });
        }

        SetLogInButton();
    }

    public void MuteClick()
    {
        if (GameController.instance.muted == 0)
        {
            GameController.instance.muted = 1;
            PlayerPrefs.SetInt("muted", 1);
        }
        else
        {
            GameController.instance.muted = 0;
            PlayerPrefs.SetInt("muted", 0);
        }

        SetMuteButton();
        PlayerPrefs.Save();
    }

    public void GlobalClick()
    {
        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform) Social.Active).ShowLeaderboardUI(SpaceBomber.GPGSIds.leaderboard_high_scores);
        }
    }

    public void BackClick()
    {
        highScoresAnimator.SetTrigger("back");
        menuAnimator.SetTrigger("back");
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

    void SetLogInButton()
    {
        if (Social.localUser.authenticated)
        {
            buttons[6].GetComponentInChildren<Text>().text = "Logout";
        }
        else
        {
            buttons[6].GetComponentInChildren<Text>().text = "Login";
        }
    }

    void SetMuteButton()
    {
        if (GameController.instance.muted == 0)
        {
            buttons[5].GetComponentInChildren<Text>().text = "Mute";
        }
        else
        {
            buttons[5].GetComponentInChildren<Text>().text = "Unmute";
        }
    }

    #endregion
}
