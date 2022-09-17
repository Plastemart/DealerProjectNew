using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DealerPortal.Common;
using DealerPortal.Models;

namespace DealerPortal.Common
{
    public class Util
    {

        public static News GetNews(spGetNews_Result NewsObj)
        {
            var x = NewsObj;

            return new News()
            {
                LinkPage = x.LinkPage,
                MetatagDescription = x.MetatagDescription,
                MetatagKeywords = x.MetatagKeywords,
                MetatagTitle = x.MetatagTitle,
                News1 = x.News,
                NewsDate = x.CreatedDate,
                NewsId = x.NewsId,
                NewsTitle = x.NewsTitle

            };
        }

        public static string GetCustomTitle(string title, int numOfChar)
        {
            //return string.IsNullOrEmpty(title) ? "" : title.Substring(0, title.Length < numOfChar ? title.Length : numOfChar); ;
            return string.IsNullOrEmpty(title) ? "" : title.Length < numOfChar ? title.Substring(0, title.Length < numOfChar ? title.Length : numOfChar) : title.Substring(0, title.Length < numOfChar ? title.Length : numOfChar) + "...";
        }

        public static string NewsDetailUrl(News news)
        {
            var url = "/News-Plastics-Information/" + news.NewsTitle.Trim().Replace(' ', '-').Replace('/', '-') + "/" + news.NewsId;
            return url;
        }
    }

}