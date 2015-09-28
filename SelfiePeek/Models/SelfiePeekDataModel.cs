using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SelfiePeek.Models
{
    /// <summary>
    /// Basic DataModel for the application
    /// Compressed of the information extracted from the request to the Instagram API
    /// </summary>
    public class SelfiePeekDataModel : BaseModel
    {
        private Uri _Thumbnail;
        public Uri Thumbnail 
        {
            get { return _Thumbnail; }
            set
            {
                if (_Thumbnail == value)
                {
                    return;
                }
                _Thumbnail = value;
                NotifyPropertyChanged();
            }
        }

        private string _Caption;
        public string Caption
        {
            get { return _Caption; }
            set
            {
                if (_Caption == value)
                {
                    return;
                }
                _Caption = value;
                NotifyPropertyChanged();
            }
        }

        private List<string> _Tags;
        public List<string> Tags
        {
            get { return _Tags; }
            set
            {
                if (_Tags == value)
                {
                    return;
                }
                _Tags = value;
                NotifyPropertyChanged();
            }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName == value)
                {
                    return;
                }
                _UserName = value;
                NotifyPropertyChanged();
            }
        }

        private int _ColSpan;
        public int ColSpan 
        {
            get { return _ColSpan; }
            set
            {
                if (_ColSpan == value)
                {
                    return;
                }
                _ColSpan = value;
                NotifyPropertyChanged();
            }
        }
    }
}