var app = angular.module('myApp', []);  //创建模块
app.controller('ProductPriceController', function ($scope) {
    $scope.IndexList = function () {
        $.ajax({
            url: "/NewEmailDomainPT/NewEmailDomainPT/IndexData",
            data: $('#Myform').serialize(),
            type: "POST",
            dataType: "json",
            success: function (data) {

            }
        });
    }
})