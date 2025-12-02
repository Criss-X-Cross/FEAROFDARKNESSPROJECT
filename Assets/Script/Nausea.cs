using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class NauseaPostFX : MonoBehaviour
{
    [Header("References")]
    public Volume volume;
    public Sanity sanity;

    [Header("Sanity Threshold")]
    public float effectStartSanity = 80f;   // Efek mulai jika sanity < 80

    LensDistortion lens;
    ChromaticAberration chroma;
    Vignette vignette;
    DepthOfField dof;

    float currentT = 0f;  // nilai lerp yang smooth

    void Start()
    {
        volume.profile.TryGet(out lens);
        volume.profile.TryGet(out chroma);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out dof);

        ResetEffects();  // benar-benar nol
    }

    void Update()
    {
        if (sanity == null)
            return;

        float s = sanity.CurrentSanity;

        float targetT = (s < effectStartSanity)
            ? Mathf.InverseLerp(effectStartSanity, 0, s)
            : 0f;

        // membuat efek naiknya smooth
        currentT = Mathf.Lerp(currentT, targetT, Time.deltaTime * 3f);

        ApplyEffects(currentT);
    }

    void ApplyEffects(float t)
    {
        // DISTORTION
        if (lens != null)
        {
            lens.intensity.value = Mathf.Lerp(0f, -0.5f, t);
            lens.scale.value = Mathf.Lerp(1f, 0.9f, t);
        }

        // CHROMA
        if (chroma != null)
            chroma.intensity.value = Mathf.Lerp(0f, 1f, t);

        // VIGNETTE
        if (vignette != null)
            vignette.intensity.value = Mathf.Lerp(0f, 0.45f, t);

        // DOF BLUR (Gaussian)
        if (dof != null)
        {
            dof.mode.value = DepthOfFieldMode.Gaussian;
            dof.gaussianStart.value = Mathf.Lerp(10f, 2.5f, t);
            dof.gaussianEnd.value = Mathf.Lerp(50f, 2.5f, t);
            dof.gaussianMaxRadius.value = Mathf.Lerp(0f, 0.7f, t);
        }
    }

    void ResetEffects()
    {
        if (lens != null)
        {
            lens.intensity.value = 0f;
            lens.scale.value = 1f;
        }

        if (chroma != null)
            chroma.intensity.value = 0f;

        if (vignette != null)
            vignette.intensity.value = 0f;

        if (dof != null)
        {
            // Normal camera (tidak blur sama sekali)
            dof.mode.value = DepthOfFieldMode.Off;

        }

        currentT = 0f;
    }
}
