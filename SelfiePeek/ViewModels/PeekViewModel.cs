using SelfiePeek.Models;
using SelfiePeek.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebRequestAsync;
using Windows.Storage;

namespace SelfiePeek.ViewModels
{
    /// <summary>
    /// Main ViewModel for the application
    /// </summary>
    public class PeekViewModel : BaseModel
    {
        public PeekViewModel()
        {
            Loaded = false;
        }

        /// <summary>
        /// Function used to make request to Instagram API Asyncronously
        /// Parse the data response from the API
        /// Update the ObservableCollection containing the data
        /// </summary>
        public async void InitAsync()
        {
            try
            {
                if (ConnectionManager.CheckInternetAccess())
                {

                    ActivityVisible = true;
                    MessageVisible = true;
                    Message = "Loading most recent #Selfies";
                    AsyncWebRequest client = new AsyncWebRequest();
                    var apiRequestURL = SharedStrings.InstagramAPIBaseURI + SharedStrings.CurrentToken;
                    var resp = await client.MakeRequest<InstagramRawBase>(apiRequestURL);
                    int colSpan = 0;
                    if (resp != null && resp.data != null && resp.data.Count > 0)
                    {
                        var data = resp.data;
                        var selfies = new List<SelfiePeekDataModel>();
                        foreach (var item in data)
                        {
                            var extracted = new SelfiePeekDataModel();
                            extracted.UserName = item.user.username;
                            extracted.Thumbnail = new Uri(item.images.standard_resolution.url);
                            extracted.Caption = item.caption.text;
                            var tags = new List<string>();
                            foreach (var tag in item.tags)
                            {
                                tags.Add(tag);
                            }
                            extracted.Tags = tags;
                            extracted.ColSpan = (colSpan % 3 == 0) ? 2 : 1;
                            colSpan++;
                            selfies.Add(extracted);
                        }
                        if (SelfieList != null && SelfieList.Count > 0)
                        {
                            SelfieList.Clear();
                        }
                        SelfieList = new ObservableCollection<SelfiePeekDataModel>(selfies);
                        Loaded = true;
                    }
                    Message = "";
                    ActivityVisible = false;
                    MessageVisible = false;
                }
                else
                {
                    ActivityVisible = false;
                    MessageVisible = true;
                    Message = "We could not connect to the internet, click refresh to try again!";
                }
            }
            catch(Exception)
            {
                Message = "Oops!! We have encountered some problems, try to refresh";
                //This is a workaround, in case the token is invalid, or reauthorization is required
                //If the token is invalid the parse of the response will throw an exception
                //Caught here, this makes so that the token be null, and when the user wants to refresh
                //They will have to reauthorize the application
                SharedStrings.CurrentToken = null;
            }
            finally
            {
                ActivityVisible = false;
                MessageVisible = true;
            }
        }

        #region Properties Region
        //Property Used to display Message over the main User UI, via Data Binding
        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                if (_Message == value)
                {
                    return;
                }
                _Message = value;
                NotifyPropertyChanged();
            }
        }

        //Property to show or hide the message, via Data Binding
        private bool _MessageVisible;
        public bool MessageVisible
        {
            get { return _MessageVisible; }
            set
            {
                if (_MessageVisible == value)
                {
                    return;
                }
                _MessageVisible = value;
                NotifyPropertyChanged();
            }
        }

        //Property to show or hide the Activity Indicator, via Data Binding
        private bool _ActivityVisible;
        public bool ActivityVisible
        {
            get { return _ActivityVisible; }
            set
            {
                if (_ActivityVisible == value)
                {
                    return;
                }
                _ActivityVisible = value;
                NotifyPropertyChanged();
            }
        }

        //Property to determine if the first load has already been executed.
        private bool _Loaded;
        public bool Loaded
        {
            get { return _Loaded; }
            set
            {
                if (_Loaded == value)
                {
                    return;
                }
                _Loaded = value;
                NotifyPropertyChanged();
            }
        }

        //ObservableCollection for data binding with the UI
        private ObservableCollection<SelfiePeekDataModel> _SelfieList;
        public ObservableCollection<SelfiePeekDataModel> SelfieList 
        {
            get { return _SelfieList; }
            set
            {
                if (_SelfieList == value)
                {
                    return;
                }
                _SelfieList = value;
                NotifyPropertyChanged();
            }
        }
        #endregion
    }
}