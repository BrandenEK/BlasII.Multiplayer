using BlasII.ModdingAPI;
using Il2CppSystem.Collections.Generic;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Defense.Data;
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

    public void UpdateEquipment(int type, string name)
    {
        Animator anim;
        Dictionary<int, Animator> lookup;

        switch (type)
        {
            case 0:
                anim = _armor;
                lookup = Resources.FindObjectsOfTypeAll<ArmorsCollection>().First().armorsAnimsLookUp;
                break;
            case 1:
                anim = _weapon;
                lookup = Resources.FindObjectsOfTypeAll<WeaponsCollection>().First().weaponsAnimsLookUp;
                break;
            case 2:
                anim = _weaponfx;
                lookup = Resources.FindObjectsOfTypeAll<WeaponEffectsCollection>().First().weaponEffectsAnimsLookUp;
                break;
            default:
                ModLog.Error($"Failed to update equipment for type {type}");
                return;
        }

        foreach (var x in lookup.Values)
            ModLog.Info(x.runtimeAnimatorController.name);

        if (string.IsNullOrEmpty(name))
        {
            anim.runtimeAnimatorController = null;
            return;
        }

        if (TryGetAnimator(name, lookup, out RuntimeAnimatorController controller))
        {
            anim.runtimeAnimatorController = controller;
            return;
        }

        ModLog.Error($"Failed to find animator {name} for type {type}");
    }

    private bool TryGetAnimator(string name, Dictionary<int, Animator> lookup, out RuntimeAnimatorController anim)
    {
        foreach (var a in lookup.Values)
        {
            if (a.runtimeAnimatorController.name == name)
            {
                anim = a.runtimeAnimatorController;
                return true;
            }
        }

        anim = null;
        return false;
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
