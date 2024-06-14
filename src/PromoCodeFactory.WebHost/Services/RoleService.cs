using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Helpers;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Services
{
    public class RoleService(IRepository<Role> roleRepository)
    {
        public async Task<List<RoleResponse>> GetAllAsync()
        {
            var roles = await roleRepository.GetAllAsync();

            return roles.ConvertToModelList();
        }
    }
}
