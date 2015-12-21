using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections;
using System.Threading;
namespace mythGraStepThree
{
    public class mythDownloader
    {
        public int timeout = 1000;
        private static mythDownloader m_mythdownloader = null;
        private Dictionary<string, string> mdict = null;
        public static mythDownloader GetInstance()
        {
            if (m_mythdownloader == null)
            {
                m_mythdownloader = new mythDownloader();
            }
            return m_mythdownloader;
        }
        public mythDownloader()
        {
            mdict = new Dictionary<string, string>();
        }
        public string SingleStep(string url)
        {
            string ret = string.Empty;
            if (mdict.ContainsKey(url))
            {
                ret = mdict[url];
            }
            else
            {

                string s = MythGetHtml(url, Encoding.UTF8,timeout);
                if (s.Equals("\r\n") || s.Equals(string.Empty))
                {
                    ret = "Rejected:" + url;
                }
                else
                {
                    ret = GetTitle(s);
                }
                if (!mdict.ContainsKey(url))
                {
                    mdict[url] = ret;
                }
            }
            return ret;
        }
        private string GetTitle(string html)
        {
            string pattern = @"(?si)<title(?:\s+(?:""[^""]*""|'[^']*'|[^""'>])*)?>(?<title>.*?)</title>";
            return Regex.Match(html, pattern).Groups["title"].Value.Trim();
        }
        private string GetCharset(string html)
        {
            string pattern = "charset=(?<charset>.*)>";
            string tmp = Regex.Match(html, pattern).Groups["charset"].Value.Trim();
            string[] splitstr = tmp.Split('>');
            if (splitstr.Length >= 1)
            {
                string ret = splitstr[0].Replace("/", "");
                ret = ret.Replace("\"", "");
                return ret.Trim();
            }
            else
            {
                return string.Empty;
            }
        }
        private string MythGetHtml(string url, Encoding encoding,int mtimeout)
        {

            try
            {
                HttpWebRequest request;
                if (url.Contains("http://") || url.Contains("https://"))
                    request = (HttpWebRequest)HttpWebRequest.Create(url);    //创建一个请求示例
                else
                    request = (HttpWebRequest)HttpWebRequest.Create("http://" + url);    //创建一个请求示例
                request.AllowAutoRedirect = true;
                request.Timeout = mtimeout;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();//获取响应，即发送请求
                // response.Headers["charset"];
                Stream responseStream = response.GetResponseStream();
                //  StreamReader streamReader;
                var tmpbyte = new byte[1024];
                int reallen = responseStream.Read(tmpbyte, 0, tmpbyte.Length);
                string str = Encoding.UTF8.GetString(tmpbyte, 0, reallen);
                string charset = GetCharset(str);
                if (charset.ToUpper() == "UTF-8")
                {
                    return str;
                }
                else
                {
                    if (charset == string.Empty)
                    {
                        charset = response.CharacterSet;
                    }
                    return Encoding.GetEncoding(charset).GetString(tmpbyte, 0, reallen);
                }
            }
            catch (Exception ee)
            {
                return "<title>ReadHtmlFailed:" + url + "</title>";
            }
        }
    }
}
