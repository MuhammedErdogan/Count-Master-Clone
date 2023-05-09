using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Manager
{
    public class SaveManager
    {
        #region Keys

        private const string KEY_FIRST_OPEN = "KEY_FIRST_OPEN";

        public const string KEY_MAX_SCORE = "KEY_MAX_SCORE";
        public const string KEY_TOTAL_SCORE = "KEY_TOTAL_SCORE";

        public const string KEY_TOTAL_PLAY_COUNT = "KEY_TOTAL_PLAY_COUNT";

        public const string KEY_LEVEL_INDEX = "KEY_LEVEL_INDEX";

        #endregion

        public static void Init()
        {
            if (LoadInt(KEY_FIRST_OPEN) == 0)
            {
                return;
            }

            ClearSave();
            SaveInt(KEY_FIRST_OPEN, 0);
            ApplySave();
        }

        public static void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public static string LoadString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public static int LoadInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static void SaveFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static float LoadFloat(string key, float defaultValue = 0.0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static void SaveBool(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public static bool LoadBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public static void SaveObject<T>(string key, T value)
        {
            string json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(key, json);
        }

        public static T LoadObject<T>(string key, T defaultValue = default(T))
        {
            string json = PlayerPrefs.GetString(key, "");
            if (string.IsNullOrEmpty(json))
            {
                return defaultValue;
            }
            return JsonUtility.FromJson<T>(json);
        }

        public static void ApplySave()
        {
            PlayerPrefs.Save();
        }

        public static void ClearSave()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
