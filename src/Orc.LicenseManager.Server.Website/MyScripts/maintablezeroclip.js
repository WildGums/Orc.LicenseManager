$(document).ready(function () {
    if ($('table').length > 0) {
        console.info("lmtable detected, loading zeroclipboard");
        ZeroClipboard.config({
            moviePath: "http://cdnjs.cloudflare.com/ajax/libs/zeroclipboard/1.3.2/ZeroClipboard.swf"
        });
        var client = new ZeroClipboard();
        client.on('load', function(client) {
            // alert( "movie is loaded" );
            client.on('datarequested', function(client) {
                client.setText($(this).attr("data-clipboard-text"));
            });
            client.on('complete', function(client, args) {
                toastr.success("Copied text to clipboard!");
            });
        });
        client.on('wrongflash', function() {
            console.info("Wrong flash found!");
            ZeroClipboard.destroy();
        });
        client.on('noflash', function() {
            console.info("No flash found!");
            ZeroClipboard.destroy();
        });
        $("table").on("mouseover", ".copy-button", function () {
            client.clip($(this));
        });
    }
});