using UnityEngine;
using System.Collections;

public class ResizeGUITextureByCamera : MonoBehaviour
{
    Vector2 resolucaoInicial;
    Vector2 proporcao;
	
	public Camera camera1;
	
	void Start()
	{
		if (camera1 == null)
		{
			camera1 = GameObject.Find("Main Camera").GetComponent<Camera>();
		}
		
		StartResize();
	}
	
    public void StartResize()
    {

        resolucaoInicial = new Vector2(1024, 768);
		//resolucaoInicial = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
		
        if (resolucaoInicial.x != camera1.pixelWidth || resolucaoInicial.y != camera1.pixelHeight)
        {
            redimensionar();
        }
    }

    void redimensionar()
    {
        proporcao.x = camera1.pixelWidth / resolucaoInicial.x;
        proporcao.y = camera1.pixelHeight / resolucaoInicial.y;

        this.guiTexture.pixelInset = new Rect(
									this.guiTexture.pixelInset.x * proporcao.x,
                                    this.guiTexture.pixelInset.y * proporcao.y,
                                    this.guiTexture.pixelInset.width * proporcao.x,
                                    this.guiTexture.pixelInset.height * proporcao.y);
    }
}