# Mental Health Narrative (Title TBD)

This README describes the technical details behind the game.

## Player Input

We use the new Unity **Input System** to handle player input.

* **Manual:** https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/index.html
* **Workflow Overview:** https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/Workflow-Actions.html

View or edit the project-wide actions: `Edit > Project Settings > Input System Package`

### Gotchas

There's a bug in version 1.7.0 of the Input System that breaks the action map GUI
(see https://forum.unity.com/threads/unable-to-use-input-system-panel.1450204/ for context).

The Unity Package Manager seems to only show version 1.7.0.
Go to https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/changelog/CHANGELOG.html#182---2024-04-29
and click the "Add version 1.8.2 by name" link to download the latest version.

## Player

We have a **Player** prefab that represents the player character.
