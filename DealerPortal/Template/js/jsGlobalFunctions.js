/// <reference path="jquery.min.js" />
//jQuery.browser = {};
//(function () {
//    jQuery.browser.msie = false;
//    jQuery.browser.version = 0;
//    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
//        jQuery.browser.msie = true;
//        jQuery.browser.version = RegExp.$1;
//    }
//})();

/* Check Numeric*/
function CheckIsNumeric(e, tx) {
    var AsciiCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    if ((AsciiCode < 47 && AsciiCode != 8 && AsciiCode != 9) || (AsciiCode > 57 || (AsciiCode == 47))) {
        return false;
    }
}

function CheckIsAlpha(e, tx) {
    var AsciiCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    if (AsciiCode == 8 || AsciiCode == 9 || AsciiCode == 32 || AsciiCode == 37 || AsciiCode == 39 || AsciiCode == 46 || AsciiCode == 127 || (AsciiCode >= 65 && AsciiCode <= 90) || (AsciiCode >= 97 && AsciiCode <= 122)) {
        //Only for Alphbets
        return true;
    }
    else {
        return false;
    }
}

function CheckIsAlphaNumeric(e, tx) {
    var AsciiCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    if ((AsciiCode > 47 && AsciiCode < 58) || (AsciiCode > 64 && AsciiCode < 91) || (AsciiCode > 96 && AsciiCode < 123)) {
        return true;
    }
    return false;
}

function CheckIsSpaceUnderscore(e, tx) {
    var AsciiCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    if ((AsciiCode != 32 && AsciiCode != 95)) {
        return true;
    }
    return false;
}

/* Check Decimal*/
function CheckIsDecimal(e, tx) {
    var AsciiCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    // alert(AsciiCode);
    if ((AsciiCode < 46 && AsciiCode != 8 && AsciiCode != 9) || (AsciiCode > 57) || (AsciiCode == 47)) {
        //Only Numbers Allowed
        return false;
    }

    var selecttxt = '';
    if (window.getSelection) {
        selecttxt = window.getSelection();
    } else if (document.getSelection) {
        selecttxt = document.getSelection();
    } else if (document.selection) {
        selecttxt = document.selection.createRange().text;
    }

    if (selecttxt == '') {
        var num = tx.value;
        var len = num.length;
        var indx = -1;
        indx = num.indexOf('.');
        if (indx != -1) {
            if ((AsciiCode == 46)) {
                // (.) Not Allowed More than One
                return false;
            }
            var dgt = num.substr(indx, len);
            var count = dgt.length;
            //alert (count);
            if (count == 3) {
                //Only 2 Decimal Digits Allowed
                if (AsciiCode != 8 && AsciiCode != 9)
                    return false;

            }
        }
        else if (num != 0 && len == 10) {
            if (AsciiCode != 46)
                return false;
        }
    }
}

/* Check Decimal*/
function CheckIsDecimalWithMinus(e, tx) {
    var AsciiCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    // alert(AsciiCode);
    if ((AsciiCode < 45 && AsciiCode != 8 && AsciiCode != 9) || (AsciiCode > 57) || (AsciiCode == 47)) {
        //Only Numbers Allowed
        return false;
    }

    var selecttxt = '';
    if (window.getSelection) {
        selecttxt = window.getSelection();
    } else if (document.getSelection) {
        selecttxt = document.getSelection();
    } else if (document.selection) {
        selecttxt = document.selection.createRange().text;
    }

    if (selecttxt == '') {
        var num = tx.value;
        var len = num.length;
        var indx = -1;
        var indxMinus = -1;

        indx = num.indexOf('.');
        indxMinus = num.indexOf('-');

        if (indxMinus != -1) {
            if ((AsciiCode == 45)) {
                // (-) Not Allowed More than One
                return false;
            }
        }

        if (indx != -1) {
            if ((AsciiCode == 46)) {
                // (.) Not Allowed More than One
                return false;
            }
            var dgt = num.substr(indx, len);
            var count = dgt.length;
            //alert (count);
            if (count == 3) {
                //Only 2 Decimal Digits Allowed
                if (AsciiCode != 8 && AsciiCode != 9)
                    return false;
            }
        }
        else if (num != 0 && len == 9) {
            if (AsciiCode != 46)
                return false;
        }
    }
}

/*Check Four Decomal Place*/
function CheckIsDecimalFourPlace(e, tx) {
    var AsciiCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    // alert(AsciiCode);
    if ((AsciiCode < 46 && AsciiCode != 8 && AsciiCode != 9) || (AsciiCode > 57) || (AsciiCode == 47)) {
        //Only Numbers Allowed
        return false;
    }

    var selecttxt = '';
    if (window.getSelection) {
        selecttxt = window.getSelection();
    } else if (document.getSelection) {
        selecttxt = document.getSelection();
    } else if (document.selection) {
        selecttxt = document.selection.createRange().text;
    }

    if (selecttxt == '') {
        var num = tx.value;
        var len = num.length;
        var indx = -1;
        indx = num.indexOf('.');
        if (indx != -1) {
            if ((AsciiCode == 46)) {
                // (.) Not Allowed More than One
                return false;
            }
            var dgt = num.substr(indx, len);
            var count = dgt.length;
            //alert (count);
            if (count == 5) {
                //Only 2 Decimal Digits Allowed
                if (AsciiCode != 8 && AsciiCode != 9)
                    return false;

            }
        }
        else if (num != 0 && len == 9) {
            if (AsciiCode != 46)
                return false;
        }
    }
}

/*Check Four Decomal Place*/
function CheckIsDecimalFourPlaceWithMinus(e, tx) {
    var AsciiCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
    // alert(AsciiCode);
    if ((AsciiCode < 45 && AsciiCode != 8 && AsciiCode != 9) || (AsciiCode > 57) || (AsciiCode == 47)) {
        //Only Numbers Allowed
        return false;
    }

    var selecttxt = '';
    if (window.getSelection) {
        selecttxt = window.getSelection();
    } else if (document.getSelection) {
        selecttxt = document.getSelection();
    } else if (document.selection) {
        selecttxt = document.selection.createRange().text;
    }

    if (selecttxt == '') {

        var num = tx.value;
        var len = num.length;
        var indx = -1;
        var indxMinus = -1;

        indx = num.indexOf('.');
        indxMinus = num.indexOf('-');

        if (indxMinus != -1) {
            if ((AsciiCode == 45)) {
                // (-) Not Allowed More than One
                return false;
            }
        }

        if (indx != -1) {
            if ((AsciiCode == 46)) {
                // (.) Not Allowed More than One
                return false;
            }
            var dgt = num.substr(indx, len);
            var count = dgt.length;
            //alert (count);
            if (count == 5) {
                //Only 2 Decimal Digits Allowed
                if (AsciiCode != 8 && AsciiCode != 9)
                    return false;
            }
        }
        else if (num != 0 && len == 7) {
            if (AsciiCode != 46)
                return false;
        }
    }
}

/* Validate Decimal */
function ValidateDecimal(e, tx) {
    var txval = $(tx).val().trim();
    try {
        if (!parseFloat(txval)) {
            txval = 0.00;
        }
    } catch (e) {
        txval = 0.00;
    }
    $(tx).val(parseFloat(txval).toFixed(2));
}

function ExportToExcel(divId) {
    window.open('data:application/vnd.ms-excel,' + encodeURIComponent($('div[id$=' + divId + ']').html()));
}

function Nullhandler(value) {
    var returnValue = '';
    if (value != undefined && value != '' && value != null) {
        returnValue = value;
    }
    return returnValue;
}

function NullhandlerInt(value) {
    var returnValue = 0;
    if (value != undefined && value != '' && value != null) {
        returnValue = value;
    }
    return returnValue;
}

/* validate email id */
jQuery.fn.IsEmailOnly =
 function () {
     return this.each(function () {
         $(this).change(function (e) {
             if (this.value != "" && this.value != null && this.value != undefined) {
                 var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
                 if (mailformat.test(this.value) == false) {
                     this.focus();
                     var EmailErrorMsg = $("#EmailErrorMsg")
                     EmailErrorMsg.empty();
                     $("#EmailErrorMsg").append("Invalid Email Format.")
                     alert($("#EmailErrorMsg").text());
                 }
                 else {
                     return true;
                 }
             }
             else {
                 return true;
             }
         });
     });
 };

/* validate email id */
jQuery.fn.IsValidPassword =
 function () {
     return this.each(function () {
         $(this).change(function (e) {
             if (this.value != "" && this.value != null && this.value != undefined) {
                 var mailformat = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/;
                 if (mailformat.test(this.value) == false) {
                     alert('Invalid Password');
                     this.focus();
                     $(this).val('');
                     return false;
                 }
                 else {
                     return true;
                 }
             }
             else {
                 return true;
             }
         });
     });
 };



/* validate mobile */
jQuery.fn.IsMobileOnly =
    function () {
        return this.each(function () {
            $(this).change(function (e) {
                if (this.value != "" && this.value != null && this.value != undefined) {
                    var mobileFormat = /^([7-9]{1})([0-9]{9})$/;
                    if (mobileFormat.test(this.value) == false || (this.length > 10 || this.length < 10)) {
                        alert('Invalid Mobile Format');
                        $(this).focus();
                        $(this).val('');
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return true;
                }
            });
        });
    };

///////////////////////// Model Popups ///////////////////
function DisplayPopupValidation(strMessage, textBox) {
    $("#PopupValidation").text(strMessage);

    $("#PopupValidation").dialog({
        autoOpen: false,
        modal: true,
        closeOnEscape: true,
        draggable: false,
        resizable: false,
        title: 'Validation',
        open: function (event, ui) {
            $('.ui-button-text').css({
                'font-family': 'Verdana',
                'font-size': '12px'
            });
            $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
        },
        buttons: {
            Ok: function () {
                textBox.focus();
                $(this).dialog("close");
            }
        },
        show: {
            effect: "fadeIn",
            duration: 500
        },
        hide: {
            effect: "fadeOut",
            duration: 500
        }
    });

    $('#PopupValidation').dialog('open');
}

function DisplayPopupError(strMessage) {
    $("#PopupErrorMessage").text(strMessage);

    $("#PopupErrorMessage").dialog({
        autoOpen: false,
        modal: true,
        closeOnEscape: true,
        draggable: false,
        resizable: false,
        title: 'Error',
        open: function (event, ui) {
            $('.ui-button-text').css({
                'font-family': 'Verdana',
                'font-size': '12px'
            });
            $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
        },
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        },
        show: {
            effect: "fadeIn",
            duration: 500
        },
        hide: {
            effect: "fadeOut",
            duration: 500
        }
    });
    $("#PopupPleaseWait").dialog('close');
    $('#PopupErrorMessage').dialog('open');
}

function DisplayPopupInformation(strMessage) {
    $("#PopupErrorMessage").text(strMessage);

    $("#PopupErrorMessage").dialog({
        autoOpen: false,
        modal: true,
        closeOnEscape: true,
        draggable: false,
        resizable: false,
        title: 'Information',
        open: function (event, ui) {
            $('.ui-button-text').css({
                'font-family': 'Verdana',
                'font-size': '12px'
            });
            $(this).parent().children().children('.ui-dialog-titlebar-close').hide();
        },
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        },
        show: {
            effect: "fadeIn",
            duration: 500
        },
        hide: {
            effect: "fadeOut",
            duration: 500
        }
    });
    $("#PopupPleaseWait").dialog('close');
    $('#PopupErrorMessage').dialog('open');
}

function ValidateEmailId(emailTextBox) {
    var retValue = false;
    if (emailTextBox.val() != '' && emailTextBox.val() != null && emailTextBox.val() != undefined) {
        var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        if (mailformat.test(emailTextBox.val()) == false) {
            alert('Invalid email format');
            emailTextBox.val("");
            emailTextBox.focus();
        }
        else {
            retValue = true;
        }
    }
    return retValue;
}

function ValidateMobileNo(mobileTextBox) {
    var retValue = false;
    if (mobileTextBox.val() != '' && mobileTextBox.val() != null && mobileTextBox.val() != undefined) {
        var mobileFormat = /^([7-9]{1})([0-9]{9})$/;
        if (mobileFormat.test(mobileTextBox.val()) == false || (mobileTextBox.val().length > 10 || mobileTextBox.val().length < 10)) {
            alert('Invalid mobile no. format');
            mobileTextBox.val("");
            mobileTextBox.focus();
        }
        else {
            retValue = true;
        }
    }
    return retValue;
}



function SetFocusAtBottom() {
    $("html, body").animate({ scrollTop: $(document).height() }, 1000);
}

function checkfile(objFileUpload, objSpanError) {
    try {
        objSpanError.text("");
        if (objFileUpload.value == "") {
            objSpanError.text("Please upload a valid file");
            objFileUpload.focus();
            return false;
        }
        else {
            var file = getNameFromPath(objFileUpload.value);
            if (file != null) {
                var extension = file.substr((file.lastIndexOf('.') + 1)).toLowerCase();
                switch (extension) {
                    case 'jpg':
                    case 'jpeg':
                    case 'png':
                    case 'gif':
                        flag = true;
                        break;
                    default:
                        flag = false;
                }
            }
            if (flag == false) {
                objSpanError.text("You can upload only jpg,jpeg,png and gif extension file");
                return false;
            }
            else {
                var size = GetFileSize(objFileUpload.id);
                if (size > 3) {
                    objSpanError.text("You can upload file up to 3 MB");
                    return false;
                }
                else {
                    objSpanError.text("");
                }
            }
            return true;
        }
    }
    catch (e) {
        alert(e.message + "\nFunction : checkfile");
    }
    return false;
}

function GetFileSize(fileid) {
    try {
        var fileSize = 0;
        //for IE
        if ($.browser.msie) {
            //before making an object of ActiveXObject, 
            //please make sure ActiveX is enabled in your IE browser
            var objFSO = new ActiveXObject("Scripting.FileSystemObject"); var filePath = $("#" + fileid)[0].value;
            var objFile = objFSO.getFile(filePath);
            var fileSize = objFile.size; //size in kb
            fileSize = fileSize / 1048576; //size in mb 
        }
            //for FF, Safari, Opeara and Others
        else {
            fileSize = $("#" + fileid)[0].files[0].size //size in kb
            fileSize = fileSize / 1048576; //size in mb 
        }
        return fileSize;
    }
    catch (e) {
        alert(e.message + "\nFunction : GetFileSize");
    }
}

function getNameFromPath(strFilepath) {
    try {
        var objRE = new RegExp(/([^\/\\]+)$/);
        var strName = objRE.exec(strFilepath);
        if (strName == null) {
            return null;
        }
        else {
            return strName[0];
        }
    }
    catch (e) {
        alert(e.message + "\nFunction : GetFileSize");
    }
    return "";
}

function ConvertJSONDateToDDMMMYYYY(dateObject) {
    try {
        if (dateObject == null || dateObject == undefined || dateObject == "") {
            return "";
        }
        else {
            var varDate = new Date(parseInt(dateObject.substr(6)));
            var day = varDate.getDate();
            var month = varDate.getMonth() + 1;
            var year = varDate.getFullYear();
            var Hour = varDate.getHours();
            var Minute = varDate.getMinutes();
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }
            var date = day + "/" + month + "/" + year;
            return date;
        }
    }
    catch (e) {
        alert(e.message);
    }
    return "";
};

function GetHTMLDate(date) {
    if (!date) {
        return "";
    }
    var now = "";
    now = new Date(date);
    if (now) {
        var dateSplit = date.split("/");
        now = new Date(dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2]);
    }
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    //var today = now.getFullYear() + "-" + (month) + "-" + (day);
    var today = day + "/" + month + "/" + "/" + now.getFullYear();
    return today;
};

function ConvertJSONDateToMMDDYY(dateObject) {
    try {
        if (dateObject == null || dateObject == undefined || dateObject == "") {
            return "";
        }
        else {
            var varDate = new Date(parseInt(dateObject.substr(6)));
            var day = varDate.getDate();
            var month = varDate.getMonth() + 1;
            var year = varDate.getFullYear();
            var Hour = varDate.getHours();
            var Minute = varDate.getMinutes();
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }
            var date = month + "/" + day + "/" + year;
            return date;
        }
    }
    catch (e) {
        alert(e.message);
    }
    return "";
};


function ConvertJSONDateToMMDDYYYY(dateObject) {
    try {
        if (dateObject == null || dateObject == undefined || dateObject == "") {
            return "";
        }
        else {
            var varDate = new Date(parseInt(dateObject.substr(6)));
            var day = varDate.getDate();
            var month = varDate.getMonth() + 1;
            var year = varDate.getFullYear();
            var Hour = varDate.getHours();
            var Minute = varDate.getMinutes();
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }
            var date = month + "/" + day + "/" + year;
            return date;
        }
    }
    catch (e) {
        alert(e.message);
    }
    return "";
};


function ConvertJSONDateToMMDDYYYYHHMMSS(dateObject) {
    try {
        if (dateObject == null || dateObject == undefined || dateObject == "") {
            return "";
        }
        else {
            var day = 0;
            var month = 0;
            var year = 0;
            var Hour = 0;
            var Minute = 0;
            var Sec = 0;

            var varDate = new Date(parseInt(dateObject.substr(6)));
            varDate = new Date(new Date(parseInt(dateObject.substr(6))).setHours(varDate.getHours(), varDate.getMinutes(), varDate.getSeconds()));
            day = varDate.getDate();
            month = varDate.getMonth() + 1;
            year = varDate.getFullYear();
            Hour = varDate.getHours();
            Minute = varDate.getMinutes();
            Sec = varDate.getSeconds();
            //var totalmin = (Hour * 60) + Minute
            //var diffmin = 1440 - totalmin
            //var hh = parseInt((diffmin / 60));
            //var min = (diffmin % 60);
            //if (diffmin != 1440) {
            //    varDate = new Date(new Date(parseInt(dateObject.substr(6))).setHours(varDate.getHours() + hh, varDate.getMinutes() + min, 0, 0));
            //    day = varDate.getDate();
            //    month = varDate.getMonth() + 1;
            //    year = varDate.getFullYear();
            //    Hour = varDate.getHours();
            //    Minute = varDate.getMinutes()
            //}
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }
            var date = month + "/" + day + "/" + year + " " + Hour + ":" + Minute + ":" + Sec;
            return date;
        }
    }
    catch (e) {
        alert(e.message);
    }
    return "";
};

function ConvertJSONDateToDDMMYYYY(dateObject) {
    try {
        if (dateObject == null || dateObject == undefined || dateObject == "") {
            return "";
        }
        else {
            var varDate = new Date(parseInt(dateObject.substr(6)));
            var day = varDate.getDate();
            var month = varDate.getMonth() + 1;
            var year = varDate.getFullYear();
            var Hour = varDate.getHours();
            var Minute = varDate.getMinutes();
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }
            var date = day + "-" + month + "-" + year;
            return date;
        }
    }
    catch (e) {
        alert(e.message);
    }
    return "";
};

function ConvertJSONDateToYYYYMMDD(dateObject) {
    try {
        if (dateObject == null || dateObject == undefined || dateObject == "") {
            return "";
        }
        else {
            var varDate = new Date(parseInt(dateObject.substr(6)));
            var day = varDate.getDate();
            var month = varDate.getMonth() + 1;
            var year = varDate.getFullYear();
            var Hour = varDate.getHours();
            var Minute = varDate.getMinutes();
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }
            var date = year + "-" + month + "-" + day ;
            return date;
        }
    }
    catch (e) {
        alert(e.message);
    }
    return "";
};


function ConvertJSONDateToDDMMYYYYNew(dateObject) {
    try {
        if (dateObject == null || dateObject == undefined || dateObject == "") {
            return "";
        }
        else {
            var varDate = new Date(dateObject);
            var day = varDate.getDate();
            var month = varDate.getMonth() + 1;
            var year = varDate.getFullYear();
            var Hour = varDate.getHours();
            var Minute = varDate.getMinutes();
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }
            var date = year + "-" + month + "-" + day;
            return date;
        }
    }
    catch (e) {
        alert(e.message);
    }
    return "";
};
