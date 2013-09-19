using UnityEngine;
using System.Collections;

public class VoltarButton : MonoBehaviour
{

	Color c1;
	Color c2;
	
	void Start ()
	{
		c1 = this.guiTexture.color;
		c2 = new Color(0.5f,0.5f,0.2f,0.5f);
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
				Application.LoadLevel(0);
			}
		}
		else
		{
			this.guiTexture.color = c1;
		}
	}
}
