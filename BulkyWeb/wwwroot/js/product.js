var dataTable;


$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/product/getall'},
        "columns": [
            { data: 'title', width: "15%" },
            { data: 'isbn', width: "10%" },
            { data: 'priceList', width: "15%" },
            { data: 'author', width: "10%" },
            { data: 'category.name', width: "15%" },
            {
                data: 'id',
                render: function (data) {
                    return `
                <div class="text-center">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary btn-sm text-white" style="cursor:pointer; width:100px;">
                        Edit
                    </a>
                    <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger btn-sm text-white" style="width:70px;">
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
