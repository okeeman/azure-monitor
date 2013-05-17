// On page load this will hide the table. 
$("table").hide();

var clickCount = 0;

// This function will only run when invoked by a button click.
function displayLoadingImage() {
    if (clickCount === 0) {
        $("#updateArea").css("background-image", "url('../../Images/ajax-loader.gif')");
        clickCount++;    
    }
};

    

