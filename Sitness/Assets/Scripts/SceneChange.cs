using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneChange : MonoBehaviour
{
    public static class SceneNames
    {
        public const string MainMenu = "MainMenu";
        public const string PlanetWalking = "PlanetWalking";
        public const string CloudCreation = "CloudCreationExercise";
        public const string CoolDown = "CoolDown";
    }
    
    public InputActionReference returnToMainMenuButton;

    public CanvasGroup canvasGroup;

    public bool doFade = false;
    public bool changeScene = false;
    public string nextSceneName = "";

    [SerializeField]
    [Range(0f, 1f)]
    private float endAlphaVal;

    [SerializeField]
    [Range(1f, 2f)]
    private float fadeDuration;

    private float elapsedTime;

    void Start()
    {
        StartScene(1f);
    }

    private void OnEnable()
    {
        returnToMainMenuButton.action.performed += LoadMainMenuScene;
    }

    private void OnDisable()
    {
        returnToMainMenuButton.action.performed -= LoadMainMenuScene;
    }

    // TODO Add some nicer transition between scenes

    public void LoadWarmUpScene()
    {
        Debug.Log("Loading warm up scene...");
        //SceneManager.LoadScene("CloudCreationExercise");
        EndScene(2f, SceneNames.CloudCreation);
    }

    public void LoadMainExerciseScene()
    {
        Debug.Log("Loading main exercise scene...");
        //SceneManager.LoadScene("PlanetWalking");
        EndScene(2f, SceneNames.PlanetWalking);
    }

    public void LoadCoolDownScene()
    {
        Debug.Log("Loading cool down scene...");
        //SceneManager.LoadScene("CoolDown");
        EndScene(2f, SceneNames.CoolDown);
    }

    public void LoadMainMenuScene(InputAction.CallbackContext callbackContext)
    {
        LoadMainMenuScene();
    }

    public void LoadMainMenuScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != SceneNames.MainMenu)
        {
            Debug.Log("Loading main menu scene...");
            //SceneManager.LoadScene("MainMenu");
            GameEvents.current.ExerciseExit();
            EndScene(2f, SceneNames.MainMenu);
        }
    }

    public void ReloadCurrentScene()
    {
        Debug.Log("Reloading current scene...");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    void Update()
    {
        if (doFade)
        {
            if (elapsedTime <= fadeDuration)
            {
                if (canvasGroup.alpha < endAlphaVal)
                {
                    canvasGroup.alpha += Time.deltaTime / fadeDuration;
                }
                else
                {
                    canvasGroup.alpha -= Time.deltaTime / fadeDuration;
                }
            }
            else
            {
                canvasGroup.alpha = endAlphaVal;
                if (changeScene && nextSceneName != "")
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                doFade = false;
            }
            elapsedTime += Time.deltaTime;
        }
    }

    private void StartScene(float duration)
    {
        canvasGroup.alpha = 1f;
        TriggerFade(0f, duration);
    }

    public void EndScene(float duration, string nextScene)
    {
        changeScene = true;
        canvasGroup.alpha = 0f;
        nextSceneName = nextScene;
        TriggerFade(1f, duration);
    }

    private void TriggerFade(float endAlpha, float duration)
    {
        if (!doFade)
        {
            doFade = true;
            endAlphaVal = endAlpha;
            fadeDuration = duration;
            elapsedTime = 0f;
        }
    }

}
