using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyDropper : MonoBehaviour {

    public static EnemyDropper instance;

    public GameObject enemy;
    GameObject temp;
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

            yield return new WaitForSeconds(Random.Range(1f,5f));

            gameObject.transform.localPosition = Vector3.up;//new Vector3(0.7f - Random.Range(0.4f, 0.10f),transform.localPosition.y,0.7f - Random.Range(0.4f, 0.10f));
            }
        }

    // Update is called once per frame
    void Update () {
	
	}
}
