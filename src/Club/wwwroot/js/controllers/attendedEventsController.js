// unapprovedUsersController.js
(function () {
    "use strict";
    angular.module("app-attended-events")
        .controller("attendedEventsController", attendedEventsController)
    .controller("submissionsController", submissionsController);;

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

    

    function submissionsController($http) {
        var tt = this;

        tt.queriedSubmissions = [];

        //vm.currentPage = 1;
        //vm.pageSize = 10;
        //vm.totalPages = 0;

        tt.load = function fetchNewData() {
            console.log($("#id"));
            var url = "/api/submissions/"+ $("#id").val();
            $http.get(url)
                .then(function (response) {
                    console.log(response.data);
                    angular.copy(response.data, tt.queriedSubmissions);
                    //vm.totalPages = response.data.totalPages;
                },
                function () {

                });
        }

        //vm.nextPage = function() {
        //    if (vm.currentPage < vm.totalPages) {
        //        vm.currentPage++;
        //        vm.load();
        //    }
        //}
        //vm.previousPage = function () {
        //    if (vm.currentPage > 1) {
        //        vm.currentPage--;
        //        vm.load();
        //    }
        //}


        tt.load();
    }

})();