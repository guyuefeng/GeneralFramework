using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Text.RegularExpressions;


namespace GF.Core
{
    /// <summary>
    /// 缓存与公共数据处理类
    /// </summary>
    public sealed class SiteDat
    {
        private static string _lanCache = "Dat-Rewrite";
        private static string _urlCache = "Dat-Language";

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        public static void SetDat(string key, object obj)
        {
            Cache cache = HttpContext.Current.Cache;
            if (cache.Get(key) == null) 
            { 
                cache.Insert(key, obj); 
            }
            else 
            { 
                cache[key] = obj; 
            }
        }

        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="key">键</param>
        public static object GetDat(string key)
        {
            Cache cache = HttpContext.Current.Cache;
            return cache.Get(key);
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void ClearDat()
        {
            Cache cache = HttpContext.Current.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// 清空指定前缀的缓存
        /// </summary>
        /// <param name="pf">前缀</param>
        public static void ClearDat(string pf)
        {
            Cache cache = HttpContext.Current.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (cacheEnum.Key.ToString().StartsWith(pf, true, null)) { cache.Remove(cacheEnum.Key.ToString()); }
            }
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        public static IDictionaryEnumerator GetCacheEnumerator()
        {
            Cache cache = HttpContext.Current.Cache;
            return cache.GetEnumerator();
        }


        /// <summary>
        /// 删除缓存
        /// </summary>
        public static object RemoveDat(string key)
        {
            Cache cache = HttpContext.Current.Cache;
            return cache.Remove(key);
        }

        /// <summary>
        /// 初始化语言包
        /// </summary>
        private static Dictionary<string, string> InitLan()
        {
            string path = Path.Combine(SiteCfg.Router, "Common/Language.xml");
            Dictionary<string, string> cacheObj = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                XmlTextReader reader = new XmlTextReader(path);
                while (reader.Read())
                {
                    if (reader.Name == "item")
                    {
                        string key = reader.GetAttribute("key");
                        string value = reader.GetAttribute("value");
                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                        {
                            cacheObj.Add(key, value);
                        }
                    }
                }
                reader.Close();
            }
            return cacheObj;
        }

        /// <summary>
        /// 获取语言缓存
        /// </summary>
        /// <param name="key">键</param>
        public static string GetLan(string key)
        {
            string str = string.Empty;
            Dictionary<string, string> lanCache = (Dictionary<string, string>)GetDat(_lanCache);
            if (lanCache == null)
            {
                lanCache = InitLan();
                SetDat(_lanCache, lanCache);
            }
            if (!string.IsNullOrEmpty(key))
            {
                str = lanCache.TryGetValue(key, out str) ? str : string.Empty;
            }
            return str;
        }

        /// <summary>
        /// 初始化重写缓存
        /// </summary>
        private static Dictionary<string, string> InitRewrite()
        {
            XmlTextReader reader = new XmlTextReader(Path.Combine(SiteCfg.Router, "Common/Rewrite.xml"));
            Dictionary<string, string> cacheObj = new Dictionary<string, string>();
            string attribute = string.Empty;
            for (string rewriteUrl = string.Empty; reader.Read(); rewriteUrl = string.Empty)
            {
                if (reader.Name == "rule")
                {
                    attribute = reader.GetAttribute("look");
                    rewriteUrl = reader.GetAttribute("to");
                    if (!string.IsNullOrEmpty(attribute) && !string.IsNullOrEmpty(attribute))
                    {
                        attribute = string.Format("{0}{1}", SiteCfg.Path, attribute);
                        rewriteUrl = string.Format("{0}{1}", SiteCfg.Path, rewriteUrl);
                        cacheObj.Add(attribute, rewriteUrl);
                    }
                }
                attribute = string.Empty;
            }
            reader.Close();
            return cacheObj;
        }

        /// <summary>
        /// 取出重写模式
        /// </summary>
        /// <param name="strVal">当前路径</param>
        /// <returns>返回重写模式</returns>
        public static string GetUrl(string strVal)
        {
            Dictionary<string, string> urlCache = (Dictionary<string, string>)GetDat(_urlCache);
            if (urlCache == null)
            {
                urlCache = InitRewrite();
                SetDat(_urlCache, urlCache);
            }
            if (urlCache.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in urlCache)
                {
                    if (!string.IsNullOrEmpty(strVal) && !string.IsNullOrEmpty(pair.Key))
                    {
                        if (Regex.IsMatch(strVal, pair.Key, RegexOptions.IgnoreCase))
                        {
                            return Regex.Replace(strVal, pair.Key, pair.Value, RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 设置验证码值
        /// </summary>
        /// <param name="val">值</param>
        public static void SetVerifyCode(object val)
        {
            HttpContext.Current.Session.Add(SiteCfg.Token + "VerifyCode", val);
        }

        /// <summary>
        /// 获取目前验证码值
        /// </summary>
        public static string GetVerifyCode
        {
            get
            {
                object vc = HttpContext.Current.Session[SiteCfg.Token + "VerifyCode"];
                if (string.IsNullOrEmpty(Convert.ToString(vc))) { return string.Empty; }
                else { return vc.ToString(); }
            }
        }

        /// <summary>
        /// 验证验证码值
        /// </summary>
        /// <param name="val">要对比的值</param>
        /// <returns>返回是否匹配的布尔型数据</returns>
        public static bool CheckVerifyCode(string val)
        {
            return GetVerifyCode == val;
        }

    }
}
