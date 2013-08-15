using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SharpAlg.Native;
using SharpAlg.Native.Builder;
using Android.InputMethodServices;
using Android.Views.InputMethods;

namespace SharpAlg.Android.UI {
    [Activity(Label = "SharpAlg.Android.UI", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity {
        //int count = 1;
        KeyboardView mKeyboardView;
        EditText expressionText;
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            //Keyboard mKeyboard = new Keyboard(this, Resource.Xml.hexkbd);

            //// Lookup the KeyboardView
            //mKeyboardView = (KeyboardView)FindViewById(Resource.Id.keyboardview);
            //// Attach the keyboard to the view
            //mKeyboardView.Keyboard = mKeyboard;
            //// Do not show the preview balloons
            //mKeyboardView.PreviewEnabled = false;

            //mKeyboardView.OnKeyboardActionListener = new MyKeyboardActionListener(mKeyboardView.Handle);

            Window.SetSoftInputMode(SoftInput.StateHidden);

            expressionText = FindViewById<EditText>(Resource.Id.ExpressionTextBox);
            expressionText.TextChanged += expressionText_TextChanged;
            expressionText.Click +=expressionText_Click;
            //expressionText.FocusChange +=expressionText_FocusChange;
        }

        void expressionText_FocusChange(object sender, View.FocusChangeEventArgs e) {
            if( e.HasFocus )
                showCustomKeyboard(expressionText); 
            else 
                hideCustomKeyboard();
        }

        void expressionText_Click(object sender, EventArgs e) {
            showCustomKeyboard(expressionText);
        }
        public void hideCustomKeyboard() {
            mKeyboardView.Visibility = ViewStates.Gone;
            mKeyboardView.Enabled = false;
        }

        public void showCustomKeyboard(View v) {
            //mKeyboardView.Visibility = ViewStates.Visible;
            //mKeyboardView.Enabled = true;
            if(v != null) ((InputMethodManager)GetSystemService(Activity.InputMethodService)).HideSoftInputFromWindow(v.WindowToken, 0);
        }

        public bool isCustomKeyboardVisible() {
            return mKeyboardView.Visibility == ViewStates.Visible;
        }
        void expressionText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e) {
            var expressionText = FindViewById<EditText>(Resource.Id.ExpressionTextBox);
            var resultText = FindViewById<TextView>(Resource.Id.ResultText);

            var builder = ExprBuilderFactory.CreateDefault();
            var parser = expressionText.Text.ParseCore(builder);
            if(parser.errors.Count > 0) {
                resultText.Text = parser.errors.Errors;
            } else {
                resultText.Text = parser.Expr.Print();
            }
        }
    }
    class MyKeyboardActionListener : Java.Lang.Object, KeyboardView.IOnKeyboardActionListener {
        private IntPtr intPtr;

        public MyKeyboardActionListener(IntPtr intPtr) {
            this.intPtr = intPtr;
        }
        void KeyboardView.IOnKeyboardActionListener.OnKey(global::Android.Views.Keycode primaryCode, global::Android.Views.Keycode[] keyCodes) {

        }

        void KeyboardView.IOnKeyboardActionListener.OnPress(global::Android.Views.Keycode primaryCode) {
        }

        void KeyboardView.IOnKeyboardActionListener.OnRelease(global::Android.Views.Keycode primaryCode) {
        }

        void KeyboardView.IOnKeyboardActionListener.OnText(Java.Lang.ICharSequence text) {
        }

        void KeyboardView.IOnKeyboardActionListener.SwipeDown() {
        }

        void KeyboardView.IOnKeyboardActionListener.SwipeLeft() {
        }

        void KeyboardView.IOnKeyboardActionListener.SwipeRight() {
        }

        void KeyboardView.IOnKeyboardActionListener.SwipeUp() {
        }
    }
}

