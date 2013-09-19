using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinManager : MonoBehaviour
{
	//List<Coin> coins;
	public int numberOfCoins;

	public CoinManager(int initialNumberOfCoins)
	{
		//coins = new List<Coin>(initialNumberOfCoins);
		numberOfCoins = initialNumberOfCoins;
	}

	void Start ()
	{

	}

	void Update ()
	{
	
	}

	public bool RemoveCoins(int numberOfCoins)
	{
		if (this.numberOfCoins > numberOfCoins)
		{
			this.numberOfCoins -= numberOfCoins;
			return true;
		}
		else
			return false;
	}
}
