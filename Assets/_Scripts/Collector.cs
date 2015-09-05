using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour 
{
    public float rescaleFactor = 0.03f;

    public bool active = false;
    public bool launched = false;

    public int launchDelay = 0;

    [HideInInspector]
    public float points = 0f;

    [HideInInspector]
    public int hpLoss = 0;

    [HideInInspector]
    public int collectedCounter = 0;

    private ParticleSystem[] effects;
    private float effectScaleFactor = 0.1f;

	// Use this for initialization
	void Awake() 
    {
        effects = GetComponentsInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update() 
    {
	    if (active)
        {
            if (gameObject.transform.localScale.x < 4.9f)
                gameObject.transform.localScale += new Vector3(rescaleFactor, rescaleFactor, rescaleFactor);
        }
	}

    public void Launch()
    {
        gameObject.tag = "CollectorLaunched";

        float scale = gameObject.transform.localScale.x;
        foreach(ParticleSystem effect in effects)
        {
            effect.startSize = scale * effectScaleFactor;
        }

        effects[0].Play();
    }

    void OnTriggerStay(Collider other)
    {
        if (gameObject.tag == "CollectorLaunched")
        {
            if (other.tag == "Good")
            {
                points += other.gameObject.GetComponent<Collectable>().points;
                collectedCounter += 1;
            }
            else if (other.tag == "Bad")
            {
                hpLoss += 1;
                collectedCounter += 1;
            }

            other.gameObject.GetComponent<Collectable>().PlayExplosion();
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Collider>().enabled = false;

            Destroy(other.gameObject, 1f);
        }  
    }


}
