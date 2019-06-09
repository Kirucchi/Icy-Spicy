using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private int currentScene;
    private GameObject objectsToMove;
    private int dimension = 0;
    private float cooldown = 0.5f;
    
	void Start () {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        objectsToMove = GameObject.Find("ObjectsToMove");
	}

    private float timePassed = 0;
	void Update () {

        timePassed += Time.deltaTime;

        if (timePassed >= cooldown && Input.GetKeyDown(KeyCode.Z)) {
            if (dimension == 0) {
                objectsToMove.transform.position += new Vector3(0, -15, 0);
                dimension = 1;
            }
            else {
                objectsToMove.transform.position += new Vector3(0, 15, 0);
                dimension = 0;
            }
            timePassed = 0;
        }
		
	}

    public void LoadNextLevel() {
        SceneManager.LoadScene(currentScene + 1);
    }

    public void RestartLevel() {
        SceneManager.LoadScene(currentScene);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void ToMainMenu() {
        SceneManager.LoadScene(0);
    }
}
