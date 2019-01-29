using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PhotonManager : Photon.PunBehaviour {

    private AsyncOperation async;

	// Use this for initialization
	void Start () {
        //先預載場景
        async = PhotonNetwork.LoadLevelAsync("FightVR");
        async.allowSceneActivation = false;

        PhotonNetwork.ConnectUsingSettings("TSVRv1.0");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public override void OnConnectedToMaster()
    {
        print("已連線至伺服器");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("已連接至大廳");
    }

    //房間設定&開、進房間，註冊給開始按鈕
    public void JoinRoom()
    {

        RoomOptions room = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("GameRoom", room, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        print("已加入房間，目前共"+PhotonNetwork.room.MaxPlayers +"人");
        //進入房間後轉場景，生成玩家
        DontDestroyOnLoad(this);
        async.allowSceneActivation = true;
        CreatePlayer();
    }

    
    private void  CreatePlayer() {
        PhotonNetwork.Instantiate("[CameraRig]",transform.position,Quaternion.identity,0);  //生成位置還要再調整
        print("我是第"+PhotonNetwork.player.ID+"號玩家");
    }

    

    
}
