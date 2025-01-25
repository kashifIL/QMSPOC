$(function () {
    var l = abp.localization.getResource("QMSPOC");
	
	var itemBomService = window.qMSPOC.itemBoms.itemBoms;
	
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
        viewUrl: abp.appPath + "ItemBoms/CreateModal",
        scriptUrl: abp.appPath + "Pages/ItemBoms/createModal.js",
        modalClass: "itemBomCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "ItemBoms/EditModal",
        scriptUrl: abp.appPath + "Pages/ItemBoms/editModal.js",
        modalClass: "itemBomEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            code: $("#CodeFilter").val(),
			versionMin: $("#VersionFilterMin").val(),
			versionMax: $("#VersionFilterMax").val(),
			description: $("#DescriptionFilter").val(),
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
                                visible: abp.auth.isGranted('QMSPOC.ItemBoms.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.itemBom.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('QMSPOC.ItemBoms.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    itemBomService.delete(data.record.itemBom.id)
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
			{ data: "itemBom.code" },
			{ data: "itemBom.version" },
			{ data: "itemBom.description" },
            {
                data: "item.code",
                
                defaultContent : ""
            }        
    ];
    
    
        var showDetailRows = abp.auth.isGranted('QMSPOC.ItemBomDetails') ;
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
    
    if(abp.auth.isGranted('QMSPOC.ItemBoms.Delete')) {
        dataTableColumns.unshift({
                targets: 0,
                data: null,
                orderable: false,
                className: 'select-checkbox',
                width: "0.5rem",
                render: function (data) {
                    return '<input type="checkbox" class="form-check-input select-row-checkbox" data-id="' + data.itemBom.id + '"/>'
            }
        });
    }
    else {
        $("#BulkDeleteCheckboxTheader").remove();
    }

    var dataTable = $("#ItemBomsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[3, "asc"]],
        ajax: abp.libs.datatables.createAjax(itemBomService.getList, getFilter),
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
                       
                       itemBomService.deleteAll(getFilter())
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

                        itemBomService.deleteByIds(selectedRecordsIds)
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

    $("#NewItemBomButton").click(function (e) {
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

        itemBomService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/app/item-boms/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'code', value: input.code },
                            { name: 'versionMin', value: input.versionMin },
                            { name: 'versionMax', value: input.versionMax }, 
                            { name: 'description', value: input.description }, 
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
    
    
    
    
    
        $('#ItemBomsTable').on('click', 'td.details-control', function () {
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
        return $.ajax(abp.appPath + "ItemBoms/ChildDataGrid?itemBomId=" + data.itemBom.id)
            .done(function (result) {
                return result;
            });
    }
    
    function initDataGrids(data) {
        initItemBomDetailGrid(data)
    }
    
        function initItemBomDetailGrid(data) {
        if(!abp.auth.isGranted("QMSPOC.ItemBomDetails")) {
            return;
        }
        
        var itemBomId = data.itemBom.id;

        
        var itemBomDetailService = window.qMSPOC.itemBomDetails.itemBomDetails;

        var itemBomDetailCreateModal = new abp.ModalManager({
            viewUrl: abp.appPath + "ItemBomDetails/CreateModal",
            scriptUrl: abp.appPath + "Pages/ItemBomDetails/createModal.js",
            modalClass: "itemBomDetailCreate"
        });

        var itemBomDetailEditModal = new abp.ModalManager({
            viewUrl: abp.appPath + "ItemBomDetails/EditModal",
            scriptUrl: abp.appPath + "Pages/ItemBomDetails/editModal.js",
            modalClass: "itemBomDetailEdit"
        });

        var itemBomDetailDataTable = $("#ItemBomDetailsTable-" + itemBomId).DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            searching: false,
            responsive: true,
            scrollX: true,
            autoWidth: true,
            scrollCollapse: true,
            order: [[1, "asc"]],
            ajax: abp.libs.datatables.createAjax(itemBomDetailService.getListWithNavigationPropertiesByItemBomId, {
                itemBomId: itemBomId,
                maxResultCount: 5
            }),
            columnDefs: [
                {
                    rowAction: {
                        items:
                            [
                                {
                                    text: l("Edit"),
                                    visible: abp.auth.isGranted('QMSPOC.ItemBomDetails.Edit'),
                                    action: function (data) {
                                        itemBomDetailEditModal.open({
                                            id: data.record.itemBomDetail.id
                                        });
                                    }
                                },
                                {
                                    text: l("Delete"),
                                    visible: abp.auth.isGranted('QMSPOC.ItemBomDetails.Delete'),
                                    confirmMessage: function () {
                                        return l("DeleteConfirmationMessage");
                                    },
                                    action: function (data) {
                                        itemBomDetailService.delete(data.record.itemBomDetail.id)
                                            .then(function () {
                                                abp.notify.success(l("SuccessfullyDeleted"));
                                                itemBomDetailDataTable.ajax.reloadEx();
                                            });
                                    }
                                }
                            ]
                    },
                    width: "1rem"
                },
                { data: "itemBomDetail.qty", width: "0.1rem" },
			{ data: "itemBomDetail.uom", width: "0.1rem" },
            {
                data: "item.code",
                width: "0.1rem",
                defaultContent : ""
            }
            ]
        }));

        itemBomDetailCreateModal.onResult(function () {
            itemBomDetailDataTable.ajax.reloadEx();
        });

        itemBomDetailEditModal.onResult(function () {
            itemBomDetailDataTable.ajax.reloadEx();
        });

        $("button.NewItemBomDetailButton").off("click").on("click", function (e) {
            itemBomDetailCreateModal.open({
                itemBomId: $(this).data("itembom-id")
            });
        });
    }
    
    
});
