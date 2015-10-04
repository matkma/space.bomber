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

    [HideInInspector]
    public AudioController audioController;

    private bool levelLaunched = false;
    private bool backToMenu = false;

    [HideInInspector]
    public int muted;

    [HideInInspector]
    public int highScore;

    [HideInInspector]
    public int bestRush;

    [HideInInspector]
    public int redsCount;
    private int tmpRedsCount;

    [HideInInspector]
    public int itemsCount;
    private int tmpItemsCount;

    [HideInInspector]
    public int combosCount;
    private int tmpCombosCount;

    [HideInInspector]
    public int bestCombo;

    [HideInInspector]
    public int firstTime;

    [HideInInspector]
    public bool rushMode = false;

    private bool []achievements;

    private int collectorAchievementStepCounter;

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
        bestRush = PlayerPrefs.GetInt("bestRush", 0);
        redsCount = PlayerPrefs.GetInt("redsCount", 0);
        itemsCount = PlayerPrefs.GetInt("itemsCount", 0);
        combosCount = PlayerPrefs.GetInt("combosCount", 0);
        bestCombo = PlayerPrefs.GetInt("bestCombo", 0);
        firstTime = PlayerPrefs.GetInt("firstTime", 1);

        achievements = new bool[26];

        for (int i = 0; i < achievements.Length; i++)
        {
            achievements[i] = false;
        }

        tmpCombosCount = 0;
        tmpItemsCount = 0;
        tmpRedsCount = 0;

        muted = PlayerPrefs.GetInt("muted", 1);

        audioController = gameObject.GetComponent<AudioController>();

        if (muted == 1)
        {
            GameController.instance.audioController.Mute(true);
        }
        else
        {
            GameController.instance.audioController.Mute(false);
        }

        collectorAchievementStepCounter = itemsCount % 10;

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

    public void CheckDamageAchievements(int inc)
    {
        tmpRedsCount += inc;

        if (!achievements[10] && redsCount + tmpRedsCount >= 10000)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_lethally_wounded, tmpRedsCount, (bool success) => { });
            achievements[10] = true;
        }

        if (!achievements[11] && redsCount + tmpRedsCount >= 2000)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_badly_wounded, tmpRedsCount, (bool success) => { });
            achievements[11] = true;
        }

        if (!achievements[12] && redsCount + tmpRedsCount >= 500)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_deeply_wounded, tmpRedsCount, (bool success) => { });
            achievements[12] = true;
        }

        if (!achievements[13] && redsCount + tmpRedsCount >= 50)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_wounded, tmpRedsCount, (bool success) => { });
            achievements[13] = true;
        }

        if (!achievements[14] && redsCount + tmpRedsCount >= 1)
        {
            GetAchievement(SpaceBomber.GPGSIds.achievement_injured);
            achievements[14] = true;
        }

    }

    public void UpdateAllAchievements()
    {
        redsCount += tmpRedsCount;
        combosCount += tmpCombosCount;
        itemsCount += tmpItemsCount;

        PlayerPrefs.SetInt("redsCount", redsCount);
        PlayerPrefs.SetInt("combosCount", combosCount);
        PlayerPrefs.SetInt("itemsCount", itemsCount);

        PlayerPrefs.Save();


        if (redsCount <= 10000)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_lethally_wounded, tmpRedsCount, (bool success) => { });

            if (redsCount <= 2000)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_badly_wounded, tmpRedsCount, (bool success) => { });

                if (redsCount <= 500)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_deeply_wounded, tmpRedsCount, (bool success) => { });

                    if (redsCount <= 50)
                    {
                        PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_wounded, tmpRedsCount, (bool success) => { });

                        if (redsCount <= 1)
                        {
                            GetAchievement(SpaceBomber.GPGSIds.achievement_injured);
                        }
                    }
                }
            }
        }

        if (combosCount <= 5000)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_osensei, tmpCombosCount, (bool success) => { });

            if (combosCount <= 1000)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_sensei, tmpCombosCount, (bool success) => { });

                if (combosCount <= 250)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_ninja, tmpCombosCount, (bool success) => { });

                    if (combosCount <= 100)
                    {
                        PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_senpai, tmpCombosCount, (bool success) => { });

                        if (combosCount <= 25)
                        {
                            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_adept, tmpCombosCount, (bool success) => { });

                            if (combosCount >= 1)
                            {
                                GetAchievement(SpaceBomber.GPGSIds.achievement_combo_apprentice);
                            }
                        }
                    }
                }
            }
        }

        if (itemsCount <= 50000)
        {
            collectorAchievementStepCounter += tmpItemsCount;
            int step = 0;

            while (collectorAchievementStepCounter >= 10)
            {
                collectorAchievementStepCounter -= 10;
                step += 1;
            }

            if (step != 0)
                PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_legendary_collector, step, (bool success) => { });

            if (itemsCount <= 10000)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_grand_collector, tmpItemsCount, (bool success) => { });

                if (itemsCount <= 2500)
                {
                    PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_scrupulous_collector, tmpItemsCount, (bool success) => { });

                    if (itemsCount <= 1000)
                    {
                        PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_patient_collector, tmpItemsCount, (bool success) => { });

                        if (itemsCount <= 100)
                        {
                            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_collector, tmpItemsCount, (bool success) => { });
                        }
                    }
                }
            }
        }

        tmpCombosCount = 0;
        tmpItemsCount = 0;
        tmpRedsCount = 0;
    }

    public void CheckCombosAchievements()
    {
        tmpCombosCount += 1;

        if (!achievements[0] && combosCount + tmpCombosCount >= 1)
        {
            GetAchievement(SpaceBomber.GPGSIds.achievement_combo_apprentice);
            achievements[0] = true;
        }

        if (!achievements[1] && combosCount + tmpCombosCount >= 25)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_adept, tmpCombosCount, (bool success) => { });
            achievements[1] = true;
        }

        if (!achievements[2] && combosCount + tmpCombosCount >= 100)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_senpai, tmpCombosCount, (bool success) => { });
            achievements[2] = true;
        }

        if (!achievements[3] && combosCount + tmpCombosCount >= 250)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_ninja, tmpCombosCount, (bool success) => { });
            achievements[3] = true;
        }

        if (!achievements[4] && combosCount + tmpCombosCount >= 1000)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_sensei, tmpCombosCount, (bool success) => { });
            achievements[4] = true;
        }

        if (!achievements[5] && combosCount + tmpCombosCount >= 5000)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_combo_osensei, tmpCombosCount, (bool success) => { });
            achievements[5] = true;
        }
    }

    public void CheckCollectorAchievements(int inc)
    {
        tmpItemsCount += inc;

        if (!achievements[6] && itemsCount + tmpItemsCount >= 10000)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_grand_collector, tmpItemsCount, (bool success) => { });
            achievements[6] = true;
        }

        if (!achievements[7] && itemsCount + tmpItemsCount >= 2500)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_scrupulous_collector, tmpItemsCount, (bool success) => { });
            achievements[7] = true;
        }

        if (!achievements[8] && itemsCount + tmpItemsCount >= 1000)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_patient_collector, tmpItemsCount, (bool success) => { });
            achievements[8] = true;
        }

        if (!achievements[9] && itemsCount + tmpItemsCount >= 100)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(SpaceBomber.GPGSIds.achievement_collector, tmpItemsCount, (bool success) => { });
            achievements[9] = true;
        }
    }


    public void CheckBestComboAchievements(int combo)
    {
        switch(combo)
        {
            case 5:
                if (!achievements[15])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_mini_combo);
                    achievements[15] = true;
                }
                break;
            case 6:
                if (!achievements[16])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_typical_combo);
                    achievements[16] = true;
                }
                break;
            case 7:
                if (!achievements[17])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_average_combo);
                    achievements[17] = true;
                }
                break;
            case 8:
                if (!achievements[18])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_good_combo);
                    achievements[18] = true;
                }
                break;
            case 9:
                if (!achievements[19])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_great_combo);
                    achievements[19] = true;
                }
                break;
            case 10:
                if (!achievements[20])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_awesome_combo);
                    achievements[20] = true;
                }
                break;
            case 11:
                if (!achievements[21])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_excellent_combo);
                    achievements[21] = true;
                }
                break;
            case 12:
                if (!achievements[22])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_mega_combo);
                    achievements[22] = true;
                }
                break;
            case 13:
                if (!achievements[23])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_legendary_combo);
                    achievements[23] = true;
                }
                break;
            case 14:
                if (!achievements[24])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_incredible_combo);
                    achievements[24] = true;
                }
                break;
            default:
                if (combo >= 15 && !achievements[25])
                {
                    GetAchievement(SpaceBomber.GPGSIds.achievement_godlike_combo);
                    achievements[25] = true;
                }
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
