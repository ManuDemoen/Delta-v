using Robust.Shared.GameStates;

namespace Content.Shared._DV.Overlays;

/// <summary>
/// Worn by a zookeeper. Shows hunger, thirst, and health bars for animal mobs tagged with ZookeeperVisible.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ShowZookeeperHudComponent : Component { }
