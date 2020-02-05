using Foundation;
using System;
using UIKit;

namespace Hackathon
{
    public partial class SettingsViewController : UIViewController
    {
        public OptionsListDataModel OLDM;
        private const int HighValue = 10000;

        public SettingsViewController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            OLDM = new OptionsListDataModel(new UILabel());
            PickerViewSelect.Model = OLDM;
            ViberateSwitch.On = settings.Viberate;
        }
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            PickerViewSelect.Model.Selected(PickerViewSelect, 0, 0);
            if (OLDM.selectedIndex != 0)
            {
                if (OLDM.selectedIndex - 1 == 13)
                {
                    if (settings.HighScore >= HighValue)
                    {
                        settings.SelectedMonth = OLDM.selectedIndex - 1;
                        DatabaseManagement.UpdateData();
                    }
                    else
                    {
                        SwipeLabel.Text = "High Score must be greater than: " + HighValue;
                        SwipeLabel.TextColor = UIColor.Red;
                    }
                }
                else
                {
                    settings.SelectedMonth = OLDM.selectedIndex - 1;
                    SwipeLabel.Text = "Swipe Down to Set";
                    SwipeLabel.TextColor = UIColor.SystemPurpleColor;
                    Console.WriteLine("{0}", OLDM.selectedIndex);
                    DatabaseManagement.UpdateData();
                }
            }
        }

        partial void Slider_Changed(UISwitch sender)
        {
            if(ViberateSwitch.On)
            {
                //set viberate to be active
                settings.Viberate = true;
            }
            else
            {
                //set viberate to be inactive
                settings.Viberate = false;
            }
            DatabaseManagement.UpdateData();
        }
    }

    public class OptionsListDataModel : UIPickerViewModel
    {
        public string[] listNames = new string[15] {
        "Current Theme: " + GetMonth(),
        "Automatic",
        "January",
        "February",
        "March",
        "April",                                         
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December",
        "Promotional"};

        private UILabel title;

        public OptionsListDataModel(UILabel title)
        {
            this.title = title;
        }
        public string selected;
        public int selectedIndex;

        private static string GetMonth()
        {
            string[] listMonth =
                { "Auto", "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec", "Promo" };
            return listMonth[settings.SelectedMonth];
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return listNames.Length;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (component == 0)
                return listNames[row];
            else
                return row.ToString();
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            //personLabel.Text = $"This person is: {names[pickerView.SelectedRowInComponent(0)]},\n they are number {pickerView.SelectedRowInComponent(1)}";
            selected = listNames[pickerView.SelectedRowInComponent(0)]; //0 is text, 1 is integer
            selectedIndex = (int)pickerView.SelectedRowInComponent(0);
        }

        public override nfloat GetComponentWidth(UIPickerView pickerView, nint component) //not sure what these do
        {
            if (component == 0)
                return 240f;
            else
                return 40f;
        }

        public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
        {
            return 40f;
        }


    }
}
