var img = document.getElementsByTagName('img');

for (var i in img) {
    img[i].oncontextmenu = function () {
        return false;
    }
}