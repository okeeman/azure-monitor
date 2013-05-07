function displayStatus() {
        $("table").show();

        var allinstanceshealthy = $("#allinstanceshealthyinput").data("allinstanceshealthy");
        
        if (allinstanceshealthy === "True") {
            $("#updateArea").css("background-image", "url('../../Images/green-heart-icon.png')");
            document.getElementById('resultDiv').innerHTML = 'All Instances Healthy';
            $("#resultDiv").css("color", "green");
        }
        else{
            $("#updateArea").css("background-image", "url('../../Images/red-heart-icon.png')");
            document.getElementById('resultDiv').innerHTML = 'Not all instances are healthy';
            $("#resultDiv").css("color", "whitesmoke");
        }
        
        $("#updateArea").css("background-position", "right center");
};


