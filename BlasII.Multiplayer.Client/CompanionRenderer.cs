using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Defense.Data;
using System.Linq;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

//public class CompanionRenderer
//{
//    private Animator _armor;
//    private Animator _weapon;
//    private Animator _weaponfx;

//    public CompanionRenderer(Transform companion)
//    {
//        ModLog.Info("Creating new CompanionRenderer");

//        _armor = CreateAnim("armor", companion);
//        _weapon = CreateAnim("weapon", companion);
//        _weaponfx = CreateAnim("weapon_effects", companion);
//    }

//    private Animator CreateAnim(string name, Transform parent)
//    {
//        GameObject player = CoreCache.PlayerSpawn.PlayerInstance.transform.GetChild(0).GetChild(0).Find(name).gameObject;

//        var child = new GameObject(name);
//        child.transform.SetParent(parent);
//        child.transform.localPosition = Vector3.zero;

//        var sr = child.AddComponent<SpriteRenderer>();
//        sr.sortingLayerName = "Player";
//        sr.sortingOrder = -1; // 0, 2, 1000

//        var anim = child.AddComponent<Animator>();
//        anim.runtimeAnimatorController = player.GetComponent<Animator>().runtimeAnimatorController;

//        return anim;
//    }

//    //public void UpdateArmor(int state, float time)
//    //{
//    //    _armor.Play(state, 0, time);
//    //}

//    //public void UpdateWeapon(int state, float time)
//    //{
//    //    _weapon.Play(state, 0, time);
//    //}

//    //public void UpdateWeaponfx(int state, float time)
//    //{
//    //    _weaponfx.Play(state, 0, time);
//    //}

//    public void UpdateAnim(int state, float time)
//    {
//        _armor.Play(state, 0, time);
//        _weapon.Play(state, 0, time);
//        _weaponfx.Play(state, 0, time);
//    }
//}

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
        GameObject player = CoreCache.PlayerSpawn.PlayerInstance.transform.GetChild(0).GetChild(0).Find(name).gameObject;

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
        if (type == 0) // Armor
        {
            RuntimeAnimatorController anim = GetArmorAnimator(name);
            _armor.runtimeAnimatorController = anim;
        }
        else if (type == 1) // Weapon
        {
            RuntimeAnimatorController anim = GetWeaponAnimator(name);
            _weapon.runtimeAnimatorController = anim;
        }
    }

    private RuntimeAnimatorController GetArmorAnimator(string name)
    {
        var collection = Resources.FindObjectsOfTypeAll<ArmorsCollection>().First();
        
        foreach (var anim in collection.armorsAnimsLookUp.Values)
        {
            if (anim.runtimeAnimatorController.name == name)
                return anim.runtimeAnimatorController;
        }

        ModLog.Error($"Failed to find armor anim: {name}");
        return null;
    }

    private RuntimeAnimatorController GetWeaponAnimator(string name)
    {
        var collection = Resources.FindObjectsOfTypeAll<WeaponsCollection>().First();

        foreach (var anim in collection.weaponsAnimsLookUp.Values)
        {
            if (anim.runtimeAnimatorController.name == name)
                return anim.runtimeAnimatorController;
        }

        ModLog.Error($"Failed to find weapon anim: {name}");
        return null;
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
