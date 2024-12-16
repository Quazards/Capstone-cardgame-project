using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip menuMusic;
    public AudioClip battleMusic1;
    public AudioClip battleMusic2;
    public AudioClip cardDrawSound;
    public AudioClip cardFlipSound;
    public AudioClip cardPutSound;
    public AudioClip damageTakenSound;
    public AudioClip zeroDamageSound;

    public static AudioManager Instance;

    private bool isUsingRegularBattle = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene();
    }

    private void PlayMusicForScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "MainMenu")
        {
            musicSource.clip = menuMusic;
            musicSource.Play();
        }
        else if(currentScene == "Post-Game Scene")
        {
            if (isUsingRegularBattle)
                return;

            musicSource.clip = battleMusic2;
            musicSource.Play();
            isUsingRegularBattle = true;
        }
        else if(currentScene == "CombatScene")
        {
            if(DataPersistenceManager.instance.gameData.encounterCount == 4)
            {
                musicSource.clip = battleMusic1;
                musicSource.Play();
                isUsingRegularBattle = false;
            }
            else
            {
                if (isUsingRegularBattle)
                    return;

                musicSource.clip = battleMusic1;
                musicSource.Play();
                isUsingRegularBattle = true;
            }
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
