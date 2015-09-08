using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Collectable : MonoBehaviour
{
    #region variables

    public float points = 50f;

    [HideInInspector]
    public bool speededUp = false;

    [HideInInspector]
    public bool slowedDown = false;

    protected Rigidbody rb;
    protected Vector3 destination;
    protected float speed;

    protected ParticleSystem effect;

    #endregion

    #region Awake function

    protected virtual void Awake() 
    {
        float scale = Random.Range(-0.05f, 0.05f);
        transform.localScale += new Vector3(scale, scale, scale);

        effect = GetComponentInChildren<ParticleSystem>();

        rb = GetComponent<Rigidbody>();

        rb.angularVelocity = new Vector3(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f));

        float destinationX = Random.Range(-1.5f, 1.5f);
        float destinationY = Random.Range(-2.0f, 4.0f);

        destination = new Vector3(destinationX, destinationY, 0f);




        speed = Random.Range(0.7f, 3.0f);

        Vector3 velocityVector = destination - transform.position;
        velocityVector.Normalize();

        rb.velocity = velocityVector * speed;
	}

    #endregion

    #region Public functions

    public void PlayExplosion()
    {
        effect.Play();
    }

    #endregion

    #region Protected functions

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Border")
        {
            GameObject.Find("LevelController").GetComponent<LevelController>().missedItems += 1;
            Destroy(gameObject);
        }
    }

    #endregion
}
