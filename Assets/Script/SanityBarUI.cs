using UnityEngine;
using UnityEngine.UI;

public class SanityBarUI : MonoBehaviour
{
    public Sanity sanity;          
    public Slider slider;           // UI bar
    public CanvasGroup canvasGroup; // buat fade-in/out bar

    void Start()
    {
        canvasGroup.alpha = 0;
        slider.value = 0;
    }

    void Update()
    {
        // Update nilai bar
        slider.value = sanity.NormalizedSanity;

        // Jika sedang refill → bar tampil
        if (sanity.IsRefilling)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, Time.deltaTime * 5f);
        }
        else
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, Time.deltaTime * 5f);
        }
    }
}
