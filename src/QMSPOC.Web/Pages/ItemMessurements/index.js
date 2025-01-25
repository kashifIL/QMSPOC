$(function () {
    var l = abp.localization.getResource("QMSPOC");
	
	var itemMessurementService = window.qMSPOC.itemMessurements.itemMessurements;
	
        var lastNpIdId = '';
        var lastNpDisplayNameId = '';

        var _lookupModal = new abp.ModalManager({
            viewUrl: abp.appPath + "Shared/LookupModal",
            scriptUrl: abp.appPath + "Pages/Shared/lookupModal.js",
            modalClass: "navigationPropertyLookup"
        });

        $('.lookupCleanButton').on('click', '', function () {
            $(this).parent().find('input').val('');
        });

        _lookupModal.onClose(function () {
            var modal = $(_lookupModal.getModal());
            $('#' + lastNpIdId).val(modal.find('#CurrentLookupId').val());
            $('#' + lastNpDisplayNameId).val(modal.find('#CurrentLookupDisplayName').val());
        });
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "ItemMessurements/CreateModal",
        scriptUrl: abp.appPath + "Pages/ItemMessurements/createModal.js",
        modalClass: "itemMessurementCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "ItemMessurements/EditModal",
        scriptUrl: abp.appPath + "Pages/ItemMessurements/editModal.js",
        modalClass: "itemMessurementEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            code: $("#CodeFilter").val(),
			version: $("#VersionFilter").val(),
			itemId: $("#ItemIdFilter").val()
        };
    };
    
    
    
    var dataTableColumns = [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('QMSPOC.ItemMessurements.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.itemMessurement.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('QMSPOC.ItemMessurements.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    itemMessurementService.delete(data.record.itemMessurement.id)
                                        .then(function () {
                                            abp.notify.success(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reloadEx();;
                                        });
                                }
                            }
                        ]
                },
                width: "1rem"
            },
			{ data: "itemMessurement.code" },
			{ data: "itemMessurement.version" },
            {
                data: "item.code",
                
                defaultContent : ""
            }        
    ];
    
    
        var showDetailRows = abp.auth.isGranted('QMSPOC.ItemMeasuremetnDetails') ;
    if(showDetailRows) {
        dataTableColumns.unshift({
            class: "details-control text-center",
            orderable: false,
            data: null,
            defaultContent: '<i class="fa fa-chevron-down"></i>',
            width: "0.1rem"
        });
    }
    else {
        $("#DetailRowTHeader").remove();
    }
    
    if(abp.auth.isGranted('QMSPOC.ItemMessurements.Delete')) {
        dataTableColumns.unshift({
                targets: 0,
                data: null,
                orderable: false,
                className: 'select-checkbox',
                width: "0.5rem",
                render: function (data) {
                    return '<input type="checkbox" class="form-check-input select-row-checkbox" data-id="' + data.itemMessurement.id + '"/>'
            }
        });
    }
    else {
        $("#BulkDeleteCheckboxTheader").remove();
    }

    var dataTable = $("#ItemMessurementsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[3, "asc"]],
        ajax: abp.libs.datatables.createAjax(itemMessurementService.getList, getFilter),
        columnDefs: dataTableColumns
    }));
    
    
    dataTable.on("xhr", function () {
       selectOrUnselectAllCheckboxes(false);
       showOrHideContextMenu();
        $("#select_all").prop("indeterminate", false);
        $("#select_all").prop("checked", false);
    });    
    
    function selectOrUnselectAllCheckboxes(selectAll) {
        $(".select-row-checkbox").each(function () {
            $(this).prop("checked", selectAll);
        });
    }


    $("#select_all").click(function () {
        if($(this).is(":checked")) {
            selectOrUnselectAllCheckboxes(true);
        }
        else {
            $(".select-row-checkbox").each(function () {
                selectOrUnselectAllCheckboxes(false);
            });
        }

        showOrHideContextMenu();
    });
    
    dataTable.on("change", "input[type='checkbox'].select-row-checkbox", function () {
       var unSelectedCheckboxes = $("input[type='checkbox'].select-row-checkbox:not(:checked)");
       
       if(unSelectedCheckboxes.length >= 1) {
           var dataRecordTotal = dataTable.context[0].json.data.length;
           if(unSelectedCheckboxes.length === dataRecordTotal) {
               $("#select_all").prop("indeterminate", false);
               $("#select_all").prop("checked", false);
           }
           else {
               $("#select_all").prop("indeterminate", true);
           }   
       }
       else {
           $("#select_all").prop("indeterminate", false);
           $("#select_all").prop("checked", true);
       }

        showOrHideContextMenu();
    });
    
    var showOrHideContextMenu = function () {
        var selectedCheckboxes = $("input[type='checkbox'].select-row-checkbox:is(:checked)");
        var selectedCheckboxCount = selectedCheckboxes.length;
        var dataRecordTotal = dataTable.context[0].json.data.length;
        var recordsTotal = dataTable.context[0].json.recordsTotal;
        
        if(selectedCheckboxCount >= 1) {
            $("#bulk-delete-context-menu").removeClass("d-none");
            
            $("#items-selected-info-message").html(
                selectedCheckboxCount === 1
                ? l("OneItemOnThisPageIsSelected")
                : l("NumberOfItemsOnThisPageAreSelected", selectedCheckboxCount));
            
            $("#items-selected-info-message").removeClass("d-none");
            
            if(selectedCheckboxCount === dataRecordTotal && recordsTotal > dataRecordTotal) {
                $("#select-all-items-btn").html(l("SelectAllItems", recordsTotal));
                $("#select-all-items-btn").removeClass("d-none");

                $("#select-all-items-btn").off("click");
                $("#select-all-items-btn").click(function () {
                    $(this).data("selected", true);
                    $(this).addClass("d-none");
                    $("#items-selected-info-message").html(l("AllItemsAreSelected", recordsTotal));
                    $("#clear-selection-btn").removeClass("d-none");
                });
                
                $("#clear-selection-btn").off("click");
                $("#clear-selection-btn").click(function () {
                    $("#select-all-items-btn").data("selected", false);
                    $("#select_all").prop("checked", false);
                    selectOrUnselectAllCheckboxes(false);
                    showOrHideContextMenu();
                });
                
            }
            else {
                $("#select-all-items-btn").addClass("d-none");
                $("#select-all-items-btn").data("selected", false);
                $("#clear-selection-btn").addClass("d-none");
            }
            
            $("#delete-selected-items").off("click");
            $("#delete-selected-items").click(function () {
                if($("#select-all-items-btn").data("selected") === true) {
                    abp.message.confirm(l("DeleteAllRecords"), function (confirmed) {
                       if(!confirmed) {
                           return;
                       } 
                       
                       itemMessurementService.deleteAll(getFilter())
                           .then(function () {
                               dataTable.ajax.reloadEx();
                               selectOrUnselectAllCheckboxes(false);
                               showOrHideContextMenu();
                           });
                    });
                }
                else {
                    var selectedCheckboxes = $("input[type='checkbox'].select-row-checkbox:is(:checked)");
                    var selectedRecordsIds = [];

                    for(var i = 0; i < selectedCheckboxes.length; i++)
                    {
                        selectedRecordsIds.push($(selectedCheckboxes[i]).data("id"));
                    }

                    abp.message.confirm(l("DeleteSelectedRecords", selectedCheckboxes.length), function (confirmed) {
                        if(!confirmed) {
                            return;
                        }

                        itemMessurementService.deleteByIds(selectedRecordsIds)
                            .then(function () {
                                dataTable.ajax.reloadEx();
                                selectOrUnselectAllCheckboxes(false);
                                showOrHideContextMenu();
                            });
                    });
                }
            });
        }
        else {
            $("#bulk-delete-context-menu").addClass("d-none");
            $("#select-all-items-btn").addClass("d-none");
            $("#items-selected-info-message").addClass("d-none");
            $("#clear-selection-btn").addClass("d-none");
        }
    };

    
    

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();        
    });

    $("#NewItemMessurementButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        itemMessurementService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/app/item-messurements/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'code', value: input.code }, 
                            { name: 'version', value: input.version }, 
                            { name: 'itemId', value: input.itemId }
                            ]);
                            
                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
            }
        )
    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
        var iconCss = $("#AdvancedFilterSection").is(":visible") ? "fa ms-1 fa-angle-up" : "fa ms-1 fa-angle-down";
        $(this).find("i").attr("class", iconCss);
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reloadEx();
            selectOrUnselectAllCheckboxes(false);
            showOrHideContextMenu();
        }
    });

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reloadEx();
        selectOrUnselectAllCheckboxes(false);
        showOrHideContextMenu();
    });
    
    
    
    
    
        $('#ItemMessurementsTable').on('click', 'td.details-control', function () {
        $(this).find("i").toggleClass("fa-chevron-down").toggleClass("fa-chevron-up");
        
        var tr = $(this).parents('tr');
        var row = dataTable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            var data = row.data();
            
            detailRows(data)
                .done(function (result) {
                    row.child(result).show();
                    initDataGrids(data);
                });

            tr.addClass('shown');
        }
    } );

    function detailRows (data) {
        return $.ajax(abp.appPath + "ItemMessurements/ChildDataGrid?itemMessurementId=" + data.itemMessurement.id)
            .done(function (result) {
                return result;
            });
    }
    
    function initDataGrids(data) {
        initItemMeasuremetnDetailGrid(data)
    }
    
        function initItemMeasuremetnDetailGrid(data) {
        if(!abp.auth.isGranted("QMSPOC.ItemMeasuremetnDetails")) {
            return;
        }
        
        var itemMessurementId = data.itemMessurement.id;

        
        var itemMeasuremetnDetailService = window.qMSPOC.itemMeasuremetnDetails.itemMeasuremetnDetails;

        var itemMeasuremetnDetailCreateModal = new abp.ModalManager({
            viewUrl: abp.appPath + "ItemMeasuremetnDetails/CreateModal",
            scriptUrl: abp.appPath + "Pages/ItemMeasuremetnDetails/createModal.js",
            modalClass: "itemMeasuremetnDetailCreate"
        });

        var itemMeasuremetnDetailEditModal = new abp.ModalManager({
            viewUrl: abp.appPath + "ItemMeasuremetnDetails/EditModal",
            scriptUrl: abp.appPath + "Pages/ItemMeasuremetnDetails/editModal.js",
            modalClass: "itemMeasuremetnDetailEdit"
        });

        var itemMeasuremetnDetailDataTable = $("#ItemMeasuremetnDetailsTable-" + itemMessurementId).DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            responsive: true,
            scrollX: true,
            autoWidth: true,
            scrollCollapse: true,
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(itemMeasuremetnDetailService.getListByItemMessurementId, {
                itemMessurementId: itemMessurementId,
                maxResultCount: 5
            }),
            columnDefs: [
                {
                    rowAction: {
                        items:
                            [
                                {
                                    text: l("Edit"),
                                    visible: abp.auth.isGranted('QMSPOC.ItemMeasuremetnDetails.Edit'),
                                    action: function (data) {
                                        itemMeasuremetnDetailEditModal.open({
                                            id: data.record.id
                                        });
                                    }
                                },
                                {
                                    text: l("Delete"),
                                    visible: abp.auth.isGranted('QMSPOC.ItemMeasuremetnDetails.Delete'),
                                    confirmMessage: function () {
                                        return l("DeleteConfirmationMessage");
                                    },
                                    action: function (data) {
                                        itemMeasuremetnDetailService.delete(data.record.id)
                                            .then(function () {
                                                abp.notify.success(l("SuccessfullyDeleted"));
                                                itemMeasuremetnDetailDataTable.ajax.reloadEx();
                                            });
                                    }
                                }
                            ]
                    },
                    width: "1rem"
                },
                { data: "type", width: "0.1rem" },
			{ data: "value", width: "0.1rem" },
			{ data: "uom", width: "0.1rem" }
            ]
        }));

        itemMeasuremetnDetailCreateModal.onResult(function () {
            itemMeasuremetnDetailDataTable.ajax.reloadEx();
        });

        itemMeasuremetnDetailEditModal.onResult(function () {
            itemMeasuremetnDetailDataTable.ajax.reloadEx();
        });

        $("button.NewItemMeasuremetnDetailButton").off("click").on("click", function (e) {
            itemMeasuremetnDetailCreateModal.open({
                itemMessurementId: $(this).data("itemmessurement-id")
            });
        });
    }
    
    
});
