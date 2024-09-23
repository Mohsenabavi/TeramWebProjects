var markers = [];
var map;
deleteFileUrl = "/JobApplicant/RemoveUploadedFiles/";

// Map Functions

var dateOptionBirthDate = {
    "altField": '#BirthDate',
    "altFormat": 'YYYY/MM/DD',
    "calendarType": "persian",
    "initialValue": false,
    "minDate": null,
    "maxDate": null
}

var dateOptionStartDateMilitaryService = {
    "altField": '#StartDateMilitaryService',
    "altFormat": 'YYYY/MM/DD',
    "calendarType": "persian",
    "initialValue": false,
    "minDate": null,
    "maxDate": null

}

var dateOptionEndDateMilitaryService = {
    "altField": '#EndDateMilitaryService',
    "altFormat": 'YYYY/MM/DD',
    "calendarType": "persian",
    "initialValue": false,
    "minDate": null,
    "maxDate": null
}

var dateOption = {
    "altFormat": 'YYYY/MM/DD',
    "calendarType": "persian",
    "initialValue": false,
    "minDate": null,
    "maxDate": null
}
function initMap() {

    map = new L.Map('map', {
        key: 'web.f54dc3c9a89e40c9b78193a89cc1ba53',
        maptype: 'dreamy',
        poi: true,
        traffic: false,
        center: [32.60438593966579, 51.567678914951756],
        zoom: 6
    });
    addMarkerAfterClick();
}

function addMarkerAfterClick() {
    var theMarker = {};
    map.on('click', function (e) {
        removeMarkers();
        lat = e.latlng.lat;
        lon = e.latlng.lng;
        $("#Latitude").val(lat);
        $("#Longitude").val(lon);
        if (theMarker != undefined) {
            map.removeLayer(theMarker);
        };
        theMarker = L.marker([lat, lon]).addTo(map);
        markers.push(theMarker);
    });
}

function removeMarkers() {
    for (i = 0; i < markers.length; i++) {
        map.removeLayer(markers[i]);
    }
}

// End of Map functions

function successSubmitForm(data) {
    if (data.result != undefined && data.result.toLowerCase() == 'fail') {
        teram().showErrorMessage(data.message.value);
        return;
    }

    if (data.result != undefined && data.result.toLowerCase() == 'ok') {
        teram().showSuccessfullMessage(data.message.value);
        return;
    }

    $("#frmWorkWithUs").hide();
    $(".previewContainer").html(data).show();
}

$(document).on('click', '#btnReturn', function () {
    $("#frmWorkWithUs").show();
    $(".previewContainer").hide();
});



//Conditional Show And Hide and Enable Or Disable page Elements
$('input[name="Gender"]').on('change', function () {
    var selectedValue = $('input[name="Gender"]:checked').val();
    if (selectedValue === "1") {
        $(".militaryArea").show();
        $(".militaryArea").removeClass("d-none");
    }
    if (selectedValue === "2") {
        $(".militaryArea").hide();
    }
});

$('input[name="MarriageStatus"]').on('change', function () {
    var selectedValue = $('input[name="MarriageStatus"]:checked').val();
    if (selectedValue === "1") {
        $(".mriedPersonalInfo").hide();
    }
    if (selectedValue === "2") {
        $(".mriedPersonalInfo").show();
        $(".mriedPersonalInfo").removeClass("d-none");
    }
});

$('input[name="HasWorkingRelatives"]').on('change', function () {
    var selectedValue = $('input[name="HasWorkingRelatives"]:checked').val();
    if (selectedValue.toLowerCase() === "false") {
        $("#WorkingRelatives").prop("disabled", true)
    }
    if (selectedValue.toLowerCase() === "true") {
        $('#WorkingRelatives').prop("disabled", false);
    }
});

$('input[name="HasWorkingRelativeInPackingCompanies"]').on('change', function () {
    var selectedValue = $('input[name="HasWorkingRelativeInPackingCompanies"]:checked').val();
    if (selectedValue.toLowerCase() === "false") {
        $("#WorkingRelativeInPackingCompanyName").prop("disabled", true)
    }
    if (selectedValue.toLowerCase() === "true") {

        $('#WorkingRelativeInPackingCompanyName').prop("disabled", false);
    }
});

$('input[name="HasSpecialDisease"]').on('change', function () {
    var selectedValue = $('input[name="HasSpecialDisease"]:checked').val();
    if (selectedValue.toLowerCase() === "false") {
        $("#SpecialDisease").prop("disabled", true)
    }
    if (selectedValue.toLowerCase() === "true") {

        $('#SpecialDisease').prop("disabled", false);
    }
});

$('input[name="HasCriminalRecord"]').on('change', function () {
    var selectedValue = $('input[name="HasCriminalRecord"]:checked').val();
    if (selectedValue.toLowerCase() === "false") {
        $("#CriminalRecord").prop("disabled", true)
    }
    if (selectedValue.toLowerCase() === "true") {

        $('#CriminalRecord').prop("disabled", false);
    }
});

$(document).on("change", "#MilitaryServiceStatus", function () {
    var selectedValue = $("#MilitaryServiceStatus").val();

    if (selectedValue == "1") {
        $("#UnitOfmilitaryService").removeAttr("disabled");
        $(".pDatePickerPersianStartDateMilitaryService").removeAttr("disabled");
        $(".pDatePickerPersianEndDateMilitaryService ").removeAttr("disabled");
        $(".medicalExemptionReasonDiv").addClass("d-none");
        customValidationForMedicalExemptionReson(false);
    }
    else {
        $("#UnitOfmilitaryService").attr('disabled', 'disabled');
        $(".pDatePickerPersianStartDateMilitaryService").attr("disabled", "disabled");
        $(".pDatePickerPersianEndDateMilitaryService ").attr("disabled", "disabled");
        if (selectedValue == "3") {
            $(".medicalExemptionReasonDiv").removeClass("d-none");
            customValidationForMedicalExemptionReson(true);
        }
        else {
            $(".medicalExemptionReasonDiv").addClass("d-none");
            customValidationForMedicalExemptionReson(false);
        }
    }
});

//End of Conditional Show And Hide and Enable Or Disable page Elements





$(document).on('click', "#btnFinalSave", function (e) {
    e.preventDefault();

    var form_data = new FormData($('#frmWorkWithUs')[0]);
    form_data.append("IsAgreed", document.getElementById('IsAgreed').checked);
    var request = new XMLHttpRequest();
    Notiflix.Loading.hourglass('در حال پردازش اطلاعات');
    request.onreadystatechange = function () {
        if (request.readyState == XMLHttpRequest.UNSENT) {
            Notiflix.Loading.pulse('در حال پردازش اطلاعات ');
        }
        if (request.readyState == XMLHttpRequest.DONE) {
            Notiflix.Loading.remove();
            var res = jQuery.parseJSON(request.responseText);
            var message = jQuery.parseJSON(request.responseText).message.value;
            if (res.result.toLowerCase() === "ok") {
                window.location.href = "/JobApplicant/Index"
            }
            else if (res.result.toLowerCase() === "fail") {
                teram().showErrorMessage(message);
            }
        }
    }
    request.open("POST", '/WorkWithUs/SaveInfo');
    request.send(form_data);
});




// Dynamic rows
$(document).on('click', "#addTrainingRecord", function () { addTrainingRecord(); });
$(document).on('click', "#addResume", function () { addResume(); });
$(document).on('click', "#addLanguage", function () { addLanguage(); });
$(document).on('click', "#addSkill", function () { addComputerSkill(); });
$(document).on('click', "#addEducation", function () { addEducation(); });
$(document).on('click', "#addEmergencyContacts", function () { addEmergencyContacts(); });
function addTrainingRecord(e) {
    var length = null;
    var divParent = "";

    divParent = $('#TrainingRecordrow');
    length = $('.education-background-row').length;

    $.ajax({
        url: "/WorkWithUS/GetTrainingRecord",
        data: { index: length },
        type: 'POST',
        success: function (data) {
            divParent.before(data);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            var customoption = { "altField": "#StartDateTraining-" + length };
            var options = $.extend(dateOption, customoption);
            datePickerInitilize(".startDateTraining", options);
            options = null;
            customoption = null;
            customoption = { "altField": "#EndDateTraining-" + length };
            options = $.extend(dateOption, customoption);
            datePickerInitilize(".endDateTraining", options);
            $('.select2').select2({
                width: '100%',
                dir: 'rtl'
            });
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });

}
function addResume(e) {
    var length = null;
    var divParent = $('#resumerow');
    length = $(".resume-row").length;

    $.ajax({
        url: "/WorkWithUS/GetResume",
        data: { index: length },
        type: 'POST',
        success: function (data) {
            divParent.before(data);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            var customoption = { "altField": "#StartDateResume-" + length };
            var options = $.extend(dateOption, customoption);
            datePickerInitilize(".startDateResume", options);
            options = null;
            customoption = null;
            customoption = { "altField": "#EndDateResume-" + length };
            options = $.extend(dateOption, customoption);
            datePickerInitilize(".endDateResume", options);
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function addLanguage(e) {
    var length = null;
    var divParent = $('#languagerow');
    length = $(".languages-row").length;

    $.ajax({
        url: "/WorkWithUS/GetLanguages",
        data: { index: length },
        type: 'POST',
        success: function (data) {
            divParent.before(data);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            var customoption = { "altField": "#StartDateResume-" + length };
            var options = $.extend(dateOption, customoption);
            datePickerInitilize(".startDateResume", options);
            options = null;
            customoption = null;
            customoption = { "altField": "#EndDateResume-" + length };
            options = $.extend(dateOption, customoption);
            datePickerInitilize(".endDateResume", options);
            $('.select2').select2({
                width: '100%',
                dir: 'rtl'
            });
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function addComputerSkill() {

    var length = $('.skill-row').length;
    $.ajax({
        url: "/WorkWithUS/GetComputerSkills",
        data: { index: length },
        type: 'POST',
        success: function (data) {
            $('#skillrow').before(data);
            var customoption = { "altField": "#StartDateEducation-" + length };
            var options = $.extend(dateOption, customoption);
            datePickerInitilize(".startDateEducation", options);
            options = null;
            customoption = null;
            customoption = { "altField": "#EndDateEducation-" + length };
            options = $.extend(dateOption, customoption);
            datePickerInitilize(".endDateEducation", options);
            $('.select2').select2({
                width: '100%',
                dir: 'rtl'
            });
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function addEducation() {
    var length = $('.education-row').length;
    $.ajax({
        url: "/WorkWithUS/GetEducation",
        data: { index: length },
        type: 'POST',
        success: function (data) {
            $('#educationrow').before(data);
            var customoption = { "altField": "#StartDateEducation-" + length };
            var options = $.extend(dateOption, customoption);
            datePickerInitilize(".startDateEducation", options);
            options = null;
            customoption = null;
            customoption = { "altField": "#EndDateEducation-" + length };
            options = $.extend(dateOption, customoption);
            datePickerInitilize(".endDateEducation", options);
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
function addEmergencyContacts(e) {

    var length = null;
    var divParent = $('#EmergencyContactsrow');

    length = $('.EmergencyContacts-row').length;

    $.ajax({
        url: "/WorkWithUS/GetEmergencyContact",
        data: { index: length },
        type: 'POST',
        success: function (data) {
            divParent.before(data);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
        },
        error: function () {
            alert("Dynamic content load failed.");
        }
    });
}
// End of Dynamic Rows



// Geographic Regional Elements
$(document).on("change", "#ProvinceId", function () {

    var id = "#" + $(this).parent().data().id;

    var cityId = "#BirthLocationId";
    var cityName = "";

    var selectedCity = $(cityId).val();
    if (selectedCity && selectedCity != "" && selectedCity != 0 && selectedCity != -1) {
        cityName = $(cityId + " option:selected").text()
    }

    $.post("/GeographicRegion/GetAllCitiesByProvinceId",
        { provinceId: $(this).val() },
        function (content) {

            var selectedCityObject = $(id).parent().data();
            if (selectedCityObject != null)
                var selectedCity = $(id).parent().data().selectedcity;

            $(cityId).empty();
            $(cityId).append(
                '<option value="">-- یک شهر انتخاب کنید --</option>'
            );
            $.each(content.results,
                function (index, item) {
                    $(cityId).append(
                        '<option value="' + item.id + '">' + item.text + '</option>'
                    );
                });
            if (selectedCity) {
                $(cityId).val(selectedCity).trigger('change');
            }
        });
});
$(document).on('change', '#HomeProvinceId', function () { GetContactTownship(); });
$(document).on('change', '#MilitaryProvinceId', function () { GetMilitaryTownship(); });
function GetContactTownship() {

    var cityId = "#HomeCityId";
    var selectedCity = $(cityId).val();
    if (selectedCity && selectedCity != "" && selectedCity != 0 && selectedCity != -1) {
        cityName = $(cityId + " option:selected").text()
    }
    $.post("/WorkWithUS/GetGeoByParent",
        { parentId: $("#HomeProvinceId").val() },
        function (content) {
            var selectedCity = $("#HomeProvinceId").parent().data().selectedcity;
            $(cityId).empty();
            $(cityId).append(
                '<option value="">-- یک شهر انتخاب کنید --</option>'
            );
            $.each(content.results,
                function (index, item) {
                    $(cityId).append(
                        '<option value="' + item.id + '">' + item.text + '</option>'
                    );
                });
            if (selectedCity) {
                $(cityId).val(selectedCity).trigger('change');
            }
        });
}
function GetMilitaryTownship() {
    var cityId = "#MilitaryServiceCityId";
    var selectedCity = $(cityId).val();
    if (selectedCity && selectedCity != "" && selectedCity != 0 && selectedCity != -1) {
        cityName = $(cityId + " option:selected").text()
    }
    $.post("/WorkWithUS/GetGeoByParent",
        { parentId: $("#MilitaryProvinceId").val() },
        function (content) {
            var selectedCity = $("#MilitaryProvinceId").parent().data().selectedcity;
            $(cityId).empty();
            $(cityId).append(
                '<option value="">-- یک شهر انتخاب کنید --</option>'
            );
            $.each(content.results,
                function (index, item) {
                    $(cityId).append(
                        '<option value="' + item.id + '">' + item.text + '</option>'
                    );
                });
            if (selectedCity) {
                $(cityId).val(selectedCity).trigger('change');
            }
        });
}
// End of  Geographic Regional Elements


function customValidationForWorkingRelatives() {
    function toggleValidation() {
        var hasWorkingRelatives = $('input[name="HasWorkingRelatives"]:checked').val();
        var workingRelativesInput = $('#WorkingRelatives')[0];
        // If "True" is selected, add the required attribute; otherwise, remove it
        if (hasWorkingRelatives === "True") {
            workingRelativesInput.setAttribute('required', 'required');
            workingRelativesInput.setCustomValidity('لطفا نام افراد را وارد نمایید');
        } else {
            workingRelativesInput.removeAttribute('required');
            workingRelativesInput.setCustomValidity(''); // Clear custom validation message
            $("#WorkingRelatives-error").empty();
            $("#WorkingRelatives-error").removeClass("field-validation-error");
        }
        // Manually trigger validation update
        workingRelativesInput.reportValidity();
    }

    // Initial call to set validation based on the initial radio button state
    toggleValidation();

    // Event handler for radio button change
    $('input[name="HasWorkingRelatives"]').change(function () {
        toggleValidation();
    });
}

function customValidationForMedicalExemptionReson(hasMedicalExemption) {
    function toggleValidation() {      
        var MedicalExemptionInput = $('#MedicalExemptionReason')[0];
        // If "True" is selected, add the required attribute; otherwise, remove it
        if (hasMedicalExemption === true) {
            MedicalExemptionInput.setAttribute('required', 'required');
            MedicalExemptionInput.setCustomValidity('لطفا دلیل معافیت پزشکی را وارد نمایید');
        } else {
            MedicalExemptionInput.removeAttribute('required');
            MedicalExemptionInput.setCustomValidity(''); // Clear custom validation message
            $("#MedicalExemptionReason-error").empty();
            $("#MedicalExemptionReason-error").removeClass("field-validation-error");
        }
        // Manually trigger validation update
        MedicalExemptionInput.reportValidity();
    }
    // Initial call to set validation based on the initial radio button state
    toggleValidation();

    // Event handler for radio button change
    $('input[name="MilitaryServiceStatus"]').change(function () {
        toggleValidation();
    });
}

function customValidationForWorkingRelativesCompany() {
    function toggleValidationForCompany() {
        var hasWorkingRelatives = $('input[name="HasWorkingRelativeInPackingCompanies"]:checked').val();
        var workingRelativesInput = $('#WorkingRelativeInPackingCompanyName')[0];
        // If "True" is selected, add the required attribute; otherwise, remove it
        if (hasWorkingRelatives === "True") {
            workingRelativesInput.setAttribute('required', 'required');
            workingRelativesInput.setCustomValidity('لطفا نام شرکت را وارد نمایید');
        } else {
            workingRelativesInput.removeAttribute('required');
            workingRelativesInput.setCustomValidity(''); // Clear custom validation message
            $("#WorkingRelativeInPackingCompanyName-error").empty();
            $("#WorkingRelativeInPackingCompanyName-error").removeClass("field-validation-error");
        }
        // Manually trigger validation update
        workingRelativesInput.reportValidity();
    }
    // Initial call to set validation based on the initial radio button state
    toggleValidationForCompany();

    // Event handler for radio button change
    $('input[name="HasWorkingRelativeInPackingCompanies"]').change(function () {
        toggleValidationForCompany();
    });
}

function customeValidationForHasSpecialDisease() {

    function toggleValidationForCompany() {
        var hasWorkingRelatives = $('input[name="HasSpecialDisease"]:checked').val();
        var workingRelativesInput = $('#SpecialDisease')[0];
        // If "True" is selected, add the required attribute; otherwise, remove it
        if (hasWorkingRelatives === "True") {
            workingRelativesInput.setAttribute('required', 'required');
            workingRelativesInput.setCustomValidity('لطفا نام بیماری (ها) را وارد نمایید');
        } else {
            workingRelativesInput.removeAttribute('required');
            workingRelativesInput.setCustomValidity(''); // Clear custom validation message
            $("#SpecialDisease-error").empty();
            $("#SpecialDisease-error").removeClass("field-validation-error");
        }
        // Manually trigger validation update
        workingRelativesInput.reportValidity();
    }
    // Initial call to set validation based on the initial radio button state
    toggleValidationForCompany();

    // Event handler for radio button change
    $('input[name="HasSpecialDisease"]').change(function () {
        toggleValidationForCompany();
    });
}
function customeValidationForHasCriminalRecord() {
    function toggleValidationForCompany() {
        var hasWorkingRelatives = $('input[name="HasCriminalRecord"]:checked').val();
        var workingRelativesInput = $('#CriminalRecord')[0];
        // If "True" is selected, add the required attribute; otherwise, remove it
        if (hasWorkingRelatives === "True") {
            workingRelativesInput.setAttribute('required', 'required');
            workingRelativesInput.setCustomValidity('لطفا شرح سابقه را وارد نمایید');
        } else {
            workingRelativesInput.removeAttribute('required');
            workingRelativesInput.setCustomValidity(''); // Clear custom validation message
            $("#CriminalRecord-error").empty();
            $("#CriminalRecord-error").removeClass("field-validation-error");
        }
        // Manually trigger validation update
        workingRelativesInput.reportValidity();
    }
    // Initial call to set validation based on the initial radio button state
    toggleValidationForCompany();

    // Event handler for radio button change
    $('input[name="HasCriminalRecord"]').change(function () {
        toggleValidationForCompany();
    });


}

$(function () {

    customValidationForWorkingRelatives();
    customValidationForWorkingRelativesCompany();
    customeValidationForHasSpecialDisease();
    customeValidationForHasCriminalRecord();
    datePickerInitilize(".birthDate", dateOptionBirthDate);
    datePickerInitilize(".startDateMilitaryService", dateOptionStartDateMilitaryService);
    datePickerInitilize(".endDateMilitaryService", dateOptionEndDateMilitaryService);
    //////////////////resume////////////////////////////////////
    var customoption = { "altField": "#StartDateResume-0" };
    var options = $.extend(dateOption, customoption);
    datePickerInitilize(".startDateResume", options);
    customoption = null;
    options = null;
    customoption = { "altField": "#EndDateResume-0" };
    options = $.extend(dateOption, customoption);
    datePickerInitilize(".endDateResume", options);
    ///////////////////////Training////////////////////////////
    customoption = null;
    options = null;
    customoption = { "altField": "#StartDateTraining-0" };
    options = $.extend(dateOption, customoption);
    datePickerInitilize(".startDateTraining", options);
    customoption = null;
    options = null;
    customoption = { "altField": "#EndDateTraining-0" };
    options = $.extend(dateOption, customoption);
    datePickerInitilize(".endDateTraining", options);
    //////////////////////Education////////////////////////////
    customoption = null;
    options = null;
    customoption = { "altField": "#StartDateEducation-0" };
    options = $.extend(dateOption, customoption);
    datePickerInitilize(".startDateEducation", options);
    customoption = null;
    options = null;
    customoption = { "altField": "#EndDateEducation-0" };
    options = $.extend(dateOption, customoption);
    datePickerInitilize(".endDateEducation", options);
    initialValidation();
    initFunction();
    $("#WorkingRelatives").prop("disabled", true);
    $("#WorkingRelativeInPackingCompanyName").prop("disabled", true);
    $("#SpecialDisease").prop("disabled", true);
    $("#CriminalRecord").prop("disabled", true);
    CreateFileUploader(null);
    $("#accordion").accordion();
});

function CreateFileUploader(result) {
    initialFileUploader('.fileuploader', result);
}

initFunction = function () {

    initMap();
    var lat = $("#Latitude").val();
    var lon = $("#Longitude").val();
    theMarker = L.marker([lat, lon]).addTo(map);
    markers.push(theMarker);

    initialValidation();
    $("#ProvinceId").val($("#SelectedProvinceId").val()).trigger("change");
    $("#ProvinceId").parent().data("selectedcity", $("#SelectedCityId").val());

    $("#HomeProvinceId").val($("#SelectedHomeCityProvinceId").val()).trigger("change");
    $("#HomeProvinceId").parent().data("selectedcity", $("#SelectedHomeCityId").val());
}

$(document).on('click', '.removeInfo', function () {
    $(this).parent().remove();
});
$(document).on('change', '.chkAgreed', function () {

    if ($('.chkAgreed').prop("checked") == true) {
        $('#btnFinalSave').prop('disabled', false);
    }
    else {
        $('#btnFinalSave').prop('disabled', true);
    }
});

