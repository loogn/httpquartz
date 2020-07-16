using System.Threading.Tasks;
using Autowired.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace HttpQuartz.Server.Tools.Filters
{
    [AppService]
    public class MyExceptionFilterAttribute: ExceptionFilterAttribute
    {
        private ILogger<MyExceptionFilterAttribute> _logger;
        public MyExceptionFilterAttribute(ILogger<MyExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            return base.OnExceptionAsync(context);
        }
    }
}
