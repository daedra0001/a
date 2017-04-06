using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CrawlerDll
{
    public static class Methods
    {
        public static string Read(string url)
        {
            var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myHttpWebRequest.AllowAutoRedirect = true;
            myHttpWebRequest.UserAgent = @"Mozilla/5.2 (Windows NT 6.1) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.122 Safari/534.30";
            myHttpWebRequest.Accept = @"*/*";

            var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            var streamResponse = myHttpWebResponse.GetResponseStream();
            var streamRead = new StreamReader(streamResponse, Encoding.UTF8);

            string result = streamRead.ReadToEnd();

            streamResponse.Close();
            myHttpWebResponse.Close();
            streamRead.Close();

            return result;
        }

        /// <summary>
        /// 根据内容返回查找处理后的数据
        /// </summary>
        /// <param name="content">输入</param>
        /// <param name="element">节点名称</param>
        /// <param name="sigh">检验该段内容是否需要</param>
        /// <param name="str_start">返回内容开始处标示字符</param>
        /// <param name="str_end">返回内容结束处标示字符</param>
        /// <returns></returns>
        public static List<string> GetData(string content, string element, string sigh, string str_start, string str_end)
        {
            var _List = new List<string>();

            string pattern = "<" + element + @"[^>]*(/>|>(\S|\s)*?</" + element + ">)";
            var _Regex = new Regex(pattern);
            MatchCollection matches = _Regex.Matches(content);

            var enu = matches.GetEnumerator();
            while (enu.MoveNext() && enu.Current != null)
            {
                string str = ((Match)(enu.Current)).Value;

                if (!string.IsNullOrEmpty(sigh))
                {
                    if (str.IndexOf(sigh) != -1)
                    {
                        var str2 = GetData2(str, str_start, str_end);
                        if (!string.IsNullOrEmpty(str2))
                        {
                            _List.Add(str2);
                        }
                    }
                }
                else
                {
                    var str2 = GetData2(str, str_start, str_end);
                    if (!string.IsNullOrEmpty(str2))
                    {
                        _List.Add(str2);
                    }
                }
            }

            return _List;
        }

        static string GetData2(string content, string str_start, string str_end)
        {
            if (!string.IsNullOrEmpty(str_start) || !string.IsNullOrEmpty(str_end))
            {
                int index_start = content.IndexOf(str_start);
                int index_end = content.IndexOf(str_end);
                if (index_start != -1 && index_end != -1 & index_start <= index_end)
                {
                    int startindex = index_start + str_start.Length;
                    int length = index_end - startindex;
                    return content.Substring(startindex, length);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return content;
            }
        }

        public static string ReadFromJson(string json, string propertynamee)
        {
            var _List = new List<string>();

            var _JObject = JObject.Parse(json);

            return _JObject[propertynamee].ToString();
        }

        public static T ReadFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


    }
}