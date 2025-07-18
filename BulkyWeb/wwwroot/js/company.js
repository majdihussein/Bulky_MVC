﻿var dataTable;


$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/company/getall'},
        "columns": [
            { data: 'name', width: "15%" },
            { data: 'streetAddress', width: "10%" },
            { data: 'city', width: "15%" },
            { data: 'state', width: "10%" },
            { data: 'phoneNumber', width: "15%" },
            {
                data: 'id',
                render: function (data) {
                    return `
                <div class="text-center">
                    <a href="/admin/company/upsert?id=${data}" class="btn btn-primary btn-sm text-white" style="cursor:pointer; width:100px;">
                        Edit
                    </a>
                    <a onClick=Delete('/admin/company/delete/${data}') class="btn btn-danger btn-sm text-white" style="width:70px;">
                        Delete
                    </a>
                </div>`;
                },
                width: "10%"
            }
        ]
    });
}

function Delete(url)
{
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) { 
                    dataTable.ajax.reload();
                    Toastr.success(data.message);
                }
            })
        }
    });
}
