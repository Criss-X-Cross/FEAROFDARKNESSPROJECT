using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TriggerEvent : MonoBehaviour
{
    [Header("Trigger Event Settings")]
    [SerializeField] bool destroyOnTriggerEnter;
    [SerializeField] string tagFilter;
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;

    [Header("Delay Settings")]
    [SerializeField] float delayToMenu = 5f; // waktu tunggu sebelum balik menu

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
        {
            return;
        }

        hasTriggered = true;

        onTriggerEnter.Invoke();

        Invoke(nameof(BackToMainMenu), delayToMenu);

        //once triggered, destroy this component
        //if (destroyOnTriggerEnter)
        //{
        //    Destroy(gameObject);
        //}

        Debug.Log("Trigger Enter");
    }
    void OnTriggerExit(Collider other)
    {
        if(!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
        {
            return;
        }
        onTriggerExit.Invoke();
        Debug.Log("Exit");
    }

    void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
