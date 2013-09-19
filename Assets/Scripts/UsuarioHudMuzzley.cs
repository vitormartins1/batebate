using UnityEngine;
using System.Collections;

public class UsuarioHudMuzzley : MonoBehaviour
{
	public Texture Foto;
	
	public GameObject fundoTexture;
	public GameObject fotoTexture;
	public GameObject nomeUsuario;
	
	GameObject bg;
	public GameObject foto;
	public GameObject nome;
	GameObject coinsTxt;
	
	public string fotoURL;
	public string nomeStr;
	
	public Color cor;
	
	public bool stringsRecebidas = false;
	
	public float coins;
	public string id;
	
	public STATE state;
	
	void Start ()
	{
		state = STATE.WAITING;
		//Ajustar(new Rect(InterfaceSalaMuzzley.Instance.fundo.pixelInset.x, InterfaceSalaMuzzley.Instance.fundo.pixelInset.y, Screen.width/5, Screen.height));
	}
	
	void Update ()
	{
		switch (state) {
		case STATE.WAITING:
			if (stringsRecebidas)
			{
				StartCoroutine(waitQr(fotoURL));
			}
			break;
		case STATE.LOADED:
			UpdateCoins();
			break;
		}
	}
	
	public void Ajustar(Rect fundo, int multiplicador)
	{
		float valor1 = 0;
		
		bg = (GameObject)Instantiate(fundoTexture);
		//bg.guiTexture.pixelInset = new Rect(fundo.x + valor1,2*( -fundo.width - valor1), fundo.width - valor1*2, fundo.width / 2);
		bg.guiTexture.pixelInset = new Rect(fundo.x + valor1,/*multiplicador*( -InterfaceSalaMuzzley.Instance.QRCode.pixelInset.width)*/(multiplicador * -fundo.width / 3) -bg.guiTexture.pixelInset.height / 2, fundo.width - valor1*2, fundo.width / 3);
		bg.guiTexture.color = cor;
		
		float valor2 = 10;
		
		foto = (GameObject)Instantiate(fotoTexture);
		foto.guiTexture.pixelInset = new Rect(bg.guiTexture.pixelInset.x +valor2, bg.guiTexture.pixelInset.y + valor2, bg.guiTexture.pixelInset.height - valor2*2, bg.guiTexture.pixelInset.height - valor2*2);
		
		nome = (GameObject)Instantiate(nomeUsuario);
		nome.guiText.pixelOffset = new Vector2(
			bg.guiTexture.pixelInset.width - bg.guiTexture.pixelInset.width/3,
			bg.guiTexture.pixelInset.y + bg.guiTexture.pixelInset.height/1.5f);
		
		coinsTxt = (GameObject)Instantiate(nomeUsuario);
		coinsTxt.guiText.pixelOffset = new Vector2(
			bg.guiTexture.pixelInset.width - bg.guiTexture.pixelInset.width/3,
			bg.guiTexture.pixelInset.y + bg.guiTexture.pixelInset.height/3);
		
//		go2.gameObject.transform.parent = go.gameObject.transform;
//		go3.gameObject.transform.parent = go.gameObject.transform;
		
//		go2.gameObject.transform.position = new Vector3(go2.transform.position.x, go2.transform.position.y, 1);
//		go3.gameObject.transform.position = new Vector3(go3.transform.position.x, go3.transform.position.y, 1);
	}
	
	public void Reajustar(Rect fundo, int multiplicador)
	{
		float valor1 = 0;
		bg.guiTexture.pixelInset = new Rect(fundo.x + valor1,/*multiplicador*( -InterfaceSalaMuzzley.Instance.QRCode.pixelInset.width)*/(multiplicador * -fundo.width / 3) -bg.guiTexture.pixelInset.height / 2, fundo.width - valor1*2, fundo.width / 3);
		bg.guiTexture.color = cor;
		
		float valor2 = 10;
		foto.guiTexture.pixelInset = new Rect(bg.guiTexture.pixelInset.x +valor2, bg.guiTexture.pixelInset.y + valor2, bg.guiTexture.pixelInset.height - valor2*2, bg.guiTexture.pixelInset.height - valor2*2);
		
		nome.guiText.pixelOffset = new Vector2(
			bg.guiTexture.pixelInset.width - bg.guiTexture.pixelInset.width/3,
			bg.guiTexture.pixelInset.y + bg.guiTexture.pixelInset.height/1.5f);
		
		coinsTxt.guiText.pixelOffset = new Vector2(
			bg.guiTexture.pixelInset.width - bg.guiTexture.pixelInset.width/3,
			bg.guiTexture.pixelInset.y + bg.guiTexture.pixelInset.height/3);
	}
	
	void UpdateCoins()
	{
		if (id != null && coinsTxt != null && MuzzleyManager.Instance.fluxo != MuzzleyManager.FLUXO.PLACAR && MuzzleyManager.Instance.participantes[id].gameObject.GetComponentInChildren<CollidersManager>())
		{
			coins = MuzzleyManager.Instance.participantes[id].gameObject.GetComponentInChildren<CollidersManager>().coins;
			coinsTxt.guiText.text = "Modeas: " + coins.ToString();
		}
	}
	
	IEnumerator waitQr(string url)
	{
		WWW www = new WWW(url);
        yield return www;
		foto.guiTexture.texture = www.texture;
		nome.guiText.text = nomeStr;
		state = STATE.LOADED;
        //renderer.material.mainTexture = www.texture;
	}
	
	public void Destruir()
	{
		Destroy(bg);
		Destroy(foto);
		Destroy(nome);
		Destroy(coinsTxt);
		Destroy(this.gameObject);
	}
	
	public enum STATE
	{
		WAITING,
		LOADED,
	}
}
