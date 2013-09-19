using UnityEngine;
using System.Collections;

public class TwoPlayerButton : MonoBehaviour
{
	Color c1;
	Color c2;
	
	void Start ()
	{
		c1 = this.guiTexture.color;
		c2 = new Color(1f,1f,1f,0.5f);
	}
	
	void Update ()
	{
		Button();
	}
	
	void Button()
	{
		if (this.guiTexture.HitTest(Input.mousePosition))
	    {
			this.guiTexture.color = c2;
			
	        if (Input.GetMouseButtonDown(0))
	        {
				Application.LoadLevel(2);
			}
		}
		else
		{
			this.guiTexture.color = c1;
		}
	}
}
