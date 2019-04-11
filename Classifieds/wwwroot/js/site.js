// Write your JavaScript code
function GetSubMenus(menuId,url, callback) {
    $.ajax({
        url: url,
        type: "get",
        data: {id: menuId},
        dataType: "json"
    }).done(function (data, textStatus, jqXHR) {
        callback(data.results);
    }).fail(function (jqXHR, errorText, errorThrown) {
        alert("Failed to load sub-menus!");
    });
}
$(document).ready(function () {
    $("#menu").on("change", function () {
        var id = $(this).val();
        var url = "/Menu/SubMenus/";
        GetSubMenus(id, url, function (data) {
            console.log(data); 

            $("#submenu").empty();
            $("#submenu").append("<option value= 0>Select Sub - Menu</option>");

            for (var i = 0; i < data.length; i++) {
                $("#submenu").append("<option value='"
                    + data[i].id + "'>" + data[i].name + "</option>");
            }
        });
    });

    $("#submenu").on("change", function () {
        var id = $(this).val();
        document.getElementById("MenuID").value = id;
        
    });

    $("#menu").on("change", function () {
        document.getElementById("ParentID").value = $(this).val();
        document.getElementById("MenuID").value = 0;
        console.log($(this).val());
    });

    $("submit-ad").on("click", function () {
        document.getElementById("SubmittedDate").value = new Date();
    });
});