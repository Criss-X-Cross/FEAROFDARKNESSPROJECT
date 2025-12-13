using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    public CanvasGroup menu1;
    public CanvasGroup menu2;
    public CanvasGroup menu3;

    public float fadeDuration = 0.3f;

    private CanvasGroup currentMenu;

    void Start()
    {
        currentMenu = menu1;
        ShowInstant(menu1);
        HideInstant(menu2);
        HideInstant(menu3);
    }

    // ===== PUBLIC BUTTON CALLS =====
    public void GoToMenu1() => SwitchMenu(menu1);
    public void GoToMenu2() => SwitchMenu(menu2);
    public void GoToMenu3() => SwitchMenu(menu3);

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // ===== CORE LOGIC =====
    void SwitchMenu(CanvasGroup nextMenu)
    {
        if (currentMenu == nextMenu) return;

        StopAllCoroutines();
        StartCoroutine(FadeSwitch(currentMenu, nextMenu));
        currentMenu = nextMenu;
    }

    IEnumerator FadeSwitch(CanvasGroup from, CanvasGroup to)
    {
        // disable input on old menu
        from.interactable = false;
        from.blocksRaycasts = false;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            from.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
        from.alpha = 0;

        // enable new menu
        to.gameObject.SetActive(true);
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            to.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        to.alpha = 1;
        to.interactable = true;
        to.blocksRaycasts = true;
    }

    void ShowInstant(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    void HideInstant(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}
