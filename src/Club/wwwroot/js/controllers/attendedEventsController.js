// unapprovedUsersController.js
(function () {
    "use strict";
    angular.module("app-attended-events")
        .controller("attendedEventsController", attendedEventsController);

    function attendedEventsController($http) {
        var vm = this;

        vm.queriedUsers = [];

        vm.currentPage = 1;
        vm.pageSize = 10;
        vm.totalPages = 0;

        vm.load = function fetchNewData() {
            console.log($("#id"));
            var url = "/api/events/attended/"+ $("#id").val() +"?pageSize=" + vm.pageSize + "&pageNumber=" + vm.currentPage;
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


        vm.load();
    }

})();