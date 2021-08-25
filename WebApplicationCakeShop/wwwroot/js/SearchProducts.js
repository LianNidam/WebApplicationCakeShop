$(function () {
    $('form').submit(function (e) {
        e.preventDefault();
        var query = $('#query').val();
        $('tbody').load('/Cakes/SearchPtable?query=' + query);
    });
});