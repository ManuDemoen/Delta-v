using Content.Client.Overlays;
using Content.Shared._DV.Overlays;
using Robust.Shared.Prototypes;

namespace Content.Client._DV.Overlays;

/// <summary>
/// Health bar overlay that only renders bars for mobs with <see cref="ZookeeperHudVisibleComponent"/>.
/// </summary>
public sealed class AnimalHealthBarOverlay : EntityHealthBarOverlay
{
    private readonly IEntityManager _entManager;

    public AnimalHealthBarOverlay(IEntityManager entManager, IPrototypeManager prototype)
        : base(entManager, prototype)
    {
        _entManager = entManager;
    }

    protected override bool ShouldShow(EntityUid uid) =>
        _entManager.HasComponent<ZookeeperHudVisibleComponent>(uid);
}
