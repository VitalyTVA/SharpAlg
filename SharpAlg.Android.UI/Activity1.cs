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
        EditText expressionText;
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Window.SetSoftInputMode(SoftInput.StateHidden);

            expressionText = FindViewById<EditText>(Resource.Id.ExpressionTextBox);
            expressionText.TextChanged += expressionText_TextChanged;
            expressionText.Click +=expressionText_Click;
            expressionText.FocusChange +=expressionText_FocusChange;

            var button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += button_Click;

            var button2 = FindViewById<Button>(Resource.Id.button2);
            button2.Click += button1_Click;

        }

        void button1_Click(object sender, EventArgs e) {
            Insert("x");
        }

        void button_Click(object sender, EventArgs e) {
            Insert("y");
        }

        void Insert(string s) {
            int selStart = expressionText.SelectionStart;
            string text = expressionText.Text.Insert(selStart, s);
            expressionText.EditableText.Insert(expressionText.SelectionStart, new Java.Lang.String(s));
        }

        void expressionText_FocusChange(object sender, View.FocusChangeEventArgs e) {
            if( e.HasFocus )
                showCustomKeyboard(expressionText); 
        }

        void expressionText_Click(object sender, EventArgs e) {
            showCustomKeyboard(expressionText);
        }

        public void showCustomKeyboard(View v) {
            //if(v != null) ((InputMethodManager)GetSystemService(Activity.InputMethodService)).HideSoftInputFromWindow(v.WindowToken, 0);
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
}

