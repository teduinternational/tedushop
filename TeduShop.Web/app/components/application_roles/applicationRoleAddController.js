(function (app) {
    'use strict';

    app.controller('applicationRoleAddController', applicationRoleAddController);

    applicationRoleAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function applicationRoleAddController($scope, apiService, notificationService, $location, commonService) {
        $scope.role = {
            Id: 0
        }

        $scope.addAppRole = addAppRole;

        function addAppRole() {
            apiService.post('/api/applicationRole/add', $scope.role, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.role.Name + ' đã được thêm mới.');

            $location.url('application_roles');
        }
        function addFailed(response) {
            notificationService.displayError(response.data.Message);
            notificationService.displayErrorValidation(response);
        }

        function loadRoles() {
            apiService.get('/api/applicationRole/getlistall',
                null,
                function (response) {
                    $scope.roles = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách quyền.');
                });

        }

        loadRoles();
    }
})(angular.module('tedushop.application_roles'));