using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.YuyaKaido.CardStackViewLib;
using static Com.YuyaKaido.CardStackViewLib.CardStackView;

namespace CardStackViewExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ProgressBar progressBar;
        private CardStackView cardStackView;
        private TouristSpotCardAdapter adapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Setup();
            await ReloadAsync();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.activity_main, menu);
            return true;
        }

        public override bool  OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_activity_main_reload:
                    ReloadAsync();
                    break;
                case Resource.Id.menu_activity_main_add_first:
                    AddFirst();
                    break;
                case Resource.Id.menu_activity_main_add_last:
                    AddLast();
                    break;
                case Resource.Id.menu_activity_main_remove_first:
                    RemoveFirst();
                    break;
                case Resource.Id.menu_activity_main_remove_last:
                    RemoveLast();
                    break;
                case Resource.Id.menu_activity_main_swipe_left:
                    SwipeLeft();
                    break;
                case Resource.Id.menu_activity_main_swipe_right:
                    SwipeRight();
                    break;
                case Resource.Id.menu_activity_main_reverse:
                    Reverse();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private TouristSpot CreateTouristSpot()
        {
            return new TouristSpot() { Name = "Yasaka Shrine", City = "Kyoto", Url = "https://source.unsplash.com/Xq1ntWruZQI/600x800" };
        }

        private List<TouristSpot> CreateTouristSpots()
        {
            List<TouristSpot> spots = new List<TouristSpot>();
            spots.Add(new TouristSpot() { Name = "Yasaka Shrine", City = "Kyoto", Url = "https://source.unsplash.com/Xq1ntWruZQI/600x800"});
            spots.Add(new TouristSpot() { Name = "Fushimi Inari Shrine", City = "Kyoto", Url = "https://source.unsplash.com/NYyCqdBOKwc/600x800" });
            spots.Add(new TouristSpot() { Name = "Bamboo Forest", City = "Kyoto", Url = "https://source.unsplash.com/buF62ewDLcQ/600x800" }); 
            spots.Add(new TouristSpot() { Name = "Brooklyn Bridge", City = "New York", Url = "https://source.unsplash.com/THozNzxEP3g/600x800" }); 
            spots.Add(new TouristSpot() { Name = "Empire State Building", City = "New York", Url = "https://source.unsplash.com/USrZRcRS2Lw/600x800" }); 
            spots.Add(new TouristSpot() { Name = "The statue of Liberty", City = "New York", Url = "https://source.unsplash.com/PeFk7fzxTdk/600x800" }); 
            spots.Add(new TouristSpot() { Name = "Louvre Museum", City = "Paris", Url = "https://source.unsplash.com/LrMWHKqilUw/600x800" }); 
            spots.Add(new TouristSpot() { Name = "Eiffel Tower", City = "Paris", Url = "https://source.unsplash.com/HN-5Z6AmxrM/600x800" }); 
            spots.Add(new TouristSpot() { Name = "Big Ben", City = "London", Url = "https://source.unsplash.com/CdVAUADdqEc/600x800" }); 
            spots.Add(new TouristSpot() { Name = "Great Wall of China", City = "China", Url = "https://source.unsplash.com/AWh9C-QjhE4/600x800" }); 
            return spots;
        }

        private TouristSpotCardAdapter CreateTouristSpotCardAdapter()
        {
            var  adapter = new TouristSpotCardAdapter(ApplicationContext);
            adapter.AddAll(CreateTouristSpots());
            return adapter;
        }

        private void Setup()
        {
            progressBar = FindViewById<ProgressBar>(Resource.Id.activity_main_progress_bar);
            cardStackView = FindViewById<CardStackView>(Resource.Id.activity_main_card_stack_view);
            cardStackView.SetCardEventListener(new CardEventListener(OnCardDragging, OnCardSwiped, OnCardReversed, OnCardMovedToOrigin, OnCardClicked));
        }

        public void OnCardDragging(float percentX, float percentY)
        {
            Console.WriteLine("CardStackView", "onCardDragging");
        }

        public void OnCardSwiped(SwipeDirection direction)
        {
            Console.WriteLine("CardStackView", "onCardSwiped: " + direction.ToString());
            Console.WriteLine("CardStackView", "topIndex: " + cardStackView.TopIndex);
            if (cardStackView.TopIndex == adapter.Count - 5)
            {
                Console.WriteLine("CardStackView", "Paginate: " + cardStackView.TopIndex);
                Paginate();
            }
        }

        public void OnCardReversed()
        {
            Console.WriteLine("CardStackView", "onCardReversed");
        }

        public void OnCardMovedToOrigin()
        {
            Console.WriteLine("CardStackView", "onCardMovedToOrigin");
        }

        public void OnCardClicked(int index)
        {
            Console.WriteLine("CardStackView", "onCardClicked: " + index);
        }

        private async Task ReloadAsync()
        {
            try
            {
                cardStackView.Visibility = ViewStates.Gone;
                progressBar.Visibility = ViewStates.Visible;

                await Task.Delay(1000);
                adapter = CreateTouristSpotCardAdapter();
                cardStackView.SetAdapter(adapter);
                cardStackView.Visibility = ViewStates.Visible;
                progressBar.Visibility = ViewStates.Gone;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

            }

        }

        private LinkedList<TouristSpot> ExtractRemainingTouristSpots()
        {
            var  spots = new LinkedList<TouristSpot>();
            for (int i = cardStackView.TopIndex; i < adapter.Count; i++)
            {
                spots.AddLast(adapter.GetItem(i));
            }
            return spots;
        }

        private void AddFirst()
        {
            var spots = ExtractRemainingTouristSpots();
            spots.AddFirst(CreateTouristSpot());
            adapter.Clear();
            adapter.AddAll(spots);
            adapter.NotifyDataSetChanged();
        }

        private void AddLast()
        {
            LinkedList<TouristSpot> spots = ExtractRemainingTouristSpots();
            spots.AddLast(CreateTouristSpot());
            adapter.Clear();
            adapter.AddAll(spots);
            adapter.NotifyDataSetChanged();
        }

        private void RemoveFirst()
        {
            LinkedList<TouristSpot> spots = ExtractRemainingTouristSpots();
            if (spots.Count == 0)
            {
                return;
            }

            spots.RemoveFirst();
            adapter.Clear();
            adapter.AddAll(spots);
            adapter.NotifyDataSetChanged();
        }

        private void RemoveLast()
        {
            LinkedList<TouristSpot> spots = ExtractRemainingTouristSpots();
            if (spots.Count ==0)
            {
                return;
            }

            spots.RemoveLast();
            adapter.Clear();
            adapter.AddAll(spots);
            adapter.NotifyDataSetChanged();
        }

        private void Paginate()
        {
            cardStackView.SetPaginationReserved();
            adapter.AddAll(CreateTouristSpots());
            adapter.NotifyDataSetChanged();
        }

        public void SwipeLeft()
        {
            var spots = ExtractRemainingTouristSpots();
            if (spots.Count ==0)
            {
                return;
            }

            View target = cardStackView.TopView;
            View targetOverlay = cardStackView.TopView.GetOverlayContainer();

            ValueAnimator rotation = ObjectAnimator.OfPropertyValuesHolder(
                    target, PropertyValuesHolder.OfFloat("rotation", -10f));

            rotation.SetDuration(200);
            ValueAnimator translateX = ObjectAnimator.OfPropertyValuesHolder(
                    target, PropertyValuesHolder.OfFloat("translationX", 0f, -2000f));
            ValueAnimator translateY = ObjectAnimator.OfPropertyValuesHolder(
                    target, PropertyValuesHolder.OfFloat("translationY", 0f, 500f));
            translateX.StartDelay = 100;
            translateY.StartDelay= 100;
            translateX.SetDuration(500);
            translateY.SetDuration(500);
            AnimatorSet cardAnimationSet = new AnimatorSet();
            cardAnimationSet.PlayTogether(rotation, translateX, translateY);

            ObjectAnimator overlayAnimator = ObjectAnimator.OfFloat(targetOverlay, "alpha", 0f, 1f);
            overlayAnimator.SetDuration(200);
            AnimatorSet overlayAnimationSet = new AnimatorSet();
            overlayAnimationSet.PlayTogether(overlayAnimator);

            cardStackView.Swipe(SwipeDirection.Left, cardAnimationSet, overlayAnimationSet);
        }

        public void SwipeRight()
        {
            var spots = ExtractRemainingTouristSpots();
            if (spots.Count==0)
            {
                return;
            }

            View target = cardStackView.TopView;
            View targetOverlay = cardStackView.TopView.GetOverlayContainer();

            ValueAnimator rotation = ObjectAnimator.OfPropertyValuesHolder(
                    target, PropertyValuesHolder.OfFloat("rotation", 10f));
            rotation.SetDuration(200);
            ValueAnimator translateX = ObjectAnimator.OfPropertyValuesHolder(
                    target, PropertyValuesHolder.OfFloat("translationX", 0f, 2000f));
            ValueAnimator translateY = ObjectAnimator.OfPropertyValuesHolder(
                    target, PropertyValuesHolder.OfFloat("translationY", 0f, 500f));
            translateX.StartDelay = 100;
            translateY.StartDelay = 100;
            translateX.SetDuration(500);
            translateY.SetDuration(500);
            AnimatorSet cardAnimationSet = new AnimatorSet();
            cardAnimationSet.PlayTogether(rotation, translateX, translateY);

            ObjectAnimator overlayAnimator = ObjectAnimator.OfFloat(targetOverlay, "alpha", 0f, 1f);
            overlayAnimator.SetDuration(200);
            AnimatorSet overlayAnimationSet = new AnimatorSet();
            overlayAnimationSet.PlayTogether(overlayAnimator);

            cardStackView.Swipe(SwipeDirection.Right, cardAnimationSet, overlayAnimationSet);
        }

        private void Reverse()
        {
            cardStackView.Reverse();
        }
    }
}

