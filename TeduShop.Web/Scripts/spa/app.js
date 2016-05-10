/// <reference path="../plugins/angular/angular.js" />

var myApp = angular.module('myModule', []);

myApp.controller("schoolController", schoolController);
myApp.controller("studentController", studentController);
myApp.controller("teacherController", teacherController);

function schoolController($scope) {
    $scope.message = "Announcement from school.";
}
//declare
function studentController($scope) {
    $scope.message = "This is my message from Student";
}

function teacherController($scope) {
    $scope.message = "This is my message from Teacher";
}