using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameSreens : MonoBehaviour
{
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private bool pauseAux;
    public AudioSource actionSound;
    private GameObject canvasInstance;
    [SerializeField] private GameObject canvas;
    private Image pauseMenu;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        if (canvas != null)
        {
            canvasInstance = Instantiate(canvas);
        }
        Transform imageTransform = canvasInstance.transform.Find("PauseMenu"); // Reemplaza "Imagen" por el nombre real de tu objeto
        if (imageTransform != null)
        {
            pauseMenu = imageTransform.GetComponent<Image>();
        }
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
        if(pauseMenu == null)
        {
            GameObject imageTransform = GameObject.Find("PauseMenu");
            pauseMenu = imageTransform.transform.GetComponent<Image>();
        }
        pauseMenu.gameObject.SetActive(false);

        // Oculta y bloquea el cursor al reanudar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ChangeScene()
    {
        Time.timeScale = 1f;
        if (actionSound != null)
        {
            actionSound.Play();
            Debug.Log("entrta a if");
            StartCoroutine(LoadSceneAfterSound(actionSound.clip.length));
        }
        else
        {
            SceneManager.LoadScene("Scenes/MainMenu");
        }
    }

    private IEnumerator LoadSceneAfterSound(float delay)
    {
        Debug.Log("inicisa");
        yield return new WaitForSeconds(delay);
        Debug.Log("termina");
        SceneManager.LoadScene("Scenes/MainMenu");
        Debug.Log("casrga");
        pauseMenu.gameObject.SetActive(false);
    }

    private void PauseGame()
    {
        Debug.Log("pausado");
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        pauseAux = false;

        // Desbloquea y muestra el cursor al pausar
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
