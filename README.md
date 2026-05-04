# Zalabya — A Breath of the Wild Tribute, Built in Unity

A 3D action-adventure prototype inspired by *The Legend of Zelda: Breath of the Wild 2*, built from the ground up in Unity. You play as Link, scaling cliffs, gliding across the overworld, wielding a sword and bow, and unleashing the Sheikah Slate's runes against Bokoblins, Moblins, and two custom-scripted bosses.

## Highlights

- **Faithful BotW movement loop** — sprint, jump, climb up vertical surfaces (hold `Shift`), and glide back down to chain traversal across two open-world levels.
- **Sheikah Slate runes** — switch between Remote Bombs, Magnesis (lift and fling metal objects with physics), and Stasis (freeze a target in time, then transfer absorbed force on release).
- **Combat with weight** — sword combos, bow with arrow physics and particle trails, fall damage, a 12-heart health system, and an enemy AI layer driven by Unity's NavMesh.
- **Two bespoke boss fights** — fully scripted, multi-phase encounters with health bars, charge attacks, fireballs, bubble shields, and phase-2 escalations.
- **Polished menu flow** — main menu, options panel with separated music / SFX volume sliders backed by an `AudioMixer`, pause menu, and a game-over screen wired across every scene.
- **Universal Render Pipeline** with cel-shaded materials, custom skyboxes, and a temple-on-water final scene that leans hard into the BotW visual language.

## What It Does

Boot the game, pick a level from the main menu, and you drop into a Hyrule-flavoured overworld as Link, kitted with all four core abilities from day one. Explore, climb, glide, fight, and survive long enough to reach the boss arena at the end of each level. Two playable levels (`Level 1`, `Level 2`) lead into two distinct boss encounters (`Boss 1`, `Boss 2`), each with their own scene, model, animation set, and AI script. Audio settings persist via `PlayerPrefs`, and the pause menu can be triggered mid-fight without breaking the game state.

## Tech Stack

| Layer | Choice |
|---|---|
| Engine | **Unity 2021.3.10f1 LTS** |
| Render pipeline | **Universal Render Pipeline (URP) 12.1.7** |
| Language | C# (.NET / Mono runtime) |
| Camera | **Cinemachine 2.8.9** (virtual cameras, stasis zoom-in) |
| Input | **Unity Input System 1.4.4** |
| AI | Unity NavMesh (`NavMeshAgent`) for enemy pathfinding |
| Tweening | DOTween (DG.Tweening) for stasis / camera effects |
| UI | Unity UI + TextMesh Pro 3.0.6 |
| Animation | Mecanim Animator + Timeline 1.6.4 |
| Audio | Unity AudioMixer (separate Music / SoundEffects buses) |

## Project Structure

```
Zalabya/
├── Assets/
│   ├── Scripts/                    # Core gameplay scripts
│   │   ├── Main.cs                 # Menu flow, scene loading, volume persistence
│   │   ├── Abilities.cs            # Sheikah Slate rune switcher (Bomb / Magnesis / Stasis)
│   │   ├── PlayerClimbing.cs       # Wall climbing state machine
│   │   ├── PlayerController.cs     # Player entry point
│   │   ├── Bomb.cs                 # Remote Bomb explosion + AoE damage
│   │   └── Arrow.cs                # Bow arrow physics + on-hit damage routing
│   ├── Player/States/              # Stasis + StasisAttack rune behaviours
│   ├── Models/
│   │   ├── boss 1/                 # Phase-based boss script with charge attacks
│   │   ├── boos 2/                 # Second boss with fireball patterns + portals
│   │   ├── Enemies/                # Bokoblin / Moblin NavMesh AI
│   │   └── Bomb/, glider/, Erika/  # Player gear + character models
│   ├── Scenes/
│   │   ├── Main Menu.unity
│   │   ├── Level 1.unity
│   │   ├── Level 2.unity
│   │   ├── Boss 1.unity
│   │   ├── Boss 2.unity
│   │   └── Abilities.unity
│   ├── Sounds/                     # AudioMixer + VolumeSettings.cs
│   └── (third-party packs)         # Cel shading, Hyrule fonts, environment props
├── Packages/                       # Unity package manifest
└── ProjectSettings/
```

## Getting Started

**Requirements**
- [Unity Hub](https://unity.com/download)
- **Unity 2021.3.10f1** (LTS) — install via Unity Hub → Installs → Add → 2021.3.10f1
- Any recent Visual Studio / Rider / VS Code with C# support
- Windows / macOS / Linux Editor (project is configured for the Standalone target)

**Run the project**
1. Clone the repo:
   ```bash
   git clone https://github.com/anaselnemr/Zalabya.git
   ```
2. Open Unity Hub → **Add project** → select the cloned `Zalabya/` folder.
3. Open it with Unity **2021.3.10f1**. The first import will take several minutes (large asset packs and shaders need to compile).
4. In the Project window, open `Assets/Scenes/Main Menu.unity`.
5. Press **Play**, hit **Play** in the menu, and pick **Level 1** or **Level 2**.

**Controls**
| Action | Key |
|---|---|
| Move | `WASD` |
| Jump | `Space` |
| Sprint | `Left Shift` |
| Climb (when next to a wall) | hold `Left Shift` |
| Switch rune | `1` Bomb · `3` Magnesis · `4` Stasis |
| Use rune | `Mouse` |
| Pause | `Esc` |

## Course Context

Built as a B.Sc. Computer Science & Engineering coursework project at the **German University in Cairo (GUC)** during the 2022–2023 academic year. Targeted as a capstone-style team submission exploring 3D gameplay programming, animation state machines, AI pathfinding, physics-driven abilities, and end-to-end Unity production — from menu flow to boss design.

## Authors

Anas ElNemr  ·  Ahmed Eltawel
