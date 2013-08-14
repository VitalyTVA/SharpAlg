using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SharpAlg.Native;
using SharpAlg.Native.Builder;

namespace SharpAlg.Android.UI {
    [Activity(Label = "SharpAlg.Android.UI", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity {
        //int count = 1;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var expressionText = FindViewById<EditText>(Resource.Id.ExpressionTextBox);
            expressionText.TextChanged += expressionText_TextChanged;
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

