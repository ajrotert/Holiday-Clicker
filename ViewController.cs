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
        UIImage background = UIImage.FromBundle("HalloweenBKG");
        UIImage falling = UIImage.FromBundle("Bat");
        Thread GameThread;
        int GameObjects = 1000;
        int ObjectSpeed = 200;
        int ActiveItems = 0;
        int CollectedItems = 0;
        int level = 0;
        public static bool isStarted;

        UIImpactFeedbackGenerator heavyFeedback;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Settings are loaded from database, and set
            DatabaseManagement.SetSettings();
            HighScoreLabel.Text += settings.HighScore;

        }
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (settings.Help)
            {
                PerformSegue("InstructionSeuge", new NSObject());
            }
            else
            {
                isStarted = true;
            }

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
                background = UIImage.FromBundle("NewYearBKG");
                falling = UIImage.FromBundle("FireworksNY");
            }
            else if (month == 2)
            {
                background = UIImage.FromBundle("ValentinesBKG");
                falling = UIImage.FromBundle("Chocolates");
            }
            else if (month == 3)
            {
                background = UIImage.FromBundle("STPatrickBKG");
                falling = UIImage.FromBundle("Leprechaun");
            }
            else if (month == 4)
            {
                background = UIImage.FromBundle("EasterBKG");
                falling = UIImage.FromBundle("Bunny");
            }
            else if (month == 5)
            {
                background = UIImage.FromBundle("MemorialDayBKG");
                falling = UIImage.FromBundle("BulletCase");
            }
            else if (month == 6)
            {
                background = UIImage.FromBundle("SummerBKG");
                falling = UIImage.FromBundle("Sunglasses");
            }
            else if (month == 7)
            {
                background = UIImage.FromBundle("IndependenceDay");
                falling = UIImage.FromBundle("Flag");
            }
            else if (month == 8)
            {
                background = UIImage.FromBundle("BackToSchoolBKG");
                falling = UIImage.FromBundle("Bus");
            }
            else if (month == 9)
            {
                background = UIImage.FromBundle("LaborDayBKG");
                falling = UIImage.FromBundle("Hammer");
            }
            else if (month == 10)
            {
                background = UIImage.FromBundle("HalloweenBKG");
                falling = UIImage.FromBundle("Bat");
            }
            else if (month == 11)
            {
                background = UIImage.FromBundle("ThanksgivingBKG");
                falling = UIImage.FromBundle("Turkey");
            }
            else if (month == 12)
            {
                background = UIImage.FromBundle("ChristmasBKG");
                falling = UIImage.FromBundle("Present");
            }
            else if (month == 13)
            {
                background = UIImage.FromBundle("NeutralBKG");
                falling = UIImage.FromBundle("Neutral");
            }
            View.BackgroundColor = UIColor.FromPatternImage(background);
        }
        public void Game()
        { //Main game thread, used to generate falling objects
            while (!ended)
            {
                if (isStarted)
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
                button.SetBackgroundImage(falling, UIControlState.Normal);
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
                if (isStarted)
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
                    if(settings.Help && settings.HighScore > 100)
                    {
                        settings.Help = false;
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
