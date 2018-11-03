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

namespace Dartboard
{
    class CustomListViewAdapter : BaseAdapter<string>
    {
        private readonly IList<string> _items;
        private readonly Context _context;
        private readonly Constants.ViewType _typeOfView; 

        public CustomListViewAdapter(Context c, IList<string> items, Constants.ViewType viewType)
        {
            _context = c;
            _items = items;
            _typeOfView = viewType; 
        }

        public override string this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }

        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            var view = convertView;

            if (view == null)
            {
                var inflater = LayoutInflater.FromContext(_context);
                if (_typeOfView == Constants.ViewType.NameListItem)
                {
                    view = inflater.Inflate(Resource.Layout.name_list_item, parent, false);
                }
                else if(_typeOfView == Constants.ViewType.SuggestedName)
                {
                    view = inflater.Inflate(Resource.Layout.suggested_name, parent, false);
                }
                
            }

            view.FindViewById<TextView>(Resource.Id.itemText).Text = item;

            return view;
        }
    }
}