using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using GooglePlayGames;

public class MenuController : MonoBehaviour
{
    #region variables

    public Button[] buttons;
    public GameObject mainMenu;
    public GameObject instruction;
    public GameObject highScores;
    public GameObject startGame;
    public Text bestScore;
    public Text bestRush;
    public GameObject[] instrunctionPages;

    private Button nextButton;
    private Button prevButton;
    private GameObject buttonsPanel;

    private Animator menuAnimator;
    private Animator instructionAnimator;
    private Animator highScoresAnimator;
    private Animator startGameAnimator;
    private int page = 1;

    private AudioSource source;

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
        startGame.SetActive(true);

        nextButton = GameObject.Find("Next").GetComponent<Button>();
        prevButton = GameObject.Find("Prev").GetComponent<Button>();

        menuAnimator = mainMenu.GetComponent<Animator>();
        instructionAnimator = instruction.GetComponent<Animator>();
        highScoresAnimator = highScores.GetComponent<Animator>();
        startGameAnimator = startGame.GetComponent<Animator>();

        if (PlayerPrefs.GetInt("firstTime", 1) == 1)
        {
            menuAnimator.SetTrigger("firstTime");
        }

        prevButton.enabled = false;
	}

    #endregion

    #region Public functions

    public void StartGameClick()
    {
        menuAnimator.SetTrigger("forward");
        startGameAnimator.SetTrigger("forward");
    }

    public void NormalModeClick()
    {
        if (source != null)
            source.Stop();
        GameController.instance.rushMode = false;
        GameController.instance.InitGame();
    }

    public void RushModeClick()
    {
        if (source != null)
            source.Stop();
        GameController.instance.rushMode = true;
        GameController.instance.InitGame();
    }

    public void InstructionClick()
    {
        menuAnimator.SetTrigger("forward");
        instructionAnimator.SetTrigger("forward");

        if (GameController.instance.firstTime == 1)
        {
            GameController.instance.firstTime = 0;
            PlayerPrefs.SetInt("firstTime", 0);
            PlayerPrefs.Save();
        }
    }

    public void HighScoresClick()
    {
        TimeSpan time = TimeSpan.FromSeconds(GameController.instance.bestRush);

        bestScore.text = GameController.instance.highScore.ToString();

        bestRush.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);

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
            PlayGamesPlatform.Instance.SignOut();
        }
        else
        {
            Social.localUser.Authenticate((bool success) => { });
        }

    }

    public void MuteClick()
    {
        if (GameController.instance.muted == 0)
        {
            GameController.instance.muted = 1;
            PlayerPrefs.SetInt("muted", 1);
            GameController.instance.audioController.Mute(true);
        }
        else
        {
            GameController.instance.muted = 0;
            PlayerPrefs.SetInt("muted", 0);
            GameController.instance.audioController.Mute(false);
        }

        PlayerPrefs.Save();
    }

    public void GlobalClick()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
    }

    public void BackClick()
    {
        highScoresAnimator.SetTrigger("back");
        menuAnimator.SetTrigger("back");
    }

    public void StartGameBackClick()
    {
        startGameAnimator.SetTrigger("back");
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

    #endregion
}
