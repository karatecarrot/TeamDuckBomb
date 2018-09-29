using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner: MonoBehaviour 
{
	public int EnemiesAlive = 0;

	public Wave[] waves;

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;
	private float countdown = 2f;

	public Text waveCountdownText;

	private int waveIndex = 0;

    //public GameManager gameManager;

	void Update ()
	{
		if (EnemiesAlive > 0)
		{
			return;
		}

		if (waveIndex == waves.Length) 
		{
			//gameManager.WinLevel ();
			this.enabled = false;
		}

		if (countdown <= 0f)
        {
			StartCoroutine (SpawnWave ());
			countdown = timeBetweenWaves;
			return;
		}

		countdown -= Time.deltaTime;

		countdown = Mathf.Clamp (countdown, 0f, Mathf.Infinity);

		//waveCountdownText.text = string.Format ("Next Wave: " + "{0:00.00}", countdown);
	}

	IEnumerator SpawnWave ()
	{
		//PlayerStats.Rounds++;

		Wave wave = waves [waveIndex];

		EnemiesAlive = wave.count;

		for (int i = 0; i < wave.count; i++) 
		{
			SpawnEnemy (wave.enemy);
			yield return new WaitForSeconds (1f / wave.rate); 
		}

		waveIndex++;
	}

	void SpawnEnemy (GameObject enemy)
	{
        int spawnNumber = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnNumber];

        Instantiate(enemy, RandomPointInBox(spawnPoint.position, spawnPoint.localScale), spawnPoint.rotation);
    }

    private static Vector3 RandomPointInBox(Vector3 center, Vector3 size)
    {

        return center + new Vector3((Random.value - 0.5f) * size.x, 0, (Random.value - 0.5f) * size.z);
    }
}