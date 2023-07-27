using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;

namespace OrionBot.YouTube_API
{
    public class ExecutionEngineException
    {
        public string channelId = "UCkV3JksgFci4l8eUxcsB41Q";
        public string apiKey = "AIzaSyC_9JVOBHGV5bsVPVGxeLH3w2A0xnfxBo0";
        public YouTubeVideo _video=new YouTubeVideo();

        public YouTubeVideo GetLatestVideo()
        {
            string videoId;
            string videoUrl;
            string videoTitle;
            string thumbnail;
            DateTime? videoPublishedAt;


            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName="BenimUygulamam"
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.ChannelId = channelId;
            searchListRequest.MaxResults = 1;
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;

            var searchListRespose = searchListRequest.Execute();

            foreach ( var searchResult in searchListRespose.Items)
            {
                if(searchResult.Id.Kind=="youtube#vide")
                {
                    videoId = searchResult.Id.VideoId;
                    videoUrl = $"https://www.youtube.com/watch?v={videoId}";
                    videoTitle = searchResult.Snippet.Title;
                    videoPublishedAt = searchResult.Snippet.PublishedAt;
                    thumbnail = searchResult.Snippet.Thumbnails.Default__.Url;


                    return new YouTubeVideo()
                    {
                        videoId = videoId,
                        videoUrl = videoUrl,
                        videoTitle = videoTitle,
                        thumbnail = thumbnail,
                        PublishedAt = videoPublishedAt
                    };
                }
            }

            return null;
        }

    }
}
