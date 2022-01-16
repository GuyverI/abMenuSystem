using System;
using System.Collections.Generic;
using UnityEngine;

namespace abMenuSystem
{
    public class MenuManager : MonoBehaviour
    {
        public delegate void OnMenuEventCallback(string menuId, string menuEvent);

        public event OnMenuEventCallback MenuEvent;

        public bool OpenMenu(string id)
        {
            MenuPair newMenuPair = Array.Find(_menuList, menuPair => menuPair.id == id);
            return OpenAndAddMenu(newMenuPair.menu);
        }

        public void ShowTopMenu()
        {
            Menu topMenu = _menuStack.Peek();
            if (topMenu != null)
                topMenu.Show();
        }

        public void HideTopMenu()
        {
            Menu topMenu = _menuStack.Peek();
            if (topMenu != null)
                topMenu.Hide();
        }

        public void CloseTopMenu()
        {
            HideTopMenu();
            _menuStack.Pop();
        }

        public bool IsActive => HasMenus && _menuStack.Peek().Active;

        public string TopMenuId => HasMenus ? GetMenuId(_menuStack.Peek()) : "";



        [Serializable]
        struct MenuPair
        {
            public string id;
            public Menu menu;
        }

        void Awake()
        {
            foreach (MenuPair menuPair in _menuList)
            {
                menuPair.menu.MenuChangeEvent += OnMenuEvent;
                menuPair.menu.Hide();
            }
        }

        string GetMenuId(Menu menu)
        {
            MenuPair foundMenuPair = Array.Find(_menuList, menuPair => menuPair.menu == menu);
            if (foundMenuPair.id != null)
                return foundMenuPair.id;
            return "";
        }

        bool OpenAndAddMenu(Menu newMenu)
        {
            if (newMenu == null)
                return false;

            if (HasMenus)
            {
                Menu curMenu = _menuStack.Peek();
                if (curMenu == newMenu)
                    return false;
            }

            newMenu.Show();
            _menuStack.Push(newMenu);

            return true;
        }

        void OnMenuEvent(string evId, Menu menu)
        {
            MenuEvent?.Invoke(GetMenuId(menu), evId);
        }

        bool HasMenus => _menuStack.Count > 0;

        [SerializeField]
        MenuPair[] _menuList;

        readonly Stack<Menu> _menuStack = new Stack<Menu>();
    }
}