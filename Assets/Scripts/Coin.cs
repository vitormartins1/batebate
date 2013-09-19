using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour 
{
	COIN_STATE state;
	float rotateVelocity;

	void Start () 
	{
		rotateVelocity = 35;
		state = COIN_STATE.UNSAVED;
	}

	void Update () 
	{
		switch (state)
		{
		case COIN_STATE.UNSAVED:
			TurnAround();
			break;
		case COIN_STATE.SAVED:
			break;
		}
	}

	void TurnAround()
	{
		transform.Rotate(0, rotateVelocity * Time.deltaTime, 0);
	}
	
	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "bumperCar")
		{
			collision.gameObject.GetComponentInChildren<CollidersManager>().coins++;
			collision.gameObject.GetComponentInChildren<CollidersManager>().UpdateText();
			Destroy(this.gameObject);
		}
	}

	public enum COIN_STATE
	{
		UNSAVED,
		SAVED,
	}
}
