using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Trigger Event Settings")]
    [SerializeField] bool destroyOnTriggerEnter;
    [SerializeField] string tagFilter;
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;


    void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
        {
            return;
        }

        onTriggerEnter.Invoke();

        //once triggered, destroy this component
        //if (destroyOnTriggerEnter)
        //{
        //    Destroy(gameObject);
        //}

        Debug.Log("Trigger Enter");
    }
    void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
        {
            return;
        }
        onTriggerExit.Invoke();
        Debug.Log("Exit");
    }
}
