using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartWaveAnnouncement : MonoBehaviour
{
    private const string HudSceneName = "GameHUDScene";
    private const string AnnouncementName = "GameStartWaveAnnouncement";

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterSceneLoadHandler()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void ShowIfHudSceneAlreadyLoaded()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == HudSceneName && scene.isLoaded)
            {
                Create(scene);
            }
        }
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == HudSceneName)
        {
            Create(scene);
        }
    }

    private static void Create(Scene scene)
    {
        Canvas canvas = FindCanvas(scene);
        if (canvas == null || canvas.transform.Find(AnnouncementName) != null) return;

        GameObject root = new GameObject(AnnouncementName, typeof(RectTransform), typeof(CanvasGroup), typeof(Image), typeof(GameStartWaveAnnouncement));
        root.transform.SetParent(canvas.transform, false);
        root.transform.SetAsLastSibling();

        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = new Vector2(0.5f, 0.5f);
        rootRect.anchorMax = new Vector2(0.5f, 0.5f);
        rootRect.pivot = new Vector2(0.5f, 0.5f);
        rootRect.anchoredPosition = Vector2.zero;
        rootRect.sizeDelta = new Vector2(560f, 140f);

        Image background = root.GetComponent<Image>();
        background.color = new Color(0.02f, 0.025f, 0.035f, 0.78f);
        background.raycastTarget = false;

        CanvasGroup group = root.GetComponent<CanvasGroup>();
        group.alpha = 0f;
        group.blocksRaycasts = false;
        group.interactable = false;

        Text referenceText = canvas.GetComponentInChildren<Text>(true);

        GameObject textObject = new GameObject("Text", typeof(RectTransform), typeof(Text), typeof(Outline), typeof(Shadow));
        textObject.transform.SetParent(root.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(24f, 14f);
        textRect.offsetMax = new Vector2(-24f, -14f);

        Text text = textObject.GetComponent<Text>();
        text.text = "1WAVE \uC2DC\uC791!";
        text.alignment = TextAnchor.MiddleCenter;
        text.font = referenceText != null ? referenceText.font : Font.CreateDynamicFontFromOSFont("Malgun Gothic", 56);
        text.fontSize = 56;
        text.fontStyle = FontStyle.Bold;
        text.color = new Color(1f, 0.92f, 0.58f, 1f);
        text.raycastTarget = false;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 28;
        text.resizeTextMaxSize = 56;

        Outline outline = textObject.GetComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        outline.effectDistance = new Vector2(3f, -3f);

        Shadow shadow = textObject.GetComponent<Shadow>();
        shadow.effectColor = new Color(0f, 0f, 0f, 0.65f);
        shadow.effectDistance = new Vector2(0f, -6f);
    }

    private static Canvas FindCanvas(Scene scene)
    {
        GameObject[] roots = scene.GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            Canvas canvas = roots[i].GetComponentInChildren<Canvas>(true);
            if (canvas != null)
            {
                return canvas;
            }
        }

        return null;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        yield return AnimateIn();
        yield return new WaitForSecondsRealtime(1.1f);
        yield return AnimateOut();

        Destroy(gameObject);
    }

    private IEnumerator AnimateIn()
    {
        float elapsed = 0f;
        const float duration = 0.34f;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            canvasGroup.alpha = t;
            rectTransform.localScale = Vector3.one * Mathf.Lerp(0.68f, 1.08f, t);
            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0f, 34f), Vector2.zero, t);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private IEnumerator AnimateOut()
    {
        float elapsed = 0f;
        const float duration = 0.45f;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            canvasGroup.alpha = 1f - t;
            rectTransform.localScale = Vector3.one * Mathf.Lerp(1f, 1.16f, t);
            rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, new Vector2(0f, -38f), t);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}
