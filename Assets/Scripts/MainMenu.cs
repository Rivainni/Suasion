using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    Button settingsButton;
    [SerializeField]
    Button continueButton;
    [SerializeField]
    LevelLoader levelLoader;
    DataPersistenceManager dataPersistenceManager;
    void Start()
    {
        dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
        if (dataPersistenceManager.SaveExists())
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
        PlayMusic("Title");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void StartGame()
    {
        dataPersistenceManager.NewGame();
        levelLoader.LoadLevel("Intro Cutscene");
    }

    public void ContinueGame()
    {
        levelLoader.LoadLevel("Main Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        GameObject settings = Instantiate(settingsMenu, transform.position, Quaternion.identity, transform);
        Settings settingsScript = settings.GetComponent<Settings>();
        settingsScript.settingsButton = settingsButton;
    }

    void PlayMusic(string music)
    {
        AkSoundEngine.PostEvent("Enter_" + music, gameObject);
    }
}