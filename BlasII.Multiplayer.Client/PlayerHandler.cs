using Basalt.Framework.Networking.Client;
using BlasII.ModdingAPI;
using BlasII.Multiplayer.Core.Packets;
using Il2CppTGK.Game;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class PlayerHandler
{
    private Vector2 _lastPosition;
    private int _lastAnimationState;
    private float _lastAnimationTime;
    private float _lastAnimationLength;
    private bool _lastDirection;

    private string _lastArmorName;
    private string _lastWeaponName;
    private string _lastWeaponfxName;

    public PlayerHandler(NetworkClient client)
    {
        client.OnClientConnected += OnClientConnected;
    }

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

    private void OnClientConnected(string ip)
    {
        ModLog.Info("Sending all status packets");

        Main.Multiplayer.NetworkHandler.Send(new PositionPacket(null, _lastPosition.x, _lastPosition.y));
        Main.Multiplayer.NetworkHandler.Send(new AnimationPacket(null, _lastAnimationState, _lastAnimationTime, _lastAnimationLength));
        Main.Multiplayer.NetworkHandler.Send(new DirectionPacket(null, _lastDirection));
        Main.Multiplayer.NetworkHandler.Send(new EquipmentPacket(null, 0, _lastArmorName));
        Main.Multiplayer.NetworkHandler.Send(new EquipmentPacket(null, 1, _lastWeaponName));
        Main.Multiplayer.NetworkHandler.Send(new EquipmentPacket(null, 2, _lastWeaponfxName));
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

        Main.Multiplayer.NetworkHandler.Send(new PositionPacket(null, x, y));
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
        _lastAnimationTime = currAnimationTime;
        _lastAnimationLength = animState.length;

        Main.Multiplayer.NetworkHandler.Send(new AnimationPacket(null, currAnimationState, currAnimationTime, animState.length));
    }

    private void CheckDirection(Transform tpo)
    {
        bool currDirection = tpo.localScale.x >= 0;

        if (_lastDirection == currDirection)
            return;

        ModLog.Warn($"New direction: {currDirection}");
        _lastDirection = currDirection;

        Main.Multiplayer.NetworkHandler.Send(new DirectionPacket(null, currDirection));
    }

    private void CheckArmor(Animator armor)
    {
        string currArmorName = armor.runtimeAnimatorController?.name ?? string.Empty;

        if (_lastArmorName == currArmorName)
            return;

        ModLog.Warn($"New armor: {currArmorName}");
        _lastArmorName = currArmorName;

        Main.Multiplayer.NetworkHandler.Send(new EquipmentPacket(null, 0, currArmorName));
    }

    private void CheckWeapon(Animator weapon)
    {
        string currWeaponName = weapon.runtimeAnimatorController?.name ?? string.Empty;

        if (_lastWeaponName == currWeaponName)
            return;

        ModLog.Warn($"New weapon: {currWeaponName}");
        _lastWeaponName = currWeaponName;

        Main.Multiplayer.NetworkHandler.Send(new EquipmentPacket(null, 1, currWeaponName));
    }

    private void CheckWeaponEffects(Animator weaponfx)
    {
        string currWeaponfxName = weaponfx.runtimeAnimatorController?.name ?? string.Empty;

        if (_lastWeaponfxName == currWeaponfxName)
            return;

        ModLog.Warn($"New weaponfx: {currWeaponfxName}");
        _lastWeaponfxName = currWeaponfxName;

        Main.Multiplayer.NetworkHandler.Send(new EquipmentPacket(null, 2, currWeaponfxName));
    }

    private const int PRECISION = 5;
}
