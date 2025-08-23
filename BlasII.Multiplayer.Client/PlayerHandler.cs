using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class PlayerHandler
{
    private Vector2 _lastPosition;
    private int _lastAnimationState;
    private float _lastAnimationTime;
    private bool _lastDirection;

    private string _lastArmorName;
    private string _lastWeaponName;
    private string _lastWeaponfxName;

    public void OnEnterScene() // Not sure if this actually does anything
    {
        _lastPosition = Vector2.zero;
        _lastAnimationState = 0;
        _lastAnimationTime = 0;
        _lastDirection = false;

        _lastArmorName = string.Empty;
        _lastWeaponName = string.Empty;
        _lastWeaponfxName = string.Empty;
    }

    public void OnLeaveScene()
    {
        _lastPosition = Vector2.zero;
        _lastAnimationState = 0;
        _lastAnimationTime = 0;
        _lastDirection = false;

        _lastArmorName = string.Empty;
        _lastWeaponName = string.Empty;
        _lastWeaponfxName = string.Empty;
    }

    public void OnUpdate()
    {
        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);
        Transform graphic = tpo.GetChild(0);

        Animator armor = graphic.Find("armor").GetComponent<Animator>();
        Animator weapon = graphic.Find("weapon").GetComponent<Animator>();
        Animator weaponfx = graphic.Find("weapon_effects").GetComponent<Animator>();

        CheckPosition(tpo);
        CheckAnimation(armor);
        CheckDirection(tpo);

        CheckArmor(armor);
        CheckWeapon(weapon);
        CheckWeaponEffects(weaponfx);
    }

    private void CheckPosition(Transform tpo)
    {
        float x = Mathf.Round(tpo.position.x * PRECISION) / PRECISION;
        float y = Mathf.Round(tpo.position.y * PRECISION) / PRECISION;
        Vector2 currPosition = new(x, y);

        if (_lastPosition == currPosition)
            return;

        ModLog.Warn($"New position: {currPosition}");
        _lastPosition = currPosition;

        // Send packet
        Main.Multiplayer.CompanionHandler.TempGetPosition(currPosition);
    }

    private void CheckAnimation(Animator armor)
    {
        var animState = armor.GetCurrentAnimatorStateInfo(0);
        int currAnimationState = animState.nameHash;
        float currAnimationTime = animState.normalizedTime * animState.length;

        if (_lastAnimationState == currAnimationState && _lastAnimationTime < currAnimationTime)
            return;

        ModLog.Warn($"New animation: {currAnimationState}");
        _lastAnimationState = currAnimationState;

        // Send packet
        float length = animState.length;
        Main.Multiplayer.CompanionHandler.TempGetAnimation(currAnimationState, currAnimationTime, length);
    }

    private void CheckDirection(Transform tpo)
    {
        bool currDirection = tpo.localScale.x >= 0;

        if (_lastDirection == currDirection)
            return;

        ModLog.Warn($"New direction: {currDirection}");
        _lastDirection = currDirection;

        // Send packet
        Main.Multiplayer.CompanionHandler.TempGetDirection(currDirection);
    }

    private void CheckArmor(Animator armor)
    {
        string currArmorName = armor.runtimeAnimatorController.name;

        if (_lastArmorName == currArmorName)
            return;

        ModLog.Warn($"New armor: {currArmorName}");
        _lastArmorName = currArmorName;

        // Send packet
        Main.Multiplayer.CompanionHandler.TempGetEquipment(0, currArmorName);
    }

    private void CheckWeapon(Animator weapon)
    {
        string currWeaponName = weapon.runtimeAnimatorController.name;

        if (_lastWeaponName == currWeaponName)
            return;

        ModLog.Warn($"New weapon: {currWeaponName}");
        _lastWeaponName = currWeaponName;

        // Send packet
        Main.Multiplayer.CompanionHandler.TempGetEquipment(1, currWeaponName);
    }

    private void CheckWeaponEffects(Animator weaponfx)
    {
        string currWeaponfxName = weaponfx.runtimeAnimatorController.name;

        if (_lastWeaponfxName == currWeaponfxName)
            return;

        ModLog.Warn($"New weaponfx: {currWeaponfxName}");
        _lastWeaponfxName = currWeaponfxName;

        // Send packet
        Main.Multiplayer.CompanionHandler.TempGetEquipment(2, currWeaponfxName);
    }

    private const int PRECISION = 5;
}
