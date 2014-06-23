// FadeIn all Notifications that are marked as hidden
function openClosedNotifications() {
    var notList = document.getElementsByClassName("divNotList")[0];
    var currentNotListItem = notList.firstChild;

    var foundHiddenChild = false;
    while (currentNotListItem != null && foundHiddenChild == false)
    {
        if ($(currentNotListItem).is(":hidden"))
        {
            foundHiddenChild = true;
            $(currentNotListItem).fadeIn("medium", openClosedNotifications);
        }
        currentNotListItem = currentNotListItem.nextSibling;
    }
}

// Restart timerUpdate
function restartTimer() {
    var timerUpdate = $find('MainContent_timerUpdate');
    timerUpdate._stopTimer();
    timerUpdate._startTimer();
}