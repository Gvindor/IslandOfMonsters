using System.Collections;
using UnityEngine;
using AppsFlyerSDK;
using System.Collections.Generic;

namespace SF
{
    public static class Analytics
    {
        const string LevelStartEvent = "level_start";
        const string LevelFinishedEvent = "level_finished";

        public static void LogLevelStart(int level)
        {
            AppsFlyer.sendEvent(LevelStartEvent, new Dictionary<string, string>() { { "level", level.ToString() } });
        }

        public static void LogLevelFinished(int level)
        {
            AppsFlyer.sendEvent(LevelFinishedEvent, new Dictionary<string, string>() { { "level", level.ToString() } });
        }
    }
}