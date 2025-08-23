using BlasII.ModdingAPI;
using UnityEngine;

namespace BlasII.Multiplayer.Client.Components;

public class CompanionTransform
{
    private Transform _transform;

    public CompanionTransform(Transform companion)
    {
        ModLog.Info("Creating new CompanionTransform");

        _transform = companion;
        _transform.position = Vector3.zero;
        _transform.rotation = Quaternion.identity;
        _transform.localScale = Vector3.one;
    }

    public void UpdatePosition(Vector3 position)
    {
        _transform.position = position;
    }

    public void UpdateDirection(bool direction)
    {
        _transform.localScale = new Vector3(direction ? 1 : -1, 1, 1);
    }
}
