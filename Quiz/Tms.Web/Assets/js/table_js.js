$(function () {
    /* # Data tables
      ================================================== */

    //===== Setting Datatable defaults =====//

    $.extend($.fn.dataTable.defaults, {
        autoWidth: false,
        pagingType: 'full_numbers',
        dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
        language: {
            search: '<span>Filter:</span> _INPUT_',
            lengthMenu: '<span>Show:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': '>', 'previous': '<' }
        }
    });

    //===== Default datatable =====//
    $('.datatable table').dataTable();

    //===== Datatable with pager =====//
    $('.datatable-pager table').dataTable({
        pagingType: 'simple',
        language: {
            paginate: { 'next': 'Next →', 'previous': '← Previous' }
        }
    });



    //===== Media datatable =====//
    $('.datatable-media table').dataTable({
        columnDefs: [{
            orderable: false,
            targets: [0]
        }],
        order: [[1, 'asc']]
    });



    //===== Custom sort columns =====//
    $('.datatable-custom-sort table').dataTable({
        columnDefs: [{
            orderable: false,
            targets: [0, 2]
        }],
        order: [[1, 'asc']]
    });



    //===== Invoices datatable =====//
    $('.datatable-invoices table').dataTable({
        columnDefs: [{
            orderable: false,
            targets: [1, 6]
        }],
        order: [[0, 'desc']]
    });



    //===== Tasks datatable =====//
    $('.datatable-tasks table').dataTable({
        columnDefs: [{
            orderable: false,
            targets: [5]
        }]
    });



    //===== Saving state =====//
    $('.datatable-ajax-source table').dataTable({
        ajax: 'media/datatable_ajax_source.txt'
    });



    //===== Saving state =====//
    $('.datatable-state-saving table').dataTable({
        stateSave: true
    });



    //===== Datatable with selectable rows =====//
    $('.datatable-selectable table').dataTable({
        dom: '<"datatable-header"Tfl>t<"datatable-footer"ip>',
        tableTools: {
            sRowSelect: 'multi',
            aButtons:
            [{
                sExtends: 'collection',
                sButtonText: 'Tools <span class="caret"></span>',
                sButtonClass: 'btn btn-primary',
                aButtons: ['select_all', 'select_none']
            }]
        }
    });

    //===== Datatable with tools =====//
    $('.datatable-tools table').dataTable({
        dom: '<"datatable-header"Tfl>t<"datatable-footer"ip>',
        tableTools: {
            sRowSelect: "single",
            sSwfPath: "media/swf/copy_csv_xls_pdf.swf",
            aButtons: [
                {
                    sExtends: 'copy',
                    sButtonText: 'Copy',
                    sButtonClass: 'btn btn-default'
                },
                {
                    sExtends: 'print',
                    sButtonText: 'Print',
                    sButtonClass: 'btn btn-default'
                },
                {
                    sExtends: 'collection',
                    sButtonText: 'Save <span class="caret"></span>',
                    sButtonClass: 'btn btn-primary',
                    aButtons: ['csv', 'xls', 'pdf']
                }
            ]
        }
    });



    //===== Datatable with custom column filtering =====//
    // Setup - add a text input to each footer cell
    $('.datatable-add-row table tfoot th').each(function () {
        var title = $('.datatable-add-row table thead th').eq($(this).index()).text();
        $(this).html('<input type="text" class="form-control" placeholder="Filter ' + title + '" />');
    });

    // DataTable
    var table = $('.datatable-add-row table').DataTable();

    // Apply the filter
    $(".datatable-add-row table tfoot input").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    $('.dataTables_filter input[type=search]').attr('placeholder', 'Type to filter...');
});