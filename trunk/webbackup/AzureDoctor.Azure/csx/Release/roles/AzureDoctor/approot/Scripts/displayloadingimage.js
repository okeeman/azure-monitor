// On page load this will hide the table. The function will only run when invoked by a button click.
$("table").hide();

var clickCount = 0;
// this runs on every click? clickCount never goes above 1???
function displayLoadingImage() {
    if (clickCount === 0) {
        $("#updateArea").css("background-image", "url('../../Images/ajax-loader.gif')");
        clickCount++;    
    }
};

    

