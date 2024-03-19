using Tms.DataAccess;
using Tms.Models.Role;
using System.Collections.Generic;
using System.Linq;

namespace Tms.Services.AutoMap
{
    public static class RoleMapper
    {
        #region Mapping Role
        public static RoleModel MapToModel(this Role entity)
        {
            return new RoleModel
            {
                RoleId = entity.RoleId,
                Code = entity.Code,
                Name = entity.Name,
                Note = entity.Note,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate
            };
        }
        public static RoleModel MapToModel(this Role entity, RoleModel model)
        {
            model.RoleId = entity.RoleId;
            model.Code = entity.Code;
            model.Name = entity.Name;
            model.Note = entity.Note;
            model.Status = entity.Status;
            return model;
        }
        public static Role MapToEntity(this RoleModel model)
        {
            return new Role
            {
                RoleId = model.RoleId,
                Code = model.Code,
                Name = model.Name,
                Note = model.Note,
                Status = model.Status,
                CreatedDate = model.CreatedDate,
                CreatedByUserId = model.CreatedByUserId
            };
        }
        public static Role MapToEntity(this RoleModel model, Role entity)
        {
            entity.RoleId = model.RoleId;
            entity.Code = model.Code;
            entity.Name = model.Name;
            entity.Note = model.Note;
            entity.Status = model.Status;
            entity.CreatedDate = model.CreatedDate;
            entity.CreatedByUserId = model.CreatedByUserId;
            return entity;
        }
        public static List<Role> MapToEntities(this List<RoleModel> models)
        {
            return models.Select(x => x.MapToEntity()).ToList();
        }

        public static List<RoleModel> MapToModels(this List<Role> entities)
        {
            return entities.Select(x => x.MapToModel()).ToList();
        }
        #endregion
    }
}
