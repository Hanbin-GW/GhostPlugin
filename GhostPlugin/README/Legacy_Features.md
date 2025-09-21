# Legacy Features
this features is no more using in GhostPlugin anymore

## `SpawnPrimitiveToy.cs`

This feature demonstrates how to spawn small primitive objects (e.g., cubes) using the `PrimitiveObjectToy` system from `AdminToys`. It instantiates networked physical objects near the player using Unity's `PrimitiveType` and applies basic physics.

### Key Details:

* **Object Type**: Small cube (`PrimitiveType.Cube`)
* **Spawning Location**: At or near the player’s current position
* **Customization**: Color, physics (gravity, mass, drag), and collider
* **Lifecycle**: Automatically destroyed after 15 seconds
* **Use Case**: Useful for legacy toy-based effects, visual markers, or debugging tools

#### Note:

This method relies on iterating through `NetworkClient.prefabs` to find the correct toy prefab. It is considered **legacy** due to newer systems offering more structured spawning logic.

---

## `AudioManagemanet.cs`

This legacy audio feature demonstrates how to use **SCPSLAudioApi** to play custom music tracks on the server. It handles audio playback for the host using a shared `AudioPlayerBase` instance.

### Key Details:

* **API Used**: `SCPSLAudioApi`
* **Functionality**:

    * Play a specific audio file via file path
    * Stop currently playing music
    * Controls looping and volume
* **Use Case**: Can be used to play lobby music or trigger audio events during gameplay

#### Note:

This implementation assumes that the audio file exists on the server. It is considered **legacy** due to direct dependency on `AudioPlayerBase` and lack of user-specific playback or async control.

---

### Play Audio – Method Summary

* `PlaySpecificMusic(string filepath)`
  Plays a specified audio file using the host's `AudioPlayerBase`. Looping is disabled and volume is set to 20.

* `StopLobbyMusic()`
  Stops any currently playing track on the shared audio player.

