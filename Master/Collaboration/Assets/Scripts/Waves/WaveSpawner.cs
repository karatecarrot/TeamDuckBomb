using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveSpawner: MonoBehaviour 
{
	public int EnemiesAlive = 0;
    public AudioClip beginWave;
    public AudioClip endWave;

	//public Wave[] waves;
    public List<Wave> waves = new List<Wave>();

	public Transform[] spawnPoints;

	public float timeBetweenWaves = 5f;
	private float countdown = 2f;

	public Text waveCountdownText;

	private int waveIndex = 0;
    private int bossWave = 10;

    //public GameManager gameManager;

	void Update ()
	{
		if (EnemiesAlive > 0)
		{
			return;
		}

		if (countdown <= 0f)
        {
            GetComponent<AudioSource>().PlayOneShot(beginWave);
            if (waveIndex == waves.Count)
            {
                waves.Add(new Wave());
                Wave _Wave = waves[waves.Count - 1];
                _Wave.count = waves[waves.Count - 2].count + 1;
                _Wave.rate = 1;

                for (int i = 0; i < Random.Range(1, 5); i++)
                {
                    _Wave.enemies.Add(_GameManager.instance.enemies[Random.Range(0, _GameManager.instance.enemies.Count)]);
                }

                if (waveIndex == bossWave)
                {
                    _Wave.enemies.Add(_GameManager.instance.bossBot);
                    bossWave += 10;
                }
            }

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
			SpawnEnemy (wave.enemies[Random.Range(0, wave.enemies.Count)]);
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