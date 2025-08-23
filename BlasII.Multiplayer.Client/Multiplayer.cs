using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using Il2CppTGK.Game;
using System.Collections.Generic;
using UnityEngine;

namespace BlasII.Multiplayer.Client;

public class Multiplayer : BlasIIMod
{
    public PlayerHandler PlayerHandler { get; }

    internal Multiplayer() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        PlayerHandler = new PlayerHandler();
    }

    protected override void OnInitialize()
    {
        // Perform initialization here
        CoreCache.PlayerSpawn.add_OnPlayerSpawned(new System.Action(OnPlayerSpawn));
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
        if (!SceneHelper.GameSceneLoaded || CoreCache.PlayerSpawn.PlayerInstance == null)
            return;

        UpdateTest();

        PlayerHandler.Update();
    }

    protected override void OnSceneLoaded(string sceneName)
    {
        _companions.Clear();
    }

    protected override void OnSceneUnloaded(string sceneName)
    {
        _companions.Clear();
    }

    private void OnPlayerSpawn()
    {
        ModLog.Warn("Player was spawned");
    }

    private void SpawnTest()
    {
        ModLog.Info("Testing spawn");

        var companion = new Companion("Test" + _companions.Count);
        _companions.Add(companion);

        // temp
        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);
        companion.Transform.UpdatePosition(tpo.position);
    }

    private void UpdateTest()
    {
        if (!SceneHelper.GameSceneLoaded)
            return;

        // Change this to a script on the player object with a reference to the animator
        // Although, the armor object might be replaced

        Transform player = CoreCache.PlayerSpawn.PlayerInstance.transform;
        Transform tpo = player.GetChild(0);
        Transform graphic = tpo.GetChild(0);
        Animator anim = graphic.Find("armor").GetComponent<Animator>();

        var animState = anim.GetCurrentAnimatorStateInfo(0);
        int state = animState.nameHash;
        float time = animState.normalizedTime;

        foreach (var companion in _companions)
        {
            //companion.Transform.UpdatePosition(tpo.position);
            companion.Transform.UpdateScale(tpo.localScale);
            companion.Renderer.UpdateAnim(state, time);
        }
    }

    private readonly List<Companion> _companions = [];
}
