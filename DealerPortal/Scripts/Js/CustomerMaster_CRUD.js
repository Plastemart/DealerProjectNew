var selectedRow = null

function GetHtml() {
    return "<tr><td class='Id'>$[Id]</td>"
        + "<td class='ddlAddrsType'>$[ddlAddrsType]</td>"
        + "<td class='txtAddress'>$[txtAddress]</td>"
        + "<td class='txtCity'>$[txtCity]</td>"
        + "<td class='txtState'>$[txtState]</td>"
        + "<td class='txtCountry'>$[txtCountry]</td>"
        + "<td class='txtPinCode'>$[txtPinCode]</td>"
        + "<td class='txtEmail'>$[txtEmail]</td>"
        + "<td class='txtMobile'>$[txtMobile]</td>"
        + "<td class='action'><a href='#' id='A_Edit'> $[action] </a> <a href='#' id='A_delete'>$[Delete]</a></td></tr>";
}

function onFormSubmit() {
    if (validate()) {
        var formData = readFormData();
        if (selectedRow == null)
            insertNewRecord(formData);
        else
            updateRecord(formData);
        resetForm();
    }
}

function readFormData() {
    var formData = {};
    formData["fullName"] = $('input[name="name"]').val().trim();
    formData["empCode"] = $('input[name="name"]').val().trim();
    formData["salary"] = $('input[name="name"]').val().trim();
    formData["city"] = $('input[name="name"]').val().trim();
    return formData;
}

$("form#addUser").submit(function () {
    var user = {};
    var nameInput = $('input[name="name"]').val().trim();
    var addressInput = $('input[name="address"]').val().trim();
    var ageInput = $('input[name="age"]').val().trim();
    if (nameInput && addressInput && ageInput) {
        $(this).serializeArray().map(function (data) {
            user[data.name] = data.value;
        });
        var lastUser = users[Object.keys(users).sort().pop()];
        user.id = lastUser.id + 1;
            
        addUser(user);
    } else {
        alert("All fields must have a valid value.");
    }
});

function insertNewRecord(data) {
    var table = document.getElementById("employeeList").getElementsByTagName('tbody')[0];
    var newRow = table.insertRow(table.length);
    cell1 = newRow.insertCell(0);
    cell1.innerHTML = data.fullName;
    cell2 = newRow.insertCell(1);
    cell2.innerHTML = data.empCode;
    cell3 = newRow.insertCell(2);
    cell3.innerHTML = data.salary;
    cell4 = newRow.insertCell(3);
    cell4.innerHTML = data.city;
    cell4 = newRow.insertCell(4);
    cell4.innerHTML = `<a onClick="onEdit(this)">Edit</a>
                       <a onClick="onDelete(this)">Delete</a>`;
}

function resetForm() {
    document.getElementById("fullName").value = "";
    document.getElementById("empCode").value = "";
    document.getElementById("salary").value = "";
    document.getElementById("city").value = "";
    selectedRow = null;
}

function onEdit(td) {
    selectedRow = td.parentElement.parentElement;
    document.getElementById("fullName").value = selectedRow.cells[0].innerHTML;
    document.getElementById("empCode").value = selectedRow.cells[1].innerHTML;
    document.getElementById("salary").value = selectedRow.cells[2].innerHTML;
    document.getElementById("city").value = selectedRow.cells[3].innerHTML;
}
function updateRecord(formData) {
    selectedRow.cells[0].innerHTML = formData.fullName;
    selectedRow.cells[1].innerHTML = formData.empCode;
    selectedRow.cells[2].innerHTML = formData.salary;
    selectedRow.cells[3].innerHTML = formData.city;
}

function onDelete(td) {
    if (confirm('Are you sure to delete this record ?')) {
        row = td.parentElement.parentElement;
        document.getElementById("employeeList").deleteRow(row.rowIndex);
        resetForm();
    }
}
function validate() {
    isValid = true;
    var ddlAddrsType = $("#ddlAddrsType");
    if (ddlAddrsType.val() == "" || ddlAddrsType.val() == "0" || ddlAddrsType.val() == "-1" || ddlAddrsType.val() == undefined || ddlAddrsType.val() == null) {
        //If the "Please Select" option is selected display error.
        alert("Please select an Address Type!");
        isValid = false;
    }
    var txtAddress = $("#txtAddress");
    if (txtAddress.val() == "" || txtAddress.val() == undefined || txtAddress.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        alert("Please add Address!");
        isValid = false;
    }
    var txtCity = $("#txtCity");
    if (txtCity.val() == "" || txtCity.val() == undefined || txtCity.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        alert("Please add City!");
        isValid = false;
    }
    var txtPinCode = $("#txtPinCode");
    if (txtPinCode.val() == "" || txtPinCode.val() == undefined || txtPinCode.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        alert("Please add PinCode!");
        isValid = false;
    }
    var txtEmail = $("#txtEmail");
    if (txtEmail.val() == "" || txtEmail.val() == undefined || txtEmail.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        alert("Please add Email!");
        isValid = false;
    }
    var txtMobile = $("#txtMobile");
    if (txtMobile.val() == "" || txtMobile.val() == undefined || txtMobile.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        alert("Please add contact details!");
        isValid = false;
    }
    return isValid;
}