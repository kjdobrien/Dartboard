using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.InputMethodServices;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;

namespace Dartboard
{
    class KeyboardListener : Java.Lang.Object, KeyboardView.IOnKeyboardActionListener
    {
        public event EventHandler<KeyboardOnKeyEventArgs> Key;
        public event EventHandler<KeyboardKeyCodeEventArgs> Press;
        public event EventHandler<KeyboardKeyCodeEventArgs> Release;
        public event EventHandler<KeyboardOnTextEventArgs> Text;
        public event EventHandler OnSwipeDown;
        public event EventHandler OnSwipeLeft;
        public event EventHandler OnSwipeRight;
        public event EventHandler OnSwipeUp;

        public const int CodeBackSpace = -5;
        public const int CodeEnter = 55000; 

        private readonly Activity _activity;

        public KeyboardListener(Activity activity)
        {
            _activity = activity;
        }

        public void OnKey(Android.Views.Keycode primaryCode, Android.Views.Keycode[] keyCodes)
        {
            if (Key != null)
                Key(this, new KeyboardOnKeyEventArgs
                {
                    KeyCodes = keyCodes,
                    PrimaryCode = primaryCode

                });

            //View focusCurrent = _activity.Window.CurrentFocus; 
            //EditText edittext = (EditText)focusCurrent;
            //IEditable editable = edittext.EditableText;
            //int start = edittext.SelectionStart;

            //if (primaryCode.ToString() == CodeBackSpace.ToString())
            //{
            //    if (editable != null && start > 0) editable.Delete(start - 1, start);
            //}
            //else if (primaryCode.ToString() == CodeEnter.ToString())
            //{

            //}
            //else
            //{
            //    editable.Insert(start, Character.ToString((char)primaryCode));
            //}
        }

        public void OnPress(Android.Views.Keycode primaryCode)
        {
            if (Press != null)
                Press(this, new KeyboardKeyCodeEventArgs { PrimaryCode = primaryCode });
        }

        public void OnRelease(Android.Views.Keycode primaryCode)
        {
            if (Release != null)
                Release(this, new KeyboardKeyCodeEventArgs { PrimaryCode = primaryCode });
        }

        public void OnText(ICharSequence text)
        {
            if (Text != null)
                Text(this, new KeyboardOnTextEventArgs { Text = text });

        }

        public void SwipeDown()
        {
            if (OnSwipeDown != null)
                OnSwipeDown(this, EventArgs.Empty);
        }

        public void SwipeLeft()
        {
            if (OnSwipeLeft != null)
                OnSwipeLeft(this, EventArgs.Empty);
        }

        public void SwipeRight()
        {
            if (OnSwipeRight != null)
                OnSwipeRight(this, EventArgs.Empty);
        }

        public void SwipeUp()
        {
            if (OnSwipeUp != null)
                OnSwipeUp(this, EventArgs.Empty);
        }
    }

    public class KeyboardOnKeyEventArgs : KeyboardKeyCodeEventArgs
    {
        public Android.Views.Keycode[] KeyCodes { get; set; }
    }

    public class KeyboardKeyCodeEventArgs : EventArgs
    {
        public Android.Views.Keycode PrimaryCode { get; set; }
    }

    public class KeyboardOnTextEventArgs : EventArgs
    {
        public ICharSequence Text { get; set; }
    }

    
}