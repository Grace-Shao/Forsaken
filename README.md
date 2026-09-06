# For/Saken

> HUE wants your sick companion, Eva, erased... but why?

In this 2D boss-rush experience, play as ONE, a caretaker robot forced to learn combat to protect the only life it was designed to save. Battle HUE and their relentless mechanized enforcers, unlocking new abilities as you fight toward freedom and safety. But the closer ONE and Eva get to freedom, the more ONE learns about the consequences of her actions.

As the truth behind HUE's mission unfolds, ONE must ultimately decide:

*Is protecting one life worth the collapse of humanity?*

**Play on Itch.io:** [graces203.itch.io/forsaken](https://graces203.itch.io/forsaken)

**Playtime:** ~10-20 minutes.

<img src="ONE__HUE_Cross_Swords_Square_Final%20(1).png" alt="Forsaken gameplay" width="320">

## Background

Forsaken was made in collaboration with VGDEV (Video Game Development Club) at Georgia Tech. @Grace-Shao (Creative Lead, 2nd Technical Lead) and @arahman302 (Main Technical Lead) were the team leads of For/Saken. Around 40 students contributed across programming, art, and music.

No generative AI was used to create the game's art or music. AI was used ONLY for code debugging and troubleshooting (and assisting with this README).

## Technical Overview

Forsaken is a Unity 2D project built with Unity `6000.0.63f1`. The project uses the Input System, Animator & Timeline, Unity's 2D tooling, UI systems, and many more!

### State-driven gameplay

Characters share a small hierarchical state-machine framework built around `StateMachine` and `State`. States own their enter, update, exit, and transition behavior, and can contain substates. This keeps movement, attacks, reactions, and boss phases separate instead of putting every behavior in one update loop.

- `PlayerStateMachine` coordinates movement, jumping, acceleration, coyote time, jump buffering, dashing, melee attacks, ranged attacks, blocking, parrying, health, and energy.
- Player behavior is split into states such as walking, running, jumping, attacking, shooting, dashing, blocking, parrying, and hurt.
- `BossStateMachine` drives HUE's combat logic, including targeting, stun windows, grapples, charged dashes, lasers, melee and ranged attacks, enemy summons, and the ultimate sequence.
- HUE uses stage states (`StageOne`, `StageTwo`, and `StageThree`) with attack substates. The boss chooses behavior from player distance and progresses through stages as the fight continues.

### Combat and encounter systems

Combat is component-based: player and boss weapons handle their own melee and ranged behavior, while shared interfaces such as `IDamageable`, `IInteractable`, and `Weapon` keep interactions decoupled. Parrying can slow enemies, damage responses trigger feedback effects, and energy gates abilities such as the dash and ranged attack.

`GameManager`, `TriggerBattle`, and `MobRushManager` coordinate encounters and transitions. `CutsceneManager` uses Unity Timeline to pause combat, reset character states, and resume fights cleanly around story scenes.

### Other supporting systems (UI / Save System / Audio / VFX)

The repository also includes dedicated systems for dialogue, ability UI, save data, pause/menu flow, camera behavior, cutscene playback, audio control, FMOD content, damage flashes, and 2D visual effects. Scenes, prefabs, scripts, animation data, UI assets, and audio are kept in their corresponding `Assets` folders so gameplay code and authored content remain easy to find.

## Repository Layout

- `Assets/Scripts/` - gameplay code, state machines, managers, interfaces, weapons, UI, and effects
- `Assets/Final Scenes/` - playable game scenes
- `Assets/Prefabs/` - reusable characters, enemies, weapons, and encounter objects
- `Assets/Cutscenes/` and `Assets/*.playable` - Timeline-driven story sequences
- `Assets/audio/` and `Assets/Plugins/` - audio content and integrations
- `ProjectSettings/` and `Packages/` - Unity project configuration and dependencies
- `Builds/` - exported platform builds

## Project Setup

For the full Unity, Git, scene workflow, and collaboration setup, see the [Project Setup wiki](https://github.com/Grace-Shao/Forsaken/wiki/Project-Setup).
TLDR:
1. Install Unity `6000.0.63f1` through Unity Hub.
2. Clone the repository and add the project folder to Unity Hub.
3. Open the project and start from the scenes in `Assets/Final Scenes/`.

## High level Documentation

The project wiki contains high level explanations of the game's architecture & gameplay systems, and Unity Prefab best practices:

- [Home](https://github.com/Grace-Shao/Forsaken/wiki) - Overview of the project's documentation.
- [Boss](https://github.com/Grace-Shao/Forsaken/wiki/Boss) - HUE's staged state machine, attack flow, and boss behavior.
- [Character Prefabs Best Practices](https://github.com/Grace-Shao/Forsaken/wiki/Character-Prefabs-Best-Practices) - Conventions for building and maintaining character prefabs.
- [Interfaces](https://github.com/Grace-Shao/Forsaken/wiki/Interfaces) - Shared interfaces used to connect gameplay systems.
- [Managers](https://github.com/Grace-Shao/Forsaken/wiki/Managers) - Responsibilities and coordination patterns for core managers.
- [Player](https://github.com/Grace-Shao/Forsaken/wiki/Player) - Player architecture, abilities, and state behavior.
- [Player State Flow-chart](https://github.com/Grace-Shao/Forsaken/wiki/Player#player-state-flow-chart) - Visual overview of player state transitions.
