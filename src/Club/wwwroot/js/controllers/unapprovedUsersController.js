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
        vm.totalPages = 0;

        vm.load = function fetchNewData() {
            var url = "/api/users/unapproved?pageSize=" + vm.pageSize + "&pageNumber=" + vm.currentPage;
            $http.get(url)
                .then(function (response) {
                    //console.log(response.data);
                    angular.copy(response.data.items, vm.queriedUsers);
                    vm.totalPages = response.data.totalPages;
                },
                function () {

                });
        }

        vm.nextPage = function() {
            if (vm.currentPage < vm.totalPages) {
                vm.currentPage++;
                vm.load();
            }
        }
        vm.previousPage = function () {
            if (vm.currentPage > 1) {
                vm.currentPage--;
                vm.load();
            }
        }

        vm.approveUser = function (id, approved) {
            var url = "/api/users/approve";
            var data = { 'id': id, 'approved': approved };
            $http({
                method: 'PUT',
                url: url,
                data: data
            }).then(function successCallback(response) {
                $("#" + id).animate({
                    opacity: 0.3
                });
            }, function errorCallback(response) {
            });
        };

        vm.load();
    }

})();