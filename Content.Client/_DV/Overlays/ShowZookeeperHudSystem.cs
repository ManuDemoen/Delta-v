using Content.Client.Overlays;
using Content.Shared._DV.Overlays;
using Content.Shared.Inventory.Events;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Robust.Client.Graphics;
using Robust.Shared.Prototypes;

namespace Content.Client._DV.Overlays;

public sealed class ShowZookeeperHudSystem : EquipmentHudSystem<ShowZookeeperHudComponent>
{
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly HungerSystem _hunger = default!;
    [Dependency] private readonly ThirstSystem _thirst = default!;

    // Icons shown when the animal is at the "Okay" satiation level (green variants)
    private static readonly ProtoId<SatiationIconPrototype> HungerOkayFallback = "HungerIconSatiated";
    private static readonly ProtoId<SatiationIconPrototype> ThirstOkayFallback = "ThirstIconHydrated";

    private AnimalHealthBarOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        _overlay = new(EntityManager, _prototype);
        _overlay.DamageContainers.Add("Biological");

        // Subscribe on ZookeeperHudVisibleComponent to avoid conflicting with
        // ShowHungerIconsSystem / ShowThirstIconsSystem which own those component subscriptions.
        SubscribeLocalEvent<ZookeeperHudVisibleComponent, GetStatusIconsEvent>(OnGetStatusIcons);
    }

    private void OnGetStatusIcons(EntityUid uid, ZookeeperHudVisibleComponent _, ref GetStatusIconsEvent ev)
    {
        if (!IsActive)
            return;

        if (TryComp<HungerComponent>(uid, out var hunger))
        {
            if (_hunger.TryGetStatusIconPrototype(hunger, out var hungerIcon))
                ev.StatusIcons.Add(hungerIcon);
            else if (hunger.CurrentThreshold == HungerThreshold.Okay)
                ev.StatusIcons.Add(_prototype.Index(HungerOkayFallback));
        }

        if (TryComp<ThirstComponent>(uid, out var thirst))
        {
            if (_thirst.TryGetStatusIconPrototype(thirst, out var thirstIcon))
                ev.StatusIcons.Add(thirstIcon);
            else if (thirst.CurrentThirstThreshold == ThirstThreshold.Okay)
                ev.StatusIcons.Add(_prototype.Index(ThirstOkayFallback));
        }
    }

    protected override void UpdateInternal(RefreshEquipmentHudEvent<ShowZookeeperHudComponent> component)
    {
        base.UpdateInternal(component);

        if (!_overlayMan.HasOverlay<AnimalHealthBarOverlay>())
            _overlayMan.AddOverlay(_overlay);
    }

    protected override void DeactivateInternal()
    {
        base.DeactivateInternal();
        _overlayMan.RemoveOverlay(_overlay);
    }
}
