function openClosedNotifications() {
    var notList = document.getElementsByClassName("divNotList")[0];
    var currentNotListItem = notList.firstChild;
    while (currentNotListItem != null)
    {
        if ($(currentNotListItem).is(":hidden"))
        {
            $(currentNotListItem).slideDown("slow");
        }
        currentNotListItem = currentNotListItem.nextSibling;
    }
}