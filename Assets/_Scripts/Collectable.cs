using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Collectable : MonoBehaviour 
{
    public float points = 50f;

    private Rigidbody rb;
    private Vector3 destination;
    private float speed;

    private ParticleSystem effect;
	
	void Awake() 
    {
        float scale = Random.Range(-0.05f, 0.05f);
        transform.localScale += new Vector3(scale, scale, scale);

        effect = GetComponentInChildren<ParticleSystem>();

        rb = GetComponent<Rigidbody>();

        rb.angularVelocity = new Vector3(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f));

        float destinationX = Random.Range(-1.5f, 1.5f);
        float destinationY = Random.Range(-2.0f, 4.0f);

        destination = new Vector3(destinationX, destinationY, 0f);

        speed = Random.Range(0.7f, 3.5f);

        Vector3 velocityVector = destination - transform.position;
        velocityVector.Normalize();

        rb.velocity = velocityVector * speed;
	}
	
	// Update is called once per frame
	void Update () 
    {
	 
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Border")
        {
            GameObject.Find("LevelController").GetComponent<LevelController>().missedItems += 1;
            Destroy(gameObject);
        }
    }

    public void PlayExplosion()
    {
        effect.Play();
    }
}
