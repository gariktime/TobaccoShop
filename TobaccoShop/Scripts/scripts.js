//function AddToCart(data) {
//    var count = data[cart_count];
//    $('#cartProductsCount').append('<p>' + count + '</p>');
//}

$(document).ready(function () {
    var min_price = $("#MinPrice").val();
    var max_price = $("#MaxPrice").val();
    var min_height = $("MinHeight").val();
    var max_height = $("MaxHeight").val();

    //$("#price_min #price_max").on("change keydown keypress keyup mousedown click mouseup", function () {
    //    //$("#price_range").prop("from").val($("#price_min").val());
    //    var slider = $("#price_range").data("ionRangeSlider");
    //    slider.update({
    //        from: $("#price_min").val(),
    //        to: $("#price_max").val()
    //    });
    //});
        
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

    $("#orderstatus ul li a").click(function () {
        $("#orderstatus ul li a").removeClass("active");
        $(this).toggleClass("active");
    });
});