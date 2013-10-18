using UnityEngine;
using System.Collections;

public class BumperCarBase : MonoBehaviour
{
	// These variables allow the script to power the wheels of the car.
	public WheelCollider FrontLeftWheel;
	public WheelCollider FrontRightWheel;

	// These variables are for the gears, the array is the list of ratios. The script
	// uses the defined gear ratios to determine how much torque to apply to the wheels.
	public float[] GearRatio;
	public int CurrentGear = 0;
	public int AppropriateGear;
	
	public int indexPosicaoInicial;

	// These variables are just for applying torque to the wheels and shifting gears.
	// using the defined Max and Min Engine RPM, the script can determine what gear the
	// car needs to be in.
	public float EngineTorque = 230.0f;
	public float MaxEngineRPM = 3000.0f;
	public float MinEngineRPM = 1000.0f;
	private float EngineRPM = 0.0f;
	public float rotacaoAuto;
	private bool muzzleyOn;
	public float rotacaoRoda;
	public float aceleracaoRoda;
	public float powerUpTime;
	public float powerUpTimeInitial;
	public float multiplicadorDeVelocidade;
	
	public Texture lentoImg;
	public Texture invisivelImg;
	public Texture turboImg;
	public Texture escudoImg;
	public Texture coletorImg;
	
	public GameObject borrachao;
	public GameObject carenagem;
	public GameObject volante;
	
	public Material borrachaMat;
	public Material carenagemMat;
	public Material volanteMat;
	public Material translucido;
	
	public GUITexture puHud;
	public GUIText puTime;
	
	public STEERSIDE steerside;
	public BUMPERCAR carState;
	
	public void  StartCar ()
	{
		// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
		//rigidbody.centerOfMass += Vector3(0, -1, .25);
		rigidbody.centerOfMass = new Vector3(rigidbody.centerOfMass.x, rigidbody.centerOfMass.y -1, rigidbody.centerOfMass.z + 0.25f);
		muzzleyOn = false;
		rotacaoAuto = 28;
		carState = BUMPERCAR.NORMAL;
		powerUpTimeInitial = 16;
		multiplicadorDeVelocidade = 1;
		carenagemMat = carenagem.renderer.material;
		borrachaMat = borrachao.renderer.material;
		volanteMat = volante.renderer.material;
		AtivarHudPowerUp(false);
		if (steerside != STEERSIDE.IA)
		{
			steerside = STEERSIDE.FRENTE;
		}
    }

	public void UpdateCar ()
	{
	
		// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
		EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm)/2 * GearRatio[CurrentGear];
		//print(EngineRPM);
		ShiftGears();

		// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
		// up to twice it's pitch, where it will suddenly drop when it switches gears.
		audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0f ;
		// this line is just to ensure that the pitch does not reach a value higher than is desired.
		if ( audio.pitch > 2.0f ) {
			audio.pitch = 2.0f;
		}
		InputPlayer();
		
		CarState();
	}
	
	public void UpdateInputs(float aceleracao, float rotacao)
	{
		aceleracaoRoda = aceleracao * multiplicadorDeVelocidade;
		rotacaoRoda = rotacao;
	}
	
	void InputPlayer() {
		// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
		// multiplied by the user input variable.
		FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * aceleracaoRoda;
			
		// the steer angle is an arbitrary value multiplied by the user input.
		
//		if (rotacaoRoda < 0.1f && rotacaoRoda > -0.1f)
//		{
//			if (FrontLeftWheel.steerAngle <= 0.1f && FrontLeftWheel.steerAngle >= -0.1f)
//			{
//				FrontLeftWheel.steerAngle = 0;	
//				//volante.transform.Rotate(0,0,-3);
//				print("Setando algulo da roda para 0");
//			}
//			
//			if (FrontLeftWheel.steerAngle > 0.1f )
//			{
//				FrontLeftWheel.steerAngle -= 3;	
//				volante.transform.Rotate(0,0,-3);
//			}
//			else if (FrontLeftWheel.steerAngle < -0.1f)
//			{
//				FrontLeftWheel.steerAngle += 3;
//				volante.transform.Rotate(0,0,+3);
//			}
//		}
//		else
//		{
//			FrontLeftWheel.steerAngle += rotacaoRoda;
//			volante.transform.Rotate(0,0,rotacaoRoda);
//		}
		
		if (steerside != STEERSIDE.IA)
		{
			if (rotacaoRoda == -1 && FrontLeftWheel.steerAngle > 0)
			{
				//volante.transform.Rotate(0, 0, FrontLeftWheel.steerAngle);
				FrontLeftWheel.steerAngle = 0;
			}
			else if (rotacaoRoda == 1 && FrontLeftWheel.steerAngle < 0)
			{
				//steerside = STEERSIDE.DIREITA;
				//volante.transform.Rotate(0, 0, FrontLeftWheel.steerAngle);
				FrontLeftWheel.steerAngle = 0;
			}
			//if (FrontLeftWheel.steerAngle == 0)
			//	steerside = STEERSIDE.FRENTE;
		}
		
		switch (steerside)
		{
			case STEERSIDE.ESQUERDA:
			if (rotacaoRoda < 0.1f && rotacaoRoda > -0.1f)
			{	
				if (FrontLeftWheel.steerAngle < -0.2f)
				{
					if (FrontLeftWheel.steerAngle <= -rotacaoAuto)
					{
						FrontLeftWheel.steerAngle += rotacaoAuto;	
						volante.transform.Rotate(0,0,+rotacaoAuto);
					}
					else
					{
						FrontLeftWheel.steerAngle = 0;	
						volante.transform.Rotate(0,0,+FrontLeftWheel.steerAngle);
					}
				}
				else
				{
					volante.transform.Rotate(0, 0, FrontLeftWheel.steerAngle);
					FrontLeftWheel.steerAngle = 0;
					steerside = STEERSIDE.FRENTE;
				}
			}
			else if (FrontLeftWheel.steerAngle > -30f)
			{
				FrontLeftWheel.steerAngle += rotacaoRoda;
				volante.transform.Rotate(0,0,rotacaoRoda);
			}
		break;
			case STEERSIDE.DIREITA:
			if (rotacaoRoda < 0.1f && rotacaoRoda > -0.1f)
			{	
				/*if (FrontLeftWheel.steerAngle < 0) {
					volante.transform.Rotate(0, 0, FrontLeftWheel.steerAngle);
					FrontLeftWheel.steerAngle = 0;
					steerside = STEERSIDE.FRENTE;
				}
				else */if (FrontLeftWheel.steerAngle > 0.2f)
				{
					if (FrontLeftWheel.steerAngle >= rotacaoAuto)
					{
						FrontLeftWheel.steerAngle -= rotacaoAuto;	
						volante.transform.Rotate(0,0,-rotacaoAuto);
					}
					else
					{
						FrontLeftWheel.steerAngle = 0;	
						volante.transform.Rotate(0,0,-FrontLeftWheel.steerAngle);
					}
				}
				else
				{
					volante.transform.Rotate(0, 0, FrontLeftWheel.steerAngle);
					FrontLeftWheel.steerAngle = 0;
					steerside = STEERSIDE.FRENTE;
				}
			}
			else if (FrontLeftWheel.steerAngle < 30f)
			{
				FrontLeftWheel.steerAngle += rotacaoRoda;
				volante.transform.Rotate(0,0,rotacaoRoda);
			}
		break;
			case STEERSIDE.FRENTE:
			FrontLeftWheel.steerAngle += rotacaoRoda;
			volante.transform.Rotate(0,0,rotacaoRoda);
			break;
		case STEERSIDE.IA:
			FrontLeftWheel.steerAngle += rotacaoRoda;
			volante.transform.Rotate(0,0,rotacaoRoda);
			break;
		}
		
		if (steerside != STEERSIDE.IA)
		{
			if (rotacaoRoda == -1)
			{
				steerside = STEERSIDE.ESQUERDA;
			}
			else if (rotacaoRoda == 1)
			{
				steerside = STEERSIDE.DIREITA;
			}
			if (FrontLeftWheel.steerAngle == 0)
				steerside = STEERSIDE.FRENTE;
		}
		
//		if (steerside != STEERSIDE.IA)
//		{
//			if (FrontLeftWheel.steerAngle < -0.2f)
//				steerside = STEERSIDE.ESQUERDA;
//			else if (FrontLeftWheel.steerAngle > 0.2f)
//				steerside = STEERSIDE.DIREITA;
//			if (FrontLeftWheel.steerAngle == 0)
//				steerside = STEERSIDE.FRENTE;
//		}
		
//		if (FrontLeftWheel.steerAngle == 0)
//			steerside = STEERSIDE.FRENTE;
//		else if (FrontLeftWheel.steerAngle < 0)
//			steerside = STEERSIDE.ESQUERDA;
//		else if (FrontLeftWheel.steerAngle > 0)
//			steerside = STEERSIDE.DIREITA;
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
	
	void CarState()
	{
		powerUpTime -= Time.deltaTime;
		if (puTime != null)
			puTime.text = ((int)powerUpTime).ToString();
		if (powerUpTime <= 0) {
			AtivarHudPowerUp(false);
		}
	}
	
	void AtivarHudPowerUp(bool b)
	{
		if (puHud != null)
			puHud.gameObject.SetActive(b);
		if (puTime != null)
			puTime.gameObject.SetActive(b);
		
		powerUpTime = powerUpTimeInitial;
		carState = BUMPERCAR.NORMAL;
		SetVisible();
		multiplicadorDeVelocidade = 1;
	}
	
	void AtivarHudPowerUp(bool b, Texture t, BUMPERCAR state)
	{
		AtivarHudPowerUp(false);
		
		carState = state;
		powerUpTime = powerUpTimeInitial;
		
		if (puHud != null)
		{
			puHud.gameObject.SetActive(b);
			puHud.texture = t;
		}
		if (puTime != null)
			puTime.gameObject.SetActive(b);
		
		switch (carState) {
			case BUMPERCAR.NORMAL:
			multiplicadorDeVelocidade = 1;
		break;
			case BUMPERCAR.COLETOR:
		break;
			case BUMPERCAR.ESCUDO:
		break;
			case BUMPERCAR.INVISIVEL:
			SetInvisible();
		break;
			case BUMPERCAR.LENTO:
			multiplicadorDeVelocidade = 0.1f;
		break;
			case BUMPERCAR.TURBO:
			multiplicadorDeVelocidade = 2;
		break;
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
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "PowerUp")
		{
			switch (other.gameObject.GetComponent<PowerUp>().state)
			{
			case PowerUp.POWERUP.ESCUDO:
				AtivarHudPowerUp(true, escudoImg, BUMPERCAR.ESCUDO);
				break;
			case PowerUp.POWERUP.INVISIVEL:
				AtivarHudPowerUp(true, invisivelImg, BUMPERCAR.INVISIVEL);
				break;
			case PowerUp.POWERUP.LENTO:
				AtivarHudPowerUp(true, lentoImg, BUMPERCAR.LENTO);
				break;
			case PowerUp.POWERUP.TURBO:
				AtivarHudPowerUp(true, turboImg, BUMPERCAR.TURBO);
				break;
			}
			
			Destroy(other.gameObject);
		}
	}
	
	public enum STEERSIDE
	{
		FRENTE,
		ESQUERDA,
		DIREITA,
		IA,
	}
	
	public enum BUMPERCAR
	{
		NORMAL,
		LENTO,
		INVISIVEL,
		TURBO,
		ESCUDO,
		COLETOR,
	}
}