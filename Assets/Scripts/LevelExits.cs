using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExits : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            // Debug.Log("Last Scene");
            
            FindObjectOfType<GameSession>().ResetGameSession();
            SceneManager.LoadScene(0);
 
            //nextSceneIndex = 0;
        }
        else{
        FindObjectOfType<ScenePersists>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
 
        }
   }



}
