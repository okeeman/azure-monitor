function displayStatus() {
        $("table").show();

        var allinstanceshealthy = $("#allinstanceshealthyinput").data("allinstanceshealthy");
        
        if (allinstanceshealthy === "True") {
            $("#updateArea").css("background-image", "url('../../Images/green-heart-icon.png')");
        }
        else{
            $("#updateArea").css("background-image", "url('../../Images/red-heart-icon.png')");
        }
        
        $("#updateArea").css("background-position", "right");
};


