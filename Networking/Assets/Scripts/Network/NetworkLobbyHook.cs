using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

//Will override the current LobbyHook
public class NetworkLobbyHook : LobbyHook {

    //When player loads, takes lobbyPlayer info, gamePlayer, and giving lobby player levels
    //to the gameplayer

    //This connects all values that are in player info to values that are in the game
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //Grab lobby player from PlayerInfo
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        //Grab local player
        SetupLocalPlayer localPlayer = gamePlayer.GetComponent<SetupLocalPlayer>();

        localPlayer.playerColor = lobby.playerColor;
        localPlayer.pname = lobby.playerName;
    }

}
