/* # HRM
================================================== */
hrm = {
    /* Role */
    role: {
        init: function () {			
            // Dropdownlist
            common.staticDropDownList(config.ddlPageSize, config.data.pageSize, config.defaultPageSize, null, '65px', true);

            // List            
            hrm.role.list(config.defaultPageIndex, 0, 0, false);

            // Register Event
            $(config.ddlPageSize).change(function () {
                hrm.role.list(config.defaultPageIndex, 0, 0, false);
            });

            $(config.txtSearch).keypress(function (event) {
                if (event.which == 13) {
                    hrm.role.list(config.defaultPageIndex, 0, 0, false);
                    event.preventDefault();
                }
            });
        },

        // List
        list: function (pageIndex, selectedRoleId, selectedUserId, isListTypeRadio) {
            var params = {
                keyword: $(config.txtSearch).val(), pageIndex: pageIndex,
                pageSize: isListTypeRadio == true ? config.defaultPageSize : $(config.ddlPageSize).val(),
                isListTypeRadio: isListTypeRadio, selectedRoleId: selectedRoleId
            };
            
            $.ajax({ url: config.serviceBase.roleList, data: params, type: 'POST' }, function () { })
                .done(function (res) {
                    if (res.Success && res.Data.length > 0) {                                                                           
                            $.Mustache.load(config.html.role + '?v=' + config.version)
                                .done(function () {
                                    if (isListTypeRadio) {
                                        // Selected Role                                                                        
                                        utils.setHiddenField('UserId', selectedUserId);
                                        for (var i = 0; i < res.Data.length; i++) {
                                            res.Data[i]['SelectedRoleId'] = res.Data[i].RoleId == selectedRoleId ? true : false;                                            
                                        }                                            
                                        console.log(res.Data);

                                        // Show Table Role Selected
                                        setTimeout(function () {
                                            $(config.modal.content).html('');
                                            $(config.modal.content).mustache(config.template.listRoleTypeRadio, res);

                                            $(config.modal.content + ' ' + config.pagerInfo).html(res.PagerInfo);
                                            $(config.modal.content + ' ' + config.pager).html(res.Pager);

                                            common.registerUniform();                                                                              

                                            $(config.modal.id).modal('show');
                                        }, 500);
                                    }
                                    else {
                                        utils.setHiddenField('CurrentPageIndex', pageIndex);

                                        $(config.tableBody).html('');
                                        $(config.tableBody).mustache(config.template.listRole, res);

                                        $(config.pagerInfo).html(res.PagerInfo);
                                        $(config.pager).html(res.Pager);

                                        $('.panel-heading span').text(res.PagerInfo);
                                        $('[data-hover="dropdown"]').dropdownHover();
                                        $('.tree-page').popover({ html: true, show: true });
                                        $('.tip').tooltip();
                                    }
                                });
                                               
                    } else {
                        $(config.tableBody).html(common.noData(7));
                        $(config.pagerInfo).html('');
                        $(config.pager).html('');
                    }
                });
        },        
        
        // Action        
        manipulate: function (action, id, name) {
            var params = {};
            if (id != null)
                params = { RoleId: id, Action: action };

            if (action == config.action.create || action == config.action.getById)
                $(config.modal.dialog).removeClass('modal-sm');
            
            switch (action) {
                case config.action.create:
                    $(config.modal.content).html('');
                    $(config.modal.content).mustache(config.template.createRole);
                    
                    common.staticDropDownList(config.ddlStatus, config.data.statuses, 1, null, null, true);

                    $(config.modal.id).modal('show');
                    break;

                case config.action.getById:
                    $(config.modal.content).html('');

                    $.ajax({ url: config.serviceBase.roleAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $(config.modal.content).mustache(config.template.getRole, res);

                                common.staticDropDownList(config.ddlStatus, config.data.statuses, res.Data.RecordStatus, null, null, true);

                                $(config.modal.id).modal('show');
                            }
                        });
                    break;

                case config.action.save:
                    params = $.extend(params, $(config.modal.form).serializeObject(), { RecordStatus: $(config.ddlStatus).val() });
                    $.ajax({ url: config.serviceBase.roleAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $.jGrowl('Save information role success.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                $(config.modal.id).modal('hide');
                                hrm.role.list(utils.getHiddenField('CurrentPageIndex'), 0, 0, false);                                
                            }
                        });
                    break;

                case config.action.del:                                        
                    $.ajax({ url: config.serviceBase.roleAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $.jGrowl('Role successfully deleted.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                hrm.role.list(config.defaultPageIndex, 0, 0, false);
                            } else {
                                $.jGrowl('You must delete the information functions and membership before deleting role.', { sticky: false, theme: 'growl-warning', header: 'Message' });
                            }
                            $(config.modal.id).modal('hide');
                        });                    
                    break;                    
            }            
        }        
    },

    /* Role Page Relation */
    rolePageRelation: {
        regionList: '#lstRolePageRelation',

        // List RolePageRelation
        list: function (roleId, roleName) {
            
            function loadPage() {                
                var d = $.Deferred();
                $.ajax({ url: config.serviceBase.pageList, data: { keyword: '' }, type: 'POST' }, function () { })
                    .done(function (res) {                        
                        if (res.Success && res.Data.length > 0) {
                            $.Mustache.load(config.html.role + '?v=' + config.version).done(function () {
                                $(config.modal.content).html('');
                                $(config.modal.content).mustache(config.template.listRolePageRelation, res);
                                                                   
                                d.resolve();                                    
                            });
                        }
                    });
                return d.promise();                
            }

            $.when(loadPage()).then(function () {
                $.ajax({ url: config.serviceBase.pageListByRole, data: { RoleId: roleId }, type: 'POST' }, function () { })
                    .done(function (res) {                        
                        if (res.Success && res.Data.length > 0) {
                            $.each(res.Data, function (i, val) {
                                
                                $.each($(hrm.rolePageRelation.regionList + ' table tbody tr'), function () {
                                
                                    var menuId = parseInt($(this).attr('id'));
                                    var checkBox = $($(this)).find('input[type=checkbox]');
                                        
                                    if (menuId === val.PageId) {
                                        checkBox.prop('checked', true);
                                        $(checkBox.parent()).addClass('checked');
                                    }                                                                            
                                });
                            });                                                                                                            
                        }
                        
                        // Slim Scroll & Uniform                                
                        $(hrm.rolePageRelation.regionList).slimScroll({ height: '500px' });                        
                        common.registerUniform();

                        $('#rolePageRelation').html(String.format('Role "{0}" <small class="display-block">You select page & Press Save to set permissions for role</small>', roleName));
                        utils.setHiddenField('RoleId', roleId); // Important When Save Role Page Relation

                        $(config.modal.id).modal('show');                        
                    });                
            });            
        },
        
        // Save RolePageRelation
        save: function () {
            var pageSelection = [];
            
            $.each($($(hrm.rolePageRelation.regionList + ' table tbody tr')), function () {
                var menuId = parseInt($(this).attr('id'));
                var isChecked = $(this).find('td:eq(0) span').hasClass('checked');
                
                if (menuId > 0 && isChecked) 
                    pageSelection.push(menuId);
            });

            if (pageSelection.length > 0) {
                console.log(pageSelection);
                $.ajax({ url: config.serviceBase.pageSaveRolePageRelation, data: { RoleId: utils.getHiddenField('RoleId'), LstId: pageSelection }, type: 'POST' }, function () { })
                    .done(function (res) {
                        var message = res.Success ? 'Decentralized page for roles success' : 'Error decentralized page for roles';
                        var status = res.Success ? 'growl-success' : 'growl-error';
                        $.jGrowl(message, { sticky: false, theme: status, header: 'Message' });
                        
                        $(config.modal.id).modal('hide');

                        // Refresh list role
                        hrm.role.list(config.defaultPageIndex);
                    });
            } else 
                $.jGrowl('You need to select the page for roles', { sticky: false, theme: 'growl-warning', header: 'Message' });
        },
        
        // Manipulate Checkbox On Grid
        toogleCheckbox: function (obj) {
            var classId = $(obj).parents('tr:first').attr('id');
            var isChecked = $(obj).parent().hasClass('  ');

            if (classId == undefined) {
                // All Checkbox
                var element = $(obj).parents('table:first').parent().attr('id');
                if (element != undefined)
                    $.each($($('#' + element + ' table tbody tr')), function () {
                        $(this).find('td:eq(0) input[type=checkbox]').prop('checked', !isChecked);
                    });
            } else // Single Or Multiple Checkbox                
                $('.m' + classId).prop('checked', !isChecked);

            $.uniform.update();
        }
    },
    
    /* Page */
    page: {
        init: function () {            
            // List            
            hrm.page.list();

            // Register Event           
            $(config.txtSearch).keypress(function (event) {
                if (event.which == 13) {
                    hrm.page.list();
                    event.preventDefault();
                }
            });
        },

        // List
        list: function (pageIndex) {
            var params = { keyword: $(config.txtSearch).val() };
            $.ajax({ url: config.serviceBase.pageList, data: params, type: 'POST' }, function () { })
                .done(function (res) {
                    if (res.Success && res.Data.length > 0) {
                        $.Mustache.load(config.html.page + '?v=' + config.version)
                            .done(function () {
                                $(config.tableBody).html('');
                                $(config.tableBody).mustache(config.template.listPage, res);

                                $('.panel-heading span').text(res.Data.length + ' page');
                                $('[data-hover="dropdown"]').dropdownHover();
                                $('.tip').tooltip();
                            });
                    } else 
                        $(config.tableBody).html(common.noData(8));                    
                });
        },

        // Action        
        manipulate: function (action, id, name) {
            var params = {};
            if (id != null)
                params = { PageId: id, Action: action };

            if (action == config.action.create || action == config.action.getById)
                $(config.modal.dialog).removeClass('modal-sm');

            switch (action) {
                case config.action.create:
                    $(config.modal.content).html('');
                    $(config.modal.content).mustache(config.template.createPage);

                    common.spinner(config.modal.form + ' [name*=Priority]');
                    common.staticDropDownList(config.ddlStatus, config.data.statuses, 1, null, null, true);
                    common.dynamicDropDownList(config.ddlPageParent, 0, { id: 0, text: '-- Root --' }, null, true);

                    $(config.modal.id).modal('show');
                    break;

                case config.action.getById:
                    $(config.modal.content).html('');

                    $.ajax({ url: config.serviceBase.pageAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $(config.modal.content).mustache(config.template.getPage, res);

                                common.spinner(config.modal.form + ' [name*=Priority]');
                                common.staticDropDownList(config.ddlStatus, config.data.statuses, res.Data.RecordStatus, null, null, true);
                                common.dynamicDropDownList(config.ddlPageParent, res.Data.ParentId, { id: 0, text: '-- Root --' }, null, true);

                                $(config.modal.id).modal('show');
                            }
                        });
                    break;

                case config.action.save:
                    params = $.extend(params, $(config.modal.form).serializeObject(), { ParentId: $(config.ddlPageParent).val(), RecordStatus: $(config.ddlStatus).val() });
                    $.ajax({ url: config.serviceBase.pageAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $.jGrowl('Save page successfully information.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                $(config.modal.id).modal('hide');
                                hrm.page.list();
                            }
                        });
                    break;

                case config.action.del:
                    $.ajax({ url: config.serviceBase.pageAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $.jGrowl('Delete page successfully.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                hrm.page.list();
                            }
                            $(config.modal.id).modal('hide');
                        });
                    break;
            }
        }
    },
    
    /* User */
    user: {
        init: function () {
            // Dropdownlist
            common.staticDropDownList(config.ddlPageSize, config.data.pageSize, config.defaultPageSize, null, '65px', true);
            common.staticDropDownList(config.ddlSearchStatus, config.data.statuses, -1, { id: -1, text: '-- Status --' }, '120px', true);

            // List
            hrm.user.list(config.defaultPageIndex);

            // Register Event
            $(config.ddlSearchStatus + ', ' + config.ddlPageSize).change(function () {
                hrm.user.list(config.defaultPageIndex);
            });

            $(config.txtSearch).keypress(function (event) {
                if (event.which == 13) {
                    hrm.user.list(config.defaultPageIndex);
                    event.preventDefault();
                }
            });
        },

        // List
        list: function (pageIndex) {
            var params = { keyword: $(config.txtSearch).val(), status: $(config.ddlSearchStatus).val(), pageIndex: pageIndex, pageSize: $(config.ddlPageSize).val() };
            $.ajax({ url: config.serviceBase.userList, data: params, type: 'POST' }, function () { })
                .done(function (res) {                
                    if (res.Success && res.Data.length > 0) {
                        utils.setHiddenField('CurrentPageIndex', pageIndex);

                        $.Mustache.load(config.html.user + '?v=' + config.version)
                            .done(function () {
                                $(config.tableBody).html('');
                                $(config.tableBody).mustache(config.template.listUser, res);

                                $(config.pagerInfo).html(res.PagerInfo);
                                $(config.pager).html(res.Pager);

                                $('.panel-heading span').text(res.PagerInfo);
                                $('[data-hover="dropdown"]').dropdownHover();
                                $('.tree-page').popover({ html: true, show: true });
                                $('.tip').tooltip();
                            });
                    } else {
                        $(config.tableBody).html(common.noData(6));
                        $(config.pagerInfo).html('');
                        $(config.pager).html('');
                    }
                });
        },

        // Action        
        manipulate: function (action, id, name) {
            var params = {};
            if (id != null)
                params = { UserId: id, Action: action };

            if (action == config.action.create || action == config.action.getById)
                $(config.modal.dialog).removeClass('modal-sm');
            
            switch (action) {
                case config.action.create:                    
                    $(config.modal.content).html('');
                    $(config.modal.content).mustache(config.template.createUser);

                    common.staticDropDownList(config.ddlStatus, config.data.statuses, 1, null, null, true);                    

                    $(config.modal.id).modal('show');
                    break;

                case config.action.getById:
                    $(config.modal.content).html('');
                    
                    $.ajax({ url: config.serviceBase.userAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $(config.modal.content).mustache(config.template.getUser, res);
                                common.staticDropDownList(config.ddlStatus, config.data.statuses, res.Data.RecordStatus, null, null, true);                                
                                $(config.modal.id).modal('show');
                            }
                        });
                    break;

                case config.action.save:
                    params = $.extend(params, $(config.modal.form).serializeObject(), { RecordStatus: $(config.ddlStatus).val() });
                    $.ajax({ url: config.serviceBase.userAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $.jGrowl('Save profile success.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                $(config.modal.id).modal('hide');
                                hrm.user.list(utils.getHiddenField('CurrentPageIndex'));
                            }
                        });
                    break;

                case config.action.del:
                    $.ajax({ url: config.serviceBase.userAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $.jGrowl('Delete user succeed.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                hrm.user.list(utils.getHiddenField('CurrentPageIndex'));
                            }
                            $(config.modal.id).modal('hide');
                        });
                    break;

                case config.action.update:
                    $.each(($('#lstRole tbody tr')), function () {
                        var userId = utils.getHiddenField('UserId');
                        var roleId = parseInt($(this).attr('id'));                        
                        var isChecked = $(this).find('td:eq(1) span').hasClass('checked');
                        
                        if (roleId > 0 && userId > 0 && isChecked) {
                            params = $.extend(params, { RoleId: roleId, UserId: userId });
                            //console.log(params);

                            $.ajax({ url: config.serviceBase.userAction, data: params, type: 'POST' }, function () { })
                            .done(function (res) {
                                if (res.Success) {
                                    $.jGrowl('Rights role successfully updated.', { sticky: false, theme: 'growl-success', header: 'Message' });

                                    // Refresh list
                                    hrm.user.list(config.defaultPageIndex);
                                }
                                $(config.modal.id).modal('hide');
                            });
                        }
                    });
                    break;
            }
        },

        // Chage Password
        loadFormChangePassword: function () {
            $.Mustache.load(config.html.user + '?v=' + config.version)
                .done(function () {
                    $(config.modal.content).html('');
                    $(config.modal.content).mustache(config.template.changePasswordUser);

                    $(config.modal.id).modal('show');
                });
        },

        changePassword: function () {
            var params = $.extend($(config.modal.form).serializeObject(), { 'Action': config.action.change });
            $.ajax({ url: config.serviceBase.userAction, data: params, type: 'POST' }, function () { })
                .done(function (res) {                    
                    if (res.Success) {
                        $.jGrowl(res.Message, { sticky: false, theme: 'growl-success', header: 'Message' });
                        $(config.modal.id).modal('hide');                        
                    }                                                       
                    else 
                        $.jGrowl(res.Message, { sticky: false, theme: 'growl-error', header: 'Message' });                            
                });
        }
    },

    /* Department */
    department: {
        init: function () {
            // List            
            hrm.department.list();

            // Register Event           
            $(config.txtSearch).keypress(function (event) {
                if (event.which == 13) {
                    hrm.department.list();
                    event.preventDefault();
                }
            });
        },

        // List
        list: function (pageIndex) {
            var params = { keyword: $(config.txtSearch).val() };
            $.ajax({ url: config.serviceBase.departmentList, data: params, type: 'POST' }, function () { })
                .done(function (res) {
                    if (res.Success && res.Data.length > 0) {
                        $.Mustache.load(config.html.department + '?v=' + config.version)
                            .done(function () {
                                $(config.tableBody).html('');
                                $(config.tableBody).mustache(config.template.listDepartment, res);

                                $('.panel-heading span').text(res.Data.length + ' department');
                                $('[data-hover="dropdown"]').dropdownHover();
                                $('.tip').tooltip();
                            });
                    } else
                        $(config.tableBody).html(common.noData(7));
                });
        },

        // Action        
        manipulate: function (action, id, name) {
            var params = {};
            if (id != null)
                params = { DepartmentId: id, Action: action };

            if (action == config.action.create || action == config.action.getById)
                $(config.modal.dialog).removeClass('modal-sm');

            switch (action) {
                case config.action.create:                    
                    $(config.modal.content).html('');
                    $(config.modal.content).mustache(config.template.createDepartment);

                    common.spinner(config.modal.form + ' [name*=Priority]');
                    common.staticDropDownList(config.ddlStatus, config.data.statuses, 1, null, null, true);
                    common.dynamicDropDownList(config.ddlDepartmentParent, 0, { id: 0, text: '-- Root --' }, null, true);

                    $(config.modal.id).modal('show');
                    break;

                case config.action.getById:
                    $(config.modal.content).html('');

                    $.ajax({ url: config.serviceBase.departmentAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $(config.modal.content).mustache(config.template.getDepartment, res);

                                common.spinner(config.modal.form + ' [name*=Priority]');
                                common.staticDropDownList(config.ddlStatus, config.data.statuses, res.Data.RecordStatus, null, null, true);
                                common.dynamicDropDownList(config.ddlDepartmentParent, res.Data.ParentId, { id: 0, text: '-- Root --' }, null, true);

                                $(config.modal.id).modal('show');
                            }
                        });
                    break;

                case config.action.save:
                    params = $.extend(params, $(config.modal.form).serializeObject(), { ParentId: $(config.ddlDepartmentParent).val(), RecordStatus: $(config.ddlStatus).val() });
                    $.ajax({ url: config.serviceBase.departmentAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $.jGrowl('Save successful departmental information.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                $(config.modal.id).modal('hide');
                                hrm.department.list();
                            }
                        });
                    break;

                case config.action.del:
                    $.ajax({ url: config.serviceBase.departmentAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $.jGrowl('Delete successful departments.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                hrm.department.list();
                            }
                            $(config.modal.id).modal('hide');
                        });
                    break;
            }
        }
    },

    /* Position */
    position: {
        init: function () {            
            // Dropdownlist
            common.staticDropDownList(config.ddlPageSize, config.data.pageSize, config.defaultPageSize, null, '65px', true);

            // List            
            hrm.position.list(config.defaultPageIndex);

            // Register Event
            $(config.ddlPageSize).change(function () {
                hrm.position.list(config.defaultPageIndex);
            });

            $(config.txtSearch).keypress(function (event) {
                if (event.which == 13) {
                    hrm.position.list(config.defaultPageIndex);
                    event.preventDefault();
                }
            });
        },

        // List
        list: function (pageIndex) {            
            var params = { keyword: $(config.txtSearch).val(), pageIndex: pageIndex, pageSize: $(config.ddlPageSize).val() };

            $.ajax({ url: config.serviceBase.positionList, data: params, type: 'POST' }, function () { })
                .done(function (res) {
                    if (res.Success && res.Data.length > 0) {
                        utils.setHiddenField('CurrentPageIndex', pageIndex);

                        $.Mustache.load(config.html.position + '?v=' + config.version)
                            .done(function () {                                
                                $(config.tableBody).html('');
                                $(config.tableBody).mustache(config.template.listPosition, res);

                                $(config.pagerInfo).html(res.PagerInfo);
                                $(config.pager).html(res.Pager);

                                $('.panel-heading span').text(res.PagerInfo);
                                $('[data-hover="dropdown"]').dropdownHover();
                                $('.tip').tooltip();
                            });
                    } else {
                        $(config.tableBody).html(common.noData(7));
                        $(config.pagerInfo).html('');
                        $(config.pager).html('');
                    }
                });
        },

        // Action        
        manipulate: function (action, id, name) {
            var params = {};
            if (id != null)
                params = { PositionId: id, Action: action };

            if (action == config.action.create || action == config.action.getById) 
                $(config.modal.dialog).removeClass('modal-sm');            

            switch (action) {
                case config.action.create:
                    $(config.modal.content).html('');
                    $(config.modal.content).mustache(config.template.createPosition);

                    common.staticDropDownList(config.ddlStatus, config.data.statuses, 1, null, null, true);

                    $(config.modal.id).modal('show');
                    break;

                case config.action.getById:
                    $(config.modal.content).html('');

                    $.ajax({ url: config.serviceBase.positionAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            if (res.Success) {
                                $(config.modal.content).mustache(config.template.getPosition, res);

                                common.staticDropDownList(config.ddlStatus, config.data.statuses, res.Data.RecordStatus, null, null, true);

                                $(config.modal.id).modal('show');
                            }
                        });
                    break;  

                case config.action.save:
                    params = $.extend(params, $(config.modal.form).serializeObject(), { RecordStatus: $(config.ddlStatus).val() });
                    $.ajax({ url: config.serviceBase.positionAction, data: params, type: 'POST' }, function () { })
                        .done(function (res) {
                            debugger;
                            if (res.Success) {
                                $.jGrowl('Save information position success.', { sticky: false, theme: 'growl-success', header: 'Message' });
                                $(config.modal.id).modal('hide');
                                hrm.position.list(utils.getHiddenField('CurrentPageIndex'));
                            }
                        });
                    break;

                case config.action.del:
                    $.ajax({ url: config.serviceBase.positionAction, data: params, type: 'POST' }, function () { })
                    .done(function (res) {
                        if (res.Success) {
                            $.jGrowl('Delete position success.', { sticky: false, theme: 'growl-success', header: 'Message' });
                            hrm.position.list(config.defaultPageIndex);
                        }
                        $(config.modal.id).modal('hide');
                    });
                    break;
            }
        }
    }
};