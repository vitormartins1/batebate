using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BumperCarsManager : MonoBehaviour
{
	public List<GameObject> bumperCars;
	public List<GameObject> chasedBumperCars;

    private static BumperCarsManager instance;

    public static BumperCarsManager Instance
    {
        get
        {
            return instance;
        }
    }

	void Awake()
    {
        instance = this;
    }

	void Start ()
	{
		//bumperCars = new List<GameObject>();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel(0);
		}
	}
}
