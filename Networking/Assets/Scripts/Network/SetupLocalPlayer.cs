using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour {

    //Sends info through the server
    [SyncVar]
    public string pname = "player";
    //Sends info through the server
    [SyncVar]
    public Color playerColor = Color.white;

	// Use this for initialization
	void Start () {

        //If local player, enable
        if (isLocalPlayer)
        {
            GetComponent<Player>().enabled = true;
            GetComponent<PlayerStateMachine>().enabled = true;

        }

        //Grabs hold of all of the renderers
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            r.material.color = playerColor;

        //Change spawn of player
        this.transform.position = new Vector3(Random.Range(-17, 17), 0, Random.Range(-17, 17));

        this.transform.name = pname;
	}

}
