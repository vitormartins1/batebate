using UnityEngine;
using System.Collections;

public class CollidersManager : MonoBehaviour {
	
	public ColliderCollisionType.COLL_TYPE type;
	public float time;
	public bool contarTempo;
	public float coins;
	public GameObject coinPrefab;
	private int coinsContador;
	public int moedasADropar;
	public GUIText text;
	
	public enum COLL_TYPE
	{
		DOMINANTE,
		RECESSIVO,
		NENHUM,
	}
	
	void Start () {
	
		contarTempo = false;
		type = ColliderCollisionType.COLL_TYPE.NENHUM;
		coins = 20;
		moedasADropar = 5;
		UpdateText();
	}
	
	void Update () {
		
		switch (type) {
			case ColliderCollisionType.COLL_TYPE.DOMINANTE:
			type = ColliderCollisionType.COLL_TYPE.NENHUM;
		break;
			case ColliderCollisionType.COLL_TYPE.RECESSIVO:
			
			if (coins > 0 && coinsContador < moedasADropar)
			{
				coins--;
				coinsContador++;
				
				GameObject go = (GameObject)Instantiate(coinPrefab, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y + 3, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
				go.GetComponent<Rigidbody>().AddExplosionForce(6, go.transform.position, 100, 10, ForceMode.Impulse);	
				//go.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Force);
				MuzzleyManager.Instance.coins.Add(go);
				UpdateText();
			}
			
			if (coinsContador >= moedasADropar)
			{
				coinsContador = 0;
				type = ColliderCollisionType.COLL_TYPE.NENHUM;
			}
		break;
		}
		
		if (contarTempo)
		{
			time += Time.deltaTime;
		}
		
		if (time >= 1)
		{
			time = 0;
			contarTempo = false;
		}
	}
	
	public void UpdateText()
	{
		if (text != null)
			text.text = coins.ToString();
	}
}
