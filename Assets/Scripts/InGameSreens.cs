using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class InGameSreens : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private bool pauseAux;
    [SerializeField] private GameObject canvas;
    private Image pauseMenu;
    private GameObject CamMovement;

    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        Transform imageTransform = canvas.transform.Find("PauseMenu");
        if (imageTransform != null)
        {
            pauseMenu = imageTransform.GetComponent<Image>();
        }
    }
    void Start()
    {
        CamMovement = GameObject.Find("PlayerFollowCamera");
        virtualCamera = CamMovement.GetComponent<CinemachineVirtualCamera>();
    }
    private void Update()
    {
        if (starterAssetsInputs.pause)
        {
            if (pauseAux)
            {
                PauseGame();
            }
        }
        else
        {
            pauseAux = true;
        }
    }

    public void Resume()
    {
        Debug.Log("resume");
        Time.timeScale = 1f;
        if (pauseMenu == null)
        {
            GameObject imageTransform = GameObject.Find("PauseMenu");
            pauseMenu = imageTransform.transform.GetComponent<Image>();
        }
        GameObject CamMovement2 = GameObject.Find("PlayerFollowCamera");
        CinemachineVirtualCamera virtualCamera2 = CamMovement2.GetComponent<CinemachineVirtualCamera>();
        pauseMenu.gameObject.SetActive(false);
        virtualCamera2.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void ChangeScene()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Scenes/MainMenu");
        pauseMenu.gameObject.SetActive(false);

    }


    private void PauseGame()
    {
        virtualCamera.enabled = false;
        Debug.Log("pausado");
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        pauseAux = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
