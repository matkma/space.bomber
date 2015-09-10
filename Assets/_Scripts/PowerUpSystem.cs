using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpSystem : MonoBehaviour
{
    #region variables

    public PowerUp[] powerUps;

    [HideInInspector]
    public float rescaleFactor = 0f;

    [HideInInspector]
    public bool invulnerable = false;

    [HideInInspector]
    public float multiplier = 1f;

    private LevelController levelController;
    private List<PowerUp> collectedPowerUps;

    private float fasterSpawnDelay = 0.2f;
    private float slowerSpawnDelay = 1f;
    private float spawnDelay;

    private float fasterCollectorTimer = -1f;
    private float fasterSpawnTimer = -1f;
    private float slowerSpawnTimer = -1f;
    private float speedUpTimer = -1f;
    private float slowDownTimer = -1f;
    private float greaterMultiplierTimer = -1f;
    private float invulnerabilityTimer = -1f;

    private Animator invulnerabilityEffectAC;
    private GameObject background;

    #endregion

    #region Awake function

    void Awake() 
    {
        levelController = GetComponent<LevelController>();
        collectedPowerUps = new List<PowerUp>();
        collectedPowerUps.Clear();

        spawnDelay = levelController.spawnDelay;

        invulnerabilityEffectAC = GameObject.Find("InvulnerabilityEffect").GetComponent<Animator>();

        background = GameObject.Find("Background");
	}

    #endregion

    #region Update function

    void Update () 
    {
        if (fasterCollectorTimer > float.Epsilon)
        {
            fasterCollectorTimer -= Time.deltaTime;
                
            if (fasterCollectorTimer <= float.Epsilon)
            {
                fasterCollectorTimer = -1f;
                rescaleFactor = 0f;
            }
        }

        if (slowerSpawnTimer > float.Epsilon)
        {
            slowerSpawnTimer -= Time.deltaTime;

            if (slowerSpawnTimer <= float.Epsilon)
            {
                slowerSpawnTimer = -1f;
                
                if (fasterSpawnTimer <= float.Epsilon)
                {
                    levelController.spawnDelay = spawnDelay;
                }
            }
        }

        if (fasterSpawnTimer > float.Epsilon)
        {
            fasterSpawnTimer -= Time.deltaTime;

            if (fasterSpawnTimer <= float.Epsilon)
            {
                fasterSpawnTimer = -1f;

                if (slowerSpawnTimer <= float.Epsilon)
                {
                    levelController.spawnDelay = spawnDelay;
                }
            }
        }

        if (speedUpTimer > float.Epsilon)
        {
            speedUpTimer -= Time.deltaTime;

            if (speedUpTimer <= float.Epsilon)
            {
                speedUpTimer = -1f;

                ChangeSpeed(true, false);
            }
        }

        if (slowDownTimer > float.Epsilon)
        {
            slowDownTimer -= Time.deltaTime;

            if (slowDownTimer <= float.Epsilon)
            {
                slowDownTimer = -1f;

                ChangeSpeed(false, false);
            }
        }

        if (invulnerabilityTimer > float.Epsilon)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer <= float.Epsilon)
            {
                invulnerabilityTimer = -1f;

                invulnerable = false;

                invulnerabilityEffectAC.SetTrigger("NotActive");
            }
        }

        if (greaterMultiplierTimer > float.Epsilon)
        {
            greaterMultiplierTimer -= Time.deltaTime;

            background.transform.Rotate(0f, 0f, 5f);

            if (greaterMultiplierTimer <= float.Epsilon)
            {
                greaterMultiplierTimer = -1f;

                multiplier = 1f; 
            }
        }
    }

    #endregion

    #region Public functions

    public void CollectPowerUp(PowerUp powerUp)
    {
        if (powerUp != null)
        {
            collectedPowerUps.Add(powerUp);
        }
    }

    public void ResetPowerUps()
    {
        if (collectedPowerUps.Count > 0)
            collectedPowerUps.Clear();
    }

    public void LaunchPowerUps()
    {
        if (collectedPowerUps.Count > 0)
        {
            for (int i = 0; i < collectedPowerUps.Count; i++)
            {
                switch(collectedPowerUps[i].identifier)
                {
                    case "FirstAidKit":
                        if (levelController.health < 5)
                            levelController.health += 1;
                        break;

                    case "FasterCollector":
                        fasterCollectorTimer += collectedPowerUps[i].duration;
                        rescaleFactor = 0.1f;
                        break;

                    case "FasterSpawn":
                        fasterSpawnTimer += collectedPowerUps[i].duration;
                        levelController.spawnDelay = fasterSpawnDelay;
                        break;

                    case "SlowerSpawn":
                        slowerSpawnTimer += collectedPowerUps[i].duration;
                        levelController.spawnDelay = slowerSpawnDelay;
                        break;

                    case "SpeedUp":
                        speedUpTimer += collectedPowerUps[i].duration;
                        ChangeSpeed(true, true);
                        break;

                    case "SlowDown":
                        slowDownTimer += collectedPowerUps[i].duration;
                        ChangeSpeed(false, true);
                        break;

                    case "Invulnerability":
                        invulnerabilityTimer += collectedPowerUps[i].duration;
                        invulnerable = true;
                        invulnerabilityEffectAC.SetTrigger("Active");
                        break;

                    case "GreaterMultiplier":
                        greaterMultiplierTimer += collectedPowerUps[i].duration;
                        multiplier = 1.5f;
                        break;
                }
            }

            collectedPowerUps.Clear();
        }
    }

    #endregion

    #region Private functions

    void ChangeSpeed(bool up, bool on)
    {
        Collectable[] collectables = GameObject.FindObjectsOfType<Collectable>();

        if (on)
        {
            if (up)
            {
                foreach (Collectable col in collectables)
                {
                    if (!col.speededUp)
                        col.GetComponent<Rigidbody>().velocity *= 2f;
                }
            }
            else
            {
                foreach (Collectable col in collectables)
                {
                    if (!col.slowedDown)
                        col.GetComponent<Rigidbody>().velocity *= 0.5f;
                }
            }
        }
        else
        {
            if(up)
            {
                foreach (Collectable col in collectables)
                {
                    if (col.speededUp)
                        col.GetComponent<Rigidbody>().velocity *= 0.5f;
                }
            }
            else
            {
                foreach (Collectable col in collectables)
                {
                    if (col.slowedDown)
                        col.GetComponent<Rigidbody>().velocity *= 2f;
                }
            }
        }

    }

    #endregion
}
