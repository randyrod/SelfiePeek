using System;
using System.Net;
using System.Threading.Tasks;

namespace WebRequestAsync
{
    public class AsyncWebRequest
    {
        public async Task<T> MakeRequest<T>(WebRequest wr)
        {
            var task = Task.Factory.FromAsync((cb, o) => ((HttpWebRequest)o).BeginGetResponse(cb, o), res => ((HttpWebRequest)res.AsyncState).EndGetResponse(res), wr);
            var result = await task;
            var resp = result;
            var stream = resp.GetResponseStream();
            var sr = new System.IO.StreamReader(stream);
            var jSon = await sr.ReadToEndAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jSon);

        }
        public async Task<T> MakeRequest<T>(string URL)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp(URL);
            req.AllowReadStreamBuffering = true;
            var tr = await MakeRequest<T>(req);
            return tr;
        }
    }
}
