$("table").hide();

    var clickCount = 0;
   
    function displayLoadingImage() {
        if (clickCount === 0) {
            $("#updateArea").css("background-image", "url('../../Images/ajax-loader.gif')");
            clickCount++;
        }
    };