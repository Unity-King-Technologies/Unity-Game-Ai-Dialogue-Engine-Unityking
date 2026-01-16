using System.Collections.Generic;
using UnityEngine;

namespace UnityKing.DialogueAI
{
    public class IntentClassifier : MonoBehaviour
    {
        // Simple keyword-based classification (can be extended with ML/AI)
        private Dictionary<string, DialogueIntent> keywordMappings = new Dictionary<string, DialogueIntent>()
        {
            // Greeting keywords
            {"hello", DialogueIntent.Greet},
            {"hi", DialogueIntent.Greet},
            {"hey", DialogueIntent.Greet},
            {"greetings", DialogueIntent.Greet},

            // Quest keywords
            {"quest", DialogueIntent.AskForQuest},
            {"mission", DialogueIntent.AskForQuest},
            {"task", DialogueIntent.AskForQuest},
            {"job", DialogueIntent.AskForQuest},
            {"help", DialogueIntent.AskForQuest},

            // Acceptance keywords
            {"yes", DialogueIntent.AcceptQuest},
            {"accept", DialogueIntent.AcceptQuest},
            {"sure", DialogueIntent.AcceptQuest},
            {"okay", DialogueIntent.AcceptQuest},
            {"alright", DialogueIntent.AcceptQuest},

            // Rejection keywords
            {"no", DialogueIntent.RejectQuest},
            {"decline", DialogueIntent.RejectQuest},
            {"refuse", DialogueIntent.RejectQuest},
            {"pass", DialogueIntent.RejectQuest},

            // Threat keywords
            {"threat", DialogueIntent.Threaten},
            {"kill", DialogueIntent.Threaten},
            {"hurt", DialogueIntent.Threaten},
            {"attack", DialogueIntent.Threaten},
            {"fight", DialogueIntent.Threaten},

            // Goodbye keywords
            {"bye", DialogueIntent.Goodbye},
            {"goodbye", DialogueIntent.Goodbye},
            {"farewell", DialogueIntent.Goodbye},
            {"see you", DialogueIntent.Goodbye},
            {"later", DialogueIntent.Goodbye}
        };

        public DialogueIntent Classify(string input)
        {
            if (string.IsNullOrEmpty(input)) return DialogueIntent.None;

            string lowerInput = input.ToLower().Trim();

            // Direct keyword matching
            foreach (var mapping in keywordMappings)
            {
                if (lowerInput.Contains(mapping.Key))
                {
                    return mapping.Value;
                }
            }

            // Contextual classification could be added here
            // For example, checking dialogue context, player state, etc.

            return DialogueIntent.None;
        }

        public DialogueIntent ClassifyFromOption(DialogueOption option)
        {
            // If option already has an intent assigned, use it
            if (option.intent != DialogueIntent.None)
            {
                return option.intent;
            }

            // Otherwise, classify based on option text
            return Classify(option.text);
        }

        // Advanced classification methods (for future expansion)
        public List<DialogueIntent> GetPossibleIntents(string input)
        {
            List<DialogueIntent> possibleIntents = new List<DialogueIntent>();
            string lowerInput = input.ToLower().Trim();

            foreach (var mapping in keywordMappings)
            {
                if (lowerInput.Contains(mapping.Key))
                {
                    if (!possibleIntents.Contains(mapping.Value))
                    {
                        possibleIntents.Add(mapping.Value);
                    }
                }
            }

            return possibleIntents;
        }

        // Add custom keyword mappings at runtime
        public void AddKeywordMapping(string keyword, DialogueIntent intent)
        {
            keywordMappings[keyword.ToLower()] = intent;
        }

        public void RemoveKeywordMapping(string keyword)
        {
            keywordMappings.Remove(keyword.ToLower());
        }

        // Confidence scoring (for more advanced implementations)
        public float GetConfidence(DialogueIntent intent, string input)
        {
            // Simple implementation - count of matching keywords
            string lowerInput = input.ToLower().Trim();
            int matchCount = 0;

            foreach (var mapping in keywordMappings)
            {
                if (mapping.Value == intent && lowerInput.Contains(mapping.Key))
                {
                    matchCount++;
                }
            }

            // Return confidence as ratio of matched keywords to total words
            string[] words = input.Split(' ');
            return words.Length > 0 ? (float)matchCount / words.Length : 0f;
        }
    }
}
