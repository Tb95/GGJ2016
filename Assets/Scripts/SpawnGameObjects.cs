using UnityEngine;
using System.Collections;

public class SpawnGameObjects : MonoBehaviour {

	public GameObject spawnPrefab;

	public float minSecondsBetweenSpawning = 3.0f;
	public float maxSecondsBetweenSpawning = 6.0f;
	public float deltaX = 0.0f;
	public float deltaZ = 0.0f;
    public int maxEnemies;

	public GameObject comboText;
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
		GameObject clone = Instantiate(spawnPrefab, new Vector3(newX, transform.position.y + 1, newZ), transform.rotation) as GameObject;
        clone.GetComponent<Spider>().SetSpawner(this);
	}

    public void DeadEnemy()
    {
        enemyNumber--;
    }
}
