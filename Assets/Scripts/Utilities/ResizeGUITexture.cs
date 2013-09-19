using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResizeGUITexture : MonoBehaviour
{
    Vector2 resolucaoInicial;
    Vector2 proporcao;
	
	void Start()
	{
		StartResize();
	}
	
    public void StartResize()
    {

        resolucaoInicial = new Vector2(1024, 768);
		//resolucaoInicial = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
		
        if (resolucaoInicial.x != Screen.width || resolucaoInicial.y != Screen.height)
        {
            redimensionar();
        }
    }

    void redimensionar()
    {
        proporcao.x = Screen.width / resolucaoInicial.x;
        proporcao.y = Screen.height / resolucaoInicial.y;

        this.guiTexture.pixelInset = new Rect(
									this.guiTexture.pixelInset.x * proporcao.x,
                                    this.guiTexture.pixelInset.y * proporcao.y,
                                    this.guiTexture.pixelInset.width * proporcao.x,
                                    this.guiTexture.pixelInset.height * proporcao.y);
    }
}