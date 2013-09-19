using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public float smooth = 3f;		// a public variable to adjust smoothing of camera motion
	public Transform standardPos;			// the usual position for the camera, specified by a transform in the game
	public Transform lookAtPos;			// the position to move the camera to when using head look
	bool lookAtPosBool;
	
	void Start()
	{
		// initialising references
		//standardPos = GameObject.Find ("CamPos").transform;
		
		if(GameObject.Find ("LookAtPos"))
			lookAtPos = GameObject.Find ("LookAtPos").transform;
		
		
	}
	
	void FixedUpdate ()
	{
		// if we hold Alt
		if(lookAtPosBool && lookAtPos)
		{
			// lerp the camera position to the look at position, and lerp its forward direction to match 
			transform.position = Vector3.Lerp(transform.position, lookAtPos.position, Time.deltaTime * smooth);
			transform.forward = Vector3.Lerp(transform.forward, lookAtPos.forward, Time.deltaTime * smooth);
		}
		else 
		{	
			if (standardPos != null)
			{
				// return the camera to standard position and direction
				transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.deltaTime * smooth);	
				transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.deltaTime * smooth);
			}
		}
		
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			lookAtPosBool = !lookAtPosBool;
	}
}