using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : Photon.PunBehaviour {

    private bool Joined;
    

    // Use this for initialization
    void Start () {
        Joined = false;
        PhotonNetwork.ConnectUsingSettings("TSVRv1.0");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnConnectedToMaster()
    {
        print("正在連接大廳");
        PhotonNetwork.JoinLobby();
    }
       

    public override void OnJoinedLobby()
    {
        print("已連接至大廳");
    }

    public void JoinRoom()
    {
        if (!Joined) {
            RoomOptions options = new RoomOptions { MaxPlayers = 2 };
            print("正在創建或加入房間");
            PhotonNetwork.JoinOrCreateRoom("gameRoom", options, TypedLobby.Default);
            Joined = true;
        }
       
    }

    public override void OnJoinedRoom()
    {
        print("已加入房間，目前有" + PhotonNetwork.room.PlayerCount.ToString() + "人");
    }

    void CreatePlayer() {
        
    }

    void EnterGameScene() {
        if (PhotonNetwork.room.PlayerCount == 2) {
            PhotonNetwork.LoadLevelAsync(1);
        }
    }
}
