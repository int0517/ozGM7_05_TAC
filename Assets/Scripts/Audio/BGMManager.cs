using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    [SerializeField] private AudioClip titleBgm;
    [SerializeField] private AudioClip gameBgm;
    [SerializeField] private string titleSceneName = "TitleScene";
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField, Range(0f, 1f)] private float volume = 0.45f;
    [SerializeField] private float fadeDuration = 0.5f;

    private AudioSource audioSource;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayForScene(SceneManager.GetActiveScene().name, true);
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForScene(scene.name, false);
    }

    public void PlayTitleBgm()
    {
        Play(titleBgm, false);
    }

    public void PlayGameBgm()
    {
        Play(gameBgm, false);
    }

    private void PlayForScene(string sceneName, bool immediate)
    {
        if (sceneName == titleSceneName)
        {
            Play(titleBgm, immediate);
        }
        else if (sceneName == gameSceneName)
        {
            Play(gameBgm, immediate);
        }
    }

    private void Play(AudioClip clip, bool immediate)
    {
        if (clip == null || audioSource == null)
        {
            return;
        }

        if (audioSource.clip == clip && audioSource.isPlaying)
        {
            return;
        }

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        if (immediate || fadeDuration <= 0f || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            return;
        }

        fadeRoutine = StartCoroutine(FadeTo(clip));
    }

    private IEnumerator FadeTo(AudioClip nextClip)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.clip = nextClip;
        audioSource.Play();

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, volume, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = volume;
        fadeRoutine = null;
    }
}
