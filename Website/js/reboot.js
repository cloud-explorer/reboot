var reboot = {};//default workspace

$(function () {
    reboot.search = {
        init: function () {
            $("#searchButton").click(function () {
                var searchText = $("#searchText").val();
                if (searchText.length > 0) {
                    window.location.href = "?keyword=" + searchText;
                }
            });

            $("#searchText").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    $("#searchButton").click();
                }
            });
        },
        
    };

    reboot.search.init();
});



