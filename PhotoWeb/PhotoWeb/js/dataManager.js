
//��½
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
           //��½�ɹ�����ת��ע��ʧ����ʾʧ��ԭ��
           if (data == "ע��ɹ�") {
               window.location.href = "myshare.html";
           } else {
               alert(data);
           }
       })
       .error(function (data) { console.log("���ʷ�����ʧ��"); });
    };

});

//ע��
var signInApp = angular.module('signInApp', []);
signInApp.controller('siteCtrl', function ($scope, $http) {
    $scope.signUp = function () {
        $http({
            method: 'post',
            url: "http://localhost:51060/api/User/signup",
            data: { name: $scope._name, passWord: $scope._email_, email: $scope._passWord }
        })
       .success(function (data) {
           //ע��ɹ�����ת��ע��ʧ����ʾʧ��ԭ��
           if (data == "ע��ɹ�") {
               window.location.href = "myshare.html";
           } else {
               alert(data);
           }
       })
       .error(function (data) { alert("���ʷ�����ʧ��"); });
    };

});


//����
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
            alert("ͼƬ�ϴ��ɹ�");
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