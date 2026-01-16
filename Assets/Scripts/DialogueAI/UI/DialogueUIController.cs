using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace UnityKing.DialogueAI
{
    public class DialogueUIController : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject dialoguePanel;
        public TextMeshProUGUI dialogueText;
        public Transform optionsContainer;
        public Button optionButtonPrefab;

        [Header("Settings")]
        public bool useTypingEffect = true;
        public float typingSpeed = 0.05f;
        public int maxOptionsDisplayed = 5;

        private DialogueController currentDialogueController;
        private List<Button> optionButtons = new List<Button>();
        private Coroutine typingCoroutine;

        private void Start()
        {
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }

            InitializeOptionButtons();
        }

        private void InitializeOptionButtons()
        {
            if (optionButtonPrefab == null || optionsContainer == null) return;

            // Pre-instantiate option buttons
            for (int i = 0; i < maxOptionsDisplayed; i++)
            {
                Button newButton = Instantiate(optionButtonPrefab, optionsContainer);
                newButton.gameObject.SetActive(false);
                optionButtons.Add(newButton);

                // Add click listener with index
                int buttonIndex = i;
                newButton.onClick.AddListener(() => OnOptionSelected(buttonIndex));
            }
        }

        public void ShowDialogue(DialogueController controller)
        {
            currentDialogueController = controller;

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
            }
        }

        public void HideDialogue()
        {
            currentDialogueController = null;

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }

            // Stop any ongoing typing effect
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
        }

        public void UpdateDialogue(string text, List<DialogueOption> options)
        {
            if (dialogueText != null)
            {
                if (useTypingEffect)
                {
                    typingCoroutine = StartCoroutine(TypeText(text));
                }
                else
                {
                    dialogueText.text = text;
                }
            }

            UpdateOptions(options);
        }

        private System.Collections.IEnumerator TypeText(string text)
        {
            dialogueText.text = "";
            foreach (char c in text)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
            typingCoroutine = null;
        }

        private void UpdateOptions(List<DialogueOption> options)
        {
            // Hide all buttons first
            foreach (Button button in optionButtons)
            {
                button.gameObject.SetActive(false);
            }

            // Show and configure buttons for available options
            int buttonCount = Mathf.Min(options.Count, optionButtons.Count);
            for (int i = 0; i < buttonCount; i++)
            {
                Button button = optionButtons[i];
                DialogueOption option = options[i];

                button.gameObject.SetActive(true);

                // Update button text
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = option.text;
                }
                else
                {
                    // Fallback to Unity UI Text
                    Text unityText = button.GetComponentInChildren<Text>();
                    if (unityText != null)
                    {
                        unityText.text = option.text;
                    }
                }
            }
        }

        private void OnOptionSelected(int optionIndex)
        {
            if (currentDialogueController != null)
            {
                currentDialogueController.SelectOption(optionIndex);
            }
        }

        // Alternative UI update methods for different UI systems

        // For Unity UI (non-TMP)
        public void UpdateDialogueUnityUI(string text, List<DialogueOption> options)
        {
            Text unityText = dialogueText?.GetComponent<Text>();
            if (unityText != null)
            {
                unityText.text = text;
            }

            UpdateOptions(options);
        }

        // For custom UI systems
        public void UpdateDialogueCustom(string text, List<DialogueOption> options,
                                       System.Action<int> onOptionSelected = null)
        {
            if (dialogueText != null)
            {
                dialogueText.text = text;
            }

            // Custom option handling
            if (onOptionSelected != null)
            {
                for (int i = 0; i < Mathf.Min(options.Count, optionButtons.Count); i++)
                {
                    int index = i; // Capture for lambda
                    optionButtons[i].onClick.RemoveAllListeners();
                    optionButtons[i].onClick.AddListener(() => onOptionSelected(index));
                }
            }

            UpdateOptions(options);
        }

        // Skip typing effect
        public void SkipTyping()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
        }

        // Get current dialogue state
        public bool IsDialogueVisible => dialoguePanel != null && dialoguePanel.activeSelf;
        public bool IsTyping => typingCoroutine != null;

        // UI customization methods
        public void SetTypingSpeed(float speed)
        {
            typingSpeed = Mathf.Max(0.01f, speed);
        }

        public void EnableTypingEffect(bool enabled)
        {
            useTypingEffect = enabled;
            if (!enabled && typingCoroutine != null)
            {
                SkipTyping();
            }
        }

        // Dynamic UI scaling based on content
        public void AdjustUISizeForContent(string text, int optionCount)
        {
            if (dialoguePanel == null) return;

            // Example: Adjust panel height based on text length and options
            RectTransform panelRect = dialoguePanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                float baseHeight = 200f;
                float textHeight = Mathf.Ceil(text.Length / 50f) * 30f; // Rough estimate
                float optionsHeight = optionCount * 40f;
                panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x,
                                                baseHeight + textHeight + optionsHeight);
            }
        }

        // Accessibility features
        public void EnableHighContrastMode(bool enabled)
        {
            // Could modify colors, fonts, etc. for accessibility
            Color textColor = enabled ? Color.white : Color.black;
            if (dialogueText != null)
            {
                dialogueText.color = textColor;
            }

            foreach (Button button in optionButtons)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.color = textColor;
                }
            }
        }

        // Localization support
        public void SetLocalizedText(string localizedText, List<string> localizedOptions)
        {
            if (dialogueText != null)
            {
                dialogueText.text = localizedText;
            }

            // Update option buttons with localized text
            for (int i = 0; i < Mathf.Min(localizedOptions.Count, optionButtons.Count); i++)
            {
                TextMeshProUGUI buttonText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = localizedOptions[i];
                }
            }
        }
    }
}
