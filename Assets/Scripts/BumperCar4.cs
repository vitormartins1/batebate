using UnityEngine;
using System.Collections;

public class BumperCar4 : BumperCarBase
{
	void Start () {
		StartCar();
	}
	
	void Update () {
		//UpdateInputs(Input.GetAxis("Vertical3"), Input.GetAxis("Horizontal3"));
		UpdateInputs(Random.Range(-1, 1), Random.Range(-1, 1));
		UpdateCar();
	}
}
