var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { data: 'name', width: "15%" },
            { data: 'email', width: "10%" },
            { data: 'phoneNumber', width: "15%" },
            { data: 'company.name', width: "10%" },
            { data: 'role', width: "15%" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                render: function (data) {
                    var today = new Date().getTime();
                    var lockoutEnd = new Date(data.lockoutEnd).getTime();

                    if (lockoutEnd > today) {
                        // المستخدم مقفول
                        return `
                            <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-lock-fill"></i> Lock
                                </a>
                                <a href="/Admin/User/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-pencil-fill"></i> Permission
                                </a>
                            </div>
                        `;
                    } else {
                        // المستخدم غير مقفول
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-unlock-fill"></i> Unlock
                                </a>
                                <a href="/Admin/User/RoleManagement?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-pencil-fill"></i> Permission
                                </a>
                                
                            </div>
                        `;
                    }
                }
            }
        ]
    });
}


function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
               toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}