using LE.AdminService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.AdminService.Services
{
    public interface IUserService
    {
        Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagingDto<UserDto>> GetUsersPagingAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
