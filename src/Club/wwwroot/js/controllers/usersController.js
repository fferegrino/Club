// unapprovedUsersController.js
(function () {
    "use strict";
    angular.module("app-users")
        .controller("usersController", usersController);

    function usersController($http) {
        var vm = this;

        vm.queriedUsers = [];

        vm.currentPage = 1;
        vm.pageSize = 10;
        vm.totalPages = 0;

        vm.load = function fetchNewData() {
            var url = "/api/users?pageSize=" + vm.pageSize + "&pageNumber=" + vm.currentPage;
            $http.get(url)
                .then(function (response) {
                    console.log(response.data);
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

        vm.load();
    }

})();