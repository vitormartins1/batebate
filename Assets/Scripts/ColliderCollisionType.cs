using UnityEngine;
using System.Collections;

public class ColliderCollisionType : MonoBehaviour {
	
	public COLL_TYPE type;
	public CollidersManager manager;
	
	public enum COLL_TYPE
	{
		DOMINANTE,
		RECESSIVO,
		NENHUM,
	}
	
	void Start()
	{
		
	}
	
	void Update()
	{
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "collisionType" && manager.contarTempo == false)
		{
			//print(other.gameObject.name);
			manager.contarTempo = true;
			manager.type = this.type;
		}
    }
}
