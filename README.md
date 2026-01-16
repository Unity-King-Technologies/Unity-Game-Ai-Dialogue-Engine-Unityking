# Unity Game AI Dialogue Engine  
### Dynamic NPC Dialogue • Context Memory • Intent System • Quest Hooks

Created and maintained by **Unity King**  
https://unityking.com

---

## Quick Start (Important – Read First)

This repository contains **only the `Assets/` folder** with a complete **AI-powered Dialogue Engine**.

### How to Use This Repository

1. Create or open a Unity project  
   - Unity 2021 LTS or newer recommended

2. Copy the `Assets` folder from this repository  
   Paste it into your Unity project root

3. Open Unity  
   - Scripts will auto-import

4. Add Dialogue components to your NPCs  
   - Setup explained below

You are now ready to use the Dialogue Engine.

---

## 📁 Project Structure

```

Assets/
├── Scripts/
│    ├── DialogueAI/
│    │    ├── Core/
│    │    │    ├── DialogueController.cs
│    │    │    ├── DialogueContext.cs
│    │    │    └── DialogueMemory.cs
│    │    ├── IntentSystem/
│    │    │    ├── DialogueIntent.cs
│    │    │    ├── IntentClassifier.cs
│    │    └── IntentResponse.cs
│    │    ├── Nodes/
│    │    │    ├── DialogueNode.cs
│    │    │    ├── DialogueOption.cs
│    │    │    └── DialogueGraph.cs
│    │    ├── QuestHooks/
│    │    │    ├── QuestTrigger.cs
│    │    │    └── DialogueQuestBridge.cs
│    │    └── UI/
│    │         └── DialogueUIController.cs

````

---

## System Overview

The Dialogue Engine is built on **four core systems**:

### Dialogue Controller  
Controls dialogue flow and NPC interaction lifecycle.

### Context & Memory System  
Remembers:
- Player choices
- NPC relationships
- Past dialogue events
- Quest states

### Intent System  
Understands **what the player means**, not just what they click:
- Greeting
- Asking for help
- Threatening
- Accepting / rejecting quests

### Quest Hooks  
Dialogue directly affects:
- Quest start
- Quest progression
- Quest completion

---

## Core Components Explained

### DialogueController
Main entry point for NPC dialogue.

Responsibilities:
- Start / end dialogue
- Route dialogue nodes
- Communicate with UI
- Update memory & quests

```csharp
DialogueController controller;
controller.StartDialogue(player);
````

Attach this to **NPC GameObjects**.

---

### DialogueContext

Stores **runtime context**:

* Current NPC
* Current node
* Player state
* World flags

```csharp
context.currentNode
context.currentSpeaker
```

---

### DialogueMemory

Persistent memory system.

Stores:

* Player choices
* Flags (helpedNPC, betrayedNPC, etc.)
* Relationship values

```csharp
memory.SetFlag("HelpedVillager", true);
```

---

## Intent System

The Intent System allows **natural-feeling dialogue**.

### DialogueIntent

Represents an intent like:

* Greet
* AskForQuest
* Threaten
* Goodbye

### IntentClassifier

Maps player input or options to intents.

```csharp
IntentType result = classifier.Classify(input);
```

This allows:

* Dynamic responses
* AI-driven dialogue logic
* LLM integration later

---

## Dialogue Nodes & Graph

### DialogueNode

Represents a single dialogue state.

```csharp
DialogueNode
{
    text: "Hello traveler",
    options: [...]
}
```

### DialogueOption

Player choices that:

* Change context
* Trigger intents
* Fire quests

```csharp
option.nextNodeId
option.intent
```

---

## Quest Integration

### QuestTrigger

Hooks dialogue to quest logic.

Examples:

* Start quest on dialogue
* Complete quest on choice
* Fail quest on threat

```csharp
QuestTrigger.TriggerQuest("FindTheSword");
```

### DialogueQuestBridge

Connects Dialogue Engine with your Quest System.

---

## Dialogue UI

### DialogueUIController

Handles:

* Showing dialogue text
* Rendering options
* Player input

Framework is **UI-agnostic**:

* Unity UI
* TMP
* Custom UI systems

---

## Example NPC Setup

1. Create NPC GameObject
2. Add:

   * DialogueController
   * DialogueUIController
3. Assign:

   * DialogueGraph
   * Memory reference
4. Press ▶ Play

NPC will:

* Remember player
* React to choices
* Trigger quests

---

## Extending the System

You can easily add:

* Voice acting
* Relationship systems
* LLM / AI text generation
* Morality systems
* Branching storylines

---

## Use Cases

* RPGs
* Adventure games
* Story-driven games
* AI NPC experiments
* LLM-powered dialogue

---

## License

MIT License
Free for personal & commercial use.

---

## Author

**Unity King**
 [https://unityking.com](https://unityking.com)
Game AI • Systems • Tools

---

⭐ Star the repository if this helps your project.
