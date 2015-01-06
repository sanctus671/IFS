/*#######################################################################
  
  Dan Wahlin
  http://twitter.com/DanWahlin
  http://weblogs.asp.net/dwahlin
  http://pluralsight.com/training/Authors/Details/dan-wahlin

  Normally like the break AngularJS controllers into separate files.
  Kept them together here since they're small and it's easier to look through them.
  example. 

  #######################################################################*/


//This controller retrieves data from the customersService and associates it with the $scope
//The $scope is ultimately bound to the customers view
app.controller('MainController', function ($scope, requestsService) {

    //I like to have an init() for controllers that need to perform some initialization. Keeps things in
    //one place...not required though especially in the simple example below
    init();

    function init() {
        $scope.requests = requestsService.getRequests();
    }

    $scope.insertRequest = function () {
        var firstName = $scope.newRequest.firstName;
        var lastName = $scope.newRequest.lastName;
        var city = $scope.newRequest.city;
        requestsService.insertRequest(firstName, lastName, city);
        $scope.newRequest.firstName = '';
        $scope.newRequest.lastName = '';
        $scope.newRequest.city = '';
    };

    $scope.deleteRequest = function (id) {
        requestsService.deleteRequest(id);
    };
});

//This controller retrieves data from the customersService and associates it with the $scope
//The $scope is bound to the order view
app.controller('MainViewController', function ($scope, $routeParams, requestsService) {
    $scope.request = {};
    $scope.ordersTotal = 0.00;

    //I like to have an init() for controllers that need to perform some initialization. Keeps things in
    //one place...not required though especially in the simple example below
    init();

    function init() {
        //Grab customerID off of the route        
        var requestID = ($routeParams.requestID) ? parseInt($routeParams.requestID) : 0;
        if (requestID > 0) {
            $scope.request = requestsService.getRequest(requestID);
        }
    }

});





