using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDropper : MonoBehaviour {

    public static EnemyDropper instance;
    public GameObject enemy;
    float maxEnemyColldownTime;     //Max duration before enemy drops
    GameObject temp;
    /// <summary>
    /// Object pooling enemies
    /// </summary>
   [HideInInspector] public  Queue<GameObject> enemies = new Queue<GameObject>();   

    public void Awake ()
        {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        }

    // Use this for initialization
    void Start () {

        maxEnemyColldownTime = 5f;

        for (int i = 0 ; i < 20 ; ++i)
            {
            temp = Instantiate(enemy);
            temp.SetActive(false);
            enemies.Enqueue(temp);
            }
	
	}

    public void StartEnemySpawns ()
        {
        StartCoroutine(SpawnEnemies());
        }

    IEnumerator SpawnEnemies ()
        {
        while (GameManager.instance.game == GameManager.GameState.GameRunning)
            {
            if (enemies.Count > 0)
                {
                temp = enemies.Dequeue();
                temp.transform.position = gameObject.transform.position;
                temp.SetActive(true);
                }

            yield return new WaitForSeconds(Random.Range(0.8f,maxEnemyColldownTime));
            if (maxEnemyColldownTime > 1f) ;
            maxEnemyColldownTime = maxEnemyColldownTime - 0.2f;

            gameObject.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f),1,Random.Range(-0.4f, 0.4f));    ///Spawn enemies from a random location
            }
        }

    // Update is called once per frame
    void Update () {
	
	}
}
