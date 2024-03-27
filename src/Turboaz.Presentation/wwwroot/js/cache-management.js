function clearCache() {
    $(document).ready(function () {
        $('body').load(function () {
           $('body').css('background-image', 'url("../Wallpaper/Wallpaper.jpeg")');
        });
     });
}