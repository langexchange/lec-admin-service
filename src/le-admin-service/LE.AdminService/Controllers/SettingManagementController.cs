using LE.Library.Kernel;
using Microsoft.AspNetCore.Mvc;

namespace LE.AdminService.Controllers
{
    [Route("admin/api/settings")]
    [ApiController]
    public class SettingManagementController : ControllerBase
    {
        private readonly IRequestHeader _requestHeader;

        public SettingManagementController(IRequestHeader requestHeader)
        {
            _requestHeader = requestHeader;
        }


    }
}
