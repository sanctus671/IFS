
app.controller('MainController', function ($scope, requestsService,$location) {


    init();

    function init() {
        
        $scope.permissions = requestsService.getUser().permissions;
        
        //check we the correct page is loaded based on the users permissions
        if ($scope.permissions  === 'standard' && $location.path().indexOf("standard") === -1){
            document.location.href='/#/standard/';
        }        
        else if ($scope.permissions  === 'manager' && $location.path().indexOf("manager") === -1){
            document.location.href='/#/manager/';
        }
        else if ($scope.permissions  === 'accountant' && $location.path().indexOf("accountant") === -1){
            document.location.href='/#/accountant/';
        }
        else if ($scope.permissions  === 'admin' && $location.path().indexOf("admin") === -1){
            $location.path('/admin/');
        }
        
        
        $scope.currentPage = 1;
        $scope.pageSize = 10;        
        $scope.orderByField = "date";
        $scope.sortReverse = true;
        
        $scope.requests = requestsService.getRequests();

      
    }
    

    
    $scope.changeOrder = function (header) {
        $scope.orderByField = header; 
        $scope.sortReverse = !$scope.sortReverse;
        console.log($scope.orderByField);
    };
    
    $scope.insertRequest = function () {      

        requestsService.insertRequest($scope.newRequest);
        $scope.newRequest = {};
    };
    
    
    
    
    $scope.reorderRequest = function (id) {
        for (var x in $scope.requests){
            if ($scope.requests[x].id === id){
                $scope.newRequest = $.extend(true, {}, $scope.requests[x]);
                $scope.insertRequest();
            }
        }
    };




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
        if ($('.myorders').length){
            $('.myorders').removeClass('active'); 
            $('.myorders').click(function(){
                $('.myorders').addClass('active'); 
            });
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
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };
  
  $scope.openFilter = function (size) {

    var modalInstance = $modal.open({
      templateUrl: '/app/partials/filter.html',
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
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };  
  
  $scope.openEdit = function (size) {

    var modalInstance = $modal.open({
      templateUrl: '/app/partials/edit.html',
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
      //$log.info('Modal dismissed at: ' + new Date());
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




