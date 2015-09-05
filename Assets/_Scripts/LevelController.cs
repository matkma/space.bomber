using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelController : MonoBehaviour 
{
    public int health = 3;
    public int score = 0;

    public float spawnDelay = 0.5f;
    public float badChance = 25f;
    public Collectable[] goods;
    public Collectable[] bads;
    public GameObject[] spawnPoints;
    public Collector collectorPrefab;

    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button menuButton;
    public Text scoreText;
    public Text pointsText;
    public Text livesText;
    public Image damageImage;

    public GameObject endGamePanel;
    public GameObject endGameBox;
    public Text[] texts;
    public Button endGameButton;

    private float spawnCounter = 0f;
    private float cleanerCounter = 0f;
    private float cleanerDelay = 5f;
    private bool touched = false;
    private Collector collector;

    private Animator scoreAC;
    private Animator pointsAC;
    private Animator livesAC;
    private Animator damageAC;

    [HideInInspector]
    public int missedItems = 0;
    private int collectedItems = 0;
    private int bestCombo = 0;

    private bool gameOver = false;
    private float gameOverTimer = 0f;
    

	void Awake() 
    {
        scoreAC = scoreText.GetComponent<Animator>();
        pointsAC = pointsText.GetComponent<Animator>();
        livesAC = livesText.GetComponent<Animator>();
        damageAC = damageImage.GetComponent<Animator>();

        scoreText.text = "Score: " + score;

        livesText.text = "Lives: " + health;
	}
	
	void Update() 
    {
        if (!gameOver)
        {
            spawnCounter += Time.deltaTime;
            cleanerCounter += Time.deltaTime;

            if (collector != null && collector.launched == true)
            {
                if (collector.launchDelay >= 1)
                {
                    if (collector.collectedCounter > 1)
                    {
                        health -= collector.hpLoss;

                        if (health <= 0)
                        {
                            health = 0;
                            livesText.text = "Lives: " + health;
                            damageAC.SetTrigger("Damaged");
                            livesAC.SetTrigger("Scored");

                            GameOver();
                        }

                        livesText.text = "Lives: " + health;
                        livesAC.SetTrigger("Scored");

                        int points = (int)(collector.points * (Mathf.Log10(collector.collectedCounter - 1) + 1));
                        score += points;

                        scoreText.text = "Score: " + score;
                        scoreAC.SetTrigger("Scored");

                        pointsText.transform.position = Camera.main.WorldToScreenPoint(collector.transform.position);
                        pointsText.text = "+" + points + " POINTS";

                        if (collector.hpLoss > 0)
                        {
                            pointsText.color = new Color(1f, 0f, 0f, 0f);
                            pointsText.text += "\nDAMAGED!";
                            damageAC.SetTrigger("Damaged");
                        }
                        else
                            pointsText.color = new Color(0.772f, 1f, 0.345f, 0f);

                        if (collector.collectedCounter > 2)
                        {
                            pointsText.text += "\n" + collector.collectedCounter + "x COMBO!";
                            if (collector.collectedCounter > bestCombo)
                            {
                                bestCombo = collector.collectedCounter;
                            }

                            collectedItems += collector.collectedCounter;
                        }

                        pointsAC.SetTrigger("Scored");
                    }

                    collector.launched = false;
                    collector.tag = "CollectorUsed";
                    collector.GetComponent<MeshRenderer>().enabled = false;

                    collector = null;
                }

                if (collector != null)
                    collector.launchDelay += 1;
            }

            if (spawnCounter >= spawnDelay)
            {
                spawnCounter = 0f;

                int collectable = Random.Range(0, 3);
                int spawnPoint = Random.Range(0, 18);

                float badRoll = Random.Range(0f, 100f);

                if (badRoll > badChance)
                    Instantiate(goods[collectable], spawnPoints[spawnPoint].transform.position, Quaternion.identity);
                else
                    Instantiate(bads[collectable], spawnPoints[spawnPoint].transform.position, Quaternion.identity);
            }

            if (Input.touchCount > 0 && pausePanel.active == false)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (collector == null && touched == false && touchPosition.y > -2.4f)
                        {
                            Quaternion rotation = Quaternion.identity;
                            rotation.eulerAngles = new Vector3(90, 0, 0);
                            Vector3 position = new Vector3(touchPosition.x, touchPosition.y, 0f);

                            collector = (Collector)Instantiate(collectorPrefab, position, rotation);
                            collector.active = true;
                            touched = true;
                        }

                        break;

                    case TouchPhase.Ended:
                        if (collector != null && touched == true)
                        {
                            collector.active = false;
                            touched = false;
                            collector.launched = true;
                            collector.Launch();
                        }

                        break;
                }
            }

            if (cleanerCounter >= cleanerDelay)
            {
                cleanerCounter = 0f;
                UsedCollectorsCleaner();
            }
        }

        else
        {
            gameOverTimer += Time.deltaTime;

            if (gameOverTimer >= 3.2f)
            {
                endGameButton.enabled = true;

                if (score > GameController.instance.highScore)
                {
                    texts[4].enabled = true;
                    GameController.instance.highScore = score;
                    PlayerPrefs.SetInt("highScore", score);
                    PlayerPrefs.Save();
                }
            }
            else if (gameOverTimer >= 2.8f && texts[3].enabled == false)
            {
                texts[3].enabled = true;
            }
            else if (gameOverTimer >= 2.4f && texts[2].enabled == false)
            {
                texts[2].enabled = true;
            }
            else if (gameOverTimer >= 2f && texts[1].enabled == false)
            {
                texts[1].enabled = true;
            }
            else if (gameOverTimer >= 1.6f && texts[0].enabled == false)
            {
                texts[0].enabled = true;
                endGameBox.GetComponent<Animator>().SetTrigger("Score");
            }
        }
	}

    public void PauseButtonClick()
    {
        if (pauseButton.enabled == true)
        {
            pauseButton.enabled = false;
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeButtonClick()
    {
        pauseButton.enabled = true;
        pausePanel.GetComponent<Animator>().SetTrigger("Removed");
        Invoke("RemovePausePanel", 0.5f);
        Time.timeScale = 1f;
    }

    public void MenuButtonClick()
    {
        pausePanel.GetComponent<Animator>().SetTrigger("Removed");
        Time.timeScale = 1f;
        Invoke("RemovePausePanel", 0.5f);
        Invoke("GameOver", 0.5f);
    }

    private void RemovePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void EndGameButtonClick()
    {
        GameController.instance.BackToMenu();
    }

    void GameOver()
    {
        endGamePanel.SetActive(true);

        texts[0].text = score.ToString();
        texts[1].text = "Best Combo: " + bestCombo;
        texts[2].text = "Collected items: " + collectedItems;
        texts[3].text = "Missed items: " + missedItems;

        endGameButton.enabled = false;

        for (int i = 0; i < 5; i++)
        {
            texts[i].enabled = false;
        }
        
        endGameBox.GetComponent<Animator>().SetTrigger("Show");

        gameOver = true;
    }

    void UsedCollectorsCleaner()
    {
        GameObject[] collectors = GameObject.FindGameObjectsWithTag("CollectorUsed");
        foreach (GameObject trash in collectors)
        {
            if (trash.GetComponentInChildren<ParticleSystem>().isPlaying == false)
                Destroy(trash);
        }
    }
}
