using UnityEngine;
using System.Collections;

public class BumperCar3 : BumperCarBase
{
	void Start () {
		StartCar();
	}
	
	void Update () {
		UpdateInputs(Input.GetAxis("Vertical3"), Input.GetAxis("Horizontal3"));
		UpdateCar();
	}
}
