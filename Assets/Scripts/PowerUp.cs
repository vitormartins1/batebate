using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
	public POWERUP state;
	float rotateVelocity;
	
	void Start ()
	{
		rotateVelocity = 35;
	}
	
	void Update ()
	{
		Act ();
	}
	
	void Act()
	{
		TurnAround();
		
		switch (state)
		{
		case POWERUP.ESCUDO:
			break;
		case POWERUP.INVISIVEL:
			break;
		case POWERUP.LENTO:
			break;
		case POWERUP.TURBO:
			break;
		}
	}
	
	void TurnAround()
	{
		transform.Rotate(0, rotateVelocity * Time.deltaTime, 0);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		/*if (collision.gameObject.tag == "chao")
		{
			
			this.GetComponent<Rigidbody>().useGravity = false;
			this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
			this.GetComponent<Collider>().isTrigger = true;
		}*/
	}
	
	void RandomState()
	{
		int i = Random.Range(0, 4);
		
		switch (i)
		{
			case 0:
			state = POWERUP.ESCUDO;
			break;
			
			case 1:
			state = POWERUP.INVISIVEL;
			break;
			
			case 2:
			state = POWERUP.LENTO;
			break;
			
			case 3:
			state = POWERUP.TURBO;
			break;
		}
	}
	
	public enum POWERUP
	{
		LENTO,
		INVISIVEL,
		TURBO,
		ESCUDO,
		//COLETOR,
	}
}
