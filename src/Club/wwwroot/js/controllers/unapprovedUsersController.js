// unapprovedUsersController.js
(function () {
    "use strict";
    angular.module("app-unapproved-users")
        .controller("unapprovedUsersController", unapprovedUsersController);

    function unapprovedUsersController($http) {
        var vm = this;

        vm.queriedUsers = [];

        vm.currentPage = 1;
        vm.pageSize = 5;
        vm.pageNumber = 1;

        vm.load = function fetchNewData() {
            var url = "/api/users/unapproved?pageSize=" + vm.pageSize + "&pageNumber=" + vm.pageNumber;
            $http.get(url)
                .then(function (response) {
                    angular.copy(response.data.items, vm.queriedUsers);
                },
                function () {

                });
        }

        vm.approveUser = function (id, approved) {
            var url = "/api/users/approve";
            var data = { 'id': id, 'approved': approved };
            $http({
                method: 'PUT',
                url: url,
                data: data
            }).then(function successCallback(response) {
            }, function errorCallback(response) {
            });
        };

        vm.load();
    }

})();