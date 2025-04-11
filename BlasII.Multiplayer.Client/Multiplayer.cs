using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using Il2CppTGK.Game;
using System.Collections.Generic;
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
        UpdateTest();

        //if (_test == null)
        //    return;

        //Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        //Transform graphic = player.GetChild(0).GetChild(0);

        //GameObject armor = graphic.Find("armor").gameObject;
        //GameObject weapon = graphic.Find("weapon").gameObject;
        //GameObject weaponfx = graphic.Find("weapon_effects").gameObject;

        //SpriteRenderer armorSprite = armor.GetComponent<SpriteRenderer>();
        //Animator armorAnim = armor.GetComponent<Animator>();

        //var anim = _test.GetComponent<Animator>();
        //anim.Play(armorAnim.GetCurrentAnimatorStateInfo(0).nameHash, 0, armorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    private void SpawnTest()
    {
        ModLog.Info("Testing spawn");
        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);

        var companion = new Companion("Test" + _companions.Count);

        companion.Transform.UpdatePosition(tpo.position);

        _companions.Add(companion);
        //Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        //Transform tpo = player.GetChild(0);
        //Transform graphic = tpo.GetChild(0);

        //GameObject armor = graphic.Find("armor").gameObject;
        //GameObject weapon = graphic.Find("weapon").gameObject;
        //GameObject weaponeffects = graphic.Find("weapon_effects").gameObject;

        //SpriteRenderer armorSprite = armor.GetComponent<SpriteRenderer>();
        //SpriteRenderer weapons = weapon.GetComponent<SpriteRenderer>();
        //SpriteRenderer weaponfxs = weaponeffects.GetComponent<SpriteRenderer>();
        
        //Animator armorAnim = armor.GetComponent<Animator>();

        //_test = new GameObject("Test");
        //_test.transform.position = tpo.transform.position; // pos packet
        //_test.transform.rotation = tpo.transform.rotation; // const
        //_test.transform.localScale = tpo.transform.localScale; // scale packet

        //var sr = _test.AddComponent<SpriteRenderer>();
        //ModLog.Fatal(armorSprite.sortingLayerName);
        //ModLog.Fatal(armorSprite.sortingOrder);
        //ModLog.Fatal(weapons.sortingLayerName);
        //ModLog.Fatal(weapons.sortingOrder);
        //ModLog.Fatal(weaponfxs.sortingLayerName);
        //ModLog.Fatal(weaponfxs.sortingOrder);
        //sr.sortingLayerID = armorSprite.sortingLayerID; // const
        //sr.sortingOrder = armorSprite.sortingOrder; // const

        //var anim = _test.AddComponent<Animator>();
        //anim.runtimeAnimatorController = armorAnim.runtimeAnimatorController; // const

        //sr.sprite = armorSprite.sprite; // temp
    }

    private void UpdateTest()
    {
        if (!SceneHelper.GameSceneLoaded)
            return;

        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);
        Transform graphic = tpo.GetChild(0);

        GameObject armor = graphic.Find("armor").gameObject;
        GameObject weapon = graphic.Find("weapon").gameObject;
        GameObject weaponfx = graphic.Find("weapon_effects").gameObject;

        Animator armorAnim = armor.GetComponent<Animator>();
        Animator weaponAnim = weapon.GetComponent<Animator>();
        Animator weaponfxAnim = weaponfx.GetComponent<Animator>();

        var animState = armorAnim.GetCurrentAnimatorStateInfo(0);
        int state = animState.nameHash;
        float time = animState.normalizedTime;
        //ModLog.Info($"armor state: {state}");
        //ModLog.Info($"armor time: {time}");

        //var animState2 = weaponAnim.GetCurrentAnimatorStateInfo(0);
        //int state2 = animState2.nameHash;
        //float time2 = animState2.normalizedTime;
        //ModLog.Info($"weapon state: {state2}");
        //ModLog.Info($"weapon time: {time2}");

        foreach (var companion in _companions)
        {
            //companion.Transform.UpdatePosition(tpo.position);
            companion.Transform.UpdateScale(tpo.localScale);
            companion.Renderer.UpdateAnim(state, time);
        }
    }

    private readonly List<Companion> _companions = new();

    //private GameObject _test;
}
