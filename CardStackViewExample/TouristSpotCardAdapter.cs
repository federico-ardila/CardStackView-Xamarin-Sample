using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;

namespace CardStackViewExample
{
    class TouristSpotCardAdapter : ArrayAdapter<TouristSpot>
    {
        public TouristSpotCardAdapter(Context context):base(context,0)
        {

        }

        
    public override  View GetView(int position, View contentView, ViewGroup parent)
        {
            ViewHolder holder;

            if (contentView == null)
            {
                LayoutInflater inflater = LayoutInflater.From(Context);
                contentView = inflater.Inflate(Resource.Layout.item_tourist_spot_card, parent, false);
                holder = new ViewHolder(contentView);
                contentView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)contentView.Tag;
            }

            TouristSpot spot = GetItem(position);

            holder.Name.Text = spot.Name;
            holder.City.Text = spot.City;
            Glide.With(Context).Load(spot.Url).Into(holder.Image);

            return contentView;
        }

    }

    class ViewHolder : Java.Lang.Object
    {
        public TextView Name;
        public TextView City;
        public ImageView Image;

        public ViewHolder(View view)
        {
            Name = view.FindViewById<TextView>(Resource.Id.item_tourist_spot_card_name);
            City = view.FindViewById<TextView>(Resource.Id.item_tourist_spot_card_city);
            Image = view.FindViewById<ImageView>(Resource.Id.item_tourist_spot_card_image);
        }
    }
}