/// <reference path="../plugins/angular/angular.js" />

var myApp = angular.module('myModule', []);

myApp.controller("schoolController", schoolController);
myApp.service('Validator', Validator);

schoolController.$inject =['$scope','Validator'];

function schoolController($scope, Validator) {
   
    $scope.checkNumber = function () {
        $scope.message = Validator.checkNumber($scope.num);
    }
    $scope.num = 1;
}

function Validator($window) {
    return {
        checkNumber: checkNumber
    }
    function checkNumber(input) {
        if (input % 2 == 0) {
            return 'This is even';
        }
        else
            return 'This is odd';
    }

}

