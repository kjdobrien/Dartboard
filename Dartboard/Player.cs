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

using Java.Interop;



namespace Dartboard
{
    class Player : Java.Lang.Object, IParcelable
    {

        public Player()
        {
            Darts.Add(d1);
            Darts.Add(d2);
            Darts.Add(d3);
        }

        public int id { get; set; }
        public string name { get; set; }
        public int score { get; set; }

        public List<int> Darts = new List<int>();
        public int d1, d2, d3;


        #region IParcelable implementation 
        public static readonly GenericParcelableCreator<Player> _creator = new GenericParcelableCreator<Player>((parcel) => new Player(parcel));

        [ExportField("CREATOR")]
        public static GenericParcelableCreator<Player> GetCreator()
        {
            return _creator;
        }

        public Player(Parcel parcel)
        {
            id = parcel.ReadInt();
            name = parcel.ReadString();

        }


        //public IntPtr Handle
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}



        public int DescribeContents()
        {
            // 0 means no special objects
            return 0;
        }

        public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags)
        {
            dest.WriteInt(id);
            dest.WriteString(name);
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        public sealed class GenericParcelableCreator<T> : Java.Lang.Object, IParcelableCreator
        where T : Java.Lang.Object, new()
        {
            private readonly Func<Parcel, T> _createFunc;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParcelableDemo.GenericParcelableCreator`1"/> class.
            /// </summary>
            /// <param name='createFromParcelFunc'>
            /// Func that creates an instance of T, populated with the values from the parcel parameter
            /// </param>
            public GenericParcelableCreator(Func<Parcel, T> createFromParcelFunc)
            {
                _createFunc = createFromParcelFunc;
            }

            #region IParcelableCreator Implementation

            public Java.Lang.Object CreateFromParcel(Parcel source)
            {
                return _createFunc(source);
            }

            public Java.Lang.Object[] NewArray(int size)
            {
                return new T[size];
            }

            #endregion


        }
    }
}