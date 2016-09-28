using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformScript : MonoBehaviour {

    public static PlatformScript instance;

    [Range(0f,10f)] public float risingSpeed;
    List<Transform> subPlatforms = new List<Transform>();

    public void Awake ()
        {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        }

    // Use this for initialization
    void Start () {

        foreach (Transform child in transform)
            if (child.gameObject.tag == "Platform")
                {
                subPlatforms.Add(child);
                }
          
	}

    public void OnTriggerExit (Collider other)
        {
        if (other.GetComponent<Rigidbody>() && other.GetComponent<BumperBaseScript>())  //If a bumper falls off
            {
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.GetComponent<BumperBaseScript>().floored = false;

            if (other.GetComponent<Movement>() && !other.GetComponent<Movement>().isPlayer)  //If it is an AI
                {
                other.gameObject.SetActive(false);
                EnemyDropper.instance.enemies.Enqueue(other.gameObject);
                GameManager.instance.AddScore(10);

                }
            else if (other.GetComponent<Movement>() && other.GetComponent<Movement>().isPlayer)  //If it is player
                {
                GameManager.instance.GameEnd();
                }
            }

        }

	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Platform moves up, and start dropping random platforms
    /// </summary>
    public void StartPlatforms ()
        {
        StartCoroutine(Rise());
        StartCoroutine(RandomDrops());
        }
    /// <summary>
    /// Drops a random number of platforms to create pitfalls
    /// </summary>
    /// <returns></returns>
    IEnumerator RandomDrops ()
        {

        int numberOfPlatformsToDrop =0;
        List<Transform> droppedPlatforms = new List<Transform>();

        while (GameManager.instance.game == GameManager.GameState.GameRunning)
            {
            droppedPlatforms = new List<Transform>();
            numberOfPlatformsToDrop = Random.Range(0,20);

            ///Randomly choose which platforms to drop
            for (int i = 0 ; i < numberOfPlatformsToDrop ; ++i)
                droppedPlatforms.Add(subPlatforms[Random.Range(0,subPlatforms.Count)]);

            foreach (Transform s in droppedPlatforms)
                s.localPosition -= Vector3.up * 5f;

            ///How long the platform stays down
            yield return new WaitForSeconds(Random.Range(3f,10f));

            foreach (Transform s in droppedPlatforms)
                s.localPosition += Vector3.up * 5f;


            ///Next duration when the platforms drop
            yield return new WaitForSeconds(Random.Range(2f,10f));

            }
        }

    /// <summary>
    /// The entire Platform rises over time
    /// </summary>
    /// <returns></returns>
    IEnumerator Rise ()
        {
        while (GameManager.instance.game == GameManager.GameState.GameRunning)
            {
            transform.Translate(Vector3.up * Time.deltaTime * risingSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
            
            }
        }

}
