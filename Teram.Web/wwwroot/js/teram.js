var deleteFileUrl = "";



var defaultoption = {
    "inline": false,
    "format": "l",
    "viewMode": "day",
    "initialValue": false,
    "initialValueType": "persian",
    "autoClose": true,
    "position": "auto",
    "onlyTimePicker": false,
    "onlySelectOnDate": true,
    "calendarType": "persian",
    "altFormat": 'YYYY/MM/DD',
    "inputDelay": 800,
    "observer": true,
    "altFieldFormatter": function (unixDate) {
        var d = new Date(unixDate);
        return d.getFullYear() + "/" + (d.getMonth() + 1) + "/" + d.getDate() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
    },
    "calendar": {
        "persian": {
            "locale": "fa",
            "showHint": false,
            "leapYearMode": "algorithmic"
        },
        "gregorian": {
            "locale": "en",
            "showHint": false
        }
    },
    "navigator": {
        "enabled": true,
        "scroll": {
            "enabled": true
        },
        "text": {
            "btnNextText": ">",
            "btnPrevText": "<"
        }
    },
    "toolbox": {
        "enabled": true,
        "calendarSwitch": {
            "enabled": true,
            "format": "MMMM"
        },
        "todayButton": {
            "enabled": true,
            "text": {
                "fa": "امروز",
                "en": "Today"
            }
        },
        "text": {
            "btnToday": "امروز"
        }
    },
    "timePicker": {
        "enabled": false,
        "step": 1,
        "hour": {
            "enabled": true,
            "step": null
        },
        "minute": {
            "enabled": true,
            "step": null
        },
        "second": {
            "enabled": true,
            "step": null
        },
        "meridian": {
            "enabled": true
        }
    },
    "dayPicker": {
        "enabled": true,
        "titleFormat": "YYYY MMMM"
    },
    "monthPicker": {
        "enabled": true,
        "titleFormat": "YYYY"
    },
    "yearPicker": {
        "enabled": true,
        "titleFormat": "YYYY"
    },
    "responsive": true
};

$(document).on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});
var teram = function () {
    return {
        showSuccessfullMessage: function (message, timeout) {

            if (timeout > 0) {
                Notiflix.Notify.success(message, {
                    timeout: timeout
                });
            }

            else {

                Notiflix.Notify.success(message);
            }
        },
        showInfoMessage: function (message, title) {
            Notiflix.Notify.info(message)
        },
        showErrorMessage: function (message, title) {
            Notiflix.Notify.failure(message)

        },
        pageInit: function () {

            datePickerInitilize();
            $(".select2").select2({
                width: '100%',
                cache: true,
                dir: 'rtl',
                language: "fa",
                closeOnSelect: true,
                focusInputOnOpen: true,
            });
        },
        initConfirm: function (width, useGoogleFont, fontFamily, messageMaxLength) {
            Notiflix.Confirm.init({ width, useGoogleFont, fontFamily, messageMaxLength });
        },
        showConfirm: function (title, message, successFunc, failFunc) {
            Notiflix.Confirm.show(title, message, 'بله', 'خیر',
                successFunc,
                failFunc,
            );
        }
    }
}

function addThousandSeparator(value) {
    // Convert the value to a number, if it's not already
    var numberValue = parseFloat(value.replace(/,/g, ''));

    // Check if the value is a valid number
    if (!isNaN(numberValue)) {
        // Format the number with a thousand separator
        var formattedValue = numberValue.toLocaleString();
        return formattedValue;
    } else {
        // If the value is not a valid number, return the original value
        return value;
    }
}

function notificationInit() {
    Notiflix.Notify.init({
        position: 'right-top',
        fontFamily: 'IranSans',
        useIcon: true,
        timeout: 5000,
    });
    Notiflix.Loading.init({
        clickToClose: false,
        fontFamily: 'IranSans',
    });
    $(document).ajaxStart(function () {
        Notiflix.Loading.pulse('در حال پردازش اطلاعات ');
    });
    $(document).ajaxStop(function () {
        Notiflix.Loading.remove();
    });
    $(document).ajaxSuccess(function (event, xhr, settings) {

        if (xhr.responseJSON && xhr.responseJSON.IsSiteOff) {
            Notiflix.Loading.remove();
            Notiflix.Notify.failure(xhr.responseJSON.Message);

        }
    });
    $(document).ajaxError(function (event, xhr, options, exc) {
        if (xhr.status == 401) {
            Notiflix.Report.failure('عدم دسترسی', 'مدت جلسه کاری شما به اتمام رسیده است، لطفا مجددا وارد سامانه شوید', 'بستن');
        } else if (xhr.status == 403) {
            Notiflix.Report.failure('عدم دسترسی', 'کاربر دسترسی لازم برای مشاهده این صفحه را ندارد', 'بستن');
        } else {
            Notiflix.Report.failure('خطا در سامانه', 'خطایی در سامانه رخ داده است، لطفا با تیم پشتیبانی تماس بگیرید', 'بستن');
        }
    });
}

function onSuccess(data) {

    if (data.redirectUrl != null) {
        window.location.href = data.redirectUrl;
    }
    if (data.result.toLowerCase() === "ok") {
        var message = "OK";
        if (data.message) {
            if (data.message.value) {
                message = data.message.value;
            }
            else {
                message = data.message;
            }
        };

        if (data.timeout != null) {
            teram().showSuccessfullMessage(message, data.timeout);
        }

        else {
            teram().showSuccessfullMessage(message);
        }
        
        if (reloadDataTable) {
            reloadDataTable();
        }
        if (onAfterDataSave) {
            onAfterDataSave();
        }
        if (afterSuccess != null) {
            afterSuccess();
        }
    }
    else if (data.result == "redirect") {
        window.location.href = data.url;
    }
    else {
        $("#dntCaptchaRefreshButton").click();
        message = "Fail";
        if (data.message) {
            if (data.message.value) {
                message = data.message.value;
            }
            else {
                message = data.message;
            }
        };
        teram().showErrorMessage(message);
    };

}
function setBrowserFile(file, target, plainPath = false) {
    $("#" + target).val(file.url);
    $("#" + target).parent().find(".preview").attr("src", file.url);
    $("#" + target).parent().find(".selectedFile").html(file.name);
    var videoPreview = $("#" + target).parent().find(".videoPreview");
    if (videoPreview.length != 0) {
        var source = document.createElement('source');
        source.setAttribute('src', "'" + file.url + "'");

        videoPreview.appendChild(source);

    }

}
function resetForm() {
    $(':input')
        .not(':button, :submit, :reset ,:radio, :checkbox')
        .val('');
    $(':checkbox').prop('checked', false);
    $('.select2').val(null).trigger('change');


    var datePickerElement = $('input[type=text].pDatePicker');
    var datePickerHiddenElement = datePickerElement.parent().find('input:hidden:first');
    datePickerHiddenElement.val('');
}
function clearForm() {
    $(':input')
        .not(':button, :submit, :reset ,:radio, [readonly], :hidden, :checkbox')
        .val('');
    $(':hidden#Key').val('');
    $(':checkbox').prop('checked', false);
    $('.select2').val(null).trigger('change');


    var datePickerElement = $('input[type=text].pDatePicker');
    var datePickerHiddenElement = datePickerElement.parent().find('input:hidden:first');
    datePickerHiddenElement.val('');

    if (specialClearForm != null) {
        specialClearForm();
    }
}
function datePickerInitilize(selector = ".pDatePicker", customOption) {

    var options = {}
    $(selector).each(function () {
        var isCustomForJsOption = $(this).data("iscustomforjsoption");
        if (isCustomForJsOption === undefined || isCustomForJsOption === null) {
            var altField = $(this).data("altfield");
            if (altField !== undefined && altField !== null) {
                defaultoption["altField"] = "#" + altField;
            }
            if (customOption !== undefined && customOption !== null) {

                options = $.extend(defaultoption, customOption);
            }

            else { options = defaultoption; }
            pd = $(this).pDatepicker(options);
        }
        else if (selector !== ".pDatePicker") {
            if (customOption !== undefined && customOption !== null) {

                options = $.extend(defaultoption, customOption);
            }

            else { options = customOption; }
            pd = $(this).pDatepicker(options);

        }
    });
}
function initialFileUploader(selector, datainitialPreview = null) {
    var showremove = $(selector).attr("data-show-remove");
    var showcaption = $(selector).attr("data-show-caption");
    var showupload = $(selector).attr("data-show-upload");
    var language = $(selector).attr("data-language");
    var theme = $(selector).attr("data-theme");
    var removelabel = $(selector).attr("data-remove-label");
    var browselbl = $(selector).attr("data-browse-label");
    var uploadurl = $(selector).attr("data-uploadurl");
    var deleteurl = $(selector).attr("data-deleteurl");
    var autoupload = $(selector).attr("data-autoupload");
    var uploadasync = $(selector).attr("data-upload-async");
    var maxfilesize = $(selector).attr("data-maxfile-size");
    var maxFilePreviewSize = $(selector).attr("data-maxfile-previewsize");
    var multiple = $(selector).attr("data-multiple");
    var isrtl = $(selector).attr("data-rtl");
    var purifyHtml = $(selector).attr("data-purify-html");
    var initialPreviewDownloadUrl = $(selector).attr("data-initialpreview-downloadurl");
    var enableResumableUpload = $(selector).attr("data-enable-resumable-upload");
    var dropZoneEnabled = $(selector).attr("data-drop-zone-enabled");
    var initialPreviewAsData = $(selector).attr("data-initial-preview-as-data");
    var overwriteInitial = $(selector).attr("data-overwrite-initial");
    var preferIconicPreview = $(selector).attr("data-prefer-Iconic-preview");
    var allowedFileExtensions = $(selector).attr("data-allowed-file-extensions") != undefined ?
        JSON.parse($(selector).attr("data-allowed-file-extensions").replace(/'/g, '"')) : null;

    var options = {
        rtl: isrtl == 'true',
        fileActionSettings: {
            showDrag: false
        },
        uploadUrl: uploadurl,
        layoutTemplates: {
            actionUpload: ""
        },
        initialPreviewDownloadUrl: initialPreviewDownloadUrl == 'true',
        enableResumableUpload: enableResumableUpload == 'true',
        theme: theme,
        language: language,
        allowedFileExtensions: allowedFileExtensions,
        showRemove: showremove == 'true',
        showUpload: showupload == 'true',
        showDownload: showupload == 'true',
        showCaption: showcaption == 'true',
        autoupload: autoupload == 'ture',
        uploadAsync: uploadasync == 'true',
        dropZoneEnabled: dropZoneEnabled == 'true',
        maxFileSize: maxfilesize != undefined ? parseInt(maxfilesize) : 25600,
        maxFilePreviewSize: maxFilePreviewSize != undefined ? parseInt(maxFilePreviewSize) : 25600,
        browseLabel: browselbl,
        removeLabel: removelabel,
        multiple: multiple == 'true',
        previewZoomButtonIcons: {
            prev: '<i class="fa fa-caret-left fa-lg"></i>',
            next: '<i class="fa fa-caret-right fa-lg"></i>',
            toggleheader: '<i class="fa fa-fw fa-arrows-alt"></i>',
            fullscreen: '<i class="fas fa-external-link-alt"></i>',
            borderless: '<i class="fa fa-fw fa-arrows-alt"></i>',
            close: '<i class="fa fa-close"></i>'
        }

    }

    if (datainitialPreview != null) {
        var customoption = {
            initialPreview: datainitialPreview.initialPreview,
            initialPreviewConfig: datainitialPreview.initialPreviewConfig,
            preferIconicPreview: preferIconicPreview == 'true',
            initialPreviewAsData: initialPreviewAsData == 'true',
            purifyHtml: purifyHtml == 'true',
            overwriteInitial: overwriteInitial == 'true',
            previewFileIcon: '<i class="fas fa-file"></i>',
            allowedPreviewTypes: null,
            previewFileIconSettings: {
                'doc': '<i class="fas fa-file-word text-primary"></i>',
                'xls': '<i class="fas fa-file-excel text-success"></i>',
                'ppt': '<i class="fas fa-file-powerpoint text-danger"></i>',
                'pdf': '<i class="fas fa-file-pdf text-danger"></i>',
                'zip': '<i class="fas fa-file-archive text-muted"></i>',
                'htm': '<i class="fas fa-file-code text-info"></i>',
                'txt': '<i class="fas fa-file-text text-info"></i>',
                'mov': '<i class="fas fa-file-movie-o text-warning"></i>',
                'mp3': '<i class="fas fa-file-audio text-warning"></i>',
                'jpg': '<i class="fas fa-file-image text-danger"></i>',
                'gif': '<i class="fas fa-file-image text-warning"></i>',
                'png': '<i class="fas fa-file-image text-primary"></i>'
            },
            previewFileExtSettings: {
                'doc': function (ext) {
                    return ext.match(/(doc|docx)$/i);
                },
                'xls': function (ext) {
                    return ext.match(/(xls|xlsx)$/i);
                },
                'ppt': function (ext) {
                    return ext.match(/(ppt|pptx)$/i);
                },
                'zip': function (ext) {
                    return ext.match(/(zip|rar|tar|gzip|gz|7z)$/i);
                },
                'htm': function (ext) {
                    return ext.match(/(htm|html)$/i);
                },
                'txt': function (ext) {
                    return ext.match(/(txt|ini|csv|java|php|js|css)$/i);
                },
                'mov': function (ext) {
                    return ext.match(/(avi|mpg|mkv|mov|mp4|3gp|webm|wmv)$/i);
                },
                'mp3': function (ext) {
                    return ext.match(/(mp3|wav)$/i);
                }
            },
        }
        options = $.extend(options, customoption);
    }



    $(selector).fileinput("destroy").fileinput(
        options
    );

    $(selector).on('filebeforedelete',
        function (event, key, data) {
            teram().showConfirm(
                'سوال',
                'تمایل به حذف دارید؟',
                function () {
                    $.post(deleteFileUrl, { id: data.id }, function (data) {
                        if (data.result == "ok") {
                            $("#thumb-" + selector.substring(1, selector.length) + "-init-" + key).remove();
                            /*onSuccess(data);*/
                            initialFileUploader(selector);
                        }
                        else {
                            teram().showErrorMessage(data.message.value);
                        }
                    });
                },
                function () {
                },
            );

            return true;
        });
}
function initialValidation() {
    $('form').removeData('validator');
    $('form').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse('form');
}

$(document).on("click", ".openFileManager", function (e) {
    e.preventDefault();
    var target = $(this).siblings(".browseTarget").prop("id");

    $(this).popupWindow('/Browse.html?plain=true&target=' + target, {
        name: 'Filebrowser',
        height: 490,
        width: 950,
        centerScreen: 1
    });

});
$(document).on('click', '.resetForm', function () {
    resetForm();
});
$(document).on("click", ".pDatePickerIcon", function () {
    if ($(this).nextAll('.pDatePicker:first')[0] !== undefined && (!$(this).nextAll('.pDatePicker:first')[0].disabled))
        $(this).nextAll('.pDatePicker:first').click();
});
$(document).on('keydown', ".pDatePicker", function () {
    var key = event.keyCode || event.charCode;
    if ((key == 8 || key == 46) && !$(this).is('[readonly]')) {
        var datePickerHiddenElement = $(this).parent().find('input:hidden:first');
        datePickerHiddenElement.val('');
    }
});
$(document).on('click', '.toggle-password', function () {
    $(this).toggleClass('fa-eye fa-eye-slash');
    let input = $($(this).attr('toggle'));
    if (input.attr('type') == 'password') {
        input.attr('type', 'text');
    }
    else {
        input.attr('type', 'password');
    }
})

$(".persianAlphabet").on("keypress", function (event) {
    var persianAlphabet = /^[\u0600-\u06FF\s]+$/;
    var key = String.fromCharCode(event.which);
    if (persianAlphabet.test(key)) {
        return true;
    }
    return false;
});
$(".numberAlphabet").on("keypress", function (event) {
    var numberAlphabet = /^(0|[1-9][0-9]*)$/;
    var key = String.fromCharCode(event.which);
    if (numberAlphabet.test(key)) {
        return true;
    }
    return false;
});


$(document).ready(function () {
    if ($.spectrum) { //this line means if spectrum color picker exist, the page can initilize all colorpicker taghelpers
        $(".colorpicker").spectrum();
    }
    notificationInit();
    datePickerInitilize();
    if (initializeDatatable) {
        initializeDatatable();
    }
    teram().pageInit();
    //$('#CurrntSalary').on('change', function () {
    //    // Get the current value of the textbox
    //    var currentValue = $(this).val();
    //    // Format the value with a thousand separator
    //    var formattedValue = addThousandSeparator(currentValue);
    //    // Set the formatted value back to the textbox
    //    $(this).val(formattedValue);
    //});

});
