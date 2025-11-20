using UnityEngine;
using UnityEngine.UI;

namespace UserProfile.UI
{
    public class UserIcon : MonoBehaviour
    {
        [SerializeField] private Image _iconImage = null;

        private void Awake()
        {
            UserProfileStorage.OnChangedUserIcon += UpdateIcon;

            UpdateIcon(UserProfileStorage.UserIcon);
        }

        private void OnDestroy()
        {
            UserProfileStorage.OnChangedUserIcon -= UpdateIcon;
        }

        private void UpdateIcon(Sprite icon)
        {
            _iconImage.sprite = icon;
        }
        
        public void OnEnable()
        {
            LoadSavedIcon();
        }
        public void LoadSavedIcon()
        {
            Sprite loaded = UserProfileStorage.LoadCurrentIcon();

            if (loaded != null)
            {
                UserProfileStorage.UserIcon = loaded;
            }
            else
            {
                UserProfileStorage.UserIcon = UserProfileStorage.GetDefaultIcon();
            }
        }

        public void SaveIcon()
        {
            UserProfileStorage.SaveIcon();
        }

        public void StartEditing()
        {
            string panelName = "[SelectPicturePanel]";

            SelectPicturePanel panel = Resources.Load<SelectPicturePanel>(panelName);

            if (panel != null)
                Instantiate(panel);
            else
                Debug.LogError($"{panelName} not found in Resources!!!");
        }
    }
}
