$(document).ready(function () {
    loadData();
});

//Load Data function  
function loadData() {
        // Make an Ajax request to fetch the genres
        $(document).ready(function () {
        // Make an Ajax request to fetch the genres
        $.ajax({
            url: '/Genre/GetGenres',
            type: 'GET',
            success: function (response) {
                // Handle success
                var genres = response.vmGenres;
                var itemList = $('#itemList');

                // Clear the existing list
                itemList.empty();

                // Iterate over the genres and append them to the list
                genres.forEach(function (genre) {
                    var listItem = $('<li>').text(genre.Name);
                    itemList.append(listItem);
                });
            },
            error: function (error) {
                // Handle error
                console.error('Error retrieving genres:', error);
            }
        });
} 