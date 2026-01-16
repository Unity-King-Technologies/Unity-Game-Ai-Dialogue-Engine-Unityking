using UnityEngine;
using System.Collections.Generic;

namespace UnityKing.DialogueAI
{
    public class QuestTrigger : MonoBehaviour
    {
        [System.Serializable]
        public class QuestEvent
        {
            public string questId;
            public QuestEventType eventType;
            public string description;
        }

        public enum QuestEventType
        {
            StartQuest,
            CompleteQuest,
            FailQuest,
            UpdateQuest,
            CustomEvent
        }

        // Static event system for global quest triggering
        public static event System.Action<string, QuestEventType> OnQuestTriggered;

        [Header("Quest Triggers")]
        public List<QuestEvent> questEvents;

        // Static method to trigger quests from anywhere
        public static void TriggerQuest(string questId)
        {
            TriggerQuest(questId, QuestEventType.StartQuest);
        }

        public static void TriggerQuest(string questId, QuestEventType eventType)
        {
            Debug.Log($"Quest Triggered: {questId} - {eventType}");

            // Invoke static event
            OnQuestTriggered?.Invoke(questId, eventType);

            // Could integrate with Unity Events, Quest Manager, etc.
            // For example:
            // QuestManager.Instance.HandleQuestEvent(questId, eventType);
        }

        // Instance methods for component-based triggering
        public void TriggerQuestEvent(string questId)
        {
            TriggerQuest(questId);
        }

        public void TriggerQuestEvent(string questId, QuestEventType eventType)
        {
            TriggerQuest(questId, eventType);
        }

        // Trigger all configured events
        public void TriggerAllEvents()
        {
            foreach (QuestEvent questEvent in questEvents)
            {
                TriggerQuest(questEvent.questId, questEvent.eventType);
            }
        }

        // Trigger specific event by index
        public void TriggerEventByIndex(int index)
        {
            if (index >= 0 && index < questEvents.Count)
            {
                QuestEvent questEvent = questEvents[index];
                TriggerQuest(questEvent.questId, questEvent.eventType);
            }
        }

        // Conditional triggering based on dialogue context
        public void TriggerQuestIfCondition(string questId, QuestEventType eventType,
                                          DialogueController dialogueController)
        {
            if (dialogueController == null) return;

            // Example conditions - can be extended
            bool shouldTrigger = true;

            // Check memory flags
            if (dialogueController.memory != null)
            {
                // Example: only trigger if player has helped before
                if (eventType == QuestEventType.CompleteQuest)
                {
                    shouldTrigger = dialogueController.memory.GetFlag("HasHelpedNPC");
                }
            }

            if (shouldTrigger)
            {
                TriggerQuest(questId, eventType);
            }
        }

        // Unity Event integration
        public void OnDialogueIntent(DialogueIntent intent)
        {
            // Map intents to quest events
            switch (intent)
            {
                case DialogueIntent.AcceptQuest:
                    TriggerQuest("AcceptedQuest", QuestEventType.StartQuest);
                    break;
                case DialogueIntent.RejectQuest:
                    TriggerQuest("RejectedQuest", QuestEventType.FailQuest);
                    break;
                case DialogueIntent.Threaten:
                    TriggerQuest("ThreatenedQuest", QuestEventType.FailQuest);
                    break;
                    // Add more mappings as needed
            }
        }

        // Editor helper methods
        public void AddQuestEvent(string questId, QuestEventType eventType, string description = "")
        {
            questEvents.Add(new QuestEvent
            {
                questId = questId,
                eventType = eventType,
                description = description
            });
        }

        public void RemoveQuestEvent(int index)
        {
            if (index >= 0 && index < questEvents.Count)
            {
                questEvents.RemoveAt(index);
            }
        }

        public void ClearAllEvents()
        {
            questEvents.Clear();
        }
    }
}
