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

	    //fetchNewData(currentPage);
	}

})();