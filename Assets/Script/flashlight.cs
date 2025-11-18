using UnityEngine;

public class flashlight : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header ("Sway Settings")]
    [SerializeField] float swayMult;
    [SerializeField] float swaySmooth;
    // Update is called once per frame
    [Header ("Flickering")]
    [SerializeField] public float flickerRate;
    [SerializeField] public float minIntensity;
    [SerializeField] public float maxIntensity;

    private new Light light;

    private void Start()
    {
        light = GetComponent<Light>();

        InvokeRepeating(nameof(FlickerLight), 0f, flickerRate);
    }

    // Add the FlickerLight method to resolve the string reference
    private void FlickerLight()
    {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        light.intensity = randomIntensity;
    }

    private void Update()
    {
        //Sway
        float mouseX = Input.GetAxis("Mouse X") * swayMult;
        float mouseY = Input.GetAxis("Mouse Y") * swayMult;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmooth);
    }
}
