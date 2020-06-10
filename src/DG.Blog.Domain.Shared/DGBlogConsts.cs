using System.Collections.Generic;

namespace DG.Blog.Domain.Shared
{
    /// <summary>
    /// 全局常量
    /// </summary>
    public class DGBlogConsts
    {
        /// <summary>
        /// 分组
        /// </summary>
        public static class Grouping
        {
            /// <summary>
            /// 博客前台接口组
            /// </summary>
            public const string GroupName_v1 = "v1";

            /// <summary>
            /// 博客后台接口组
            /// </summary>
            public const string GroupName_v2 = "v2";

            /// <summary>
            /// 其他通用接口组
            /// </summary>
            public const string GroupName_v3 = "v3";

            /// <summary>
            /// JWT授权接口组
            /// </summary>
            public const string GroupName_v4 = "v4";
        }

        /// <summary>
        /// 语音合成欢迎词
        /// </summary>
        public const string GreetWord = "欢迎来到我的个人博客，我是阿星Plus。今日语录：{0}，{1}";

        /// <summary>
        /// 数据库表前缀
        /// </summary>
        public const string DbTablePrefix = "DG_";

        /// <summary>
        /// 缓存过期时间策略
        /// </summary>
        public static class CacheStrategy
        {
            /// <summary>
            /// 一天过期24小时
            /// </summary>

            public const int ONE_DAY = 1440;

            /// <summary>
            /// 12小时过期
            /// </summary>

            public const int HALF_DAY = 720;

            /// <summary>
            /// 8小时过期
            /// </summary>

            public const int EIGHT_HOURS = 480;

            /// <summary>
            /// 5小时过期
            /// </summary>

            public const int FIVE_HOURS = 300;

            /// <summary>
            /// 3小时过期
            /// </summary>

            public const int THREE_HOURS = 180;

            /// <summary>
            /// 2小时过期
            /// </summary>

            public const int TWO_HOURS = 120;

            /// <summary>
            /// 1小时过期
            /// </summary>

            public const int ONE_HOURS = 60;

            /// <summary>
            /// 半小时过期
            /// </summary>

            public const int HALF_HOURS = 30;

            /// <summary>
            /// 5分钟过期
            /// </summary>
            public const int FIVE_MINUTES = 5;

            /// <summary>
            /// 1分钟过期
            /// </summary>
            public const int ONE_MINUTE = 1;

            /// <summary>
            /// 永不过期
            /// </summary>

            public const int NEVER = -1;
        }

        /// <summary>
        /// 响应文本
        /// </summary>
        public static class ResponseText
        {
            /// <summary>
            /// 新增成功
            /// </summary>
            public const string INSERT_SUCCESS = "新增成功";

            /// <summary>
            /// 更新成功
            /// </summary>
            public const string UPDATE_SUCCESS = "更新成功";

            /// <summary>
            /// 删除成功
            /// </summary>
            public const string DELETE_SUCCESS = "删除成功";

            /// <summary>
            /// 什么不存在
            /// </summary>
            public const string WHAT_NOT_EXIST = "{0}:{1} 不存在";

            /// <summary>
            /// 数据为空
            /// </summary>
            public const string DATA_IS_NONE = "数据为空";

            /// <summary>
            /// IP地址格式错误
            /// </summary>
            public const string IP_IS_WRONG = "IP地址格式错误";

            /// <summary>
            /// 图片错误
            /// </summary>
            public const string IMG_IS_WRONG = "图片错误";

            /// <summary>
            /// 密码错误
            /// </summary>
            public const string PASSWORD_WRONG = "密码错误";

            /// <summary>
            /// 参数错误
            /// </summary>
            public const string PARAMETER_ERROR = "参数错误";
        }

        public static class Proxy
        {
            public const string REDIS_KEY = "proxies";

            public const int MAX_SCORE = 100;
            public const int MIN_SCORE = 0;
            public const int INITIAL_SCORE = 10;

            /// <summary>
            /// 代理池数量界限
            /// </summary>
            public const int POOL_UPPER_THRESHOLD = 50000;

            /// <summary>
            /// 通用测试网站
            /// </summary>
            public const string TEST_URL = "http://www.baidu.com";

            /// <summary>
            /// 最大批测试量
            /// </summary>
            public const long BATCH_TEST_SIZE = 10;

            public static readonly string[] User_Agent_List = new string[]
            {
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/22.0.1207.1 Safari/537.1",
                "Mozilla/5.0 (X11; CrOS i686 2268.111.0) AppleWebKit/536.11 (KHTML, like Gecko) Chrome/20.0.1132.57 Safari/536.11",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6",
                "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1090.0 Safari/536.6",
                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/19.77.34.5 Safari/537.1",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.9 Safari/536.5",
                "Mozilla/5.0 (Windows NT 6.0) AppleWebKit/536.5 (KHTML, like Gecko) Chrome/19.0.1084.36 Safari/536.5",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1063.0 Safari/536.3",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1063.0 Safari/536.3",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_0) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1063.0 Safari/536.3",
                "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1062.0 Safari/536.3",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1062.0 Safari/536.3",
                "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.1 Safari/536.3",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.1 Safari/536.3",
                "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.1 Safari/536.3",
                "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/536.3 (KHTML, like Gecko) Chrome/19.0.1061.0 Safari/536.3",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24",
                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/535.24 (KHTML, like Gecko) Chrome/19.0.1055.1 Safari/535.24",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36"
             };
        }
    }
}