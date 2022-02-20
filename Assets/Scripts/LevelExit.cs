using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        // Delay levelLoadDelay
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        // Get our current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // If we're at the last scene, wrap to scene 0
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;
        
        FindObjectOfType<ScenePersistence>().ResetScenePersistence();
        
        // Load the scene
        SceneManager.LoadScene(nextSceneIndex);
    }
}
