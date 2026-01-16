using System.Collections.Generic;
using UnityEngine;

namespace UnityKing.DialogueAI
{
    public class DialogueMemory : MonoBehaviour
    {
        private Dictionary<string, bool> flags = new Dictionary<string, bool>();
        private Dictionary<string, int> relationships = new Dictionary<string, int>();
        private List<DialogueIntent> intentHistory = new List<DialogueIntent>();

        // Flag management
        public void SetFlag(string flagName, bool value)
        {
            flags[flagName] = value;
        }

        public bool GetFlag(string flagName)
        {
            return flags.ContainsKey(flagName) ? flags[flagName] : false;
        }

        public bool HasFlag(string flagName)
        {
            return flags.ContainsKey(flagName);
        }

        // Relationship management
        public void SetRelationship(string npcName, int value)
        {
            relationships[npcName] = value;
        }

        public int GetRelationship(string npcName)
        {
            return relationships.ContainsKey(npcName) ? relationships[npcName] : 0;
        }

        public void ModifyRelationship(string npcName, int change)
        {
            int current = GetRelationship(npcName);
            SetRelationship(npcName, current + change);
        }

        // Intent history
        public void RecordIntent(DialogueIntent intent)
        {
            intentHistory.Add(intent);
        }

        public List<DialogueIntent> GetIntentHistory()
        {
            return new List<DialogueIntent>(intentHistory);
        }

        public bool HasIntentBeenUsed(DialogueIntent intent)
        {
            return intentHistory.Contains(intent);
        }

        public int GetIntentCount(DialogueIntent intent)
        {
            int count = 0;
            foreach (DialogueIntent recordedIntent in intentHistory)
            {
                if (recordedIntent == intent) count++;
            }
            return count;
        }

        // Memory persistence (basic implementation - could be extended with save/load)
        public void ClearMemory()
        {
            flags.Clear();
            relationships.Clear();
            intentHistory.Clear();
        }

        public void LoadMemory(Dictionary<string, bool> loadedFlags,
                              Dictionary<string, int> loadedRelationships,
                              List<DialogueIntent> loadedHistory)
        {
            flags = new Dictionary<string, bool>(loadedFlags);
            relationships = new Dictionary<string, int>(loadedRelationships);
            intentHistory = new List<DialogueIntent>(loadedHistory);
        }

        public (Dictionary<string, bool>, Dictionary<string, int>, List<DialogueIntent>) GetMemoryData()
        {
            return (new Dictionary<string, bool>(flags),
                   new Dictionary<string, int>(relationships),
                   new List<DialogueIntent>(intentHistory));
        }
    }
}
