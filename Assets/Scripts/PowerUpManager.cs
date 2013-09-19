using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
	public GameObject lentoPrefab;
	public GameObject escudoPrefab;
	public GameObject turboPrefab;
	public GameObject invisivelPrefab;
	
	public float time;
	public float timeToSpawn;
	
	public Collider randomArea;
	
	void Start ()
	{
	
	}
	
	void Update ()
	{
		time += Time.deltaTime;
		
		if (time >= timeToSpawn)
		{
			SpawnPowerUp();
		}
	}
	
	public Vector3 RandomPosition()
	{
		Vector3 rp = new Vector3(Random.Range(randomArea.bounds.min.x, randomArea.bounds.max.x) ,randomArea.transform.position.y + 2, Random.Range(randomArea.bounds.min.z, randomArea.bounds.max.z));
		return rp;
	}
	
	void SpawnPowerUp()
	{
		int i = Random.Range(0, 4);
		
		switch (i)
		{
			case 0:
			GameObject LentoPU = (GameObject)Instantiate(lentoPrefab, RandomPosition(), lentoPrefab.transform.rotation);
			break;
			
			case 1:
			GameObject EscudoPU = (GameObject)Instantiate(escudoPrefab, RandomPosition(), escudoPrefab.transform.rotation);
			break;
			
			case 2:
			GameObject TurboPU = (GameObject)Instantiate(turboPrefab, RandomPosition(), turboPrefab.transform.rotation);
			break;
			
			case 3:
			GameObject invisivelPU = (GameObject)Instantiate(invisivelPrefab, RandomPosition(), invisivelPrefab.transform.rotation);
			break;
		}
		
		time = 0;
	}
}
