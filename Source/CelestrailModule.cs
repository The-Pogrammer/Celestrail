using System;
using FMOD.Studio;
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

    public const string LoggerTag = nameof(CelestrailModule);

    public override void Load()
    {
        Everest.Events.LevelLoader.OnLoadingThread += AddTrailManager;
    }

    public override void Unload() {
        Everest.Events.LevelLoader.OnLoadingThread -= AddTrailManager;
    }

    private static void AddTrailManager(Level level)
    {
        level.Add(new TrailManager());
    }
}