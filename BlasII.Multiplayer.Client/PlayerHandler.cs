using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Attack.Data;
using System.Linq;
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

    public void OnUpdate()
    {
        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);
        Transform graphic = tpo.GetChild(0);
        Transform armor = graphic.Find("armor");

        CheckPosition(tpo);
        CheckAnimation(armor);
        CheckDirection(tpo);
        //CheckEquipment(graphic);

        CheckArmor(armor);
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

    private void CheckAnimation(Transform armor)
    {
        Animator anim = armor.GetComponent<Animator>();
        var animState = anim.GetCurrentAnimatorStateInfo(0);
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

    private void CheckEquipment(Transform graphic)
    {
        Transform armor = graphic.Find("armor");
        Transform weapon = graphic.Find("weapon");
        Transform weaponfx = graphic.Find("weapon_effects");

        //Animator armorAnim = armor.GetComponent<Animator>();
        string currArmorName = armor.GetComponent<Animator>().runtimeAnimatorController.name;
    }

    private void CheckArmor(Transform armor)
    {
        string currArmorName = armor.GetComponent<Animator>().runtimeAnimatorController.name;

        if (_lastArmorName == currArmorName)
            return;

        ModLog.Warn($"New armor: {currArmorName}");
        _lastArmorName = currArmorName;

        foreach (var col in Resources.FindObjectsOfTypeAll<WeaponsCollection>())
            ModLog.Info(col.name);

        //int id = -1;
        //WeaponsCollection collection = Resources.FindObjectsOfTypeAll<WeaponsCollection>().First();
        //foreach (var kvp in collection.weaponsAnimsLookUp)
        //{
        //    if (kvp.value.runtimeAnimatorController.name == currArmorName)

        //}

        //int id = collection.weaponsAnimsLookUp

        // Send packet
        Main.Multiplayer.CompanionHandler.TempGetEquipment(0, )
    }

    private const int PRECISION = 5;
}
