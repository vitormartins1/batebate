using UnityEngine;
using System.Collections;

public class BumperCar : MonoBehaviour
/*	// These variables allow the script to power the wheels of the car.
	WheelCollider FrontLeftWheel;
	WheelCollider FrontRightWheel;

	// These variables are for the gears, the array is the list of ratios. The script
	// uses the defined gear ratios to determine how much torque to apply to the wheels.
	float[] GearRatio;
	public int CurrentGear = 0;
	public int AppropriateGear;

	// These variables are just for applying torque to the wheels and shifting gears.
	// using the defined Max and Min Engine RPM, the script can determine what gear the
	// car needs to be in.
	float EngineTorque = 230.0f;
	float MaxEngineRPM = 3000.0f;
	float MinEngineRPM = 1000.0f;
	private float EngineRPM = 0.0f;



	void  Start ()
	{
		// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
		//rigidbody.centerOfMass += Vector3(0, -1, .25);
		rigidbody.centerOfMass = new Vector3(rigidbody.centerOfMass.x, rigidbody.centerOfMass.y -1, rigidbody.centerOfMass.z + 0.25f);
    }

	void  Update ()
	{
	
		// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
		EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm)/2 * GearRatio[CurrentGear];
		ShiftGears();

		// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
		// up to twice it's pitch, where it will suddenly drop when it switches gears.
		audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0f ;
		// this line is just to ensure that the pitch does not reach a value higher than is desired.
		if ( audio.pitch > 2.0f ) {
			audio.pitch = 2.0f;
		}

		// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
		// multiplied by the user input variable.
		FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
		FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
			
		// the steer angle is an arbitrary value multiplied by the user input.
		FrontLeftWheel.steerAngle = 10 * Input.GetAxis("Horizontal");
		FrontRightWheel.steerAngle = 10 * Input.GetAxis("Horizontal");
	}

	void  ShiftGears (){
		// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
		// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
		if ( EngineRPM >= MaxEngineRPM ) {
			AppropriateGear = CurrentGear;
			
			for ( int i= 0; i < GearRatio.Length; i ++ ) {
				if ( FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
		
		if ( EngineRPM <= MinEngineRPM ) {
			AppropriateGear = CurrentGear;
			
			for ( int j= GearRatio.Length-1; j >= 0; j -- ) {
				if ( FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM ) {
					AppropriateGear = j;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
	}
}*/

{
	public BUMPERCAR state;
	
	public float time;
	public float timeToBackNormal;
	public float life;
	public int coins;
	public float initialVelocity;
	public float velocity;
	public float velocityRotation;
	public GameObject coinPrefab;
	
	public GameObject borrachao;
	public GameObject carenagem;
	public GameObject volante;
	
	public Material borrachaMat;
	public Material carenagemMat;
	public Material volanteMat;
	public Material translucido;
	
	void Start ()
	{
		rigidbody.centerOfMass = new Vector3(0, 0, 0.5f);
		life = 100;
		coins = 100;
		velocityRotation = 5f;
		velocity = 600f;
		initialVelocity = velocity;
		state = BUMPERCAR.NORMAL;
		borrachaMat = borrachao.renderer.material;
		carenagemMat = carenagem.renderer.material;
		volanteMat = volante.renderer.material;
		timeToBackNormal = 7;
	}
	
	void FixedUpdate()
	{
		//rigidbody.velocity = new Vector3(0,rigidbody.velocity.y, rigidbody.velocity.z);
		
//		float velz = Input.GetAxis("Vertical") * velocity;
//		float velx = Input.GetAxis("Horizontal") * velocity;
//		
//		rigidbody.AddForce(velx, 0, velz);
		
		
		/*if (Input.GetKey(KeyCode.W))
		{
			rigidbody.AddRelativeForce(new Vector3(0, 0, velocity), ForceMode.VelocityChange);
		}
		
		if (Input.GetKey(KeyCode.S))
		{
			rigidbody.AddRelativeForce(new Vector3(0, 0, -velocity), ForceMode.VelocityChange);
		}*/
    }
	
	void Update ()
	{
		switch (state)
		{
			case BUMPERCAR.NORMAL:
			//print("normal");
		break;
			case BUMPERCAR.ESCUDO:
			time += Time.deltaTime;
			
			if (time >= timeToBackNormal)
			{
				state = BUMPERCAR.NORMAL;
				time = 0;
			}
			//print("escudo");
		break;
			case BUMPERCAR.INVISIVEL:
			time += Time.deltaTime;
			
			if (time >= timeToBackNormal)
			{
				SetVisible();
				state = BUMPERCAR.NORMAL;
				time = 0;
			}
			//print("invisivel");
		break;
			case BUMPERCAR.LENTO:
			time += Time.deltaTime;
			
			if (time >= timeToBackNormal)
			{
				SetVelocity(false);
				state = BUMPERCAR.NORMAL;
				time = 0;
			}
			//print("lento");
		break;
			case BUMPERCAR.TURBO:
			time += Time.deltaTime;
			
			if (time >= timeToBackNormal)
			{
				SetVelocity(false);
				state = BUMPERCAR.NORMAL;
				time = 0;
			}
			//("turbo");
		break;
		}
		
		float velz = Input.GetAxis("Vertical") * velocity;
		float velx = Input.GetAxis("Horizontal") * velocityRotation;
		
		rigidbody.AddRelativeForce(0, 0, velz);
		//rigidbody.AddRelativeTorque(0, velx, 0);
		transform.Rotate(0,velx,0);
		
//		if (Input.GetKey(KeyCode.A))
//		{
//			transform.Rotate(0,-velocityRotation * Time.deltaTime,0);
//		}
//		
//		if (Input.GetKey(KeyCode.D))
//		{
//			transform.Rotate(0,velocityRotation * Time.deltaTime,0);
//		}
	}

	void OnCollisionEnter (Collision collision)
	{
		// Verificamos a tag do objeto
		// Logo apos isso verificamos se o objeto que ele colidiu deu o dano ou recebeu o dano
		if (collision.gameObject.tag == "bumperCar")// && collision.rigidbody.velocity.magnitude < this.rigidbody.velocity.magnitude)
		{
//			if (collision.gameObject.GetComponent<BumperCar>())
//				collision.gameObject.GetComponent<BumperCar>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
//			else if (collision.gameObject.GetComponent<BumperCar2>())
//				collision.gameObject.GetComponent<BumperCar2>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
//			else if (collision.gameObject.GetComponent<BumperCar3>())
//				collision.gameObject.GetComponent<BumperCar3>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
//			else if (collision.gameObject.GetComponent<BumperCar4>())
//				collision.gameObject.GetComponent<BumperCar4>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
//			else
//				collision.gameObject.GetComponent<BumperCarIA>().BumperCarCrash(Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
			//collision.rigidbody.AddForce(Vector3.up * collision.relativeVelocity.magnitude * 20, ForceMode.Impulse);
			//collision.rigidbody.AddRelativeForce(collision.contacts[0].normal.normalized * 300, ForceMode.Impulse);
			//collision.rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0);
			//print ("Vermelho: " + Vector3.Dot(collision.contacts[0].normal, collision.relativeVelocity) * rigidbody.mass);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "PowerUp")
		{
			switch (other.gameObject.GetComponent<PowerUp>().state)
			{
			case PowerUp.POWERUP.ESCUDO:
				this.state = BUMPERCAR.ESCUDO;
				SetVisible();
				SetVelocity(false);
				break;
			case PowerUp.POWERUP.INVISIVEL:
				this.state = BUMPERCAR.INVISIVEL;
				SetInvisible();
				SetVelocity(false);
				break;
			case PowerUp.POWERUP.LENTO:
				this.state = BUMPERCAR.LENTO;
				SetVisible();
				SetVelocity(true);
				break;
			case PowerUp.POWERUP.TURBO:
				this.state = BUMPERCAR.TURBO;
				SetVisible();
				SetVelocity(false);
				SetTurbo();
				break;
			}
			
			time = 0;
			Destroy(other.gameObject);
		}
	}

	// Este metodo e chamado pelo carro que bater no carro que contem essa classa.
	// O metodo recebe como parametro o valor de forca da colisao e aplica dano na vida do carro e tambem faz o carro perder moedas de acordo com o valor do dano.
	public void BumperCarCrash(float damage)
	{
		if (state != BUMPERCAR.ESCUDO)
		{
			if (coins > damage/2)
		    {
				coins -= (int)damage/2;
				GameObject go = (GameObject)Instantiate(coinPrefab, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), Quaternion.identity);
				//go.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Force);
				go.GetComponent<Rigidbody>().AddExplosionForce(20, go.transform.position, 15, 9);
			}
			else
				this.life -= damage/2;
		}
	}
	
	void SetTurbo()
	{
		velocity = 39;
	}
	
	void SetVelocity(bool lentidao)
	{
		if (lentidao)
		{
			velocity -= 10;
		}
		else
		{
			velocity = initialVelocity;
		}
	}
	
	void SetInvisible()
	{
		borrachao.renderer.material = translucido;
		carenagem.renderer.material = translucido;
		volante.renderer.material = translucido;
	}
	
	void SetVisible()
	{
		borrachao.renderer.material = borrachaMat;
		carenagem.renderer.material = carenagemMat;
		volante.renderer.material = volanteMat;
	}

	public enum BUMPERCAR
	{
		NORMAL,
		LENTO,
		INVISIVEL,
		TURBO,
		ESCUDO
	}
}
