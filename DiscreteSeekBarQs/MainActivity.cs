using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Org.Adw.Library.Widgets;
using static Org.Adw.Library.Widgets.DiscreteSeekBar;
using System;
using Android.Graphics;

namespace DiscreteSeekBarQs
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        DiscreteSeekBar seekBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            seekBar = FindViewById<DiscreteSeekBar>(Resource.Id.discrete3);
            seekBar.SetOnProgressChangeListener(new OnProgressChangeListener
            {
                ProgressChanged = (seekBar, value, fromUser) =>
                {
                    if (((float)value) / seekBar.Max <= 0.6)
                    {
                        seekBar.SetScrubberColor(Color.ParseColor("#CE011A"));
                        seekBar.SetThumbColor(Color.ParseColor("#CE011A"), Color.ParseColor("#CE011A"));
                    }
                    if ((((float)value) / seekBar.Max > 0.6) && (((float)value) / seekBar.Max <= 0.8))
                    {
                        seekBar.SetScrubberColor(Color.ParseColor("#F5A623"));
                        seekBar.SetThumbColor(Color.ParseColor("#F5A623"), Color.ParseColor("#F5A623"));
                    }
                    if (((float)value) / seekBar.Max > 0.8)
                    {
                        seekBar.SetScrubberColor(Color.ParseColor("#007E44"));
                        seekBar.SetThumbColor(Color.ParseColor("#007E44"), Color.ParseColor("#007E44"));
                    }
                }
            });
        }
    }

    class OnProgressChangeListener : Java.Lang.Object, IOnProgressChangeListener
    {
        public Action<DiscreteSeekBar, int, bool> ProgressChanged { get; set; }
        public void OnProgressChanged(DiscreteSeekBar p0, int p1, bool p2)
        {
            ProgressChanged?.Invoke(p0, p1, p2);
        }

        public void OnStartTrackingTouch(DiscreteSeekBar p0)
        {
        }

        public void OnStopTrackingTouch(DiscreteSeekBar p0)
        {
        }
    }
}

