using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathFall : MonoBehaviour {

    GameUI gameUI;

	// Use this for initialization
	void Start () {

        FindObjectOfType<Player>();

    }
	
	// Update is called once per frame
	void Update () {

        gameUI.OnGameOver();

	}
}
