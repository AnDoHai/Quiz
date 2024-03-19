using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models;
using Tms.Models.Role;
using Tms.Services.AutoMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services
{
    public interface IRoleService : IEntityService<Role>
    {
        List<RoleModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage);
        bool UpdateRole(RoleModel roleModel, string roleModuleIds, out string message);
        bool Delete(int roleId, out string message);
        List<RoleModel> GetRoles();
        bool ChangeStatus(int roleId, out string message);
        bool CreateRole(RoleModel model, string roleModuleActionStr, out string message);
        IEnumerable<Role> GetListRoleAsEnumerable();

    }
    public class RoleService : EntityService<Role>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleModuleActionRepository _roleModuleActionRepository;
        public RoleService(IUnitOfWork unitOfWork, IRoleRepository roleRepository, IRoleModuleActionRepository roleModuleActionRepository)
            : base(unitOfWork, roleRepository)
        {
            _roleRepository = roleRepository;
            _roleModuleActionRepository = roleModuleActionRepository;
        }

        public List<RoleModel> Search(int currentPage, int pageSize, string textSearch, string sortColumn, string sortDirection, out int totalPage)
        {
            var roleEntities = _roleRepository.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalPage);
            if (roleEntities != null)
            {
                return roleEntities.MapToModels();
            }
            return null;
        }

        public bool UpdateRole(RoleModel roleModel, string roleModuleIds, out string message)
        {
            var roleEntity = _roleRepository.GetById(roleModel.RoleId);
            if (roleEntity != null)
            {
                if(roleModel.Code != roleEntity.Code)
                {
                    var existMaQuyen = _roleRepository.GetAll().FirstOrDefault(c => c.Code == roleModel.Code);
                    if (existMaQuyen != null)
                    {
                        message = "Mã quyền đã tồn tại";
                        return false;
                    }
                }
                if (roleModel.Name.ToLower().Trim().Replace("\t" , "") != roleEntity.Name.ToLower().Trim().Replace("\t" , ""))
                {
                    var existTenQuyen = _roleRepository.GetAll().FirstOrDefault(c => c.Name.ToLower().Trim().Replace("\t" , "") == roleModel.Name.ToLower().Trim().Replace("\t" , ""));
                    if (existTenQuyen != null)
                    {
                        message = "Tên quyền đã tồn tại";
                        return false;
                    }
                }
                roleEntity.UpdatedDate = DateTime.Now;
                roleEntity.UpdatedUserId = roleModel.UpdatedUserId;
                roleEntity.Code = roleModel.Code;
                roleEntity.Name = roleModel.Name;
                roleEntity.Note = roleModel.Note;
                roleEntity.Status = roleModel.Status;

                _roleRepository.Update(roleEntity);
                UnitOfWork.SaveChanges();

                if (!AssignRoleModuleAction(roleEntity.RoleId, roleModuleIds, out message))
                {
                    message = "Cập nhật quyền thất bại.";
                    return false;
                }

                message = "Cập nhật thành công";
                return true;
            }

            message = "Cập nhật quyền thất bại.";
            return false;
        }

        public bool CreateRole(RoleModel model, string roleModuleActionStr, out string message)
        {
            try
            {
                var existMaQuyen = _roleRepository.GetAll().FirstOrDefault(c=>c.Code == model.Code);
                var existTenQuyen = _roleRepository.GetAll().FirstOrDefault(c => c.Name.ToLower().Trim().Replace("\t" , "") == model.Name.ToLower().Trim().Replace("\t" , ""));
                if (existMaQuyen != null)
                {
                    message = "Mã quyền đã tồn tại";
                    return false;
                }
                if (existTenQuyen != null)
                {
                    message = "Tên quyền đã tồn tại";
                    return false;
                }
                var createdRole = _roleRepository.Insert(model.MapToEntity());
                UnitOfWork.SaveChanges();
                var errorMsg = string.Empty;
                if (createdRole == null)
                {
                    Log.Error("Create role error");
                    message = "Tạo mới thất bại";
                    return false;
                }
                var isSuccess = AssignRoleModuleAction(createdRole.RoleId, roleModuleActionStr, out errorMsg);
                message = "Tạo mới thành công";
                return isSuccess;
            }
            catch (Exception ex)
            {
                Log.Error("Create role error", ex);
                message = "Tạo mới thất bại";
                return false;
            }

        }

        public bool Delete(int roleId, out string message)
        {
            var entity = _roleRepository.GetById(roleId);
            if (entity != null)
            {
                var isExists = _roleRepository.GetAll().Any(c => c.RoleId == roleId
                        && c.UserRoles.Any(ce => ce.RoleId == roleId));
                if (isExists)
                {
                    message = "Bạn không thể thực hiện xóa quyền này.";
                    return false;
                }

                var roleModules = _roleModuleActionRepository.FindAll(x => x.RoleID == roleId);
                if (roleModules != null)
                {
                    _roleModuleActionRepository.DeleteMulti(roleModules);
                }
                _roleRepository.Delete(roleId);
                UnitOfWork.SaveChanges();

                message = "Xóa quyền thành công";
                return true;
            }

            message = "Xóa quyền thất bại";
            return false;
        }

        public bool ChangeStatus(int roleId, out string message)
        {
            var position = _roleRepository.Query(c => c.RoleId == roleId).FirstOrDefault();
            if (position != null)
            {
                if (position.Status)
                {
                    position.Status = false;
                }
                else
                {
                    position.Status = true;
                }

                _roleRepository.Update(position);
                UnitOfWork.SaveChanges();

                message = "Cập nhật trạng thái quyền thành công.";
                return true;
            }

            message = "Cập nhật trạng thái quyền thất bại.";
            return false;
        }

        public List<RoleModel> GetRoles()
        {
            //Igrone role system
            return _roleRepository.GetAll().ToList().MapToModels();
        }

        public bool AssignRoleModuleAction(int roleId, string roleModuleIds, out string message)
        {
            try
            {
                var roleModuleActions = roleModuleIds.Split(',').ToList();
                var moduleActionId = 0;
                roleModuleActions.RemoveAll(item => !int.TryParse(item, out moduleActionId));

                if (!string.IsNullOrEmpty(roleModuleIds) && roleModuleActions.Any())
                {
                    //var newRoleModuleAction = (roleModuleActions.Length == 2)
                    //    ? new[] { roleModuleActions[1] } : roleModuleActions;

                    var listRoleModule = roleModuleActions.Select(item => new RoleModuleAction
                    {
                        RoleID = roleId,
                        ModuleActionID = (!string.IsNullOrEmpty(item)) ? Convert.ToInt32(item) : 0
                    }).ToList();

                    //check exists role
                    var roleModuleAction = _roleModuleActionRepository.Query(c => c.RoleID == roleId).ToList();
                    if (roleModuleAction.Any())
                    {
                        _roleModuleActionRepository.DeleteMulti(roleModuleAction);
                    }

                    _roleModuleActionRepository.InsertMulti(listRoleModule);
                    UnitOfWork.SaveChanges();
                    message = "Phân Quyền thành công.";

                    return true;
                }
                else
                {
                    var roleModuleAction = _roleModuleActionRepository.Query(c => c.RoleID == roleId).ToList();
                    if (roleModuleAction.Any())
                    {
                        _roleModuleActionRepository.DeleteMulti(roleModuleAction);
                    }
                    UnitOfWork.SaveChanges();
                    message = "Phân Quyền thành công.";
                    return true;
                }
            }
            catch (Exception)
            {
                message = "Phân Quyền thất bại.";
                return false;
            }
        }


        public IEnumerable<Role> GetListRoleAsEnumerable()
        {
            IEnumerable<Role> roles = _roleRepository.GetAll().Where(x => x.RoleId > 2).ToList();
            return roles;
        }
    }
}
