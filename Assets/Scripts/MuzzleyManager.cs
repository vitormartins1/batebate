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
	string idCarroSaiu;
	string nomeDoUsuario;
	string fotoUrl;
	public GUITexture qrcodeGuiTexture;
	public GUITexture qrcodeGuiTexture2;
	bool showQr;
	public Bounds actionView;
	public List<Vector3> posicoesIniciais;
	public List<GameObject> iaBumperCars;
	public List<GameObject> coins;
	public CameraFollow cameraFollow;
	public Transform visaoDeAcao;
	public GameObject guiaDaCamera;
	public Dictionary<string, object> motion;
	public Dictionary<string, object> motionParams;
	public int jogadoresMax;
	public const float TEMPO_ESPERA = 25;
	public float tempoDeEspera = TEMPO_ESPERA;
	public const float TEMPO_JOGO = 60;
	public float tempoDeJogo = TEMPO_JOGO;
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
		qrcodeGuiTexture2.enabled = true;
		moedasObj = (GameObject)Instantiate(moedasPrefab);
		jogadoresMax = 6;
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
		idCarroSaiu = muzzley_participant.Id;
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
		qrcodeGuiTexture2.texture = www.texture;
		showQr = false;
		InterfaceSalaMuzzley.Instance.tempo.text = activityId;
        //renderer.material.mainTexture = www.texture;
	}

	public void CriarCarrosIA ()
	{
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
	}

	public void DestruirCarrosParticipantes ()
	{
		// aqui destruimos os carros dos participantes
		foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
			Destroy(item.Value.gameObject);
	}

	public void DestruirCarrosIA ()
	{
		//aqui limpamos a lista de jogadores ia e excluimos eles
		foreach (GameObject item in iaBumperCars) {
			Destroy(item.gameObject);
		}
		iaBumperCars.Clear();
	}
	
	public void EstadosDeJogo()
	{
		switch (fluxo) {
			case FLUXO.ESPERANDO_JOGADORES:
				if (participantes.Count > 0)
				{
					tempoDeEspera -= Time.deltaTime;
					GameObject.Find("Titulo2").guiText.text = "Esperando mais jogadores...  " + ((int)tempoDeEspera).ToString();
				}
				else
				{
					tempoDeEspera = TEMPO_ESPERA;
					GameObject.Find("Titulo2").guiText.text = "Venha jogar!";
					LimparMoedas();
				}
				
				if (tempoDeEspera <= 0)
				{
					Destroy(esperandoJogadorObj);
					Destroy(moedasObj);
					contagemRegressivaObj = (GameObject)Instantiate(contagemRegressivaPrefab);
					fluxo = FLUXO.JOGO;

					qrcodeGuiTexture2.enabled = false;

					if (participantes.Count > 0)
					{	
						DestruirCarrosParticipantes ();
						RecriarCarrosDestruidos();
					}
					
					CriarCarrosIA ();
					LimparMoedas();
					tempoDeEspera = TEMPO_ESPERA;
				}
				
				break;
			case FLUXO.JOGO:
				if (participantes.Count == 0) {
					fluxo = FLUXO.ESPERANDO_JOGADORES;
					esperandoJogadorObj = (GameObject)Instantiate(esperandoJogadorPrefab);
					qrcodeGuiTexture2.enabled = true;
					moedasObj = (GameObject)Instantiate(moedasPrefab);
					Destroy(contagemRegressivaObj);
					tempoDeJogo = TEMPO_JOGO;
					jogoIniciado = false;
					DestruirCarrosIA ();
					LimparMoedas();
				}
				
				if (!jogoIniciado)
				{
					if (((int)contagemRegressiva) <= 0)
					{
						contagemRegressivaObj.guiText.text = "Vai!";
						if (((int)contagemRegressiva) <= -1)
						{
							jogoIniciado = true;
							tempoDeJogo = TEMPO_JOGO;
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
						
						LimparMoedas();
						
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
												
						//Limpamos a lista de carros seguidos pela ia
						BumperCarsManager.Instance.bumperCars.Clear();
						
						carrosCriados = false;
					}
				}
				break;
			case FLUXO.PLACAR:
				if (participantes.Count == 0) {
					fluxo = FLUXO.ESPERANDO_JOGADORES;
					esperandoJogadorObj = (GameObject)Instantiate(esperandoJogadorPrefab);
					qrcodeGuiTexture2.enabled = true;
					moedasObj = (GameObject)Instantiate(moedasPrefab);
					Destroy(placarObj);
					tempoDeJogo = TEMPO_JOGO;
					InterfaceSalaMuzzley.Instance.tempo.text = /*"ID: " + */activityId + "\n" + /*"Tempo: " + */((int)tempoDeJogo).ToString();
					jogoIniciado = false;
				}
				
				if (jogarNovamente){
					JogarNovamente();
				}
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
			newCar.GetComponent<BumperCarMuzzley>().indexPosicaoInicial = participantes.Count;
			newCar.GetComponent<BumperCarMuzzley>().posicao = posicoesIniciais[newCar.GetComponent<BumperCarMuzzley>().indexPosicaoInicial];
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
			state = STATE.NENHUM_JOGADOR_ENTROU;
			if (idCarroSaiu != null)
			{
				BumperCarsManager.Instance.bumperCars.Remove(participantes[idCarroSaiu].gameObject);
				Destroy(participantes[idCarroSaiu].gameObject);
				participantes.Remove(idCarroSaiu);

				foreach (GameObject item in InterfaceSalaMuzzley.Instance.huds) {
					Debug.Log(item);
					if (item.GetComponent<UsuarioHudMuzzley>().id == idCarroSaiu)
					{
						item.GetComponent<UsuarioHudMuzzley>().Destruir();
						InterfaceSalaMuzzley.Instance.huds.Remove(item);
						break;
					}
				}
				
				idCarroSaiu = null;
			}
			break;
		}
	}

	public void RecriarCarrosDestruidos ()
	{
		// recriamos aqui os carros dos participantes destruidos
		int temp = 0;
		var buffer = new List<string>(participantes.Keys);
		foreach (string key in buffer)
		{
			GameObject newCar = (GameObject)Instantiate(bumperCarPrefab);
			//newCar.GetComponent<BumperCarMuzzley>().posicao = posicoesIniciais[Convert.ToInt32(key)];
			newCar.GetComponent<BumperCarMuzzley>().posicao = posicoesIniciais[temp];
			BumperCarsManager.Instance.bumperCars.Add(newCar);
			temp++;
			
			for (int i = 0; i < InterfaceSalaMuzzley.Instance.huds.Count; i++)
			{
				if (InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().id == key)
				{
					newCar.name = "CarroDo" + InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().nome.guiText.text;
					newCar.GetComponent<BumperCarBase>().carenagem.renderer.material.color = InterfaceSalaMuzzley.Instance.huds[i].GetComponent<UsuarioHudMuzzley>().cor;
				}
			}
			
			participantes[key] = newCar.GetComponent<BumperCarMuzzley>();
		}
		
		if (participantes.Count > 0)
		{
			foreach (KeyValuePair<string, BumperCarMuzzley> item in participantes)
			{
				item.Value.ResetarPosicao();
			}
		}
	}
	
	public void JogarNovamente()
	{
		DestruirCarrosParticipantes();
		Destroy(placarObj);
		tempoDeJogo = TEMPO_JOGO;
		InterfaceSalaMuzzley.Instance.tempo.text = /*"ID: " + */activityId + "\n" + /*"Tempo: " + */((int)tempoDeJogo).ToString();
		contagemRegressivaObj = (GameObject)Instantiate(contagemRegressivaPrefab);
		fluxo = FLUXO.JOGO;
		jogarNovamente = false;
		
		CriarCarrosIA();
		RecriarCarrosDestruidos ();
	}
	
	public void LimparMoedas()
	{
		for (int i = 0; i < coins.Count; i++)
		{
			Destroy(coins[i]);
		}
		
		coins.Clear();
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
