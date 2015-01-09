
app.controller('MainController', function ($scope, requestsService) {


    init();

    function init() {
        
        $scope.currentPage = 1;
        $scope.pageSize = 5;        
        
        $scope.requests = requestsService.getRequests();

      
    }

    $scope.reorderRequest = function (id) {
        for (var x in $scope.requests){
            if ($scope.requests[x].id === id){
                $scope.newRequest.firstName = $scope.requests[x].firstName;
                $scope.newRequest.lastName = $scope.requests[x].lastName;
                $scope.newRequest.city = $scope.requests[x].city;                
            }
        }
    }




    $scope.editRequest = function () {
        var firstName = $scope.newRequest.firstName;
        var lastName = $scope.newRequest.lastName;
        var city = $scope.newRequest.city;
        requestsService.updateRequest(firstName, lastName, city);
    }




    $scope.deleteRequest = function (id) {
        requestsService.deleteRequest(id);
    };
});


app.controller('MainViewController', function ($scope, $routeParams, requestsService) {
    $scope.request = {};
    $scope.ordersTotal = 0.00;



    init();

    function init() {
    
        var requestID = ($routeParams.reqID) ? parseInt($routeParams.reqID) : 0;
        if (requestID > 0) {

            $scope.request = requestsService.getRequest(requestID);
        }
    }

});




app.controller('ModalCtrl', function ($scope, $modal, $log) {

  $scope.items = ['item1', 'item2', 'item3'];
  
  $scope.open = function (size) {

    var modalInstance = $modal.open({
      templateUrl: '/app/partials/create.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        }
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      $log.info('Modal dismissed at: ' + new Date());
    });
  };
});



app.controller('ModalInstanceCtrl', function ($scope, $modalInstance, items, requestsService) {

  $scope.items = items;
  $scope.selected = {
    item: $scope.items[0]
  };
    $scope.insertRequest = function () {      

        requestsService.insertRequest($scope.newRequest);
        $scope.newRequest = {};
    };
    
    
  $scope.ok = function () {
    $modalInstance.close($scope.selected.item);
  };

  $scope.cancel = function () {
    $modalInstance.dismiss('cancel');
  };
});




