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
    class Player : IParcelable
    {

        List<int> Darts = new List<int>();

        public int id { get; set; }
        public string name { get; set; }

        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Player()
        {
            
        }

        public int DescribeContents()
        {
            throw new NotImplementedException();
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}