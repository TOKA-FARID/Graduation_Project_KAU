$(document).ready(function () {
    

    $('#IsAssigned').change( function () {
        
        if (this.checked) {
            
            $('#TrainerId').removeClass("d-none");
            $('#lblTrainer').removeClass("d-none");

        } else {
            $('#TrainerId').addClass("d-none");
            $('#lblTrainer').addClass("d-none");

        }
    });

    $("#inputSearch").keyup(function () {
        var input, filter, table, tr, tdActivityName, tdLocation, tdtype, i, txtNameValue, txtLocationValue, txtTypeValue;
        input = document.getElementById("inputSearch");
        filter = input.value.toUpperCase();
        table = document.getElementById("tableSearch");
        tr = table.getElementsByTagName("tr");
        for (i = 0; i < tr.length; i++) {
            tdActivityName = tr[i].getElementsByTagName("td")[0];
            tdLocation = tr[i].getElementsByTagName("td")[1];
            tdtype = tr[i].getElementsByTagName("td")[2];
           

            if (tdActivityName || tdLocation || tdtype ) {
                txtNameValue = tdActivityName.textContent || tdActivityName.innerText;
                txtLocationValue = tdLocation.textContent || tdLocation.innerText;
                txtTypeValue = tdtype.textContent || tdtype.innerText;
                

                if (txtNameValue.toUpperCase().indexOf(filter) > -1 || txtLocationValue.toUpperCase().indexOf(filter) > -1 || txtTypeValue.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = "";
                } else {
                    tr[i].style.display = "none";
                }
            }
        }
    });

    /////////////////////
});