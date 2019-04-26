
function GetAdvertDetail(url, callback) {
    $.ajax({
        url: url,
        type: "get",
    })
    .done(function (data, textStatus, jqXHR) {
        callback(data);
     })
     .fail(function(jqXHR, errorText, errorThrown){
        alert("Failed to fetch advert details"); });
}function GetSubMenus(menuId,url, callback) {
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

    $(".modal-link").on("click", function (e) {
        e.preventDefault();

        $("#advert-detail-modal").remove();

        var url = $(this).attr("href");

        GetAdvertDetail(url, function (data) {
            $(data).modal();
        });
        
    });
    //===================================UPLOADCARE WIDGET=================================
    if ($("#create-ad").is(":visible")) {
        var multiWidget = uploadcare.MultipleWidget('#uploadcareWidget');

        multiWidget.onUploadComplete(function (group) {
            if (group) {
                group;

                document.getElementById("Detail_GroupCdn").value = group.cdnUrl;
                document.getElementById("Detail_GroupCount").value = group.count;
                document.getElementById("Detail_GroupSize").value = group.size;
                document.getElementById("Detail_GroupUuid").value = group.uuid;

            }
        });
        //Get information for individual files
        multiWidget.onChange(function(group) {
            if (group) {
                $.when.apply(null, group.files()).then(

                    function () {
                        var filesInfo = arguments;
                        var html = "";

                        for (i = 0; i < filesInfo.length; i++) {
                            html += "<input type='hidden' id='Detail_AdPictures' name=Detail.AdPictures[" + i + "].Uuid " + 
                                "value = '" + filesInfo[i].uuid + "' />";
                            html += "<input type='hidden' id='Detail_AdPictures' name=Detail.AdPictures[" + i + "].CdnUrl " +
                                "value = '" + filesInfo[i].cdnUrl + "' />";
                            html += "<input type='hidden' id='Detail_AdPictures' name=Detail.AdPictures[" + i + "].Name " +
                                "value = '" + filesInfo[i].name + "' />";
                            html += "<input type='hidden' id='Detail_AdPictures' name=Detail.AdPictures[" + i + "].Size " +
                                "value = '" + filesInfo[i].size + "' />";

                        }
                        $("#pictures").append(html);
                   }
                );

            }
        });
    }
});