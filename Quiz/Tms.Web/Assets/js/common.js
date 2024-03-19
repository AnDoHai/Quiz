/* # Config
================================================== */
config = {
    version: Math.random(),
    
    // Pagination
    defaultPageIndex: 1,
    defaultSmallPageSize: 5,
    defaultPageSize: 10,

    // Grid
    loading: '',
    table: '.dataTables_wrapper table',
    tableBody: '.dataTables_wrapper tbody',
    tableBodyTr: '.dataTables_wrapper tbody tr',
    tableFooter: '.datatable-footer',
    pagerInfo: '.dataTables_info',
    pager: '.dataTables_paginate ul',

    // Control
    txtSearch: '#txtSearch',
    ddlPageSize: '#ddlPageSize',
    ddlStatus: '#ddlStatus',
    ddlPageParent: '#ddlPageParent',
    ddlDepartmentParent: '#ddlDepartmentParent',

    ddlSearchStatus: '#ddlSearchStatus',

    // Action
    action: {
        create: 'create',
        save: 'save',
        update: 'update',
        change: 'change',
        del: 'delete',
        getById: 'getById',
        published: 'published',
        remove: 'remove',
        back: 'back',
        send: 'send'
    },

    // Service Base
    serviceBase: {
        roleList: '/api/Role/List',
        roleAction: '/api/Role/Action',

        pageList: '/api/Page/List',
        pageAction: '/api/Page/Action',
        pageListByRole: '/api/Page/ListByRole',
        pageSaveRolePageRelation: '/api/Page/SaveRolePageRelation',
        pageNavigator: '/api/Page/Navigator',

        userList: '/api/User/List',
        userAction: '/api/User/Action',        

        departmentList: '/api/Department/List',
        departmentAction: '/api/Department/Action',

        positionList: '/api/Position/List',
        positionAction: '/api/Position/Action'
    },

    // Html File Path
    html: {        
        role: '/Assets/js/template/role.html',
        page: '/Assets/js/template/page.html',
        user: '/Assets/js/template/user.html',
        department: '/Assets/js/template/department.html',
        position: '/Assets/js/template/position.html'
    },

    // Region On Html File
    template: {        
        listRole: 'tpl-list-role',
        listRoleTypeRadio: 'tpl-list-role-type-radio',
        createRole: 'tpl-form-role',
        getRole: 'tpl-get-role',
        listRolePageRelation: 'tpl-list-role-page-relation',

        listPage: 'tpl-list-page',
        createPage: 'tpl-form-page',
        getPage: 'tpl-get-page',

        listUser: 'tpl-list-user',
        createUser: 'tpl-form-user',
        getUser: 'tpl-get-user',
        changePasswordUser: 'tpl-change-password',

        listDepartment: 'tpl-list-department',
        createDepartment: 'tpl-form-department',
        getDepartment: 'tpl-get-department',

        listPosition: 'tpl-list-position',
        createPosition: 'tpl-form-position',
        getPosition: 'tpl-get-position'
    },

    // Modal In System
    modal: {
        id: '#formModal',
        content: '#formModal .modal-content',
        form: '#formModal .modal-content form',
        dialog: '#formModal .modal-dialog'
    },

    // Static Data
    data: {        
        pageSize: [
            { id: 10, text: '10' },
            { id: 25, text: '25' },
            { id: 50, text: '50' },
            { id: 100, text: '100' }],        

        statuses: [
            { id: 1, text: 'Active' },
            { id: 0, text: 'Stop' }],

        errorStatus: [
            { id: 1, text: 'Complete' },
            { id: 0, text: 'Pending' }]        
    }

};

/* # Common
================================================== */
common = {
    noData: function (colspan) {
        return String.format('<tr><td valign="top" colspan="{0}">No data. Please choose another search conditions.</td></tr>', colspan);
    },

    registerUniform: function () {
        $(".styled, .multiselect-container input").uniform({ radioClass: 'choice', selectAutoWidth: false });
    },

    spinner: function (controlName) {
        $(controlName).spinner();
    },

    hideStatusSelect: function() {
        $('.select2-hidden-accessible').hide();
    },
    
    // Navigator
    dynamicNavigator: function () {
        $.when(
            $.ajax({ url: config.serviceBase.pageNavigator, type: 'POST' }, function () { })
            .done(function (res) {
                if (res.Success)
                    $('.navigation').html(res.Data);
            })
        ).then(function () {
            // Init Navigator
            $('.navigation').find('li.active').parents('li').addClass('active');
            $('.navigation').find('li').not('.active').has('ul').children('ul').addClass('hidden-ul');
            $('.navigation').find('li').has('ul').children('a').parent('li').addClass('has-ul');
            $('.navigation').find('li').has('ul').children('a').on('click', function (e) {
                e.preventDefault();

                if ($('body').hasClass('sidebar-narrow')) {
                    $(this).parent('li > ul li').not('.disabled').toggleClass('active').children('ul').slideToggle(250);
                    $(this).parent('li > ul li').not('.disabled').siblings().removeClass('active').children('ul').slideUp(250);
                }

                else {
                    $(this).parent('li').not('.disabled').toggleClass('active').children('ul').slideToggle(250);
                    $(this).parent('li').not('.disabled').siblings().removeClass('active').children('ul').slideUp(250);
                }
            });

            // Active Navigator
            common.activeNavigator();
        });
    },

    // Active Navigator
    activeNavigator: function () {
        var splitUrl = document.URL.split('/');
        var isDefault = true;

        $.each(splitUrl, function (i, val) {
            var aTagActive = val.toLowerCase();

            $.each($('.navigation li a'), function () {                
                var aElement = $(this);
                var aTagId = aElement.attr('class');                
                if (aTagId == aTagActive) {
                    isDefault = false;                    
                    aElement.parent().addClass('active');
                }
            });
        });

        if (isDefault) 
            $('.navigation li:first').addClass('active');        

        // Dropdown navigator        
        if($('.navigation li.active').find('ul').length > 0)
            $('.navigation li.active').find('ul').css('display', 'block');
    },

    dynamicDropDownList: function (ddlName, defaultValue, specialValue, width, isMultiple) {
        var options = { width: (width == null) ? "100%" : width };
        //if (isMultiple)
        //    options = $.extend(options, { minimumResultsForSearch: "-1" });
        
        switch (ddlName) {
            case config.ddlPageParent:            
                $.ajax({ url: config.serviceBase.pageList, data: { keyword: '' }, type: 'POST' }, function () { })
                    .done(function (res) {
                        if (res.Success && res.Data.length > 0) {
                            var pages = $.map(res.Data, function (item) { return { id: item.PageId, text: item.Name }; });
                            if (specialValue != null)
                                pages.unshift(specialValue);

                            options = $.extend(options, { data: pages });
                            $(ddlName).select2(options);

                            if (defaultValue != null)
                                $(ddlName).select2('val', defaultValue);
                        }
                    });
                break;

            case config.ddlDepartmentParent:
                $.ajax({ url: config.serviceBase.departmentList, data: { keyword: '' }, type: 'POST' }, function () { })
                    .done(function (res) {                        
                        if (res.Success && res.Data.length > 0) {
                            var departments = $.map(res.Data, function (item) { return { id: item.DepartmentId, text: item.Name }; });
                            if (specialValue != null)
                                departments.unshift(specialValue);

                            options = $.extend(options, { data: departments });
                            $(ddlName).select2(options);

                            if (defaultValue != null)
                                $(ddlName).select2('val', defaultValue);
                        }
                    });
                break;
        }
    },

    staticDropDownList: function (ddlName, data, defaultValue, specialValue, width, isMultiple) {
        if (specialValue != null) {
            var isExist = false;
            for (var i = 0; i < data.length; i++) {
                if (data[i].id === -1)
                    isExist = true;
            }

            if (!isExist)
                data.unshift(specialValue);
        }

        var options = { width: width == undefined ? "100%" : width, data: data };
        if (isMultiple)
            options = $.extend(options, { minimumResultsForSearch: "-1" });

        $(ddlName).select2(options);

        if (defaultValue != null)
            $(ddlName).select2('val', defaultValue);
    },

    // Confirm Delete
    confirm: function(action, message) {
        var html = '';
        html += '<div class="modal-header">';
        html += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
        html += '<h4 class="modal-title"><i class="icon-question"></i> Message</h4>';
        html += '</div>';

        html += '<div class="modal-body with-padding">';
        html += message != undefined ? String.format('<p>{0}</p>', message) : '<p>Are you sure you want to delete this record?</p>';
        html += '</div>';

        html += '<div class="modal-footer">';
        html += '<button class="btn btn-default" tabindex="2" data-dismiss="modal">Close</button>';
        html += String.format('<button onclick="{0}" tabindex="1" class="btn btn-success">Ok</button>', action);
        html += '</div>';
        
        $(config.modal.dialog).removeClass('modal-lg').addClass('modal-sm');
        $(config.modal.content).html(html);
        $(config.modal.id).modal('show');
    },
             
    // Scroll To Top
    scrollToTop: function() {        
        $('body,html').animate({ scrollTop: 0 }, 800);
        return false;
    }    
};