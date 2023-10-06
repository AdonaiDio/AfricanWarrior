using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    
    public void NewGame()
    {
        DataPersistenceManager.instance.DeleteJson();
    }
    public void LoadGame()
    {
        DataPersistenceManager.instance.LoadJson();
    }
    public void GoToSceneIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SaveGame()
    {
        DataPersistenceManager.instance.SaveJson();
    }
}
