using UnityEngine;
using System.Collections;

public class SpawnGameObjects : MonoBehaviour {

	public Pair[] spawnPrefabs;

	public float minSecondsBetweenSpawning = 3.0f;
	public float maxSecondsBetweenSpawning = 6.0f;
	public float deltaX = 0.0f;
	public float deltaZ = 0.0f;
    public int maxEnemies;

    public GameObject comboTextPlayer1;
    public GameObject comboTextPlayer2;
	public GameObject spiderTrail;
	
	private float savedTime;
	private float secondsBetweenSpawning;
    private int enemyNumber;

	// Use this for initialization
	void Start () {
		savedTime = Time.time;
		secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
        enemyNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - savedTime >= secondsBetweenSpawning) // is it time to spawn again?
		{
            if (enemyNumber < maxEnemies)
            {
                MakeThingToSpawn();
                enemyNumber++;
            }
			savedTime = Time.time; // store for next spawn
			secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
		}	
	}

	void MakeThingToSpawn()
	{
		// create a new gameObject
		float newX = transform.position.x + Random.Range(-deltaX, deltaX + 1);
		float newZ = transform.position.z + Random.Range(-deltaZ, deltaZ + 1);
        GameObject prefabToSpawn = spawnPrefabs[RandomIndex()].enemy;
		GameObject clone = Instantiate(prefabToSpawn, new Vector3(newX, transform.position.y + 1, newZ), transform.rotation) as GameObject;
        clone.GetComponent<Spider>().SetSpawner(this);
	}

    int RandomIndex()
    {
        int sum = 0;
        for (int i = 0; i < spawnPrefabs.Length; i++)
        {
            sum += spawnPrefabs[i].weigth;
        }
        int index = Random.Range(0, sum);
        int lessThan = 0;

        for (int j = 0; j < spawnPrefabs.Length; j++)
        {
            lessThan += spawnPrefabs[j].weigth;
            if (index < lessThan)
                return j;
        }

        return -1;
    }

    public void DeadEnemy()
    {
        enemyNumber--;
    }

    [System.Serializable]
    public class Pair
    {
        public GameObject enemy;
        public int weigth;
    }
}
