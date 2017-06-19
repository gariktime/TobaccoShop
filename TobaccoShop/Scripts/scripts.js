function changeOrderStatusSelection() {
    $("#orderStatus li.active").removeClass("active");
    $(this).parent().addClass("active");
}

function changeUserRoleSelection() {
    $("#userRole li.active").removeClass("active");
    $(this).parent().addClass("active");
}

$(document).ready(function () {
    var min_price = $("#MinPrice").val();
    var max_price = $("#MaxPrice").val();
    var min_height = $("MinHeight").val();
    var max_height = $("MaxHeight").val();
      
    $("#price_range").ionRangeSlider({
        type: "double",
        min: min_price,
        max: max_price,
        from: min_price,
        to: max_price,
        hide_min_max: true,
        hide_from_to: true,
        grid: false,
        onChange: function (data) {
            $("#MinPrice").val(data.from);
            $("#MaxPrice").val(data.to);
        }    
    });

    $("#height_range").ionRangeSlider({
        type: "double",
        min: min_height,
        max: max_height,
        from: min_height,
        to: max_height,
        hide_min_max: true,
        hide_from_to: true,
        grid: false,
        onChange: function (data) {
            $("#MinHeight").val(data.from);
            $("#MaxHeight").val(data.to);
        }
    });
});