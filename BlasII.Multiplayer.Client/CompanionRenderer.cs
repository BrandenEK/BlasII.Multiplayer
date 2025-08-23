using BlasII.ModdingAPI;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Defense.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class CompanionRenderer
{
    private Animator _armor;
    private Animator _weapon;
    private Animator _weaponfx;

    private int _state;
    private float _time;
    private float _length;

    public CompanionRenderer(Transform companion)
    {
        ModLog.Info("Creating new CompanionRenderer");

        _armor = CreateAnim("armor", -4, companion);
        _weapon = CreateAnim("weapon", -3, companion);
        _weaponfx = CreateAnim("weapon_effects", -2, companion);
    }

    private Animator CreateAnim(string name, int sort, Transform parent)
    {
        var child = new GameObject(name);
        child.transform.SetParent(parent);
        child.transform.localPosition = Vector3.zero;

        var sr = child.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Player";
        sr.sortingOrder = sort; // Armor = 0, Weapon = 2, Weapon VFX = 1000

        var anim = child.AddComponent<Animator>();
        //anim.runtimeAnimatorController = player.GetComponent<Animator>().runtimeAnimatorController;

        return anim;
    }

    public void UpdateAnim(int state, float time, float length)
    {
        _state = state;
        _time = time;
        _length = length;
    }

    //public void UpdateEquipment(int type, string name)
    //{
    //    Animator anim;
    //    IEnumerable<RuntimeAnimatorController> controllers; 

    //    switch (type)
    //    {
    //        case 0:
    //            anim = _armor;
    //            controllers

    //            TryGetAnimator<ArmorsCollection>(name, x => x.armorsAnimsLookUp, out RuntimeAnimatorController controller);

    //            animators = Resources.FindObjectsOfTypeAll<ArmorsCollection>().Select(x => x.armorsAnimsLookUp);
    //            break;
    //        case 1:
    //            anim = _weapon;
    //            animators = Resources.FindObjectsOfTypeAll<WeaponsCollection>().First().weaponsAnimsLookUp;
    //            break;
    //        case 2:
    //            anim = _weaponfx;
    //            animators = Resources.FindObjectsOfTypeAll<WeaponEffectsCollection>().First().weaponEffectsAnimsLookUp;
    //            break;
    //        default:
    //            ModLog.Error($"Failed to update equipment for type {type}");
    //            return;
    //    }

    //    foreach (var x in lookup.Values)
    //        ModLog.Info(x.runtimeAnimatorController.name);

    //    if (string.IsNullOrEmpty(name))
    //    {
    //        anim.runtimeAnimatorController = null;
    //        return;
    //    }

    //    if (TryGetAnimator(name, lookup, out RuntimeAnimatorController controller))
    //    {
    //        anim.runtimeAnimatorController = controller;
    //        return;
    //    }

    //    ModLog.Error($"Failed to find animator {name} for type {type}");
    //}

    //private bool TryGetAnimator(string name, Dictionary<int, Animator> lookup, out RuntimeAnimatorController anim)
    //{
    //    foreach (var a in lookup.Values)
    //    {
    //        if (a.runtimeAnimatorController.name == name)
    //        {
    //            anim = a.runtimeAnimatorController;
    //            return true;
    //        }
    //    }

    //    anim = null;
    //    return false;
    //}

    //// This can be done once in initialize
    //private IEnumerable<RuntimeAnimatorController> GetControllers<T>(Func<T, Il2CppSystem.Collections.Generic.Dictionary<int, Animator>> lookupSelector) where T : ScriptableObject
    //{
    //    var controllers = new List<RuntimeAnimatorController>();

    //    foreach (var collection in Resources.FindObjectsOfTypeAll<T>())
    //    {
    //        foreach (var anim in lookupSelector(collection).Values)
    //        {
    //            controllers.Add(anim.runtimeAnimatorController);
    //        }
    //    }

    //    return controllers;
    //}

    //private bool TryGetAnimator<T>(string name, Func<T, Dictionary<int, Animator>> lookupSelector, out RuntimeAnimatorController anim)
    //{
    //    foreach (var a in lookup.Values)
    //    {
    //        if (a.runtimeAnimatorController.name == name)
    //        {
    //            anim = a.runtimeAnimatorController;
    //            return true;
    //        }
    //    }

    //    anim = null;
    //    return false;
    //}

    // debug

    public void UpdateEquipment(int type, string name)
    {
        if (type < 0 || type > 2)
        {
            ModLog.Error($"Failed to update equipment for type {type}");
            return;
        }

        Animator anim = type switch
        {
            0 => _armor,
            1 => _weapon,
            2 => _weaponfx,
            _ => null
        };

        if (string.IsNullOrEmpty(name))
        {
            //anim.runtimeAnimatorController = null;
            //return;
        }

        RuntimeAnimatorController controller = GetAllControllers().FirstOrDefault(x => x.name == name);

        if (controller == null)
        {
            ModLog.Error($"Failed to find animator {name} for type {type}");
            return;
        }

        anim.runtimeAnimatorController = controller;
    }

    private static List<RuntimeAnimatorController> _controllers;

    private static IEnumerable<RuntimeAnimatorController> GetAllControllers()
    {
        if (_controllers != null)
            return _controllers;

        _controllers = new List<RuntimeAnimatorController>();

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

        return _controllers;
    }

    public void OnUpdate()
    {
        _time += Time.deltaTime;

        float percent = _time / _length;
        _armor.Play(_state, 0, percent);
        _weapon.Play(_state, 0, percent);
        _weaponfx.Play(_state, 0, percent);
    }
}
