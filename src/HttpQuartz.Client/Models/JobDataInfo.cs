using System;

namespace HttpQuartz.Client.Models
{
    [Serializable]
    public class JobDataInfo
    {
        /// <summary>
        /// 请求方法,GET|POST，默认GET
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 请求超时时间，默认100，单位秒
        /// </summary>
        public int timeout { get; set; } = 100;

        /// <summary>
        /// 异常时是否立即再次触发，true|false，默认false
        /// </summary>
        public bool expRefire { get; set; }

        /// <summary>
        /// 异常时是否移除触发器，true|false，默认false
        /// </summary>
        public bool expRemove { get; set; }
    }

}