$(document).ready(function () {
    let i = 0;
    i--;
    var bid = "";
    var trid = "";
    let j = "Row";
    var tOgid = "";
    var ReverserID = -1;

    $('#ddlAddrsType').change(function () {
        $("#chkCopyAdd").prop('checked', false);
        if ($("#ddlAddrsType").val() == '1') {
            $("#chkCopyAdd").prop('disabled', true);
           
        }
        else $("#chkCopyAdd").prop('disabled', false);
    });
        
    $('#chkCopyAdd').change(function () {
        if ($('#chkCopyAdd').is(':checked')) {
            if ($("#ddlAddrsType").val() == '2') {
                var tempaddressArrIn = [];
                tempaddressArrIn.length = 0;
                $.each($("#tblOne tbody tr"), function () {
                    tempaddressArrIn.push({
                        AddressTypeID: $(this).find("td").eq(1).html(),
                        AddrsType: $(this).find("td").eq(2).html(),
                        Address: $(this).find("td").eq(3).html(),
                        Country: $(this).find("td").eq(4).html(),
                        CountryID: $(this).find("td").eq(5).html(),
                        State: $(this).find("td").eq(6).html(),
                        StateID: $(this).find("td").eq(7).html(),
                        City: $(this).find("td").eq(8).html(),
                        Cityid: $(this).find("td").eq(9).html(),
                        Pin: $(this).find("td").eq(10).html(),
                        Email: $(this).find("td").eq(11).html(),
                        Mobile: $(this).find("td").eq(12).html(),
                    });
                });
                //End Add
                var obj = $.grep(tempaddressArrIn, function (obj) {
                    return obj.AddressTypeID == 1;
                });

                if (obj.length == 0) {
                    $("#chkCopyAdd").prop('checked', false);
                    alert("First Please Add Invoice Address");
                    return false;
                }
                else {

                    $("#txtAddress").val(obj[0].Address);
                    $("#ddlCountry").val(obj[0].CountryID);

                    if (Number(obj[0].CountryID) == 98) {

                        FillState(obj[0].CountryID);
                        $("#ddlState").show();
                        $("#txtState").hide();
                        $("#ddlState").val(obj[0].StateID);
                        $("#txtState").val(obj[0].State);
                    }
                    else {
                        $("#ddlState").html('');
                        $("#ddlState").html('<option value="0" selected="selected">-- Select State --</option>');
                        $("#ddlState").hide();
                        $("#txtState").show();
                        $("#ddlState").val(obj[0].StateID);
                        $("#txtState").val(obj[0].State);
                    }

                    $("#txtCity").val(obj[0].City);
                    $("#hddCityId").val(obj[0].Cityid);
                    $("#txtState").val(obj[0].State);
                    $("#txtCountry").val(obj[0].Country);
                    $("#txtPinCode").val(obj[0].Pin);
                    $("#txtEmail").val(obj[0].Email);
                    $("#txtMobile").val(obj[0].Mobile);
                }
            }
        }
        else {
            ClearCopyovertext(); 
        }
    });
   
    $(document).on("click", "#btnAdd", function () {
        //$("#btnAdd").click(function () {
        var errorMessage = "";
        let values = validate();
        var valid = values[0];
        errorMessage = values[1];
        if (valid == true) {
            if ($("#btnAdd").text() === "Save") {
                //if ($("#btnAdd").text == 'Save') {
                //trid = '#' + trid;
                j = j + 1;
                let newHtml = GetHtml()

                    .replace("$[Id]", $('#hddPartyId').val())
                    .replace("$[RowID]", 'RowID_' + j)
                    //.replace("$[ddlAddrsId]", $(ddlAddrsId).val())
                    .replace("$[ddlAddrsTypeID]", $("#ddlAddrsType").val())
                    .replace("$[ddlAddrsType]", $("#ddlAddrsType option:selected").text())
                    .replace("$[txtAddress]", $("#txtAddress").val())
                    .replace("$[txtCountry]", $("#ddlCountry option:selected").text())
                    .replace("$[ddlCountryID]", $("#ddlCountry").val())
                    .replace("$[txtState]", $("#txtState").val())
                    .replace("$[ddlStateID]", $("#ddlState").val())
                    .replace("$[txtCity]", $("#txtCity").val())
                    .replace("$[hddCityId]", $("#hddCityId").val())
                    .replace("$[txtPinCode]", $("#txtPinCode").val())
                    .replace("$[txtEmail]", $("#txtEmail").val())
                    .replace("$[txtMobile]", $("#txtMobile").val())
                    .replace("$[PartyAddressID]", --ReverserID)
                    .replace("$[action]", "Edit")
                    //.replace("$[Delete]", "Delete");
                $("#table_td").append(newHtml);
                $("#ddlAddrsType").prop('disabled', false);
                Cleartext();
            }
            if ($("#btnAdd").text() === "Update") {
                trid = '#' + trid;

                $(trid).remove();
                //trid = '#' + trid;
                j = j + 1;
                //$("#txtId").show();
                //$("#txtId").val($(this).parent().parent().find('.Id').html());
                //$("#txtId").val($(this).parent().parent().find('.Id').html());
                let newHtml = GetHtml()
                    //.replace("$[Id]", $("#txtId").val())
                    //replace("$[Id]", $("#txtId").val())
                    .replace("$[Id]", $('#hddPartyId').val())
                    .replace("$[RowID]", 'RowID_' + j)
                    .replace("$[ddlAddrsTypeID]", $("#ddlAddrsType").val())
                    .replace("$[ddlAddrsType]", $("#ddlAddrsType option:selected").text())
                    .replace("$[txtAddress]", $("#txtAddress").val())
                    .replace("$[txtCountry]", $("#ddlCountry option:selected").text())
                    .replace("$[ddlCountryID]", $("#ddlCountry").val())
                    .replace("$[txtState]", $("#txtState").val())
                    .replace("$[ddlStateID]", $("#ddlState").val())
                    .replace("$[txtCity]", $("#txtCity").val())
                    .replace("$[hddCityId]", $("#hddCityId").val())
                    .replace("$[txtPinCode]", $("#txtPinCode").val())
                    .replace("$[txtEmail]", $("#txtEmail").val())
                    .replace("$[txtMobile]", $("#txtMobile").val())
                    .replace("$[PartyAddressID]", $("#hddPartyAddressID").val())
                    .replace("$[action]", "Edit")
                    //.replace("$[Delete]", "Delete");
                $("#table_td").append(newHtml);
                $("#ddlAddrsType").prop('disabled', false);
                Cleartext();
            }
        }
        else {
            errorMessage += " Enter All Fields<br/>";
            alert(errorMessage);
        }
    });

    $(document).on("click", "#A_delete", function () {
        if (confirm("Are you sure ? Do you want to delete this Address"))
            $(this).parents("tr").remove();
        Cleartext();
        return false;
    });

    $(document).on("click", "#A_Edit", function () {

        //$("#btnAdd").hide();
        //$("#btnUpdate").show();
        // $("#btnAdd").attr('value', 'Update')
        $("#btnAdd").html('Update');
        //$("#txtId").show();
        //$("#txtId").val($(this).parent().parent().find('.Id').html());
        //tOgid = $(this).parent().parent().find('.Id').html();
        bid = this.id; // button ID
        $("#ddlAddrsType").val($(this).parent().parent().find('.ddlAddrsTypeID').html());
        let ddlAddrsTypeID = parseInt($(this).parent().parent().find('.ddlAddrsTypeID').html()) + 1;
        trid = $(this).closest('tr').attr('id');///"Row" + tid;// table row ID 
        //$('#ddlAddrsType').prop('selectedIndex', ddlAddrsTypeID);
        //$("#ddlAddrsType option:selected").val($(this).parent().parent().find('.ddlAddrsType').html());
        $("#txtAddress").val($(this).parent().parent().find('.txtAddress').html());
        $("#ddlCountry").val($(this).parent().parent().find('.ddlCountryID').html());

        if (Number($(this).parent().parent().find('.ddlCountryID').html()) == 98) {

            FillState($("#ddlCountry").val());
            $("#ddlState").show();
            $("#txtState").hide();
            $("#ddlState").val($(this).parent().parent().find('.ddlStateID').html());
            $("#txtState").val($(this).parent().parent().find('.txtState').html());
        }
        else {
            $("#ddlState").html('');
            $("#ddlState").html('<option value="0" selected="selected">-- Select State --</option>');
            $("#ddlState").hide();
            $("#txtState").show();
            $("#ddlState").val($(this).parent().parent().find('.ddlStateID').html());
            $("#txtState").val($(this).parent().parent().find('.txtState').html());
        }
       
        $("#txtCity").val($(this).parent().parent().find('.txtCity').html());
        $("#hddCityId").val($(this).parent().parent().find('.hddCityId').html());
        $("#txtState").val($(this).parent().parent().find('.txtState').html());
        $("#txtCountry").val($(this).parent().parent().find('.txtCountry').html());
        $("#txtPinCode").val($(this).parent().parent().find('.txtPinCode').html());
        $("#txtEmail").val($(this).parent().parent().find('.txtEmail').html());
        $("#txtMobile").val($(this).parent().parent().find('.txtMobile').html());
        $("#hddPartyAddressID").val($(this).parent().parent().find('.PartyAddressID').html());
        $("#ddlAddrsType").prop('disabled', true);
        $("#chkCopyAdd").prop('disabled', true);
       
        //$(this).parents("tr").remove();
        //$("#btnAdd").focus();

    });


    $(document).on("click", "#btnCancel", function () {
        Cleartext();
    });

});

function Cleartext() {
    //$("#txtId").val("");
    $("#hddPartyAddressID").val('0');
    $("#ddlAddrsType").val('0');
    $("#txtAddress").val("");
    $("#ddlCountry").val("0");
    $("#ddlState").val("0");
    $("#txtCity").val("");
    $("#hddCityId").val('0');
    $("#txtState").val("");
    $("#ddlCountry").val(98);
    $("#txtPinCode").val("");
    $("#txtEmail").val("");
    $("#txtMobile").val("");
    //$('#ddlAddrsType').prop('selectedIndex', 1);
    $("#btnAdd").html('Save');
    $("#ddlAddrsType").prop('disabled', false);
    $('#chkCopyAdd').prop('checked', false);
    $("#chkCopyAdd").prop('disabled', false);

}
function ClearCopyovertext() {
    //$("#txtId").val("");
   
    $("#txtAddress").val("");
    $("#ddlCountry").val("0");
    $("#ddlState").val("0");
    $("#txtCity").val("");
    $("#hddCityId").val('0');
    $("#txtState").val("");
    $("#ddlCountry").val(98);
    $("#txtPinCode").val("");
    $("#txtEmail").val("");
    $("#txtMobile").val("");
    //$('#ddlAddrsType').prop('selectedIndex', 1);
    $("#btnAdd").html('Save');
  
   

}


function validate() {
    isValid = true;
    var errorCounter = 0;
    var errorMessage = "";

    var ddlAddrsType = $("#ddlAddrsType");
    if (ddlAddrsType.val() == "" || ddlAddrsType.val() == "0" || ddlAddrsType.val() == "-1" || ddlAddrsType.val() == undefined || ddlAddrsType.val() == null) {

        errorMessage = errorMessage + "Please select  Address Type! <br>";
        isValid = false;
    }

    if ($("#btnAdd").text() === "Save") {
        var flag = true;
        var ddlAddrsType = $("#ddlAddrsType");
        $.each($("#tblOne tbody tr"), function () {
            if (ddlAddrsType.val() == $(this).find("td").eq(1).html() && ddlAddrsType.val() == '1') {
                flag = false;

            }
            
        });

        if (flag == false) {
            errorMessage = errorMessage + "Invoice address already exists! <br>";
            isValid = false;
        }
    }


    
    var txtAddress = $("#txtAddress");
    if (txtAddress.val() == "" || txtAddress.val() == undefined || txtAddress.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        //alert("Please add Address!");
        errorMessage = errorMessage + "Please Enter Address! <br>";
        isValid = false;
    }
    var ddlCountry = $("#ddlCountry");
    if (ddlCountry.val() == "" || ddlCountry.val() == "0" || ddlCountry.val() == "-1" || ddlCountry.val() == undefined || ddlCountry.val() == null) {

        errorMessage = errorMessage + "Please Select Country! <br>";
        isValid = false;
    }
    var ddlCountrySelected = $("#ddlCountry");
    var ddlState = $("#ddlState");
    var txtState = $("#txtState");
    if (Number(ddlCountrySelected.val()) != 0) {
        if (Number(ddlCountrySelected.val()) == 98) {
            if (ddlState.val() == "" || ddlState.val() == "0" || ddlState.val() == "-1" || ddlState.val() == undefined || ddlState.val() == null) {

                errorMessage = errorMessage + "Please Select State! <br>";
                isValid = false;
            }
        }
        else {
            if (txtState.val() == "" ) {

                errorMessage = errorMessage + "Please Enter State Name! <br>";
                isValid = false;
            }
        }
    }
    var txtCity = $("#txtCity");
    if (txtCity.val() == "" || txtCity.val() == undefined || txtCity.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        //alert("Please add City!");
        errorMessage = errorMessage + "Please Enter City! <br>";

        isValid = false;
    }
    var txtPinCode = $("#txtPinCode");
    if (txtPinCode.val() == "" || txtPinCode.val() == undefined || txtPinCode.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        //alert("Please add PinCode!");
        errorMessage = errorMessage + "Please Enter PinCode! <br>";

        isValid = false;
    }
    var txtEmail = $("#txtEmail");
    if (txtEmail.val() != "" && txtEmail.val() != null && txtEmail.val() != undefined ) {
        if (ValidateEmail(txtEmail.val()) == false) {
            errorMessage = errorMessage + "Please Enter Proper Email! <br>";

            isValid = false;
        }
    }
    
    
    var txtMobile = $("#txtMobile");
    if (txtMobile.val() == "" || txtMobile.val() == undefined || txtMobile.val() == null) {
        isValid = false;
        //document.getElementById("fullNameValidationError").classList.remove("hide");
        //alert("Please add contact details!");
        errorMessage = errorMessage + "Please  Enter Phone No! <br>";

        isValid = false;
    }
    //if (isValid == false) {
    //    alert(errorMessage);

    //}
    // return isValid;
    return [isValid, errorMessage];
}
function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)) {
        return (true)
    }
    //alert("You have entered an invalid email address!")
    return (false)
}
//$("body").on("click", "#btnSubmit", function () {
//    //Loop through the Table rows and build a JSON array.

//});

function GetHtml() {

    return "<tr id='$[RowID]'><td style='display:none;' class='Id'>$[Id]</td>"
        + "<td style='display:none;' class='ddlAddrsTypeID'>$[ddlAddrsTypeID]</td>"
        + "<td class='ddlAddrsType'>$[ddlAddrsType]</td>"
        + "<td class='txtAddress'>$[txtAddress]</td>"
        + "<td class='txtCountry'>$[txtCountry]</td>"
        + "<td style='display:none;' class='ddlCountryID'>$[ddlCountryID]</td>"
        + "<td class='txtState'>$[txtState]</td>"
        + "<td style='display:none;' class='ddlStateID'>$[ddlStateID]</td>"
        + "<td class='txtCity'>$[txtCity]</td>"
        + "<td style='display:none;' class='hddCityId'>$[hddCityId]</td>"
        + "<td class='txtPinCode'>$[txtPinCode]</td>"
        + "<td class='txtEmail'>$[txtEmail]</td>"
        + "<td class='txtMobile'>$[txtMobile]</td>"
        + "<td style='display:none;' class='PartyAddressID'>$[PartyAddressID]</td>"
        + "<td align='center'><a href='#' class='btn btn-success' id='A_Edit'><i class='fa fa-edit'></i> $[action] </a></td>";
        //+ "<td align='center'><a href='#' class='btn btn-danger' id='A_delete'><i class='fa fa-trash'></i> $[Delete]</a></td>";
        + "</tr>";
}