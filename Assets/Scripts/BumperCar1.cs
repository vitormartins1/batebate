using UnityEngine;
using System.Collections;

public class BumperCar1 : BumperCarBase {

	void Start () {
		StartCar();
	}
	
	void Update () {
		UpdateInputs(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
		UpdateCar();
	}
}
