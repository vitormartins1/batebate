using UnityEngine;
using System.Collections;

public class ResizeGUIText : MonoBehaviour
{
    Vector2 resolucaoInicial;
    Vector2 proporcao;
	public bool reposicionar;
	
	void Start()
	{
		StartResize();
	}
	
    public void StartResize()
    {
        resolucaoInicial = new Vector2(1024, 768);
		//resolucaoInicial = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
		redimensionar();
//        if (resolucaoInicial.x != Screen.width || resolucaoInicial.y != Screen.height)
//        {
//            redimensionar();
//        }
    }

    void redimensionar()
    {
        proporcao.x = Screen.width / resolucaoInicial.x;
        proporcao.y = Screen.height / resolucaoInicial.y;
		
		this.guiText.fontSize = (int)(this.guiText.fontSize * proporcao.x);
		
		if (reposicionar) {
			this.guiText.pixelOffset = new Vector2(this.guiText.pixelOffset.x * proporcao.x, this.guiText.pixelOffset.y * proporcao.y);
		}
    }
}