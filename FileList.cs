using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StorySystem.Editor
{
    public class FileList : MonoBehaviour
    {
        [SerializeField]
        private Transform _itemsContainer;
        [SerializeField]
        private Transform _itemPrefab;

        private SortedList<string, ListItem> _items;

        private Button _addButton;
        private GameObject _previousSelection;
        private Regex _nameRegex;

        private IList<string> Names     => _items.Keys;
        private IList<ListItem> Items   => _items.Values;
        private int ItemsCount          => _items.Count;

        public ListItem SelectedItem { get; private set; }
        public Action<ItemData> OnSelectionChanged { get; set; }

        public void Init()
        {
            _addButton      = GetComponentInChildren<Button>();

            _items          = new();
            _nameRegex      = new(EditorConfig.DEFAULT_ITEM_NAME + " \\d+", RegexOptions.Compiled | 
                RegexOptions.IgnoreCase);

            _addButton.onClick.AddListener(AddNew);

            LoadItems();

            if (ItemsCount == 0)
                AddNew();
            else
                SelectFirst();
        }

        private void LoadItems()
        {
            var items = DataSaveUtility.LoadItems();

            foreach (var item in items)
                Add(item);
        }

        public void AddNew()
        {
            string dataName     = SelectNewName();
            ItemData newData    = new(dataName);

            Add(newData);

            DataSaveUtility.Save(newData);

            SelectLast();
        }

        public void DestroySelected() => SelectedItem.Destroy();

        private string SelectNewName()
        {
            int freeIndex = 0;
            string name;

            do
                name = $"{EditorConfig.DEFAULT_ITEM_NAME} {++freeIndex}";
            while (Contains(name));

            return name;
        }

        private void SelectFirst()  => Select(Items[0]);
        private void SelectLast()   => Select(Items[^1]);

        private void SelectNeighbor(ListItem item)
        {
            var itemIndex   = Items.IndexOf(item);
            var neighbor    = ++itemIndex < ItemsCount ? Items[itemIndex] : Items[itemIndex - 2];

            Select(neighbor);
        }

        private void Select(ListItem item) => item.GetComponent<Selectable>().Select();

        private void Add(ItemData data)
        {
            var item = CreateItem(data);

            _items.Add(data.Name.ToLower(), item);

            item.onClick    .AddListener(() => OnItemClick(item));
            item.onDestroy  .AddListener(() => OnItemDestroy(item));

            data.OnNameChanged  += name =>
            {
                int nameIndex = Items.IndexOf(item);

                _items.Remove(Names[nameIndex]);
                _items.Add(name.ToLower(), item);
            };
        }

        private ListItem CreateItem(ItemData data)
        {
            var instance    = Instantiate(_itemPrefab, _itemsContainer);
            var item        = instance.GetComponent<ListItem>();

            item.Init(data);

            return item;
        }

        private void OnItemClick(ListItem item)
        {
            SelectedItem = item;

            OnSelectionChanged.Invoke(item.Data);
        }

        private void OnItemDestroy(ListItem item)
        {
            if (ItemsCount == 1)
                AddNew();
            else if (item == SelectedItem)
                SelectNeighbor(item);

            _items.Remove(item.Data.Name.ToLower());
        }

        public bool Contains(string name) => Names.Contains(name.ToLower());

        void Update()
        {
            var currentSelection = EventSystem.current.currentSelectedGameObject;

            if (currentSelection != null && currentSelection != _previousSelection)
                SelectionChanged(currentSelection);
        }

        private void SelectionChanged(GameObject selection)
        {
            ListItem selectedListItem;

            if ((selectedListItem = selection.GetComponent<ListItem>()) != null)
            {
                SelectedItem = selectedListItem;

                SelectedItem.onClick.Invoke();
            }
                
            _previousSelection = selection;
        }
    }
}