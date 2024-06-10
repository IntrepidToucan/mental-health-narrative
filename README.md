# Mental Health Narrative (Title TBD)

This README describes the technical details behind the game.

## Player

We have a **`Player`** prefab that represents the player character.

### Player Input

We use the new Unity **Input System** (version **1.8.2**) to handle player input.

* **Manual:** https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/index.html
* **Workflow Overview:** https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/Workflow-Actions.html

View or edit the project-wide actions: `Edit > Project Settings > Input System Package`

The logic for handling player input is in the **`PlayerInputHandler`** script on the `Player` prefab.

#### Troubleshooting

##### Cannot delete actions in Input System actions editor

If you right-click an action in the Input System actions editor and no context menu appears (i.e., you can't delete actions),
you can edit the project-wide actions asset directly in code by editing the `InputSystem_Actions.inputactions` file (inside `/Assets/Settings`).

### Player Movement

We implement player movement (walking/running, jumping, etc.) using raycasting and kinematics.
Using kinematic equations gives us more precise control over character movement than rigid body physics does
(for more context, see https://www.reddit.com/r/Unity2D/comments/4cfszl/comment/d1hs389/).

The raycasting and collision detection logic is in the **`PlayerMovementController`** script on the `Player` prefab.
To learn more about our implementation, see https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz
and https://youtu.be/hG9SzQxaCm8?feature=shared.
