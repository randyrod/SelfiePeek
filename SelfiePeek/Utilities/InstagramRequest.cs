using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRequestAsync;

namespace SelfiePeek.Utilities
{
    public class InstagramRequest
    {
        public static async Task<List<Models.SelfiePeekDataModel>> MakeInstagramRequest(int currentIdx = 0)
        {
            try
            {
                var valuesList = await Task.Run(async () =>
                {
                    List<Models.SelfiePeekDataModel> returnList = null;
                    AsyncWebRequest client = new AsyncWebRequest();
                    var apiRequestURL = SharedStrings.InstagramAPIBaseURI + SharedStrings.CurrentToken;
                    var resp = await client.MakeRequest<Models.InstagramRawBase>(apiRequestURL);
                    int colSpan = currentIdx;
                    if (resp != null && resp.data != null && resp.data.Count > 0)
                    {
                        var data = resp.data;
                        returnList = new List<Models.SelfiePeekDataModel>();
                        foreach (var item in data)
                        {
                            var extracted = new Models.SelfiePeekDataModel();
                            extracted.UserName = item.user.username;
                            extracted.Thumbnail = new Uri(item.images.standard_resolution.url);
                            extracted.Caption = item.caption.text;
                            var tags = new List<string>();
                            foreach (var tag in item.tags)
                            {
                                tags.Add(tag);
                            }
                            extracted.Tags = tags;
                            extracted.ColSpan = (colSpan++ % 3 == 0) ? 2 : 1;
                            returnList.Add(extracted);
                        }
                    }
                    return returnList;
                });
                return valuesList;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
