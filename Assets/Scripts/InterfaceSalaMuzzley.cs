using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InterfaceSalaMuzzley : MonoBehaviour
{
	public int posicaoJogador;
	
	public GUITexture fundo;
	public GUITexture QRCode;
	public GUITexture HudTempo;
	
	public GUIText tempo;
	
	public GameObject hudUsuarioPrefab;
	
	public List<GameObject> huds;
	public List<float> moedas;
	public List<GameObject> aux;
	
	public Camera cameraHud;
	
	private static InterfaceSalaMuzzley instance;

    public static InterfaceSalaMuzzley Instance
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
		posicaoJogador = 1;
		moedas = new List<float>();
		aux = new List<GameObject>();
		
//		fundo.pixelInset = new Rect(fundo.pixelInset.x, fundo.pixelInset.y, Screen.width/5, Screen.height);
//		QRCode.pixelInset = new Rect(QRCode.pixelInset.x, -fundo.pixelInset.width/2, fundo.pixelInset.width/2, fundo.pixelInset.width/2);
//		HudTempo.pixelInset = new Rect(HudTempo.pixelInset.x + QRCode.pixelInset.width, -fundo.pixelInset.width/2, fundo.pixelInset.width/2, fundo.pixelInset.width/2);
//		tempo.pixelOffset = new Vector2(HudTempo.pixelInset.x + HudTempo.pixelInset.width/2, -HudTempo.pixelInset.height/2);
		
		fundo.pixelInset = new Rect(fundo.pixelInset.x, fundo.pixelInset.y, cameraHud.pixelWidth, cameraHud.pixelHeight);
		QRCode.pixelInset = new Rect(QRCode.pixelInset.x, -fundo.pixelInset.width/2, fundo.pixelInset.width/2, fundo.pixelInset.width/2);
		HudTempo.pixelInset = new Rect(HudTempo.pixelInset.x + QRCode.pixelInset.width, -fundo.pixelInset.width/2, fundo.pixelInset.width/2, fundo.pixelInset.width/2);
		tempo.pixelOffset = new Vector2(HudTempo.pixelInset.x + HudTempo.pixelInset.width/2, -HudTempo.pixelInset.height/2);
	}
	
	void Update ()
	{
		UpdateHudPosition();
	}
	
	public void AddHud(string foto, string nome, Color c, string id)
	{
		GameObject h = (GameObject)Instantiate(hudUsuarioPrefab);
		h.GetComponent<UsuarioHudMuzzley>().fotoURL = foto;
		h.GetComponent<UsuarioHudMuzzley>().nomeStr = nome;
		h.GetComponent<UsuarioHudMuzzley>().stringsRecebidas = true;
		h.GetComponent<UsuarioHudMuzzley>().cor = c;
		h.GetComponent<UsuarioHudMuzzley>().Ajustar(fundo.pixelInset, posicaoJogador);
		h.GetComponent<UsuarioHudMuzzley>().id = id;
		
		huds.Add(h);
	}
	
	public void UpdateHudPosition()
	{
		for (int i = huds.Count - 1; i > 0; i--)
        {
            for (int j = 0; j < i; j++)
            {
                if (huds[i].GetComponent<UsuarioHudMuzzley>().coins >  huds[j].GetComponent<UsuarioHudMuzzley>().coins)// characterList[i].position.Y < characterList[j].position.Y)
                {
                    GameObject swap = huds[i];
                    huds[i] = huds[j];
                    huds[j] = swap;
                }
            }
        }
		
		for (int i = 0; i < huds.Count; i++) {
			huds[i].GetComponent<UsuarioHudMuzzley>().Reajustar(fundo.pixelInset, i+2);
		}
		
//		moedas.Clear();
//		for (int i = 0; i < huds.Count; i++) {
//			moedas.Add(huds[i].GetComponent<UsuarioHudMuzzley>().coins);
//		}
//		
//		moedas.Sort();
//		
//		aux.Clear();
//		
//		for (int i = 0; i < moedas.Count; i++) {
//			for (int j = 0; j < huds.Count; j++) {
//				if (aux.Count < huds.Count && moedas[i] == huds[j].GetComponent<UsuarioHudMuzzley>().coins) {
//					aux.Add(huds[j]);
//					break;
//				}
//			}
//		}
//		
//		huds.Clear();
//		
//		for (int i = 0; i < aux.Count; i++) {
//			huds.Add(aux[aux.Count - (i+1)]);
//		}
//		
//		for (int i = 0; i < huds.Count; i++) {
//			huds[i].GetComponent<UsuarioHudMuzzley>().Reajustar(fundo.pixelInset, i+2);
//		}
	}
}

