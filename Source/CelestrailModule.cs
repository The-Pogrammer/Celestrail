using System;
using FMOD.Studio;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.Celestrail;

public class CelestrailModule : EverestModule {
    public static CelestrailModule Instance { get; private set; }

    public override Type SettingsType => typeof(CelestrailModuleSettings);
    public static CelestrailModuleSettings CelestrailSettings => (CelestrailModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(CelestrailModuleSession);
    public static CelestrailModuleSession CelestrailSession => (CelestrailModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(CelestrailModuleSaveData);
    public static CelestrailModuleSaveData CelestrailSaveData => (CelestrailModuleSaveData) Instance._SaveData;

    public CelestrailModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(CelestrailModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(CelestrailModule), LogLevel.Info);
#endif
    }

    public static TrailManager trailManager;
    public const string LoggerTag = nameof(CelestrailModule);

    public override void Load()
    {
        Everest.Events.LevelLoader.OnLoadingThread += AddTrailManager;
        On.Celeste.PlayerHair.MoveHairBy += NewOnMoveHairBy;
    }

    public override void Unload() {
        Everest.Events.LevelLoader.OnLoadingThread -= AddTrailManager;
        On.Celeste.PlayerHair.MoveHairBy -= NewOnMoveHairBy;
    }

    //thanks to bit8289 for the idea on how to do this
    public static void NewOnMoveHairBy(On.Celeste.PlayerHair.orig_MoveHairBy orig, PlayerHair hair, Vector2 amount)
    {
        orig(hair, amount);
        trailManager.cutTrail();
    }

    private static void AddTrailManager(Level level)
    {
        trailManager = new TrailManager();
        level.Add(trailManager);
    }
}