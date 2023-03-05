using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocationManager.Services.RolesService
{
    public interface IRolesService
    {
        Task<string> GetRoleNameByUserId(string userId);

        Task<string?> GetNameById(string id);

        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
