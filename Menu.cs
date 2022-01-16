using System.Reflection;
using UnityEngine;

namespace abMenuSystem
{
    public abstract class Menu : MonoBehaviour
    {
        public const string MENU_EVENT_SHOWN = "MenuEvent.Shown";
        public const string MENU_EVENT_HIDDEN = "MenuEvent.Hidden";

        public abstract class MenuAnimatorBase
        {
            public abstract void AnimateShowing(System.Action endCallback);
            public abstract void AnimateHiding(System.Action endCallback);

            protected virtual void OnMenuAttached() { }
            protected Menu GetMenu() { return _menu; }

            void AttachMenu(Menu menu)
            {
                _menu = menu;
                OnMenuAttached();
            }

            Menu _menu = null;
        }

        public delegate void OnEventCallback(string eventId, Menu menu);

        public event OnEventCallback MenuChangeEvent;

        public virtual void Show()
        {
            Active = true;

            if (_transitionAnimator != null)
            {
                _transitionAnimator.AnimateShowing(OnShowFinished);
            }
            else
            {
                OnShowFinished();
            }
        }

        public virtual void Hide()
        {
            if (_transitionAnimator != null)
            {
                _transitionAnimator.AnimateHiding(OnHideFinished);
            }
            else
            {
                OnHideFinished();
            }
        }

        public void AttachAnimator<T>() where T : MenuAnimatorBase, new()
        {
            _transitionAnimator = new T();
            if (_transitionAnimator != null)
            {
                var privateAttachMenuMethod = typeof(MenuAnimatorBase).GetMethod("AttachMenu", BindingFlags.NonPublic | BindingFlags.Instance);
                if (privateAttachMenuMethod != null)
                {
                    privateAttachMenuMethod.Invoke(_transitionAnimator, new object[] { this });
                }
            }
        }

        public bool Active
        {
            get
            {
                return gameObject.activeInHierarchy;
            }

            set
            {
                gameObject.SetActive(value);
            }
        }


        protected void Notify(string evId)
        {
            MenuChangeEvent?.Invoke(evId, this);
        }


        void OnShowFinished()
        {
            Notify(MENU_EVENT_SHOWN);
        }

        void OnHideFinished()
        {
            Notify(MENU_EVENT_HIDDEN);
            Active = false;
        }

        MenuAnimatorBase _transitionAnimator = null;
    }
}