$(document).ready(function () {

    initialValidation();
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
    initFunction();
});

initFunction = function () {

    initialValidation();   
    $("#ProvinceId").val($("#SelectedProvinceId").val()).trigger("change");
    $("#ProvinceId").parent().data("selectedcity", $("#SelectedCityId").val());

    $("#MilitaryProvince").val($("#SelectedMilitaryServiceProvinceId").val()).trigger("change");
    $("#MilitaryTownship").val($("#SelectedMilitaryServiceCityId").val());


}