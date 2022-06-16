//CertificatesManagement
var app = angular.module('myApp', []);  //创建模块
app.controller('mycontroller', function ($scope, $compile) {
    $scope.CertificatesManagement = function () {
        layer.open({
            type: 2,
            title: '认 证 管 理',
            skin: 'layui-layer-rim', //加上边框
            area: ['1250px', '80%'], //宽高
            content: '../CertificationApplication/Certificate_Update',
            btnAlign: 'c'
        });

    }
})
