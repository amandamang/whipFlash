

$(document).ready(function () {

    

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    $("#flipButtonCard").flip({});

    // Get the modal
    var modal = document.getElementById('myModal');

    // Get the button that opens the modal
    var btn = document.getElementById("myBtn");

    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("close")[0];

    // When the user clicks the button, open the modal 
    $(btn).click(function () {
        modal.style.display = "block";
    });

    // When the user clicks on <span> (x), close the modal
    $(span).click(function () {
        modal.style.display = "none";
    });

    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target === modal) {
            modal.style.display = "none";
        }
    };
    
    function move() {
        var elem = document.getElementById("myBar");
        var width = 1;
        var id = setInterval(frame, 10);
        function frame() {
            if (width >= 100) {
                clearInterval(id);
            } else {
                width++;
                elem.style.width = width + '%';
            }
        }
    }

    var links = document.getElementsByClassName('enter_links');
});


