using UnityEngine;
using System.Collections;

public class BumperCarMuzzley : BumperCarBase {
	
	public float rotacao;
	public float aceleracao;
	public Vector3 posicao;
	
	void Start () {
		StartCar();
	}
	
	void Update () {
		//if (MuzzleyManager.Instance.jogoIniciado || MuzzleyManager.Instance.fluxo == MuzzleyManager.FLUXO.ESPERANDO_JOGADORES)
		//{
		if (FrontLeftWheel.attachedRigidbody.isKinematic)
		{
			FrontLeftWheel.attachedRigidbody.isKinematic = false;
		}
			UpdateInputs(aceleracao, rotacao);
			UpdateCar();
		//}
	}
	
	public void ResetarPosicao()
	{
		transform.position = posicao;
		aceleracao = 0;
		rotacao = 0;
		FrontLeftWheel.attachedRigidbody.velocity = Vector3.zero;
		FrontLeftWheel.attachedRigidbody.angularVelocity = Vector3.zero;
		//print(FrontLeftWheel.attachedRigidbody.velocity);
		FrontLeftWheel.attachedRigidbody.isKinematic = true;
		//FrontLeftWheel.attachedRigidbody.isKinematic = false;
		FrontLeftWheel.motorTorque = 0;
		FrontLeftWheel.steerAngle = 0;	
		
		Vector3 relative = transform.InverseTransformPoint(MuzzleyManager.Instance.arenaRef.transform.position);
		float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
		this.transform.Rotate (0, angle, 0);
	}
	
	public void MuzzleyInputRotacao(string j, string state)
	{
		if (MuzzleyManager.Instance.jogoIniciado || MuzzleyManager.Instance.fluxo == MuzzleyManager.FLUXO.ESPERANDO_JOGADORES)
		{
			//print("state: " + state + " value: " + j + " rotacao: " + rotacao);
			
			if (j == "180" || j == "135" || j == "225")
			{
				rotacao = -1;
			}
			if (j == "0" || j == "45" || j == "315")
			{
				rotacao = 1;
			}
			if (j == "-1")
			{
				rotacao = 0;
			}
	
			//print("state: " + state + " value: " + j + " rotacao: " + rotacao);
		}
	}
	
	public void MuzzleyInputAceleracao(string state)
	{
		if (MuzzleyManager.Instance.jogoIniciado || MuzzleyManager.Instance.fluxo == MuzzleyManager.FLUXO.ESPERANDO_JOGADORES)
		{
			if (state == "press")
			{
				aceleracao = 1;
			}
			else
			{
				aceleracao = 0;
			}
		}
	}
	
	public void MuzzleyInputRe(string state)
	{
		if (MuzzleyManager.Instance.jogoIniciado || MuzzleyManager.Instance.fluxo == MuzzleyManager.FLUXO.ESPERANDO_JOGADORES)
		{
			if (state == "press")
			{
				aceleracao = -0.8f;
			}
			else
			{
				aceleracao = 0;
			}
		}
	}
}