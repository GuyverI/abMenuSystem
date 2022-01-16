# abMenuSystem

This is a simple menu system for Unity. It's basically a bare minimum of what you need to quickly get started with implementing menus in your project.
The project consists of 3 classes (at least for now...):
- __Menu manager__ - controls the stack of open menus
- __Menu - the base__ class that represents a menu itself
- __Menu Animator__ - a class that controls show/hide animations for menus


## Why use it?
- You don't need to reinvent the wheel. Better use and combine the ready-made solutions to create something useful and unique.
- This library is extensible. You can basically make any menu using this system.
- If this solution doesn't fully fit you, you can use it as a starting point and make changes that you require.

____
## Documentation

### MenuManager
`public delegate void OnMenuEventCallback(string menuId, string menuEvent)` - type of callbacks for handling menu events

`public event OnMenuEventCallback MenuEvent` - for subscription of clients to handle the menu events

`public bool OpenMenu(string id)` - opens a menu and adds it to an internal stack

`public void ShowTopMenu()` - shows the top menu from the internal stack

`public void HideTopMenu()` - hides the top menu without removing it from the internal stack

`public void CloseTopMenu()` - closes the top menu and removes it from the internal stack

`public bool IsActive` - basically checking if the top menu is visible/active

`public string TopMenuId` - ID of the top menu... duh!


### Menu
`public const string MENU_EVENT_SHOWN = "MenuEvent.Shown"` - event that is fired when the menu is shown. Check it in a callback for MenuManager.MenuEvent event.

`public const string MENU_EVENT_HIDDEN = "MenuEvent.Hidden"` - needs a description? really?

`public abstract class MenuAnimatorBase` - derive from that to make a custom animation controller.

`public delegate void OnEventCallback(string eventId, Menu menu)` - come on, you know what it is!

`public event OnEventCallback MenuChangeEvent` - you most likely don't even need to subscribe for that. The Menu Manager does it. So just use the MenuManager.MenuEvent

`public virtual void Show()` - if animator is attached, it's animated

`public virtual void Hide()` - same here

`public void AttachAnimator<T>()` - creates its own animator instance so that this animator can be used only by this menu instance.

`public bool Active` - basically checks the activity of underlying game object

`protected void Notify(string evId)` - it's what you use in your own menu classes to fire events
	
	
### MenuAnimatorBase
`public abstract void AnimateShowing(System.Action endCallback)` - starts animating the show process and calls a callback at the end. Pretty obvious, no?

`public abstract void AnimateHiding(System.Action endCallback)` - same for hiding

`protected virtual void OnMenuAttached()` - override it in order to process the attachment of animator to the menu. Of course only if you need to. No one is forcing you to =)

`protected Menu GetMenu()` - returns the attached menu
