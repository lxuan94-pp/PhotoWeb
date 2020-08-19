
//登陆
var loginApp = angular.module('loginApp', []);
loginApp.controller('siteCtrl', function ($scope, $http) {
    //get all Users
    $scope.login = function () {
        $http({
            method: 'post',
            url: "http://localhost:51060/api/User/login",
            data: { name: $scope.user, passWord: $scope.passWord }
        })
       .success(function (data) {
           //登陆成功则跳转，注册失败显示失败原因
           if (data == "注册成功") {
               window.location.href = "myshare.html";
           } else {
               alert(data);
           }
       })
       .error(function (data) { console.log("访问服务器失败"); });
    };

});

//注册
var signInApp = angular.module('signInApp', []);
signInApp.controller('siteCtrl', function ($scope, $http) {
    $scope.signUp = function () {
        $http({
            method: 'post',
            url: "http://localhost:51060/api/User/signup",
            data: { name: $scope._name, passWord: $scope._email_, email: $scope._passWord }
        })
       .success(function (data) {
           //注册成功则跳转，注册失败显示失败原因
           if (data == "注册成功") {
               window.location.href = "myshare.html";
           } else {
               alert(data);
           }
       })
       .error(function (data) { alert("访问服务器失败"); });
    };

});


//分享
var shareApp = angular.module('shareApp', []);
shareApp.controller('UploaderController', function ($scope, $http) {
    $scope.save = function () {
        var fd = new FormData();
        var file = document.querySelector('input[type=file]').files[0];
        fd.append('logo', file);
        $http({
            method: 'POST',
            url: "http://localhost:51060/api/User/upload",
            data: fd,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).success(function (response) {
            alert("图片上传成功");
        });
    }
    $scope.getImage = function () {
        $http({
            method: 'post',
            url: "http://localhost:51060/api/User/get",
            data: { name: "default" }
        }).success(function (data) {
            //console.log(data);
            setting(data);
        }
        )
    }
});