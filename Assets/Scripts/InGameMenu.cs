using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    GameObject settingsMenu;
    [SerializeField]
    Button settingsButton;
    [SerializeField]
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Resume()
    {
        gameManager.PauseTimer(false);
        gameManager.LockMovement(false);
        gameObject.SetActive(false);
    }

    public void Settings()
    {
        GameObject settings = Instantiate(settingsMenu, transform.position, Quaternion.identity, transform);
        Settings settingsScript = settings.GetComponent<Settings>();
        settingsScript.settingsButton = settingsButton;
    }

    public void Exit()
    {
        gameManager.ExitGame();
    }
}
