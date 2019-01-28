using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
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
    public override void OnJoinedRoom()
    {
        print("已加入房間，目前共"+PhotonNetwork.room.MaxPlayers +"人");
    }

    
    private void  CreatePlayer() {

    }

    public void JoinRoom() {

        RoomOptions room = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom("GameRoom", room, TypedLobby.Default);
    }
}
