using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    public int highScore;

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

    #endregion

    #region Private functions

    void CreateBlackOut()
    {
        blackOut = (Canvas)Instantiate(blackOutPrefab, transform.position, Quaternion.identity);
        blackOutImage = blackOut.GetComponentInChildren<Image>();

        blackOut.gameObject.SetActive(false);
    }

    #endregion
}
