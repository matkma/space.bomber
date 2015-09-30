using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collector : MonoBehaviour
{
    #region variables

    public float rescaleFactor = 0.03f;

    public bool active = false;
    public bool launched = false;

    public int launchDelay = 0;

    [HideInInspector]
    public int explosionDuration = -1;

    [HideInInspector]
    public float points = 0f;

    [HideInInspector]
    public int hpLoss = 0;

    [HideInInspector]
    public int collectedCounter = 0;

    [HideInInspector]
    public bool rasta = false;
    private bool yellow = false;
    private bool red = false;
    private bool green = false;

    private ParticleSystem[] effects;
    private PowerUpSystem powerUpSystem;
    private Text explosionCounter;

    private float explosionTimer = -1f;
    private float effectScaleFactor = 0.1f;

    private AudioSource source;

    #endregion

    #region Awake function

    void Awake() 
    {
        effects = GetComponentsInChildren<ParticleSystem>();
        powerUpSystem = GameObject.Find("LevelController").GetComponent<PowerUpSystem>();

        explosionCounter = GameObject.Find("ExplosionCounter").GetComponent<Text>();

        source = gameObject.GetComponent<AudioSource>();
	}

    #endregion

    #region Update function

    void Update() 
    {
	    if (active)
        {
            if (gameObject.transform.localScale.x < 4.9f)
                gameObject.transform.localScale += new Vector3(rescaleFactor + powerUpSystem.rescaleFactor,
                                                                rescaleFactor + powerUpSystem.rescaleFactor,
                                                                rescaleFactor + powerUpSystem.rescaleFactor);
                
            else if (this.tag == "Collector")
            {
                if (explosionTimer == -1f)
                {
                    explosionTimer = 3f;

                    explosionCounter.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
                    explosionCounter.text = "3";

                    explosionCounter.GetComponent<Animator>().SetTrigger("count");
                }

                else
                {
                    explosionTimer -= Time.deltaTime;
                    explosionCounter.text = ((int)Mathf.Ceil(explosionTimer)).ToString();
                
                    if (explosionTimer <= float.Epsilon)
                    {
                        explosionTimer = -1f;

                        active = false;
                        launched = true;

                        GameObject.Find("LevelController").GetComponent<LevelController>().ResetTouches();

                        Launch();
                    }
                }
            }
        }

        if (this.tag == "CollectorLaunched" && explosionDuration != 0)
        {
            explosionDuration -= 1;
        }
	}

    #endregion

    #region Public functions

    public void Launch()
    {
        gameObject.tag = "CollectorLaunched";

        float scale = gameObject.transform.localScale.x;
        foreach(ParticleSystem effect in effects)
        {
            effect.startSize = scale * effectScaleFactor;
        }

        source.Play();

        explosionCounter.GetComponent<Animator>().SetTrigger("stop");

        explosionDuration = 3;

        effects[0].Play();
    }

    #endregion

    #region Private functions

    void OnTriggerStay(Collider other)
    {
        if (gameObject.tag == "CollectorLaunched")
        {
            if (other.tag == "Good")
            {
                points += other.gameObject.GetComponent<Collectable>().points;
                collectedCounter += 1;

                if (other.gameObject.GetComponent<Collectable>().color != null)
                {
                    if (other.gameObject.GetComponent<Collectable>().color == "Yellow")
                    {
                        yellow = true;
                    }
                    else if (other.gameObject.GetComponent<Collectable>().color == "Green")
                    {
                        green = true;
                    }
                }    
            }
            else if (other.tag == "Bad")
            {
                hpLoss += 1;
                collectedCounter += 1;
                red = true;
            }
            else if (other.tag == "PowerUp")
            {
                points += other.gameObject.GetComponent<PowerUp>().points;
                collectedCounter += 1;

                if (powerUpSystem != null)
                    powerUpSystem.CollectPowerUp(other.gameObject.GetComponent<PowerUp>());
            }

            other.gameObject.GetComponent<Collectable>().PlayExplosion();
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Collider>().enabled = false;

            if (red && yellow && green)
            {
                rasta = true;
            }

            Destroy(other.gameObject, 1f);
        }
    }

    #endregion
}
