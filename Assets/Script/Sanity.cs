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

    // True while player is inside a BoxCollider trigger area
    // not serialized to avoid inspector persisting an incorrect value
    private bool insideBox = false;

    public float CurrentSanity => currentSanity;
    public float NormalizedSanity => maxSanity > 0f ? currentSanity / maxSanity : 0f;
    public bool IsDepleted => currentSanity <= 0f;

    // True when currently refilling (inside box AND not already full)
    public bool IsRefilling => insideBox && currentSanity < maxSanity;

    void Start()
    {
        // ensure sane initial state
        insideBox = false;
        if (currentSanity <= 0f) currentSanity = maxSanity;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        // Detect if we are already overlapping a safe trigger at start
        DetectInitialInside();

        Debug.Log($"Sanity.Start: currentSanity={currentSanity}, insideBox={insideBox}");
    }

    void Update()
    {
        // Drain or refill depending on whether we're inside the box
        if (insideBox)
        {
            currentSanity += refillRate * Time.deltaTime;
            Debug.Log("Fill Sanity: " + currentSanity);
        }
        else
        {
            currentSanity -= drainRate * Time.deltaTime;
            Debug.Log("Drain Sanity: " + currentSanity);

        }

        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        if (IsDepleted)
        {
            // TODO: trigger death/insanity behaviour here
        }
    }

    // Try to detect initial overlap using the player's collider bounds.
    private void DetectInitialInside()
    {
        Collider myCol = GetComponent<Collider>();
        if (myCol == null)
        {
            Debug.LogWarning("Sanity: No Collider found on this GameObject; cannot check initial overlap.");
            return;
        }

        // OverlapBox uses half-extents (bounds.extents)
        Collider[] hits = Physics.OverlapBox(myCol.bounds.center, myCol.bounds.extents, transform.rotation);
        foreach (var c in hits)
        {
            if (c == myCol) continue;
            BoxCollider bc = c.GetComponent<BoxCollider>();
            bool match = (bc != null && bc.isTrigger) || c.CompareTag("SafeBox");
            if (match)
            {
                insideBox = true;
                Debug.Log($"Sanity: Detected starting inside safe area via collider '{c.name}'");
                return;
            }
        }
    }

    // Requires the safe area to use a BoxCollider (recommended as isTrigger = true)
    private void OnTriggerEnter(Collider other)
    {
        var bc = other.GetComponent<BoxCollider>();
        bool match = (bc != null && bc.isTrigger) || other.CompareTag("SafeBox");
        Debug.Log($"Sanity.OnTriggerEnter: other={other.name}, hasBoxCollider={(bc != null)}, isTrigger={(bc != null ? bc.isTrigger : false)}, compareTagSafeBox={other.CompareTag("SafeBox")}, match={match}");

        if (match)
        {
            insideBox = true;
            Debug.Log("Sanity: Entered safe box (insideBox=true)");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var bc = other.GetComponent<BoxCollider>();
        bool match = (bc != null && bc.isTrigger) || other.CompareTag("SafeBox");
        Debug.Log($"Sanity.OnTriggerExit: other={other.name}, match={match}");

        if (match)
        {
            insideBox = false;
            Debug.Log("Sanity: Exited safe box (insideBox=false)");
        }
    }
}
