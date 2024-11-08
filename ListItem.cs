using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StorySystem.Editor
{
    [Serializable]
    public class ListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector4 _defaultTextAreaPadding     = new(0, 0, -60);
        private Vector4 _highlitedTextAreaPadding   = Vector4.zero;

        [SerializeField]
        private Button _destroyButton;
        [SerializeField]
        private Button _selectButton;

        [SerializeField]
        private TMP_Text _nameText;
        [SerializeField]
        private RectMask2D _textMask;

        public ItemData Data { get; private set; }

        public Button.ButtonClickedEvent onDestroy  => _destroyButton.onClick;
        public Button.ButtonClickedEvent onClick    => _selectButton.onClick;

        public void Init(ItemData data)
        {
            if (Data != null)
                throw new Exception("ListItem already initialized");

            _nameText.text = data.Name;

            data.OnNameChanged += name => _nameText.text = name;

            _destroyButton  .onClick.AddListener(Destroy);

            Data = data;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.D))
                Destroy(gameObject);
        }

        public void Destroy()
        {
            DataSaveUtility.Delete(Data);

            onDestroy   .RemoveAllListeners();
            onClick     .RemoveAllListeners();

            GameObject.Destroy(gameObject);
        }

        //Это вынести в настрйоки префаба
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _destroyButton.gameObject.SetActive(true);

            _textMask.padding = _highlitedTextAreaPadding;
        }

        //И это тоже
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _destroyButton.gameObject.SetActive(false);

            _textMask.padding = _defaultTextAreaPadding;
        }
    }
}