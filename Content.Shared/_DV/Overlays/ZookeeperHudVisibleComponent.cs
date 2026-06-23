using Robust.Shared.GameStates;

namespace Content.Shared._DV.Overlays;

/// <summary>
/// Marks a mob as visible to the zookeeper HUD (hunger, thirst, and health bars).
/// Added to SimpleMobBase so all air-breathing animal mobs inherit it.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ZookeeperHudVisibleComponent : Component { }
