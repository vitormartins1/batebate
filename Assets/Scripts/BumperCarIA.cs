using UnityEngine;
using System.Collections;

public class BumperCarIA : BumperCarBase
{
	public BUMPERCAR_IA state;

	float timeToChangeTarget;
	float timeToChangeState;
	float distanceChasing;
	public float life;
	public int coins;
	
	GameObject currentChesed;
	GameObject currentCoinChesed;
	public GameObject coinPrefab;
	public bool colidindo;
	
	void Start ()
	{
		state = BUMPERCAR_IA.CHASING;
		rigidbody.centerOfMass = new Vector3(0, 0, 0.5f);
		distanceChasing = 1000;
		life = 100;
		coins = 100;
		colidindo = false;
		StartCar();
		ChooseNearestTarget();
	}
	
	void FixedUpdate()
	{
//		switch (state)
//		{
//		case BUMPERCAR_IA.CHASING:
//			Chasing(1);
//			break;
//		case BUMPERCAR_IA.RUNNING:
//			break;
//		case BUMPERCAR_IA.COLLECTING:
//			Collect(1);
//			break;
//		}
    }
	
	void Update ()
	{
		switch (state)
		{
		case BUMPERCAR_IA.CHASING:
			Chasing(0);
			break;
		case BUMPERCAR_IA.RUNNING:
			break;
		case BUMPERCAR_IA.COLLECTING:
			Collect(0);
			break;
		}
		
		timeToChangeState += Time.deltaTime;
		
		if (timeToChangeState >= 8)
		{
			ChangeState();
		}
	}
	
	
	// Funcao perseguir, persegue o carro definido no metodo ChooseNearestTarget()
	// De acordo com o valor de i (0 ou 1) ele vai executar comandos para o Update() ou para o FixedUpdate(0
	void Chasing (int i)
	{
		if (!currentChesed)
		{
			ChooseNearestTarget ();
		}
		
		timeToChangeTarget += Time.deltaTime;
		
		if (timeToChangeTarget >= 5)
		{
			ChooseNearestTarget ();
			timeToChangeTarget = 0;
		}
		
		if (currentChesed)
		{
			Vector3 relative = transform.InverseTransformPoint (currentChesed.transform.position);
			float angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
			
			//transform.Rotate(0,angle,0);
			
			UpdateInputs(0.4f, 0);
			FrontLeftWheel.steerAngle = angle;
			UpdateCar();
		}
		
//		if (i == 0)
//		{
//			if (!currentChesed)
//			{
//				ChooseNearestTarget ();
//			}
//			
//			timeToChangeTarget += Time.deltaTime;
//			
//			if (timeToChangeTarget >= 5)
//			{
//				ChooseNearestTarget ();
//				timeToChangeTarget = 0;
//			}
//			
//			if (currentChesed)
//			{
//				Vector3 relative = transform.InverseTransformPoint (currentChesed.transform.position);
//				float angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
//				
//				//transform.Rotate(0,angle,0);
//				
//				UpdateInputs(0.4f, angle);
//				UpdateCar();
//			}
//		}

//		if (i == 1)
//		{
//			if (currentChesed)
//			{
//				//rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0);
//				rigidbody.AddRelativeForce(new Vector3(0, 0, 300f));
//			}
//		}
	}

	//Funcao que escolhe o alvo mais proximo
	void ChooseNearestTarget()
	{
		/*for (int i = 0; i < BumperCarsManager.Instance.bumperCars.Count; i++) 
		{
			if (BumperCarsManager.Instance.bumperCars [i] != this.gameObject)
			{
				float currentDistance = Vector3.Distance (this.transform.position, BumperCarsManager.Instance.bumperCars [i].transform.position);

				if (distanceChasing > currentDistance)
				{
					distanceChasing = currentDistance;
					currentChesed = BumperCarsManager.Instance.bumperCars [i];
					BumperCarsManager.Instance.chasedBumperCars.Add(BumperCarsManager.Instance.bumperCars [i]);
				}
			}
		}*/

		/*for (int i = 0; i < BumperCarsManager.Instance.bumperCars.Count; i++) 
		{
			if (BumperCarsManager.Instance.bumperCars [i] != this.gameObject)
			{
				//if (!BumperCarsManager.Instance.chasedBumperCars.Contains(BumperCarsManager.Instance.bumperCars [i]))
				//{
					float currentDistance = Vector3.Distance (this.transform.position, BumperCarsManager.Instance.bumperCars [i].transform.position);

					currentChesed = BumperCarsManager.Instance.bumperCars [i];
					//BumperCarsManager.Instance.chasedBumperCars.Add(BumperCarsManager.Instance.bumperCars [i]);
				//}
			}
		}*/
		
		if (BumperCarsManager.Instance.bumperCars.Count > 0)
			currentChesed = BumperCarsManager.Instance.bumperCars[Random.Range(0, BumperCarsManager.Instance.bumperCars.Count)];
	}
	
	// O certo a fazer depois e ter uma lista de moedas e pegar a moeda mais proxima de voce.
	void Collect(int i)
	{
		if (!currentCoinChesed)
		{
			if (GameObject.Find("Coin(Clone)"))
				currentCoinChesed = GameObject.Find("Coin(Clone)");
			else
				ChangeState();
		}
		else
		{
			Vector3 relative = transform.InverseTransformPoint (currentCoinChesed.transform.position);
			float angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
			
			//transform.Rotate(0,angle,0);
			
			UpdateInputs(0.4f, 0);
			FrontLeftWheel.steerAngle = angle;
			UpdateCar();
		}
		
//		if (i == 0)
//		{
//			if (!currentCoinChesed)
//			{
//				if (GameObject.Find("Coin(Clone)"))
//					currentCoinChesed = GameObject.Find("Coin(Clone)");
//				else
//					ChangeState();
//			}
//			else
//			{
//				Vector3 relative = transform.InverseTransformPoint (currentCoinChesed.transform.position);
//				float angle = Mathf.Atan2 (relative.x, relative.z) * Mathf.Rad2Deg;
//				
//				//transform.Rotate(0,angle,0);
//				
//				UpdateInputs(0.4f, angle);
//				UpdateCar();
//			}
//		}
//		else if (i == 1)
//		{
//			if (currentCoinChesed)
//			{
//				rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0);
//				rigidbody.AddRelativeForce(new Vector3(0, 0, 19f), ForceMode.VelocityChange);
//			}
//		}
	}
	
//	void OnCollisionEnter (Collision collision)
//	{
//		// Verificamos a tag do objeto
//		// Logo apos isso verificamos se o objeto que ele colidiu deu o dano ou recebeu o dano
//		if (collision.gameObject.tag == "bumperCar")// && collision.rigidbody.velocity.magnitude < this.rigidbody.velocity.magnitude)
//		{
////			if (collision.gameObject.GetComponent<BumperCar>())
////				collision.gameObject.GetComponent<BumperCar>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
////			else if (collision.gameObject.GetComponent<BumperCar2>())
////				collision.gameObject.GetComponent<BumperCar2>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
////			else if (collision.gameObject.GetComponent<BumperCar3>())
////				collision.gameObject.GetComponent<BumperCar3>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
////			else if (collision.gameObject.GetComponent<BumperCar4>())
////				collision.gameObject.GetComponent<BumperCar4>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
////			else
////				collision.gameObject.GetComponent<BumperCarIA>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
//			//collision.rigidbody.AddForce(Vector3.forward * collision.relativeVelocity.magnitude, ForceMode.Impulse);
//			//print ("Vermelho: " + Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
//		}	
//	}
//	
//	void OnCollisionStay (Collision collision)
//	{
//		if (collision.gameObject.tag == "bumperCar")// && collision.rigidbody.velocity.magnitude < this.rigidbody.velocity.magnitude)
//		{
//			colidindo = true;
//		}
//	}
//	
//	void OnCollisionExit (Collision collision)
//	{
//		if (collision.gameObject.tag == "bumperCar")// && collision.rigidbody.velocity.magnitude < this.rigidbody.velocity.magnitude)
//		{
//			colidindo = false;
//		}
//	}
//	
//	public void BumperCarCrash(float damage)
//	{
//		if (coins > damage/2)
//	    {
//			coins -= (int)damage/2;
//			GameObject go = (GameObject)Instantiate(coinPrefab, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), Quaternion.identity);
//			//go.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Force);
//			go.GetComponent<Rigidbody>().AddExplosionForce(20, go.transform.position, 15, 9);
//		}
//		else
//			this.life -= damage/2;
//	}
	
	void ChangeState()
	{
		int o = Random.Range(0, 2);
		
		currentChesed = null;
		
		switch (o)
		{
			case 0:
			state = BUMPERCAR_IA.CHASING;
		break;
			case 1:
			state = BUMPERCAR_IA.COLLECTING;
		break;
			case 2:
			state = BUMPERCAR_IA.RUNNING;
		break;
		}
		
		timeToChangeState = 0;
	}

	public enum BUMPERCAR_IA
	{
		CHASING,
		RUNNING,
		COLLECTING,
	}
}