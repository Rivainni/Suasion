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
    DataPersistenceManager dataPersistenceManager;
    void Start()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        dataPersistenceManager.LoadGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
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