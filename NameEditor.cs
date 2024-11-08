using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace StorySystem.Editor
{
    public class NameEditor : MonoBehaviour
    {
        private const byte MAX_NAME_LENGHT = 64;

        private TMP_InputField _nameInputField;
        private TMP_Text _nameText;

        private FileList _fileList;
        private ItemData _curentData;

        private RectTransform _textRect;
        private RectTransform _caretRect;

        private Regex _nameRegex;

        private Color _defaultTextColor;
        private Color _wrongTextColor;
        private Color NameColor { get => _nameText.color; set => _nameText.color = value; }

        private string DataText { get => _curentData.Name; set => _curentData.Name = value; }
        private string CurrentName { get => _nameInputField.text; set => _nameInputField.text = value; }
        private string _prevName;

        private bool IsNameColorWrong => NameColor != _defaultTextColor;

        public void Init(FileList fileList)
        {
            _nameInputField     = GetComponent<TMP_InputField>();
            _nameText           = _nameInputField.textComponent;
            _fileList           = fileList;
            _textRect           = _nameText.GetComponent<RectTransform>();
            _caretRect          = _nameInputField.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            
            _nameRegex          = new(@"(\w+)", RegexOptions.Compiled);

            _fileList.OnSelectionChanged += OnSelectionChanged;

            _nameInputField.onValueChanged  .AddListener(OnNameChanged);
            _nameInputField.onEndEdit       .AddListener(OnNameEditEnd);
            _nameInputField.onDeselect      .AddListener(_ => OnDeselect());

            _defaultTextColor   = NameColor;
            _wrongTextColor     = Color.red;
        }

        private void OnSelectionChanged(ItemData data)
        {
            _curentData             = data;
            _prevName = CurrentName = data.Name;
        }

        private void OnNameChanged(string text)
        {
            text = text.ToLower();

            if (text.Length > MAX_NAME_LENGHT)
                CurrentName = text.Remove(MAX_NAME_LENGHT);

            if (DataText.ToLower() != text && IsWrongName(text))
            {
                if (!IsNameColorWrong)
                    SetWrongNameColor();

                return;
            }
            
            if (IsNameColorWrong)
                SetDefaultNameColor();
        }

        private void OnNameEditEnd(string text)
        {
            if (_prevName == text)
                return;

            if (IsWrongName(text))
            {
                CurrentName = DataText;

                return;
            }

            DataText = text;

            DataSaveUtility.ChangeName(_curentData, _prevName);

            _prevName = text;
        }

        private void OnDeselect()
        {
            _textRect.anchoredPosition = _caretRect.anchoredPosition = Vector2.zero;

            if (IsNameColorWrong)
                SetDefaultNameColor();
        }

        private bool IsWrongName(string name) => _fileList.Contains(name) || !_nameRegex.IsMatch(name);

        private void SetWrongNameColor()    => NameColor = _wrongTextColor;
        private void SetDefaultNameColor()  => NameColor = _defaultTextColor;
    }
}