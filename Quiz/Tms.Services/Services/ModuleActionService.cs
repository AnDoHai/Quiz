using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
namespace Tms.Services
{
    public interface IModuleActionService : IEntityService<ModuleAction>
    {
        List<ModuleActionModel> GetModuleActions(int roleId);
    }
    public class ModuleActionService : EntityService<ModuleAction>, IModuleActionService
    {
        private readonly IModuleActionRepository _moduleActionRepository;
        private readonly IRoleModuleActionRepository _roleModuleActionRepository;

        public ModuleActionService(IUnitOfWork unitOfWork, IRoleModuleActionRepository roleModuleActionRepository, IModuleActionRepository moduleActionRepository)
            : base(unitOfWork, moduleActionRepository)
        {
            _moduleActionRepository = moduleActionRepository;
            _roleModuleActionRepository = roleModuleActionRepository;
        }

        public List<ModuleActionModel> GetModuleActions(int roleId)
        {
            var listModule = _moduleActionRepository.GetAll().Include(c => c.RoleModuleActions).OrderBy(x => x.OrderIndex).ToList();
            if (roleId != 0)
            {
                var listChecked = _roleModuleActionRepository.Query(c => c.RoleID == roleId).ToList();
                return listModule.Select(c => new ModuleActionModel
                {
                    ModuleActionId = c.ModuleActionID,
                    Module = c.Module,
                    Action = c.Action,
                    Description = c.Description,
                    Group=c.Group,
                    IsChecked = listChecked.Any(ck => ck.ModuleActionID == c.ModuleActionID) ? true : false
                }).ToList();
            }

            return listModule.Select(c => new ModuleActionModel
            {
                ModuleActionId = c.ModuleActionID,
                Module = c.Module,
                Action = c.Action,
                Group = c.Group,
                Description = c.Description,
                IsChecked = false
            }).ToList();
        }
    }
}
