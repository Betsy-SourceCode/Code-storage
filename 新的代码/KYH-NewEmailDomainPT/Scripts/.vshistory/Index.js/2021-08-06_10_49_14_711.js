var app = angular.module('myApp', []);  //创建模块
app.controller('ProductPriceController', function ($scope) {
    $scope.IndexList = function () {
        $.ajax({
            url: "/ProductPrice/ProductPrice/IndexData",
            data: $.param({ 'Cancel': Cancel }) + '&' + from,
            type: "POST",
            dataType: "json",
            success: function (data) {

            }
        });
    }
})