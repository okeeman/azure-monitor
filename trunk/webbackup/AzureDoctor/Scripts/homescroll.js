var textArray = 'Azure Doctor - The solution to your Windows Azure cloud service monitoring needs. ';

function scrollText() {
	textArray = textArray.substring(1, textArray.length) + textArray[0];
	document.getElementById('homeScrollDiv').innerHTML = textArray;
}