using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class PlayerHandler
{
    private Vector2 _lastPosition;
    private int _lastAnimation;
    private bool _lastDirection;

    public void Update()
    {
        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);
        Transform graphic = tpo.GetChild(0);
        Animator anim = graphic.Find("armor").GetComponent<Animator>();
        var animState = anim.GetCurrentAnimatorStateInfo(0);

        CheckPosition(tpo);

        // Check animation
        int currAnimation = animState.nameHash;
        if (_lastAnimation != currAnimation)
        {
            ModLog.Warn($"New animation: {currAnimation}");
            _lastAnimation = currAnimation;

            float percentTime = animState.normalizedTime;
            float totalTime = animState.length;

            // Send packet
        }

        // Check direction
        bool currDirection = tpo.localScale.x >= 0;
        if (_lastDirection != currDirection)
        {
            ModLog.Warn($"New direction: {currDirection}");
            _lastDirection = currDirection;

            // Send packet
        }
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
    }

    private const int PRECISION = 5;
}
