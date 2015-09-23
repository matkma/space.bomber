using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;

public class GameController : MonoBehaviour
{
    #region variables

    public static GameController instance = null;

    public Canvas blackOutPrefab;

    private Canvas blackOut;
    private Image blackOutImage;

    private bool levelLaunched = false;
    private bool backToMenu = false;

    [HideInInspector]
    public int muted;

    [HideInInspector]
    public int highScore;

    [HideInInspector]
    public int redsCount;

    [HideInInspector]
    public int itemsCount;

    [HideInInspector]
    public int combosCount;

    #endregion

    #region Awake function

    void Awake() 
    {
        Input.multiTouchEnabled = false;

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        highScore = PlayerPrefs.GetInt("highScore", 0);
        redsCount = PlayerPrefs.GetInt("redsCount", 0);
        itemsCount = PlayerPrefs.GetInt("itemsCount", 0);
        combosCount = PlayerPrefs.GetInt("combosCount", 0);

        muted = PlayerPrefs.GetInt("muted", 1);

        PlayGamesPlatform.Activate();
  
        Social.localUser.Authenticate((bool success) => {});
	}

    #endregion

    #region Update function

    void Update()
    {
        if (blackOut != null && blackOut.gameObject.active == true)
        {
            blackOutImage.color += new Color(0f, 0f, 0f, 0.02f);

            if (blackOutImage.color.a >= 1f)
            {
                if (levelLaunched)
                {
                    Application.LoadLevel("Level");
                    levelLaunched = false;
                }
                else if (backToMenu)
                {
                    Application.LoadLevel("Menu");
                    backToMenu = false;
                }
                else
                {
                    Destroy(blackOut);
                }
            }
        }
    }

    #endregion

    #region Public functions

    public void InitGame()
    {
        levelLaunched = true;
        CreateBlackOut();
        blackOut.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }


    public void BackToMenu()
    {
        backToMenu = true;
        CreateBlackOut();
        blackOut.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void CheckScoreAchievements()
    {
        if (highScore >= 5000)
        {
            GetAchievement(SpaceBomber.GPGSIds.achievement_bronze);

            if (highScore >= 15000)
            {
                GetAchievement(SpaceBomber.GPGSIds.achievement_silver);

                if (highScore >= 30000)
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_gold);

                    if (highScore >= 50000)
                    {
                        GetAchievement(SpaceBomber.GPGSIds.achievement_platinum);

                        if (highScore >= 100000)
                        {
                            GetAchievement(SpaceBomber.GPGSIds.achievement_diamond);
                        }
                    }
                }
            }
        }
    }

    public void GetRastaAchievement()
    {
        GetAchievement(SpaceBomber.GPGSIds.achievement_rasta);
    }

    public void CheckDamageAchievements()
    {
        if (redsCount >= 1)
        {
            GetAchievement(SpaceBomber.GPGSIds.achievement_injured);

            if (redsCount >= 50)
            {
                GetAchievement(SpaceBomber.GPGSIds.achievement_wounded);

                if (redsCount >= 500)
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_deeply_wounded);

                    if (redsCount >= 2000)
                    {
                        GetAchievement(SpaceBomber.GPGSIds.achievement_badly_wounded);

                        if (redsCount >= 10000)
                        {
                            GetAchievement(SpaceBomber.GPGSIds.achievement_lethally_wounded);
                        }
                    }
                }
            }
        }
    }

    public void CheckCombosAchievements()
    {
        if (combosCount >= 1)
        {
            GetAchievement(SpaceBomber.GPGSIds.achievement_combo_apprentice);

            if (combosCount >= 25)
            {
                GetAchievement(SpaceBomber.GPGSIds.achievement_combo_adept);

                if (combosCount >= 100)
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_combo_senpai);

                    if (combosCount >= 250)
                    {
                        GetAchievement(SpaceBomber.GPGSIds.achievement_combo_ninja);

                        if (combosCount >= 1000)
                        {
                            GetAchievement(SpaceBomber.GPGSIds.achievement_combo_sensei);

                            if (combosCount >= 5000)
                            {
                                GetAchievement(SpaceBomber.GPGSIds.achievement_combo_osensei);
                            }
                        }
                    }
                }
            }
        }
    }

    public void CheckCollectorAchievements()
    {
        if (itemsCount >= 100)
        {
            GetAchievement(SpaceBomber.GPGSIds.achievement_collector);

            if (itemsCount >= 1000)
            {
                GetAchievement(SpaceBomber.GPGSIds.achievement_patient_collector);

                if (itemsCount >= 2500)
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_scrupulous_collector);

                    if (itemsCount >= 10000)
                    {
                        GetAchievement(SpaceBomber.GPGSIds.achievement_grand_collector);

                        if (itemsCount >= 50000)
                        {
                            GetAchievement(SpaceBomber.GPGSIds.achievement_legendary_collector);
                        }
                    }
                }
            }
        }
    }


    public void CheckBestComboAchievements(int combo)
    {
        switch(combo)
        {
            case 5:
                GetAchievement(SpaceBomber.GPGSIds.achievement_mini_combo);
                break;
            case 6:
                GetAchievement(SpaceBomber.GPGSIds.achievement_typical_combo);
                break;
            case 7:
                GetAchievement(SpaceBomber.GPGSIds.achievement_average_combo);
                break;
            case 8:
                GetAchievement(SpaceBomber.GPGSIds.achievement_good_combo);
                break;
            case 9:
                GetAchievement(SpaceBomber.GPGSIds.achievement_great_combo);
                break;
            case 10:
                GetAchievement(SpaceBomber.GPGSIds.achievement_awesome_combo);
                break;
            case 11:
                GetAchievement(SpaceBomber.GPGSIds.achievement_excellent_combo);
                break;
            case 12:
                GetAchievement(SpaceBomber.GPGSIds.achievement_mega_combo);
                break;
            case 13:
                GetAchievement(SpaceBomber.GPGSIds.achievement_legendary_combo);
                break;
            case 14:
                GetAchievement(SpaceBomber.GPGSIds.achievement_incredible_combo);
                break;
            case 15:
                GetAchievement(SpaceBomber.GPGSIds.achievement_godlike_combo);
                break;
        }
    }

    public void PostScore(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, SpaceBomber.GPGSIds.leaderboard_high_scores, (bool success) => 
            { 
                // score updated
            });
        }
    }

    #endregion

    #region Private functions

    void CreateBlackOut()
    {
        blackOut = (Canvas)Instantiate(blackOutPrefab, transform.position, Quaternion.identity);
        blackOutImage = blackOut.GetComponentInChildren<Image>();

        blackOut.gameObject.SetActive(false);
    }

    private void GetAchievement(string achievement)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievement, 100.0f, (bool success) =>
            {
                // achievement unlocked
            });
        }
    }

    #endregion
}
