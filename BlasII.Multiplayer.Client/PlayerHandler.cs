using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class PlayerHandler
{
    private Vector2 _lastPosition;
    private int _lastAnimation;
    private bool _lastDirection;

    public void OnUpdate()
    {
        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);
        Transform graphic = tpo.GetChild(0);
        Transform armor = graphic.Find("armor");

        CheckPosition(tpo);
        CheckAnimation(armor);
        CheckDirection(tpo);
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
        int currAnimation = animState.nameHash;

        if (_lastAnimation == currAnimation)
            return;

        ModLog.Warn($"New animation: {currAnimation}");
        _lastAnimation = currAnimation;

        float percentTime = animState.normalizedTime;
        float totalTime = animState.length;

        // Send packet
        Main.Multiplayer.CompanionHandler.TempGetAnimation(currAnimation, percentTime);
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

    private const int PRECISION = 5;
}
