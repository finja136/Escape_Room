using UnityEngine;
using System.Collections;

public class LightFlickerManager : MonoBehaviour // Bringt Deckenlichter zum Flackern, um "Instabilität" des Reactors deutlich zu machen. Welche Lampe wie lange und in welcher Frequenz flackert ist zufällig
{
    [Header("Lights")]
    [SerializeField] private Light[] ceilingLights;

    [Header("Timing")]
    [SerializeField] private float minInterval = 3f;
    [SerializeField] private float maxInterval = 10f;

    [SerializeField] private float minFlickerDuration = 0.1f;
    [SerializeField] private float maxFlickerDuration = 1.5f;

    [Header("Flicker Settings")]
    [SerializeField] private int minFlickers = 2;
    [SerializeField] private int maxFlickers = 8;


    void Start()
    {
        StartCoroutine(FlickerRoutine());
    }


    IEnumerator FlickerRoutine() // Coroutine, die zufällig Lampen auswählt, die flackern soll
    {
        while (true)
        {
            // Warten bis nächstes Flackern
            float waitTime = Random.Range(
                minInterval,
                maxInterval);

            yield return new WaitForSeconds(waitTime);

            // Zufällige Lampe auswählen
            int index = Random.Range(
                0,
                ceilingLights.Length);

            StartCoroutine(
                FlickerLight(ceilingLights[index]));
        }
    }


    IEnumerator FlickerLight(Light light) // Coroutine, die eine Lampe flackern lässt
    {
        float duration = Random.Range(
            minFlickerDuration,
            maxFlickerDuration);

        int flickers = Random.Range(
            minFlickers,
            maxFlickers);


        float timer = 0;

        while (timer < duration)
        {
            light.enabled = !light.enabled;

            float delay = Random.Range(
                0.03f,
                0.2f);

            yield return new WaitForSeconds(delay);

            timer += delay;
        }
        // Am Ende wieder einschalten
        light.enabled = true;
    }
}