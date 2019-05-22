function GetMenuUrl(elementId) {
    var element = document.getElementById(elementId);
    var string = element.value
    if (string !== null || string !== "") {
        var url = "/Admin/" + string.charAt(0).toUpperCase() + string.slice(1);
        $("#Url").attr("value", url);
    }
}
function SubmitForm(formId) {
    var formToSubmit = document.getElementById(formId);
    var url = $(formToSubmit).attr('action');
    var formData = $(formToSubmit).serializeArray();

    $.ajax({
        url: url,
        data: formData,
        type:"post"
    })
        .done(function (data, textStatus, jqXHR) {
            $('.modal').remove();
    //If ModelState.IsValid == false
        if (jqXHR.status === 200) {
        
        $(data).modal();
    }
    if (jqXHR.status === 201) {
        alert(data);
        window.location.reload();
    }
    })
    .fail(function (jqXHR, errorText, errorThrown) {
        alert("Failed to submit. Internal error.");
    });
}
function GetPartialView(url, callback) {
    $.ajax({
        url: url,
        type: "get",
    })
        .done(function (data, textStatus, jqXHR) {
            callback(data);
        })
        .fail(function (jqXHR, errorText, errorThrown) {
            alert("Failed to fetch view");
        });
}
function GetSubCategories(categoryId,url, callback) {
    $.ajax({
        url: url,
        type: "get",
        data: {id: categoryId},
        dataType: "json"
    }).done(function (data, textStatus, jqXHR) {
        callback(data.results);
    }).fail(function (jqXHR, errorText, errorThrown) {
        alert("Failed to load sub-categories!");
    });
}
$(document).ready(function () {
    $(document).on('hidden.bs.modal', '.modal', function () {
        $(".modal-dialog").remove();
    });


    $("#category").on("change", function () {
        var id = $(this).val();
        var url = "/Category/SubCategories/";
        GetSubCategories(id, url, function (data) {
            console.log(data); 

            $("#subcategory").empty();
            $("#subcategory").append("<option value= 0>Select Sub-Category</option>");

            for (var i = 0; i < data.length; i++) {
                $("#subcategory").append("<option value='"
                    + data[i].id + "'>" + data[i].name + "</option>");
            }
        });
    });

    $("#subcategory").on("change", function () {
        var id = $(this).val();
        document.getElementById("CategoryID").value = id;
        
    });

    $("#category").on("change", function () {
        document.getElementById("ParentID").value = $(this).val();
        document.getElementById("CategoryID").value = 0;
        console.log($(this).val());
    });
    /**
     * Advert details modal
     * 
     * */
    $(".modal-link").on("click", function (e) {
        e.preventDefault();

        $("#advert-detail-modal").remove();

        var url = $(this).attr("href");

        GetPartialView(url, function (data) {
            $(data).modal();
        });
        
    });
    $("#edit-address").on("click", function (e) {
        e.preventDefault();

        $("#address-form").remove();

        var url = $(this).attr("href");

        GetPartialView(url, function (data) {
            $(data).modal();
        });
    });
    $("#add-menu").on("click", function (e) {
        e.preventDefault();

        var url = "/Menu/Create"

        GetPartialView(url, function (data) {
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
    //=======================================ACCORDION ACTIVE============================
    $(".acc-nav-link").each(function () {
        
        var url = window.location.href;


        if (url === this.href) {
            //remove previous classes
            $(".accordion li").removeClass("active");
            $(".accordion .acc-heading").removeClass("active");
            $(".accordion h6 a").attr("aria-expanded", "false");
            $(".accordion div").removeClass("show");

            //add new classes
            $(this).addClass("active");
            $(this).closest("li").addClass("active");
            $(this).closest(".acc-heading").addClass("active");
            $(this).closest("h6 a").attr("aria-expanded", "true");

            var divId = $(this).closest("div").attr("id");
            var div = document.getElementById(divId);
            $("#" + divId).addClass("show");
        }
        
    });
   
});