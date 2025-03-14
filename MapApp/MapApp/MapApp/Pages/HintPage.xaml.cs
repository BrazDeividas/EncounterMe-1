﻿using EncounterMe;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncounterMe.Functions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using System.Threading;
using Xamarin.Forms.DetectUserLocationCange.Services;
using MapApp.Hints;
using Rg.Plugins.Popup.Services;

namespace MapApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HintPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        MainPage main;
        public Location locationToFind;
        GameLogic gameLogic;

        List<IHint> HintList = new List<IHint>();

        private int stackWidth = 0;
        private int bubbleWidth = 16;
        private int currentPosition = 0;
        private int hintNum = 0;

        HintCompass hintCompass;
        int posCompass = 0;
        ShrinkSearchCircle hintCircle;
        HintDistance hintDistance;
        int posDistance = 0;

        public int exp;
        public bool selected = false;
        public HintPage(MainPage main, Location loc, int exp)
        {
            this.main = main;
            this.locationToFind = loc;
            this.exp = exp;

            gameLogic = new GameLogic();
            InitializeComponent();
            var leftSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Left};
            var rightSwipe = new SwipeGestureRecognizer { Direction = SwipeDirection.Right };

            leftSwipe.Swiped += OnSwiped;
            rightSwipe.Swiped += OnSwiped;

            //hintImage.GestureRecognizers.Add(leftSwipe);
            //hintImage.GestureRecognizers.Add(rightSwipe);

            shade.GestureRecognizers.Add(leftSwipe);
            shade.GestureRecognizers.Add(rightSwipe);

            UpdateExperienceButton();
        }

        private void UpdateNavigation() 
        {
            var unselectedIcons = stackWidth / bubbleWidth;
            stackWidth += bubbleWidth;
            currentPosition += 1;
            UpdateStack();
        }

        public void UpdateExperienceButton()
        {
            Experience.Text = exp.ToString() + " EXP";
            if (exp >= 1000)
            {
                Experience.BackgroundColor = Color.FromHex("#5bd963");
            }
            else if(exp < 1000 && exp > 100)
            {
                Experience.BackgroundColor = Color.FromHex("#EC9F2B");
            }
            else
            {
                Experience.BackgroundColor = Color.FromHex("#DA4167");
            }
        }

        private void UpdateStack()
        {
            if (currentPosition > 4)
            {
                return;
            }

            horizontalStack.Children.Clear();
            horizontalStack.WidthRequest = stackWidth;

            if(currentPosition == 0)
            {
                currentPosition++;
            }

            for (int i = 0; i < stackWidth / bubbleWidth; i++)
            {
                if (currentPosition == i + 1)
                {
                    horizontalStack.Children.Add(new Image
                    {
                        Source = "navigation_selected.png",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Aspect = Aspect.Fill,
                        IsVisible = true,
                    });
                }
                else
                {
                    Debug.WriteLine(i);
                    horizontalStack.Children.Add(new Image
                    {
                        Source = "navigation_unselected.png",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Aspect = Aspect.Fill,
                        IsVisible = true,
                    });
                }
            }

            //debugText.Text = currentPosition.ToString();
        }
        
        public int CalculateExp(int perc)
        {
            int minusexp = exp * perc / 100;

            if (exp - minusexp < 100)
                return 0;

            if (minusexp >= 1000)
            {
                return minusexp = (minusexp / 100) * 100;
            }
            else 
            {
                return minusexp = (minusexp / 10) * 10;
            }
        }

        private async void CheckMarkOneTapped(object sender, EventArgs e)
        {
            //CompassHint

            if (hintCompass == null)
            {
                int minusExp = CalculateExp(30);
                await Navigation.PushPopupAsync(
                    new Notification.NotificationPage(
                        this, 
                        minusExp,
                        "Compass Hint",
                        "Compass hint will help you by showing the direction to the location.",
                        "hamcomp1.png"));
            }
            else
            {
                UpdateHint(posCompass);
                currentPosition = posCompass;
                UpdateStack();
            }
        }

        public void ActivateCompass(int minusExp)
        {
            checkMarkOne.Source = "compass.png";
            hintCompass = new HintCompass(this, gameLogic);
            updateCheckMark(hintCompass);
            posCompass = hintNum;

            UpdateHint(posCompass);
            currentPosition = posCompass;
            UpdateStack();

            exp = exp - minusExp;
            UpdateExperienceButton();
        }


        private async void CheckMarkTwoTapped(object sender, EventArgs e)
        {
            //DistanceHint

            if (hintDistance == null)
            {
                int minusExp = CalculateExp(20);

                await Navigation.PushPopupAsync(
                    new Notification.NotificationPage(
                        this,
                        minusExp,
                        "Distance Hint",
                        "Distance hint will become brighter the closer you are to the location.",
                        "hamorg1.png"));
            }
            else
            {
                UpdateHint(posDistance);
                currentPosition = posDistance;
                UpdateStack();
            }
        }

        public void ActivateDistance(int minusExp)
        {
            checkMarkTwo.Source = "bubble.png";
            hintDistance = new HintDistance(this, gameLogic);
            updateCheckMark(hintDistance);
            posDistance = hintNum;

            UpdateHint(posDistance);
            currentPosition = posDistance;
            UpdateStack();

            exp = exp - minusExp;
            UpdateExperienceButton();
        }
        private async void CheckMarkThreeTapped(object sender, EventArgs e)
        {
            //Shrink Circle hint

            if (hintCircle == null)
            {
                checkMarkThree.Source = "shrink.png";
                hintCircle = new ShrinkSearchCircle(main, this, locationToFind.Latitude, locationToFind.Longtitude);
                await Navigation.PopPopupAsync();
                await Navigation.PushPopupAsync(hintCircle);
            }
            else
            {
                hintCircle.Update();
                await Navigation.PopPopupAsync();
                await Navigation.PushPopupAsync(hintCircle);
            }
        }

        /*
        private async void CheckMarkFourTapped(object sender, EventArgs e)
        {
            initCheck();
            updateCheckMark(new HintCircle(this), checkMarkFour);
            //TODO: fourth Hint
        }
        */

        //I have no idea what it does, but now it works without it(?)
        private void initCheck()
        {
            if (currentPosition != 0)
            {
                HintList[currentPosition - 1].hideHint(this);
            }
        }

        
        private void updateCheckMark(IHint hint)
        {
            //for location's picture
            if (currentPosition == 0)
            {
                //HintList.Insert(currentPosition, new ShowPicture(this, locationToFind));
                hintNum++;
                hintImage.IsEnabled = false;
                HintList.Add(new ShowPicture(this, locationToFind));
                UpdateNavigation();
            }
            hintNum++;
            HintList.Add(hint);
            UpdateNavigation();
            //UpdateStack();
        }
        
        private void Test(object sender, EventArgs e)
        {
            //var locator = CrossGeolocator.Current;
            //locator.DesiredAccuracy = 50;
            //var myPosition = await locator.GetPositionAsync();

            //LocationUpdateService = DependencyService.Get<ILocationUpdateService>();

            ////main.userPosition = new Position(myPosition.Latitude, myPosition.Longitude);
            //debugText.Text = myPosition.Latitude.ToString();
        }


        void OnSwiped(object sender, SwipedEventArgs e)
        {
            //horizontalStack.Children.Clear();
            /* ???what
            if (HintList.Count() < 2)
            {
                //return;
            }
            */
            
            //debugText.Text = e.Direction.ToString();
            int newPosition;
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    newPosition = PositionBoundsCheck(1);
                    //checks if we need to update hint
                    if (newPosition != currentPosition)
                    {
                        UpdateHint(newPosition);
                        currentPosition = newPosition;
                        UpdateStack();
                    }
                    break;
                case SwipeDirection.Right:
                    newPosition = PositionBoundsCheck(-1);
                    //checks if we need to update hint
                    if (newPosition != currentPosition)
                    {
                        UpdateHint(newPosition);
                        currentPosition = newPosition;
                        UpdateStack();
                    }
                    break;
                case SwipeDirection.Up:
                    // Handle the swipe
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    break;
            }
            //debugText.Text = currentPosition.ToString();

        }

        void UpdateHint(int newPostion)
        {
            HintList[currentPosition - 1].hideHint(this);
            HintList[newPostion - 1].show(this);
            //debugText.Text = currentPosition.ToString() + " " + newPostion.ToString() + " " + HintList[newPostion - 1].ToString();
        }
        int PositionBoundsCheck(int change)
        {

            if (((currentPosition + change) <= stackWidth / bubbleWidth) && (currentPosition + change) > 0)
            {
                var newPosition = currentPosition + change;
                return newPosition;
            }

            return currentPosition;
        }

        private async void GoBack(object sender, EventArgs e)
        {
            //function to close temporary HintPage
            await Navigation.PopPopupAsync();
        }
        public async void GiveUp(object sender, EventArgs e)
        {
            //function to go back to location search
            main.ChangeSearchRadius(0);
            main.ChangeButtonToSearchForEncounter(sender, e);
            await Navigation.PopPopupAsync();
        }
        async void LocationFound(object sender, EventArgs e)
        {
            //main.ChangeSearchRadius(0); 
            //main.ChangeButtonToSearchForEncounter(sender, e);
            GiveUp(sender, e);
            await Navigation.PushPopupAsync(new Notification.NotificationWin(exp));
            
            
        }
    }
}