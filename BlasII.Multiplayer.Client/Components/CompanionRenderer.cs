using BlasII.ModdingAPI;
using System.Linq;
using UnityEngine;

namespace BlasII.Multiplayer.Client.Components;

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

        return child.AddComponent<Animator>();
    }

    public void UpdateAnim(int state, float time, float length)
    {
        _state = state;
        _time = time;
        _length = length;
    }

    public void UpdateEquipment(byte type, string name)
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

        // This isnt perfect, because both rapier effects have the same name
        RuntimeAnimatorController controller = Main.Multiplayer.AnimationStorage.GetAllControllers().FirstOrDefault(x => x.name == name);

        if (controller == null)
        {
            ModLog.Error($"Failed to find animator {name} for type {type}");
            return;
        }

        anim.runtimeAnimatorController = controller;
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
