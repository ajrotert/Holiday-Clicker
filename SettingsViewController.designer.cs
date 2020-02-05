// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Hackathon
{
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView PickerViewSelect { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SettingsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SwipeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SwitchLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch ViberateSwitch { get; set; }

        [Action ("Slider_Changed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Slider_Changed (UIKit.UISwitch sender);

        void ReleaseDesignerOutlets ()
        {
            if (PickerViewSelect != null) {
                PickerViewSelect.Dispose ();
                PickerViewSelect = null;
            }

            if (SettingsView != null) {
                SettingsView.Dispose ();
                SettingsView = null;
            }

            if (SwipeLabel != null) {
                SwipeLabel.Dispose ();
                SwipeLabel = null;
            }

            if (SwitchLabel != null) {
                SwitchLabel.Dispose ();
                SwitchLabel = null;
            }

            if (ViberateSwitch != null) {
                ViberateSwitch.Dispose ();
                ViberateSwitch = null;
            }
        }
    }
}