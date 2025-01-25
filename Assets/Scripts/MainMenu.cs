using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject[] buttons;
    public GameObject skipButton;
    public float showButtonsTime = 8f;
    public AudioSource actionSound;
    private bool buttonsShown = false;
    public float fadeDuration = 0.5f;
    private float sceneLoadTime;

    void Start()
    {
        Time.timeScale = 1f;
        sceneLoadTime = Time.time;
        skipButton.SetActive(true);
    }
    void Update()
    {
        Debug.Log("time scale" + Time.timeScale);
        Debug.Log("Time " + Time.time);
        float sceneTime = Time.time - sceneLoadTime;
        Debug.Log("sceneTime " + sceneTime);


        if (videoPlayer.isPlaying && videoPlayer.time >= showButtonsTime && !buttonsShown)
        {
            ShowButtons(true);
        }
        if (sceneTime >= 6.5f)
        {
            if (skipButton != null)
            {
                skipButton.SetActive(false);
            }
        }

    }
    void ShowButtons(bool useFadeIn)
    {
        buttonsShown = true;
        foreach (GameObject button in buttons)
        {
            button.SetActive(true); 
            if (useFadeIn)
            {
                StartCoroutine(FadeIn(button)); 
            }
            else
            {
               
                CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = button.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = 1; 
            }
        }
    }
    public void SkipToEnd()
    {
        if (videoPlayer.isPrepared)
        {
           
            videoPlayer.frame = (long)(videoPlayer.frameCount - 1);
            videoPlayer.Pause();

           
            ShowButtons(false);

         
            if (skipButton != null)
            {
                skipButton.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("El video no está listo para saltar.");
        }
    }
    public void ChangeScene()
    {
        if (actionSound != null)
        {
            actionSound.Play();
            StartCoroutine(LoadSceneAfterSound(actionSound.clip.length));
        }
        else
        {

            SceneManager.LoadScene("Scenes/Level1");
        }

    }

    public void ExitGame()
    {
        if (actionSound != null)
        {
            actionSound.Play();
            StartCoroutine(QuitAfterSound(actionSound.clip.length));
        }
        else
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private IEnumerator LoadSceneAfterSound( float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Scenes/Level1");
    }

    private IEnumerator QuitAfterSound(float delay)
    {
        yield return new WaitForSeconds(delay);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    IEnumerator FadeIn(GameObject button)
    {
        CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
        UnityEngine.UI.Button uiButton = button.GetComponent<UnityEngine.UI.Button>();

        if (canvasGroup == null)
        {
            canvasGroup = button.AddComponent<CanvasGroup>();
        }
        if (uiButton != null)
        {
            uiButton.interactable = false;
        }

        canvasGroup.alpha = 0;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.SmoothStep(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1;
        if (uiButton != null)
        {
            uiButton.interactable = true;
        }
    }
}
