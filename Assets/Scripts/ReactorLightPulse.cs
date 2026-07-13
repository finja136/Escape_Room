using UnityEngine;
using System.Collections;

public class ReactorLightPulse : MonoBehaviour // Kontrolliert Lichtpulsieren, wenn der Timer auf 5 Minuten fällt
{
    [Header("Light Parent")]
    [SerializeField] private GameObject lightParent;

    [Header("Pulse Settings")]
    [SerializeField] private float normalIntensity = 1f;
    [SerializeField] private float darkIntensity = 0.15f;

    [SerializeField] private float fadeDuration = 1f;

    private Light[] reactorLights;
    private bool active = false;


    private void Awake()
    {
        if (lightParent != null)
        {
            reactorLights = lightParent.GetComponentsInChildren<Light>();
        }
    }

    public void StartEmergencyMode()
    {
        if (!active)
        {
            active = true;
            StartCoroutine(PulseLights());
        }
    }

    IEnumerator PulseLights()
    {
        while (active)
        {
            yield return FadeTo(darkIntensity);

            yield return FadeTo(normalIntensity);
        }
    }

    IEnumerator FadeTo(float targetIntensity) // Faded von startIntensity zu targetIntensity über fadeDuration
    {
        float startIntensity = reactorLights[0].intensity;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float value = Mathf.Lerp(
                startIntensity,
                targetIntensity,
                timer / fadeDuration
            );

            foreach (Light light in reactorLights)
            {
                light.intensity = value;
            }
            yield return null;
        }
    }
}