# Ghost Plugin
A comprehensive Exiled plugin for SCP: Secret Laboratory, featuring custom roles, unique weapons, interactive items, multimedia enhancements, and integrations with other community plugins.
![img.png](img.png)
![img_1.png](img_1.png)
![img_2.png](img_2.png)
![img_3.png](img_3.png)
![img_4.png](img_4.png)
![img_5.png](img_5.png)

<!--## Features
- Integrated custom modules and self-developed methods to extend game logic.
- Injected `Project MER` (DLL-based modules) to manipulate server–client logic at runtime.
    - Utilized the `CreateSchematic()` method to dynamically spawn in-game objects (e.g., shields) in real time at the player’s aiming position.
- Used `HarmonyLib` with `IL injection` to modify compiled bytecode for deeper control.
- Dynamic Music Playback – Supports background music, situational tracks, and jukebox-style area music.
- YouTube Integration – Play audio directly from YouTube URLs (powered by AudioPlayerAPI and youtube-dl/yt-dlp)-->
## Features
- **Custom Items & Weapons** – Special firearms, unique perks, and custom items inspired by Call of Duty.
- **Custom Roles** – Adds new playable roles to diversify SCP:SL gameplay.
- **Special Ammunition System** – Primitive raycast bullets and alternative ammo types.
- **Dynamic Music Playback** – Background music, situational tracks, and jukebox system.
- **YouTube Integration** – Play music directly from YouTube URLs (via AudioPlayerAPI + youtube-dl/yt-dlp).
- **Classic Subtitles System** – Adds a retro-style subtitle overlay for immersion.
- **Blackout Mode** – Enable or disable map-wide power outages dynamically.
- **Plugin Interoperability** – Works seamlessly with other community plugins (UIU, Serpent’s Hand, etc.).

### Implementation Notes
> Music playback is implemented via [AudioPlayerAPI](https://github.com/Killers0992/AudioPlayerApi)
and `youtube-dl`/`yt-dlp` for YouTube URL support.

## Installation
### 🚫 Confidential Notice
```[!IMPORTANT]
This plugin is proprietary and strictly confidential.
Installation, redistribution, or public usage is prohibited.
Authorized use only within designated environments.
```
You should be install or add dependencies this
- [ProjectMER](https://github.com/Michal78900/ProjectMER/releases/latest)
- [AudioPlayerAPI](https://github.com/Killers0992/AudioPlayerApi)
- [Team Death Match](https://github.com/Hanbin-GW/Team-Deathmatch/releases/latest)
- ReinforceMents (My customized UIU)
- C-Squad (My customized Chaos Version UIU`)
- Invader (My Custom Serpents hand)

#### Also You need to install `yt-dlp`
ubuntu
```bash
sudo wget https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp -O /usr/local/bin/yt-dlp
sudo chmod a+rx /usr/local/bin/yt-dlp
```
windows
- Download in this link [Least Version of Yt-dlp](https://github.com/yt-dlp/yt-dlp/releases/tag/latest)

check a instllation in ubuntu
```bash
yt-dlp --version
```