$(function () {
    var l = abp.localization.getResource("QMSPOC");
	
	var itemCategoryService = window.qMSPOC.itemCategories.itemCategories;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "ItemCategories/CreateModal",
        scriptUrl: abp.appPath + "Pages/ItemCategories/createModal.js",
        modalClass: "itemCategoryCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "ItemCategories/EditModal",
        scriptUrl: abp.appPath + "Pages/ItemCategories/editModal.js",
        modalClass: "itemCategoryEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            code: $("#CodeFilter").val(),
			name: $("#NameFilter").val()
        };
    };
    
    
    
    var dataTableColumns = [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('QMSPOC.ItemCategories.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('QMSPOC.ItemCategories.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    itemCategoryService.delete(data.record.id)
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
			{ data: "code" },
			{ data: "name" }        
    ];
    
    
    
    
    if(abp.auth.isGranted('QMSPOC.ItemCategories.Delete')) {
        dataTableColumns.unshift({
                targets: 0,
                data: null,
                orderable: false,
                className: 'select-checkbox',
                width: "0.5rem",
                render: function (data) {
                    return '<input type="checkbox" class="form-check-input select-row-checkbox" data-id="' + data.id + '"/>'
            }
        });
    }
    else {
        $("#BulkDeleteCheckboxTheader").remove();
    }

    var dataTable = $("#ItemCategoriesTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[2, "asc"]],
        ajax: abp.libs.datatables.createAjax(itemCategoryService.getList, getFilter),
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
                       
                       itemCategoryService.deleteAll(getFilter())
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

                        itemCategoryService.deleteByIds(selectedRecordsIds)
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

    $("#NewItemCategoryButton").click(function (e) {
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

        itemCategoryService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/app/item-categories/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'code', value: input.code }, 
                            { name: 'name', value: input.name }
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
    
    
    
    
    
    
    
    
    
    
});
