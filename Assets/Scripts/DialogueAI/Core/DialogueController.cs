using UnityEngine;
using System.Collections.Generic;

namespace UnityKing.DialogueAI
{
    public class DialogueController : MonoBehaviour
    {
        [Header("Dialogue Setup")]
        public DialogueGraph dialogueGraph;
        public DialogueUIController uiController;
        public DialogueMemory memory;

        [Header("NPC Info")]
        public string npcName = "NPC";

        private DialogueContext currentContext;
        private bool isDialogueActive = false;

        public void StartDialogue(GameObject player)
        {
            if (isDialogueActive || dialogueGraph == null) return;

            isDialogueActive = true;
            currentContext = new DialogueContext
            {
                currentSpeaker = npcName,
                currentNodeId = "start" // Assuming start node exists
            };

            if (uiController != null)
            {
                uiController.ShowDialogue(this);
            }

            NavigateToNode(currentContext.currentNodeId);
        }

        public void EndDialogue()
        {
            if (!isDialogueActive) return;

            isDialogueActive = false;
            currentContext = null;

            if (uiController != null)
            {
                uiController.HideDialogue();
            }
        }

        public void SelectOption(int optionIndex)
        {
            if (!isDialogueActive || currentContext == null) return;

            DialogueNode currentNode = GetCurrentNode();
            if (currentNode == null || optionIndex < 0 || optionIndex >= currentNode.options.Count) return;

            DialogueOption selectedOption = currentNode.options[optionIndex];

            // Process intent
            if (selectedOption.intent != DialogueIntent.None)
            {
                ProcessIntent(selectedOption.intent);
            }

            // Navigate to next node
            if (!string.IsNullOrEmpty(selectedOption.nextNodeId))
            {
                NavigateToNode(selectedOption.nextNodeId);
            }
            else
            {
                EndDialogue();
            }
        }

        private void NavigateToNode(string nodeId)
        {
            if (dialogueGraph == null || !dialogueGraph.nodes.ContainsKey(nodeId)) return;

            currentContext.currentNodeId = nodeId;
            DialogueNode node = dialogueGraph.nodes[nodeId];

            if (uiController != null)
            {
                uiController.UpdateDialogue(node.text, node.options);
            }
        }

        private DialogueNode GetCurrentNode()
        {
            if (currentContext == null || dialogueGraph == null) return null;
            return dialogueGraph.nodes.ContainsKey(currentContext.currentNodeId)
                ? dialogueGraph.nodes[currentContext.currentNodeId]
                : null;
        }

        private void ProcessIntent(DialogueIntent intent)
        {
            // Handle different intents
            switch (intent)
            {
                case DialogueIntent.Goodbye:
                    EndDialogue();
                    break;
                case DialogueIntent.AcceptQuest:
                    // Trigger quest acceptance
                    QuestTrigger.TriggerQuest("AcceptedQuest");
                    break;
                case DialogueIntent.RejectQuest:
                    QuestTrigger.TriggerQuest("RejectedQuest");
                    break;
                    // Add more intent processing as needed
            }

            // Update memory based on intent
            if (memory != null)
            {
                memory.RecordIntent(intent);
            }
        }

        public bool IsDialogueActive => isDialogueActive;
        public DialogueContext CurrentContext => currentContext;
    }
}
