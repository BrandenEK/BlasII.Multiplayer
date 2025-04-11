using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class Multiplayer : BlasIIMod
{
    internal Multiplayer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    protected override void OnInitialize()
    {
        // Perform initialization here
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {            
            SpawnTest();
        }
    }

    protected override void OnLateUpdate()
    {
        if (_test == null)
            return;

        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform graphic = player.GetChild(0).GetChild(0);

        GameObject armor = graphic.Find("armor").gameObject;
        GameObject weapon = graphic.Find("weapon").gameObject;
        GameObject weaponeffects = graphic.Find("weapon_effects").gameObject;

        SpriteRenderer armorSprite = armor.GetComponent<SpriteRenderer>();
        Animator armorAnim = armor.GetComponent<Animator>();

        var anim = _test.GetComponent<Animator>();
        anim.Play(armorAnim.GetCurrentAnimatorStateInfo(0).nameHash, 0, armorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    private void SpawnTest()
    {
        ModLog.Info("Testing spawn");
        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);
        Transform graphic = tpo.GetChild(0);

        GameObject armor = graphic.Find("armor").gameObject;
        GameObject weapon = graphic.Find("weapon").gameObject;
        GameObject weaponeffects = graphic.Find("weapon_effects").gameObject;

        SpriteRenderer armorSprite = armor.GetComponent<SpriteRenderer>();
        Animator armorAnim = armor.GetComponent<Animator>();

        _test = new GameObject("Test");
        _test.transform.position = tpo.transform.position; // pos packet
        _test.transform.rotation = tpo.transform.rotation; // const
        _test.transform.localScale = tpo.transform.localScale; // scale packet

        var sr = _test.AddComponent<SpriteRenderer>();
        sr.sortingLayerID = armorSprite.sortingLayerID; // const
        sr.sortingOrder = armorSprite.sortingOrder; // const

        var anim = _test.AddComponent<Animator>();
        anim.runtimeAnimatorController = armorAnim.runtimeAnimatorController; // const

        sr.sprite = armorSprite.sprite; // temp
    }

    private GameObject _test;
}
