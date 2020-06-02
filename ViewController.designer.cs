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
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ActiveItems_Label { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CandyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Collected_Label { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HelpLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HighScoreLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LevelLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LevelUpLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MainView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RestartButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ScoreLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SettingsButton { get; set; }

        [Action ("CandyButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CandyButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("RestartButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RestartButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ActiveItems_Label != null) {
                ActiveItems_Label.Dispose ();
                ActiveItems_Label = null;
            }

            if (CandyButton != null) {
                CandyButton.Dispose ();
                CandyButton = null;
            }

            if (Collected_Label != null) {
                Collected_Label.Dispose ();
                Collected_Label = null;
            }

            if (HelpLabel != null) {
                HelpLabel.Dispose ();
                HelpLabel = null;
            }

            if (HighScoreLabel != null) {
                HighScoreLabel.Dispose ();
                HighScoreLabel = null;
            }

            if (LevelLabel != null) {
                LevelLabel.Dispose ();
                LevelLabel = null;
            }

            if (LevelUpLabel != null) {
                LevelUpLabel.Dispose ();
                LevelUpLabel = null;
            }

            if (MainView != null) {
                MainView.Dispose ();
                MainView = null;
            }

            if (RestartButton != null) {
                RestartButton.Dispose ();
                RestartButton = null;
            }

            if (ScoreLabel != null) {
                ScoreLabel.Dispose ();
                ScoreLabel = null;
            }

            if (SettingsButton != null) {
                SettingsButton.Dispose ();
                SettingsButton = null;
            }
        }
    }
}