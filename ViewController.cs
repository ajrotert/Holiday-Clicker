using Foundation;
using System;
using System.Threading;
using UIKit;
using CoreGraphics;

namespace Hackathon
{
    struct settings
    {
        public static bool Viberate;
        public static int SelectedMonth;
        public static int HighScore;
        public static bool Help;
    };
    public partial class ViewController : UIViewController
    { 
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        //Default values
        public Random r;
        public int score;
        public bool ended;
        public CGRect rect;
        string background = "HalloweenBKG.png";
        string falling = "Bat.png";
        Thread GameThread;
        int GameObjects = 1000;
        int ObjectSpeed = 200;
        int ActiveItems = 0;
        int CollectedItems = 0;
        int level = 0;

        UIImpactFeedbackGenerator heavyFeedback;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Settings are loaded from database, and set
            DatabaseManagement.SetSettings();
            HighScoreLabel.Text += settings.HighScore;

            start();
        }
        private void start()
        { //Sets default game values, and updates labels. 
            SetBackground();
            RestartButton.Hidden = true;
            // Perform any additional setup after loading the view, typically from a nib.
            if (settings.HighScore < 100)
            {
                HelpLabel.Text = "Tap the objects before" + Environment.NewLine + "they reach the bottom.";
                settings.Help = true;
            }
            else
                settings.Help = false;

            HelpLabel.Hidden = !(settings.Help);
            r = new Random();
            level = 1;
            score = 0;
            CollectedItems = 0;
            ActiveItems = 0;
            ScoreLabel.Text = "Score: " + score;
            Collected_Label.Text = "Collected: " + CollectedItems;
            ActiveItems_Label.Text = "Active: " + ActiveItems;
            LevelUpLabelUpdate();

            ended = false;

            ObjectSpeed = Decay();

            rect = new CGRect(CandyButton.Frame.X, CandyButton.Frame.Y, CandyButton.Frame.Width, CandyButton.Frame.Height);
            CandyButton.Hidden = true;
            heavyFeedback = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Heavy);
            heavyFeedback.Prepare();

            GameThread = new Thread(Game);
            GameThread.Start();
        }
        private void SetBackground()
        {
            int month = settings.SelectedMonth;
            if (month == 0)
                month = DateTime.Now.Date.Month;
            if (month == 1)
            {
                background = "NewYearBKG.png";
                falling = "FireworksNY.png";
            }
            else if (month == 2)
            {
                background = "ValentinesBKG.png";
                falling = "Chocolates.png";
            }
            else if (month == 3)
            {
                background = "STPatrickBKG.png";
                falling = "Leprechaun.png";
            }
            else if (month == 4)
            {
                background = "EasterBKG.png";
                falling = "Bunny.png";
            }
            else if (month == 5)
            {
                background = "MemorialDayBKG.png";
                falling = "BulletCase.png";
            }
            else if (month == 6)
            {
                background = "SummerBKG.png";
                falling = "Sunglasses.png";
            }
            else if (month == 7)
            {
                background = "IndependenceDay.png";
                falling = "Flag.png";
            }
            else if (month == 8)
            {
                background = "BackToSchoolBKG.png";
                falling = "Bus.png";
            }
            else if (month == 9)
            {
                background = "LaborDayBKG.png";
                falling = "Hammer.png";
            }
            else if (month == 10)
            {
                background = "HalloweenBKG.png";
                falling = "Bat.png";
            }
            else if (month == 11)
            {
                background = "ThanksgivingBKG.png";
                falling = "Turkey.png";
            }
            else if (month == 12)
            {
                background = "ChristmasBKG.png";
                falling = "Present.png";
            }
            else if (month == 13)
            {
                background = "NeutralBKG.png";
                falling = "Neutral.png";
            }
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile(background));
        }
        public void Game()
        { //Main game thread, used to generate falling objects
            while (!ended)
            {
                Thread thread = new Thread(DropButton);
                thread.Start();
                ActiveItems++;
                InvokeOnMainThread(delegate
                {
                    ActiveItems_Label.Text = "Active: " + ActiveItems;
                });
                Thread.Sleep(GameObjects);
                LevelCheck();
            }
            InvokeOnMainThread(delegate
            {
                 ScoreLabel.Text = "Ended: " + score;
                RestartButton.Hidden = false;
            });
        }

        internal void DropButton()
        { //Handles falling objects, objects move randomly until their bounds are outside the phones frame. 
            UIButton button = null;
            InvokeOnMainThread(delegate
            {
                button = new UIButton(rect);
                button.SetBackgroundImage(UIImage.FromFile(falling), UIControlState.Normal);
                button.TouchDragInside += (sender, e) => CandyButton_TouchUpInside((UIButton)sender);
                button.TouchUpInside += (sender, e) => CandyButton_TouchUpInside((UIButton)sender);
                button.Center = CandyButton.Center;
                View.AddSubview(button);
            });

            nfloat bottom1 = 0, bottom2 = 0, lside=0, rside=0, lside2=0, rside2=0;

            InvokeOnMainThread(delegate
            {
                bottom1 = UIScreen.MainScreen.Bounds.Bottom;
                bottom2 = button.Frame.Top;
                lside = UIScreen.MainScreen.Bounds.Left;
                rside = UIScreen.MainScreen.Bounds.Right;
            });


            while (bottom1 > bottom2)
            {
                int ychange = r.Next(25), x = r.Next(ObjectSpeed), xchange = r.Next(75), neg = r.Next(2);
                if (neg == 0)
                    xchange *= -1;
                InvokeOnMainThread(delegate
                {

                CGRect Loc_rect = new CGRect(button.Frame.X + xchange, button.Frame.Y + ychange, button.Frame.Width, button.Frame.Height);

                UIView.Animate(0.5, options: UIViewAnimationOptions.AllowUserInteraction, animation: () => {
                    button.Frame = Loc_rect;
                    button.UpdateConstraints();
                }, completion: () => { }, delay: 0);

                    bottom2 = button.Frame.Top;
                 });
                Thread.Sleep(x);
            }
            Console.WriteLine("Ended");
            InvokeOnMainThread(delegate
            {
                lside2 = button.Frame.Left;
                rside2 = button.Frame.Right;

                    ActiveItems--;
                    ActiveItems_Label.Text = "Active: " + ActiveItems;
                
                if (!button.Hidden && rside2 > lside && lside2 < rside)
                {
                    ended = true;
                    ObjectSpeed = 1;
                    if (score > settings.HighScore)
                    {
                        HighScoreLabel.Text = "High Score: " + score;
                        settings.HighScore = score;
                        DatabaseManagement.UpdateData();
                    }
                }

            });
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void CandyButton_TouchUpInside(UIButton sender)
        { //Called when falling objects are clicked, called repeatedly when objects are click and dragged. 
            try
            {
                if(settings.Viberate)
                    heavyFeedback.ImpactOccurred();
            }
            catch { Console.WriteLine("Vibrate not supported"); }
            int a = r.Next(40);
            score += a;
            ScoreLabel.Text = "Score: " + score;
            if (!sender.Hidden)
            {
                CollectedItems++;
                Collected_Label.Text = "Collected: " + CollectedItems;
            }
            sender.Hidden = true;
        }

        private int Decay()
        { //Exponential decay function used to calculate falling objects speed. 
            double x = (Math.Pow(0.95, level) * 400) + 50;
            return Convert.ToInt32(x);
        }
        private void LevelCheck()
        { //Used to check and update users current level.
            int x = score / 1000;

            if(x > level)
            {
                level = x;
                ObjectSpeed = Decay();
                LevelUpLabelUpdate();
                LevelUp();
            }
        }
        private void LevelUp()
        { //Animates level up for the user. 
            InvokeOnMainThread(delegate
            {
                LevelUpLabel.Alpha = 1;
                UIView.AnimateAsync(2, animation: () => { LevelUpLabel.Alpha = 0; });
            });

        }
        private void LevelUpLabelUpdate()
        { //Updates side level label.
            InvokeOnMainThread(delegate
            {
                LevelLabel.Text = "Level: " + level;
            });
        }
        partial void RestartButton_TouchUpInside(UIButton sender)
        {
            GameThread.Abort();
            start();
        }
    }
}
