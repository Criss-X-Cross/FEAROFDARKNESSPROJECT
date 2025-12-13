using UnityEngine;

public class Sanity : MonoBehaviour
{
    [Header("Sanity Settings")]
    [Tooltip("Maximum sanity value")]
    public float maxSanity = 100f;

    [Tooltip("How fast sanity drains per second when outside the box")]
    public float drainRate = 2f;

    [Tooltip("How fast sanity refills per second when inside the box")]
    public float refillRate = 0.5f;

    [SerializeField, Tooltip("Current sanity (visible in inspector)")]
    private float currentSanity;

    private bool insideBox = false;

    public float CurrentSanity => currentSanity;
    public float NormalizedSanity => maxSanity > 0f ? currentSanity / maxSanity : 0f;
    public bool IsDepleted => currentSanity <= 0f;

    public bool IsRefilling => insideBox && currentSanity < maxSanity;

    //Sound
    [Header("Low Sanity Sound")]
    public AudioSource heartBeat;
    public AudioSource breathing;

    public float lowSanityThreshold = 25f;
    public bool isLowSanityActive = false;

    void Start()
    {
        insideBox = false;

        currentSanity = maxSanity; // <-- PENTING
        isLowSanityActive = false;

        DetectInitialInside();

        if (heartBeat != null) heartBeat.Stop();
        if (breathing != null) breathing.Stop();
    }

        void Update()
    {
        if (insideBox)
        {
            currentSanity += refillRate * Time.deltaTime;
        }
        else
        {
            currentSanity -= drainRate * Time.deltaTime;

        }

        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        HandleLowSanitySound();

    }
    private void HandleLowSanitySound()
    {
        if (heartBeat == null || breathing == null) return;

        if (currentSanity <= lowSanityThreshold)
        {
            if (!isLowSanityActive)
            {
                heartBeat.Play();
                breathing.Play();
                isLowSanityActive = true;
            }

            // Volume makin besar saat sanity makin rendah
            float t = currentSanity / lowSanityThreshold; // 1 → 0
            heartBeat.volume = Mathf.Lerp(3f, 0.5f, t);
            breathing.volume = Mathf.Lerp(1f, 0.2f, t);
        }
        else
        {
            if (isLowSanityActive)
            {
                heartBeat.Stop();
                breathing.Stop();
                isLowSanityActive = false;
            }
        }
    }
    private void DetectInitialInside()
    {
        Collider myCol = GetComponent<Collider>();
        if (myCol == null)
        {
            return;
        }

        Collider[] hits = Physics.OverlapBox(myCol.bounds.center, myCol.bounds.extents, transform.rotation);
        foreach (var c in hits)
        {
            if (c == myCol) continue;
            BoxCollider bc = c.GetComponent<BoxCollider>();
            bool match = c.CompareTag("SafeBox"); //Triggered only in safeBox tagged areas
            if (match)
            {
                insideBox = true;
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var bc = other.GetComponent<BoxCollider>();
        bool match = other.CompareTag("SafeBox"); //Triggered only in safeBox tagged areas

        if (match)
        {
            insideBox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var bc = other.GetComponent<BoxCollider>();
        bool match = other.CompareTag("SafeBox"); //Triggered only in safeBox tagged areas

        if (match)
        {
            insideBox = false;
        }
    }
}
