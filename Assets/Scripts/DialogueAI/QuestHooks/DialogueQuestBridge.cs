using UnityEngine;
using System.Collections.Generic;

namespace UnityKing.DialogueAI
{
    public class DialogueQuestBridge : MonoBehaviour
    {
        [Header("Quest System Integration")]
        public DialogueController dialogueController;
        public DialogueMemory dialogueMemory;

        [Header("Quest Manager Reference")]
        // This would typically reference your game's Quest Manager
        // public QuestManager questManager;

        private Dictionary<string, System.Action> questEventHandlers;

        private void Awake()
        {
            InitializeQuestHandlers();
            SubscribeToQuestEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromQuestEvents();
        }

        private void InitializeQuestHandlers()
        {
            questEventHandlers = new Dictionary<string, System.Action>()
            {
                {"AcceptedQuest", HandleQuestAccepted},
                {"RejectedQuest", HandleQuestRejected},
                {"CompletedQuest", HandleQuestCompleted},
                {"FailedQuest", HandleQuestFailed},
                {"ThreatenedQuest", HandleQuestThreatened}
            };
        }

        private void SubscribeToQuestEvents()
        {
            QuestTrigger.OnQuestTriggered += HandleQuestTriggered;
        }

        private void UnsubscribeFromQuestEvents()
        {
            QuestTrigger.OnQuestTriggered -= HandleQuestTriggered;
        }

        private void HandleQuestTriggered(string questId, QuestTrigger.QuestEventType eventType)
        {
            Debug.Log($"DialogueQuestBridge: Quest {questId} triggered with event {eventType}");

            // Update dialogue memory based on quest events
            if (dialogueMemory != null)
            {
                switch (eventType)
                {
                    case QuestTrigger.QuestEventType.StartQuest:
                        dialogueMemory.SetFlag($"Quest_{questId}_Started", true);
                        break;
                    case QuestTrigger.QuestEventType.CompleteQuest:
                        dialogueMemory.SetFlag($"Quest_{questId}_Completed", true);
                        break;
                    case QuestTrigger.QuestEventType.FailQuest:
                        dialogueMemory.SetFlag($"Quest_{questId}_Failed", true);
                        break;
                }
            }

            // Call specific handlers
            if (questEventHandlers.ContainsKey(questId))
            {
                questEventHandlers[questId]?.Invoke();
            }

            // Update dialogue context if active
            if (dialogueController != null && dialogueController.IsDialogueActive)
            {
                UpdateDialogueBasedOnQuest(questId, eventType);
            }
        }

        // Specific quest event handlers
        private void HandleQuestAccepted()
        {
            if (dialogueMemory != null)
            {
                dialogueMemory.SetFlag("HasAcceptedQuest", true);
                dialogueMemory.ModifyRelationship(dialogueController?.npcName ?? "NPC", 5);
            }
            Debug.Log("Quest accepted - updating dialogue state");
        }

        private void HandleQuestRejected()
        {
            if (dialogueMemory != null)
            {
                dialogueMemory.SetFlag("HasRejectedQuest", true);
                dialogueMemory.ModifyRelationship(dialogueController?.npcName ?? "NPC", -2);
            }
            Debug.Log("Quest rejected - updating dialogue state");
        }

        private void HandleQuestCompleted()
        {
            if (dialogueMemory != null)
            {
                dialogueMemory.SetFlag("HasCompletedQuest", true);
                dialogueMemory.ModifyRelationship(dialogueController?.npcName ?? "NPC", 10);
            }
            Debug.Log("Quest completed - updating dialogue state");
        }

        private void HandleQuestFailed()
        {
            if (dialogueMemory != null)
            {
                dialogueMemory.SetFlag("HasFailedQuest", true);
                dialogueMemory.ModifyRelationship(dialogueController?.npcName ?? "NPC", -5);
            }
            Debug.Log("Quest failed - updating dialogue state");
        }

        private void HandleQuestThreatened()
        {
            if (dialogueMemory != null)
            {
                dialogueMemory.SetFlag("HasThreatenedNPC", true);
                dialogueMemory.ModifyRelationship(dialogueController?.npcName ?? "NPC", -10);
            }
            Debug.Log("NPC threatened - severely damaging relationship");
        }

        // Update dialogue flow based on quest state
        private void UpdateDialogueBasedOnQuest(string questId, QuestTrigger.QuestEventType eventType)
        {
            // This could modify the dialogue graph dynamically
            // For example, unlock new dialogue options or change NPC responses

            if (dialogueController?.dialogueGraph == null) return;

            // Example: If quest completed, NPC becomes more friendly
            if (eventType == QuestTrigger.QuestEventType.CompleteQuest)
            {
                // Could modify dialogueGraph.nodes to add new friendly responses
                Debug.Log($"Updating dialogue for completed quest: {questId}");
            }
        }

        // Public methods for external quest system integration
        public void NotifyQuestStarted(string questId)
        {
            QuestTrigger.TriggerQuest(questId, QuestTrigger.QuestEventType.StartQuest);
        }

        public void NotifyQuestCompleted(string questId)
        {
            QuestTrigger.TriggerQuest(questId, QuestTrigger.QuestEventType.CompleteQuest);
        }

        public void NotifyQuestFailed(string questId)
        {
            QuestTrigger.TriggerQuest(questId, QuestTrigger.QuestEventType.FailQuest);
        }

        // Query quest state from dialogue system
        public bool HasQuestBeenAccepted(string questId = null)
        {
            if (dialogueMemory == null) return false;

            if (string.IsNullOrEmpty(questId))
            {
                return dialogueMemory.GetFlag("HasAcceptedQuest");
            }

            return dialogueMemory.GetFlag($"Quest_{questId}_Started");
        }

        public bool HasQuestBeenCompleted(string questId = null)
        {
            if (dialogueMemory == null) return false;

            if (string.IsNullOrEmpty(questId))
            {
                return dialogueMemory.GetFlag("HasCompletedQuest");
            }

            return dialogueMemory.GetFlag($"Quest_{questId}_Completed");
        }

        public bool HasQuestBeenFailed(string questId = null)
        {
            if (dialogueMemory == null) return false;

            if (string.IsNullOrEmpty(questId))
            {
                return dialogueMemory.GetFlag("HasFailedQuest");
            }

            return dialogueMemory.GetFlag($"Quest_{questId}_Failed");
        }

        // Get relationship with current NPC
        public int GetCurrentRelationship()
        {
            if (dialogueMemory == null || dialogueController == null) return 0;
            return dialogueMemory.GetRelationship(dialogueController.npcName);
        }

        // Dynamic dialogue modification based on quest state
        public void ModifyDialogueForQuestState(DialogueGraph graph)
        {
            if (graph == null || dialogueMemory == null) return;

            // Example modifications based on memory flags
            foreach (var node in graph.nodes)
            {
                // Add special options if player has completed quests
                if (dialogueMemory.GetFlag("HasCompletedQuest"))
                {
                    // Could add "Thank you for helping before" option
                }

                // Change responses based on relationship
                int relationship = GetCurrentRelationship();
                if (relationship < -5)
                {
                    // Hostile responses
                }
                else if (relationship > 5)
                {
                    // Friendly responses
                }
            }
        }
    }
}
