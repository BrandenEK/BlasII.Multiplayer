using BlasII.ModdingAPI;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Defense.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Multiplayer.Client.Storages;

public class AnimationStorage
{
    private readonly List<RuntimeAnimatorController> _controllers = [];

    public void Initialize()
    {
        var lookups = Resources.FindObjectsOfTypeAll<ArmorsCollection>().Select(x => x.armorsAnimsLookUp)
            .Concat(Resources.FindObjectsOfTypeAll<WeaponsCollection>().Select(x => x.weaponsAnimsLookUp))
            .Concat(Resources.FindObjectsOfTypeAll<WeaponEffectsCollection>().Select(x => x.weaponEffectsAnimsLookUp));

        foreach (var lookup in lookups)
        {
            foreach (var anim in lookup.Values)
            {
                _controllers.Add(anim.runtimeAnimatorController);
            }
        }

        ModLog.Info($"Storing {_controllers.Count} equipment controllers");
    }

    public IEnumerable<RuntimeAnimatorController> GetAllControllers()
    {
        return _controllers;
    }
}
