using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

namespace UserProfile.UI
{
    public class UserNameField : MonoBehaviour
    {
        [SerializeField] private GameObject _normalState = null;
        [SerializeField] private GameObject _editingState = null;

        [Space]

        [SerializeField] private TextMeshProUGUI _nameText = null;
        [SerializeField] private TMP_InputField _editNameField = null;

        [Header("Settings")]
        [SerializeField] private int _maxNameLength = 20;

        private string nameToSave;

        private void Awake()
        {
            _normalState.SetActive(true);
            _editingState.SetActive(false);

            _editNameField.onEndEdit.AddListener(EndEdit);
            _editNameField.onValidateInput += ValidateInput;

            UserProfileStorage.OnChangedUserName += UpdateNameText;
            UpdateNameText(UserProfileStorage.UserName);
        }

        private char ValidateInput(string input, int charIndex, char addedChar)
        {
            if (Regex.IsMatch(addedChar.ToString(), "[^a-zA-Z0-9]"))
                return '\0';

            if (input.Length >= _maxNameLength)
                return '\0';

            return addedChar;
        }

        private void OnDestroy()
        {
            UserProfileStorage.OnChangedUserName -= UpdateNameText;
        }

        private void OnEnable()
        {
            ShowSavedName();
        }


        private void EndEdit(string name)
        {
            _normalState.SetActive(true);
            _editingState.SetActive(false);

            nameToSave = name;
            _nameText.text = name;
        }

        public void SaveName()
        {
            if (!string.IsNullOrWhiteSpace(nameToSave))
            {
                UserProfileStorage.UserName = nameToSave;
            }
            else
            {
                UserProfileStorage.UserName = UserProfileStorage.UserName;
            }
        }

        public void ShowSavedName()
        {
            nameToSave = UserProfileStorage.UserName;
            _nameText.text = UserProfileStorage.UserName;
        }

        private void UpdateNameText(string name)
        {
            _nameText.text = name;
        }

        public void StartEditing()
        {
            _normalState.SetActive(false);
            _editingState.SetActive(true);

            _editNameField.text = UserProfileStorage.UserName;
            _editNameField.ActivateInputField();
            _editNameField.caretPosition = UserProfileStorage.UserName.Length;
        }
    }
}
