var dataTable;


$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/order/getall?status=' + status},
        "columns": [
            { data: 'id', width: "15%" },
            { data: 'name', width: "10%" },
            { data: 'phoneNumber', width: "15%" },
            { data: 'applicationUser.email', width: "10%" },
            { data: 'orderStatus', width: "15%" },
            { data: 'orderTotal', width: "15%" },

            {
                data: 'id',
                render: function (data) {
                    return `
                <div class="text-center">
                    <a href="/admin/order/details?orderid=${data}" class="btn btn-primary btn-sm text-white" style="cursor:pointer; width:100px;">
                    Open</a>
                    
                </div>`;
                },
                width: "10%"
            }
        ]
    });
}
