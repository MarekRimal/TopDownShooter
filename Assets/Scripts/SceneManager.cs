using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void ReloadLevel() {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoadNextLevel() {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

	public void LoadLevel(string level) {
        Application.LoadLevel(level);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
