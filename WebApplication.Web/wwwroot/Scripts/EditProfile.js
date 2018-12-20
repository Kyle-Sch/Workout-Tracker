// <reference path="../Scripts/jquery-3.1.1.js" />
// <reference path="../Scripts/jquery.validate.js" />
// <reference path="../Scripts/jquery.validate-vsdoc.js" />
function bob() {
    var shipCost = '/images/' + $('#setPic input: selected').val();
    $('#profilePic').attr('src', shipCost);
    console.log(shipCost);
}
$(document).ready(function () {
    document.getElementById("setPic").addEventListener("change", function () {
        var shipCost = '/images/' + $('#setPic input: selected').val();
        $('#profilePic').attr('src',shipCost);
    });
    
});
