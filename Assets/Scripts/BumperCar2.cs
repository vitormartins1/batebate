using UnityEngine;
using System.Collections;

public class BumperCar2 : BumperCarBase
{
	void Start () {
		StartCar();
	}
	
	void Update () {
		UpdateInputs(Input.GetAxis("Vertical2"), Input.GetAxis("Horizontal2"));
		UpdateCar();
	}
}