using System.Web;
using System.Web.Mvc;

namespace OnlineRecruitment
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorLogAttribute());
        }
    }
}
