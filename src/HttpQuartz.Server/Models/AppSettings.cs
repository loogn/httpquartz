using Autowired.Core;
using Microsoft.AspNetCore.Hosting;

namespace HttpQuartz.Server.Models
{
    /// <summary>
    /// 对应appsettings.json
    /// </summary>
    [AppSetting]
    public class AppSettings
    {
        /// <summary>
        /// 网站根目录
        /// </summary>
        public string RootUrl { get; set; }
        /// <summary>
        /// 运行环境
        /// </summary>
        public IWebHostEnvironment Environment { get; set; }

        public string[] SafeClients { get; set; }
    }
}
