using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour // Das Script kommt aus dem Asset-Store und wurde für unsere Zwecke angepasst. Davor konnte man nur einen bis zu 9 stellen langen code eingeben. Jetzt ist das script mit dem KeypadPuzzleManager gekoppelt.
    {                                   // Außerdem kann man 3 codes eingeben und bekommt zusätzliches feedback über Indicator über dem Keypad. Schließlich wird das Keypad erst interactable, wenn das erste Rätselt gelöst wurde.
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;
        [Header("Combination Code (4 Numbers Max)")]
        [SerializeField] private KeypadPuzzleManager puzzleManager;

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessDeniedText = "Denied";
        [SerializeField] private int[] codes = new int[3];
        private int currentStage = 0;

        [Header("LightColors")]
        [SerializeField] private Renderer[] indicators;

        [SerializeField] private Material offMaterial;
        [SerializeField] private Material greenMaterial;
        [SerializeField] private Material redMaterial;

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); //orangy
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); //red
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); //greenish
        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;
        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;
        [Header("Hint System")]
        [SerializeField] private HintSystemManager hintSystemManager;


        private string currentInput;
        private bool displayingResult = false;
        private bool isLocked = true;
        private XRSimpleInteractable[] buttons;

        

        private void Awake()
        {
            buttons = GetComponentsInChildren<XRSimpleInteractable>();
            SetButtonsLocked(true);
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
            SetLockedVisual(true);
            for (int i = 0; i < indicators.Length; i++)
            {
                indicators[i].material = offMaterial;
            }

        }

        private void SetButtonsLocked(bool locked)
        {
            foreach (var b in buttons)
            {
                b.enabled = !locked;
            }
        }

        public void UnlockKeypad() //Wird vom EnergyPuzzleManager aufgerufen, wenn das erste Rätesel gelöst wurde
        {
            isLocked = false;

            ClearInput();
            SetLockedVisual(isLocked);
            SetButtonsLocked(isLocked);
        }

        private void SetLockedVisual(bool locked)
        {
            if (locked)
            {
                keypadDisplayText.text = "";
                panelMesh.material.SetVector("_EmissionColor", Color.black);
            }
            else
            {
                panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
                puzzleManager.ShowPuzzle(currentStage);
            }
        }

        public void AddInput(string input) // Wird von KeypadButtons aufgerufen
        {
            if (isLocked)
                return;

            audioSource.PlayOneShot(buttonClickedSfx);

            if (displayingResult) // Buttonpress geht nur durch, wenn kein Ergebenis angezeigt wird
                return;

            switch (input)
            {
                case "enter": // Für Enterbutton press wird überprüft, ob eingabe richtig ist
                    CheckCombo();
                    break;
                default:
                    if (currentInput != null && currentInput.Length == 4)
                    {
                        return;
                    }
                    currentInput += input;
                    keypadDisplayText.text = currentInput;
                    break;
            }

        }

        public void CheckCombo()
        {
            if (!int.TryParse(currentInput, out var input))
            {
                Debug.LogWarning("Invalid input");
                return;
            }

            bool correct = input == codes[currentStage];

            if (correct)
            {
                StartCoroutine(CorrectCodeRoutine());
            }
            else
            {
                StartCoroutine(WrongCodeRoutine());
            }
        }

        private IEnumerator CorrectCodeRoutine() // Setzt aktuellen indicator grün, spielt sound ab, geht zum nächsten Rätsel (puzzleManager und HintManager) oder öffnet die Tür, wenn alle Rätsel gelöst
        {
            displayingResult = true;

            indicators[currentStage].material = greenMaterial;

            audioSource.PlayOneShot(accessGrantedSfx);

            yield return new WaitForSeconds(displayResultTime);

            currentStage++;

            ClearInput();

            if (currentStage < codes.Length)
            {
                puzzleManager.ShowPuzzle(currentStage);
                
            }
            else
            {
                onAccessGranted?.Invoke();
            }
            hintSystemManager.UpdatePuzzleIndex();
            displayingResult = false;
        }

        private IEnumerator WrongCodeRoutine()
        {
            displayingResult = true;

            keypadDisplayText.text = accessDeniedText;

            indicators[currentStage].material = redMaterial;

            audioSource.PlayOneShot(accessDeniedSfx);

            yield return new WaitForSeconds(displayResultTime);

            indicators[currentStage].material = offMaterial;

            ClearInput();

            displayingResult = false;
        }

        private void ClearInput()
        {
            currentInput = "";
            keypadDisplayText.text = currentInput;
        }
    }
}