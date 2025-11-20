using System;
using UnityEngine;
using UserProfile.Utils;
using System.IO;

namespace UserProfile
{
    public static class UserProfileStorage
    {
        public static event Action<Sprite> OnChangedUserIcon = null;
        public static event Action<string> OnChangedUserName = null;


        private const string IMAGE_SAVE_FILE = "icon.png";
        private const string NAME_SAVE_KEY = "UserNameSave";

        private const string DEFAULT_USER_NAME = "USERNAME";

        private static Sprite _userIcon = null;
        private static string _userName = null;

        private static Sprite DefaultIcon
        {
            get
            {
                string spriteName = "default_user";

                Sprite sprite = Resources.Load<Sprite>(spriteName);

                if (sprite == null)
                    Debug.LogError($"{spriteName} not found in Resources!!!");

                return sprite;
            }
        }

        public static Sprite UserIcon
        {
            get => _userIcon;
            set
            {
                if (_userIcon != value)
                {
                    _userIcon = value;

                    OnChangedUserIcon?.Invoke(_userIcon);
                }
            }
        }

        public static void SaveIcon()
        {
            try
            {
                if (_userIcon != null)
                {
                    SpriteSaver.SaveSprite(IMAGE_SAVE_FILE, _userIcon);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to save icon: {ex.Message}");
            }
        }

        public static Sprite LoadCurrentIcon()
        {
            string path = Path.Combine(Application.persistentDataPath, IMAGE_SAVE_FILE);

            if (!File.Exists(path))
                return null;

            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            tex.LoadImage(fileData);

            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        public static Sprite GetDefaultIcon()
        {
            return DefaultIcon;
        }

        public static string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    if (string.IsNullOrWhiteSpace(value))
                        value = DEFAULT_USER_NAME;

                    _userName = value;

                    OnChangedUserName?.Invoke(_userName);

                    PlayerPrefs.SetString(NAME_SAVE_KEY, _userName);
                    PlayerPrefs.Save();
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            _userIcon = LoadCurrentIcon();

            if (_userIcon == null)
                _userIcon = DefaultIcon;

            _userName = PlayerPrefs.GetString(NAME_SAVE_KEY, DEFAULT_USER_NAME);
        }
    }
}