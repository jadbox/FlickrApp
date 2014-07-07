using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;

namespace WpfApplication1
{
    class Flickr
    {
        public Flickr()
        {
            
        }

        private string flickerRU(string args)
        {
            return "https://api.flickr.com/services/rest/?format=json" +
                    "&nojsoncallback=1&api_key=256663858aa10e52a838a58b7866d858" + args;
        }

        private string tagsRU(string tags)
        {
            return flickerRU("&method=flickr.photos.search&sort=random&per_page=10&tags=" + Uri.EscapeUriString(tags));
        }

        private string getOneURL(string id)
        {
            return flickerRU("&method=flickr.photos.getSizes&photo_id=" + id);
        }

        private IObservable<String> getData(string url)
        {
            WebClient wc = new WebClient();
            var o = System.Reactive.Linq.Observable
            .FromEventPattern<DownloadStringCompletedEventHandler, DownloadStringCompletedEventArgs>(
                h => h.Invoke,
                h => wc.DownloadStringCompleted += h,
                h => wc.DownloadStringCompleted -= h).Select(e => e.EventArgs.Result as string);
            wc.DownloadStringAsync(new Uri(url));
            return o;
        }

        private IObservable<dynamic> getJson(string url)
        {
            System.Console.Out.WriteLine(url);
            return getData(url).Select(x => JsonConvert.DeserializeObject(x) as dynamic);
        }

        public void getImageUrlForTagRx(string tag, Action<String> a)
        {
            getJson(tagsRU(tag))
                   .Select(data => getOneURL((string)data.photos.photo[0].id))
                   .SelectMany(c1 => getJson(c1))
                   .Select(imgdata => (string)imgdata.sizes.size[0].source)
                   .Subscribe(x=>a(x));
        }

        public String getImageUrlForTag(String imageTag)
        {
            WebClient webClient = new WebClient();
            String json = webClient.DownloadString(tagsRU(Uri.EscapeUriString(imageTag)));
            dynamic a = JsonConvert.DeserializeObject(json);
            String id = a.photos.photo[1].id;
            Console.Out.WriteLine(id);

            String jsonImage = webClient.DownloadString(getOneURL(id));
            Console.Out.WriteLine(jsonImage);

            dynamic b = JsonConvert.DeserializeObject(jsonImage);

            String source = b.sizes.size[0].source;
            Console.Out.WriteLine(source);
            return source;
        }

    }
}
