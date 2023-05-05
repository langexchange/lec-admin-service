using AutoMapper;
using LE.AdminService.Dtos;
using LE.AdminService.Infrastructure.Infrastructure;
using LE.UserService.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.AdminService.Services.Implements
{
    public class UserService : IUserService
    {
        private LanggeneralDbContext _context;
        private IMapper _mapper;
        public UserService(LanggeneralDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);
            if (user == null)
                return;
            user.IsRemoved = true;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<PagingDto<UserDto>>GetUsersPagingAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            var ids = await _context.Users.Where(x => (x.IsRemoved == null || x.IsRemoved == false)).Select(x => x.Userid).ToListAsync();

            var tookIds = ids.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var dtos = await GetUsersAsync(tookIds, cancellationToken);
            return new PagingDto<UserDto>(ids.Count, dtos);
        }

        private async Task<List<UserDto>> GetUsersAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var dtos = new List<UserDto>();
            foreach(var id in ids)
            {
                var dto = await GetUserAsync(id, cancellationToken);
                dtos.Add(dto);
            }
            return dtos;
        }

        private async Task<UserDto> GetUserAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id && (x.IsRemoved == null || x.IsRemoved == false));
            if (user == null)
                return null;

            var dto = _mapper.Map<UserDto>(user);
            if (user.NativeLang != null)
            {
                var nativeLangs = await _context.Languages.FirstOrDefaultAsync(x => x.Langid == user.NativeLang);
                dto.NativeLanguage = _mapper.Map<LanguageDto>(nativeLangs);
                dto.NativeLanguage.Level = user.NativeLevel.Value;
            }
            var targetLangs = await _context.Targetlangs.Where(x => x.Userid == user.Userid).ToListAsync();
            var targetLangDtos = _mapper.Map<List<LanguageDto>>(targetLangs);
            foreach (var targetLangDto in targetLangDtos)
            {
                var language = await _context.Languages.FirstOrDefaultAsync(y => y.Langid == targetLangDto.Id);
                targetLangDto.Name = language.Name;
                targetLangDto.LocaleCode = language.LocaleCode;
            }

            dto.TargetLanguages = targetLangDtos;

            var numOfPosts = await _context.Posts.Where(x => x.Userid == id && x.IsPublic == true && x.IsPublic == true).CountAsync();
            dto.NumOfPosts = numOfPosts;

            var friendIds = await _context.Relationships
                                       .Where(x => (x.User1 == id || x.User2 == id) && x.Action.Equals(Env.SEND_REQUEST) && x.Type == true)
                                       .ToListAsync();
            dto.NumOfPartners = friendIds.Count;
            return dto;
        }
    }
}
