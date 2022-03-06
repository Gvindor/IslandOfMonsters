using System.Collections;
using UnityEngine;
using AppsFlyerSDK;
using System.Collections.Generic;
using Facebook.Unity;

namespace SF
{
    public static class Analytics
    {
        const string LevelStartEvent = "level_start";
        const string LevelFinishedEvent = "level_finished";

        private const string LevelNumberParam = "level_number";
        private const string LevelResultParam = "result";

        public static void LogLevelStart(int num)
        {
            var parameters = new Dictionary<string, object>()
            {
                { LevelNumberParam, num },
            };

            Debug.Log($"[Analytics] {LevelStartEvent} | Num: {num}");

            LogEvent(LevelStartEvent, parameters);
        }

        public static void LogLevelFinished(int num, LevelResult result)
        {
            var parameters = new Dictionary<string, object>()
            {
                { LevelNumberParam, num },
                { LevelResultParam, result.ToString().ToLowerInvariant() },
            };

            Debug.Log($"[Analytics] {LevelFinishedEvent} | Num: {num} Result: {result}");

            LogEvent(LevelFinishedEvent, parameters);
        }

        public static void LogEvent(string logEvent, Dictionary<string, object> parameters)
        {
            var strParams = new Dictionary<string, string>();
            foreach (var item in parameters)
            {
                strParams[item.Key] = item.Value.ToString();
            }

            if (FB.IsInitialized)
                FB.LogAppEvent(logEvent, parameters: parameters);
            AppsFlyer.sendEvent(logEvent, strParams);
            //AppMetrica.Instance.ReportEvent(logEvent, parameters);

            //AppMetrica.Instance.SendEventsBuffer();
        }

        public enum LevelResult
        {
            Win,
            Lose
        }
    }
}