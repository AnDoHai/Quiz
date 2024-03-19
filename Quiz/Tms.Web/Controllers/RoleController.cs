using Tms.DataAccess;
using Tms.Models.Role;
using Tms.Services;
using Tms.Services.AutoMap;
using Tms.Web.Framework.Authentication;
using Tms.Web.Framework.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EcommerceSystem.Core.Configurations;

namespace Tms.Web.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService = (IRoleService)DependencyResolver.Current.GetService(typeof(IRoleService));
        private readonly IModuleActionService _moduleActionService = (IModuleActionService)DependencyResolver.Current.GetService(typeof(IModuleActionService));
        //private readonly IRoleModuleActionService _roleModuleActionService = (IRoleModuleActionService)DependencyResolver.Current.GetService(typeof(IRoleModuleActionService));

        [CustomAuthorize(Roles = RoleHelper.RoleListing)]
        public ActionResult Index()
        {
            int totalRecords;
            var model = new RoleSearchModel
            {
                Roles = _roleService.Search(1, SystemConfiguration.PageSizeDefault, null, null, null, out totalRecords),
                PageIndex = 1,
                PageSize = SystemConfiguration.PageSizeDefault,
                TotalRecords = totalRecords
            };
           IEnumerable<Role> a = _roleService.GetListRoleAsEnumerable();

            //foreach (var item in ViewBag.Roles)
            //{
            //    var b = item.RoleId;
            //}
            return View(model);
        }

        public ActionResult Search(int currentPage, string textSearch, string sortColumn, string sortDirection, int pageSize)
        {
            int totalRecords;
            var model = new RoleSearchModel
            {
                Roles = _roleService.Search(currentPage, pageSize, textSearch, sortColumn, sortDirection, out totalRecords),
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageIndex = currentPage,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };

            var html = RenderPartialViewToString("~/Views/Role/Partial/_TableRole.cshtml", model);
            return Json(new
            {
                IsError = false,
                HTML = html
            }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = RoleHelper.RoleCreate)]
        public ActionResult Create()
        {
            var model = new RoleModel();
            model.ModuleActions = _moduleActionService.GetModuleActions(0);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleModel model,string roleModuleActionStr)
        {
            string message = "Tạo mới quyền thất bại";
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.CreatedByUserId = CurrentUser.Email;

                if (_roleService.CreateRole(model, roleModuleActionStr, out message))
                {
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Role");
                }

                TempData["error"] = message;
                return RedirectToAction("Index", "Role");
            }

            return View(model);
        }

        [CustomAuthorize(Roles = RoleHelper.RoleEdit)]
        public ActionResult Edit(int id)
        {
            var roleEntity = _roleService.GetById(id);
            if(roleEntity != null)
            {
                var model = roleEntity.MapToModel();
                model.ModuleActions = _moduleActionService.GetModuleActions(model.RoleId);
                return View(model);
            }
            return View(new RoleModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleModel model,string roleModuleActionStr)
        {
            string message;
            if (ModelState.IsValid)
            {
                model.UpdatedUserId = CurrentUser.Email;

                var isSuccess = _roleService.UpdateRole(model, roleModuleActionStr, out message);
                if (isSuccess)
                {
                    TempData["success"] = message;
                    return RedirectToAction("Index", "Role");
                }

                TempData["error"] = message;
                return RedirectToAction("Index", "Role");
            }

            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = RoleHelper.RoleDelete)]
        public ActionResult Delete(int roleId)
        {
            string message;
            var result = _roleService.Delete(roleId, out message);
            if (result)
            {
                TempData["success"] = message;
                return Json(new { IsError = false });
            }
            return Json(new { IsError = true, Message = message });
        }

        [HttpPost]
        [CustomAuthorize(Roles = RoleHelper.RoleInvisible)]
        public ActionResult Invisibe(int roleId)
        {
            string message;
            var result = _roleService.ChangeStatus(roleId, out message);
            if (result)
            {
                return Json(new { IsError = false, Message = message });
            }
            return Json(new { IsError = true, Message = message });
        }
    }
}