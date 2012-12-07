    function refeshResult() {
        xmlHttpRequest.onreadystatechange = function () {
            if (xmlHttp.readyState == 4) {
                $('#updateArea').load('@Url.Action("TestService", "AzureDoctor")/' + '1');
                setTimeout('refreshResult', 20000);
            }
        }
    }
