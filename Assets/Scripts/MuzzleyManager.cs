using UnityEngine;
using System;	
using System.Collections;
using System.Collections.Generic;
using Muzzley;
using Muzzley.Net;

public class MuzzleyManager : MonoBehaviour {
	
	public STATE state;
	public FLUXO fluxo;
	public MuzzleyApp myMuzzleyApp;
	public string qrcodeUrl;
	public string activityId;
	public Dictionary<string, BumperCarMuzzley> participantes;
	public GameObject arenaRef;
	public GameObject bumperCarPrefab;
	public GameObject iaBumperCarPrefab;
	public GameObject esperandoJogadorPrefab;
	public GameObject esperandoJogadorObj;
	public GameObject contagemRegressivaPrefab;
	public GameObject contagemRegressivaObj;
	public GameObject placarPrefab;
	public GameObject placarObj;
	public GameObject moedasPrefab;
	public GameObject moedasObj;
	string idNovoCarro;
	string nomeDoUsuario;
	string fotoUrl;
	public GUITexture qrcodeGuiTexture;
	bool showQr;
	public Bounds actionView;
	public List<Vector3> posicoesIniciais;
	public List<GameObject> iaBumperCars;
	public CameraFollow cameraFollow;
	public Transform visaoDeAcao;
	public GameObject guiaDaCamera;
	public Dictionary<string, object> motion;
	public Dictionary<string, object> motionParams;
	public int jogadoresMax = 5;
	public float tempoDeEspera = 31;
	public float tempoDeJogo = 61;
	public float contagemRegressiva = 4;
	public bool jogoIniciado = false;
	public bool jogarNovamente = false;
	public bool carrosCriados = true;
	
	private static MuzzleyManager instance;

    public static MuzzleyManager Instance
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
	
	void CriarPosicoesNaArena()
	{
		posicoesIniciais = new List<Vector3>();
		float valor = 10;
		
		for (int i = 0; i < valor; i++) {
			posicoesIniciais.Add(new Vector3(arenaRef.collider.bounds.min.x + valor + arenaRef.collider.bounds.size.x/valor * i, arenaRef.transform.position.y + 2, arenaRef.collider.bounds.min.z + valor  + arenaRef.collider.bounds.size.z/valor * i));
			//GameObject u =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//u.transform.position = new Vector3(arenaRef.collider.bounds.min.x + valor + arenaRef.collider.bounds.size.x/valor * i, arenaRef.transform.position.y + 2, arenaRef.collider.bounds.min.z + valor  + arenaRef.collider.bounds.size.z/valor * i);
			
			posicoesIniciais.Add(new Vector3(arenaRef.collider.bounds.min.x + valor + arenaRef.collider.bounds.size.x/valor * i, arenaRef.transform.position.y + 2, arenaRef.collider.bounds.max.z - valor  - arenaRef.collider.bounds.size.z/valor * i));
			//GameObject o =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//o.transform.position = new Vector3(arenaRef.collider.bounds.min.x + valor + arenaRef.collider.bounds.size.x/valor * i, arenaRef.transform.position.y + 2, arenaRef.collider.bounds.max.z - valor  - arenaRef.collider.bounds.size.z/valor * i);
		}
	}
	
	void Start () {
		myMuzzleyApp = new MuzzleyApp();
		myMuzzleyApp.ConnectApp(OnActivityReady, "ddb0a998759d3469", null);
		state = STATE.NENHUM_JOGADOR_ENTROU;
		fluxo = FLUXO.ESPERANDO_JOGADORES;
		idNovoCarro = null;
		participantes = new Dictionary<string, BumperCarMuzzley>();
		showQr = false;
		actionView = new Bounds(gameObject.transform.position, new Vector3(1,1,1));
		esperandoJogadorObj = (GameObject)Instantiate(esperandoJogadorPrefab);
		moedasObj = (GameObject)Instantiate(moedasPrefab);
		CriarPosicoesNaArena();
		
//		motion.Add("c", "deviceMotion");
//		
//		motionParams.Add("step", 5);
//		motionParams.Add("pitch", true);
//		
//		motion.Add("p", motionParams);
	}
	
	public void ActionViewUpdate()
	{
		bool isReset = false;
		foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
		{
			if (isReset == false)
			{
				if (item.Value != null)
				{
	         		actionView = new Bounds(item.Value.transform.position, new Vector3(1,1,1));
	         		isReset = true;
				}
	       }
			if (item.Value != null)
				actionView.Encapsulate(item.Value.transform.position);
		}
		visaoDeAcao.position = new Vector3(actionView.center.x, 3, actionView.center.z);
		//cameraFollow.standardPos = visaoDeAcao;
		
		//guiaDaCamera.transform.position = visaoDeAcao.position;
		
		float factoryValue = 2.0f;
		
		if (actionView.size.x > actionView.size.y)
		{
			guiaDaCamera.transform.position = new Vector3(visaoDeAcao.position.x, visaoDeAcao.position.y + actionView.size.x / factoryValue, visaoDeAcao.position.z);
		}
		else
		{
			guiaDaCamera.transform.position = new Vector3(visaoDeAcao.position.x, visaoDeAcao.position.y + actionView.size.y / factoryValue, visaoDeAcao.position.z);
		}
		
		guiaDaCamera.transform.Rotate(0,1.2f*Time.deltaTime,0);
		
		//print(guiaDaCamera.transform.position.y);
	}
	
	public void OnActivityReady(MuzzleyActivity activity)
	{
		Debug.Log(activity.QRCodeUrl);
		
		qrcodeUrl = activity.QRCodeUrl;
		
    	activity.SetJoinHandler(OnJoin);
		activity.SetQuitHandler(OnQuit);
		activity.SetActionHandler(OnAction);
		
		showQr = true;
		activityId = activity.ActivityId;
	}
	
	private void OnJoin(MuzzleyAppParticipant muzzley_participant)
	{
		muzzley_participant.ChangeWidget(MuzzleyConstants.Widgets.GAMEPAD, motion);
		idNovoCarro = muzzley_participant.Id;
		nomeDoUsuario = muzzley_participant.Name;
		fotoUrl = muzzley_participant.PhotoUrl;
		state = STATE.JOGADOR_ENTROU;
		//print(muzzley_participant.Id);
	}
	
	private void OnQuit(MuzzleyAppParticipant muzzley_participant)
	{
		idNovoCarro = muzzley_participant.Id;
		state = STATE.JOGADOR_SAIU;
	}
	
	private void OnAction(MuzzleyAppAction muzzley_event)
	{
		//participantes[muzzley_event.Participant.Id].UpdateInputs(muzzley_event.Data
		//print(muzzley_event.Data["c"] + " " + muzzley_event.Data["e"]);
		
		if (muzzley_event.Data["c"].ToString() == "jl")
				participantes[muzzley_event.Participant.Id].MuzzleyInputRotacao(muzzley_event.Data["v"].ToString(), muzzley_event.Data["e"].ToString());
		
		if (muzzley_event.Data["c"].ToString() == "bc")
			participantes[muzzley_event.Participant.Id].MuzzleyInputAceleracao(muzzley_event.Data["e"].ToString());
		
		if (muzzley_event.Data["c"].ToString() == "bb")
			participantes[muzzley_event.Participant.Id].MuzzleyInputRe(muzzley_event.Data["e"].ToString());
		
		if (muzzley_event.Data["c"].ToString() == "bd")
		{
			if (fluxo == FLUXO.PLACAR)
				jogarNovamente = true;
		}
	}
	
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.LoadLevel(0);
		
		if (showQr && qrcodeUrl != null)
			StartCoroutine(waitQr());
		
		GerenciaDeJogadores();
		//ActionViewUpdate();
		EstadosDeJogo();
	}
	
	IEnumerator waitQr()
	{
		WWW www = new WWW(qrcodeUrl);
        yield return www;
		qrcodeGuiTexture.texture = www.texture;
		showQr = false;
		InterfaceSalaMuzzley.Instance.tempo.text = activityId;
        //renderer.material.mainTexture = www.texture;
	}
	
	public void EstadosDeJogo()
	{
		switch (fluxo) {
			case FLUXO.ESPERANDO_JOGADORES:
			
			if (participantes.Count > 0)
			{
				tempoDeEspera -= Time.deltaTime;
				GameObject.Find("Titulo").guiText.text = "Esperando mais jogadores...  " + ((int)tempoDeEspera).ToString();
			}
			else
			{
				tempoDeEspera = 31;
				GameObject.Find("Titulo").guiText.text = "Esperando Jogadores...";
				//LimparMoedas();
			}
			
			if (tempoDeEspera <= 0)
			{
				Destroy(esperandoJogadorObj);
				Destroy(moedasObj);
				contagemRegressivaObj = (GameObject)Instantiate(contagemRegressivaPrefab);
				fluxo = FLUXO.JOGO;
				
				if (participantes.Count > 0)
				{
					// resetamos a posicao dos jogadores
					foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
					{
						item.Value.ResetarPosicao();
					}
					
					// aqui destruimos os carros dos participantes
//					foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
//					{
//						Destroy(item.Value.gameObject);
//					}
//					
//					// recriamos aqui os carros destruidos
//					foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
//					{
//						GameObject newCar = (GameObject)Instantiate(bumperCarPrefab);
//						//newCar.name = "CarroDo" + InterfaceSalaMuzzley.Instance.huds[Convert.ToInt32(item.Key)].GetComponent<UsuarioHudMuzzley>().nome.guiText.text;
//						//newCar.GetComponent<BumperCarBase>().carenagem.renderer.material.color = InterfaceSalaMuzzley.Instance.huds[Convert.ToInt32(item.Key)].GetComponent<UsuarioHudMuzzley>().cor;
//						newCar.GetComponent<BumperCarMuzzley>().posicao = posicoesIniciais[Convert.ToInt32(item.Key)];
//						BumperCarsManager.Instance.bumperCars.Add(newCar);
//						
//						for (int i = 0; i < InterfaceSalaMuzzley.Instance.huds.Count; i++)
//						{
//							if (InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().id == item.Key)
//							{
//								newCar.name = "CarroDo" + InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().nome.guiText.text;
//								newCar.GetComponent<BumperCarBase>().carenagem.renderer.material.color = InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().cor;
//							}
//						}
//						
//						participantes[item.Key] = newCar.GetComponent<BumperCarMuzzley>();
//					}
//					
//					if (participantes.Count > 0)
//					{
//						foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
//						{
//							item.Value.ResetarPosicao();
//						}
//					}
				}
				
				// aqui criamos os jogadores ia
				int o = jogadoresMax - participantes.Count;
				if (o > 0)
				{
					for (int i = 0; i < o; i++)
					{
						GameObject go = (GameObject)Instantiate(iaBumperCarPrefab);
						go.transform.position = posicoesIniciais[i + participantes.Count];
						iaBumperCars.Add(go);
					}
				}
				
				tempoDeEspera = 31;
			}
			
		break;
			case FLUXO.JOGO:
			if (participantes.Count == 0) {
				fluxo = FLUXO.ESPERANDO_JOGADORES;
				esperandoJogadorObj = (GameObject)Instantiate(esperandoJogadorPrefab);
				moedasObj = (GameObject)Instantiate(moedasPrefab);
				Destroy(contagemRegressivaObj);
				tempoDeJogo = 61;
				jogoIniciado = false;
				//aqui limpamos a lista de jogadores e excluimos eles
				foreach (GameObject item in iaBumperCars) {
					Destroy(item.gameObject);
				}
				iaBumperCars.Clear();
			}
			
			if (!jogoIniciado)
			{
				if (((int)contagemRegressiva) <= 0)
				{
					contagemRegressivaObj.guiText.text = "Vai!";
					if (((int)contagemRegressiva) <= -1)
					{
						jogoIniciado = true;
						Destroy(contagemRegressivaObj);
						contagemRegressiva = 4;
					}
				}
				else if (contagemRegressivaObj != null)
				{
					contagemRegressivaObj.guiText.text = ((int)contagemRegressiva).ToString();
				}
				
				contagemRegressiva -= Time.deltaTime;
			}
			
			if (jogoIniciado)
			{
				tempoDeJogo -= Time.deltaTime;
				InterfaceSalaMuzzley.Instance.tempo.text = /*"ID: " + */activityId + "\n" + /*"Tempo: " + */((int)tempoDeJogo).ToString();
				
				if (tempoDeJogo <= 0)
				{
					fluxo = FLUXO.PLACAR;
					placarObj = (GameObject)Instantiate(placarPrefab);
					jogoIniciado = false;
					
					//LimparMoedas();
					
//					if (participantes.Count >= 1)
//					{
//						GameObject.Find("Foto1").guiTexture.texture = InterfaceSalaMuzzley.Instance.huds[0].GetComponent<UsuarioHudMuzzley>().foto.guiTexture.texture;
//						//GameObject.Find("Nome1").guiText.text = InterfaceSalaMuzzley.Instance.huds[0].GetComponent<UsuarioHudMuzzley>().nome.guiText.text; 
//						//GameObject.Find("Nome1").guiText.material.color = Color.black;
//						//GameObject.Find("Nome1").guiText.pixelOffset = new Vector2(GameObject.Find("Foto1").guiTexture.pixelInset.x + GameObject.Find("Foto1").guiTexture.pixelInset.height/2, GameObject.Find("Nome1").guiText.pixelOffset.y);
//						
//						if (participantes.Count >= 2)
//						{
//							GameObject.Find("Foto2").guiTexture.texture = InterfaceSalaMuzzley.Instance.huds[1].GetComponent<UsuarioHudMuzzley>().foto.guiTexture.texture;
//							//GameObject.Find("Nome2").guiText.text = InterfaceSalaMuzzley.Instance.huds[1].GetComponent<UsuarioHudMuzzley>().nome.guiText.text;
//							//GameObject.Find("Nome2").guiText.material.color = Color.black;
//							//GameObject.Find("Nome2").guiText.pixelOffset = new Vector2(GameObject.Find("Foto2").guiTexture.pixelInset.x + GameObject.Find("Foto2").guiTexture.pixelInset.height/2, GameObject.Find("Nome2").guiText.pixelOffset.y);
//							
//							if (participantes.Count >= 3)
//							{
//								GameObject.Find("Foto3").guiTexture.texture = InterfaceSalaMuzzley.Instance.huds[2].GetComponent<UsuarioHudMuzzley>().foto.guiTexture.texture;
//								//GameObject.Find("Nome3").guiText.text = InterfaceSalaMuzzley.Instance.huds[2].GetComponent<UsuarioHudMuzzley>().nome.guiText.text;
//								//GameObject.Find("Nome3").guiText.material.color = Color.black;
//							}
//						}
//					}
					
					// atualizamos os jogadores no placar
					int oioi = 0;
					foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
					{
						oioi++;
						GameObject.Find("Resultado").guiText.text = GameObject.Find("Resultado").guiText.text + "\n" + oioi + " - " + InterfaceSalaMuzzley.Instance.huds[oioi-1].GetComponent<UsuarioHudMuzzley>().nome.guiText.text + " - " + InterfaceSalaMuzzley.Instance.huds[oioi-1].GetComponent<UsuarioHudMuzzley>().coins + " Moedas";
					}
					
					//aqui limpamos a lista de jogadores com ia, excluimos eles e atualizams eles no placar
					foreach (GameObject item in iaBumperCars)
					{
						oioi++;
						GameObject.Find("Resultado").guiText.text = GameObject.Find("Resultado").guiText.text + "\n" + oioi + " - " + "IA" + " - " + item.GetComponent<BumperCarIA>().coins + " Moedas";
						Destroy(item.gameObject);
					}
					
					iaBumperCars.Clear();
					
					// aqui desativamos os carros dos aprticipantes
//					foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
//					{
//						item.Value.gameObject.SetActive(false);
//					}
					
					// aqui destruimos os carros dos participantes
					foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
					{
						Destroy(item.Value.gameObject);
					}
					
					// aqui limpamos a lista de carros seguidos pela ia
					BumperCarsManager.Instance.bumperCars.Clear();
					
					carrosCriados = false;
				}
			}
		break;
			case FLUXO.PLACAR:
			if (participantes.Count == 0) {
				fluxo = FLUXO.ESPERANDO_JOGADORES;
				esperandoJogadorObj = (GameObject)Instantiate(esperandoJogadorPrefab);
				moedasObj = (GameObject)Instantiate(moedasPrefab);
				Destroy(placarObj);
				tempoDeJogo = 61;
				InterfaceSalaMuzzley.Instance.tempo.text = /*"ID: " + */activityId + "\n" + /*"Tempo: " + */((int)tempoDeJogo).ToString();
				jogoIniciado = false;
			}
			
			if (jogarNovamente)
				JogarNovamente();
		break;
		}
	}
	
	public void GerenciaDeJogadores()
	{
		switch (state) {
		case STATE.JOGADOR_ENTROU:
			Color c = new Color(UnityEngine.Random.Range(0.0f, 0.5f), UnityEngine.Random.Range(0.0f, 0.5f), UnityEngine.Random.Range(0.0f, 0.5f), 0.5f);
			GameObject newCar = (GameObject)Instantiate(bumperCarPrefab);
			newCar.name = "CarroDo" + nomeDoUsuario;
			newCar.GetComponent<BumperCarBase>().carenagem.renderer.material.color = c;
			//newCar.GetComponent<BumperCarMuzzley>().posicao = posicoesIniciais[Convert.ToInt32(idNovoCarro)];
			newCar.GetComponent<BumperCarMuzzley>().posicao = posicoesIniciais[participantes.Count];
			newCar.GetComponent<BumperCarMuzzley>().ResetarPosicao();
			participantes.Add(idNovoCarro, newCar.GetComponent<BumperCarMuzzley>());
			InterfaceSalaMuzzley.Instance.posicaoJogador++;
			actionView.Encapsulate(newCar.transform.position);
			InterfaceSalaMuzzley.Instance.AddHud(fotoUrl, nomeDoUsuario, c, idNovoCarro);
			idNovoCarro = null;
			BumperCarsManager.Instance.bumperCars.Add(newCar);
			state = STATE.NENHUM_JOGADOR_ENTROU;
		break;
		case STATE.JOGADOR_SAIU:
			if (idNovoCarro != null)
			{
				BumperCarsManager.Instance.bumperCars.Remove(participantes[idNovoCarro].gameObject);
				Destroy(participantes[idNovoCarro].gameObject);
				participantes.Remove(idNovoCarro);
				
				foreach (GameObject item in InterfaceSalaMuzzley.Instance.huds) {
					if (item.GetComponent<UsuarioHudMuzzley>().id == idNovoCarro)
					{
						item.GetComponent<UsuarioHudMuzzley>().Destruir();
						InterfaceSalaMuzzley.Instance.huds.Remove(item);
						break;
					}
				}
				
				idNovoCarro = null;
			}
			state = STATE.NENHUM_JOGADOR_ENTROU;
			break;
		}
	}
	
	public void JogarNovamente()
	{
		Destroy(placarObj);
		tempoDeJogo = 61;
		InterfaceSalaMuzzley.Instance.tempo.text = /*"ID: " + */activityId + "\n" + /*"Tempo: " + */((int)tempoDeJogo).ToString();
		contagemRegressivaObj = (GameObject)Instantiate(contagemRegressivaPrefab);
		fluxo = FLUXO.JOGO;
		jogarNovamente = false;
		
		// aqui criamos os jogadores ia
		int o = jogadoresMax - participantes.Count;
		if (o > 0)
		{
			for (int i = 0; i < o; i++)
			{
				GameObject go = (GameObject)Instantiate(iaBumperCarPrefab);
				go.transform.position = posicoesIniciais[i + participantes.Count];
				iaBumperCars.Add(go);
			}
		}
		
		//reativamos aqui os carros dos jogadores
//		foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
//		{
//			item.Value.gameObject.SetActive(true);
//		}
		
		// recriamos aqui os carros destruidos
		foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
		{
			GameObject newCar = (GameObject)Instantiate(bumperCarPrefab);
			//newCar.name = "CarroDo" + InterfaceSalaMuzzley.Instance.huds[Convert.ToInt32(item.Key)].GetComponent<UsuarioHudMuzzley>().nome.guiText.text;
			//newCar.GetComponent<BumperCarBase>().carenagem.renderer.material.color = InterfaceSalaMuzzley.Instance.huds[Convert.ToInt32(item.Key)].GetComponent<UsuarioHudMuzzley>().cor;
			newCar.GetComponent<BumperCarMuzzley>().posicao = posicoesIniciais[Convert.ToInt32(item.Key)];
			BumperCarsManager.Instance.bumperCars.Add(newCar);
			
			for (int i = 0; i < InterfaceSalaMuzzley.Instance.huds.Count; i++)
			{
				if (InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().id == item.Key)
				{
					newCar.name = "CarroDo" + InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().nome.guiText.text;
					newCar.GetComponent<BumperCarBase>().carenagem.renderer.material.color = InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().cor;
				}
			}
			
			participantes[item.Key] = newCar.GetComponent<BumperCarMuzzley>();
		}
		
		if (participantes.Count > 0)
		{
			foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
			{
				item.Value.ResetarPosicao();
			}
		}
	}
	
	public void LimparMoedas()
	{
		while (GameObject.Find("Coin(Clone)"))
		{
			Destroy(GameObject.Find("Coin(Clone)"));
		}
		
	}
	
	public enum STATE
	{
		JOGADOR_ENTROU,
		JOGADOR_SAIU,
		NENHUM_JOGADOR_ENTROU,
	}
	
	public enum FLUXO
	{
		ESPERANDO_JOGADORES,
		JOGO,
		PLACAR,
	}
}
