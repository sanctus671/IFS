
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
    
    $scope.changePageSize = function (size) {
        
        $scope.pageSize = size; 
        console.log($scope.pageSize);

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






    $scope.deleteRequest = function (id) {
        requestsService.deleteRequest(id);
    };
    
  $scope.filterRequests = function(field,options1,options2,value,date1,date2){
      console.log("here");
        if (options1){
            requestsService.filterRequests(field, options1, value, date1, date2);
        }
        else{
            requestsService.filterRequests(field, options2, value, date1, date2);
        }
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
            $('.back-button').click(function(){
                $('.myorders').addClass('active'); 
            });            
            
        }
    }
    
    $scope.insertRequest = function () {      

        requestsService.insertRequest($scope.newRequest);
        $scope.newRequest = {};
    };    
    $scope.deleteRequest = function (id) {
        requestsService.deleteRequest(id);
    };    

});




app.controller('ModalCtrl', function ($scope, $modal, $log) {

  $scope.items = ['item1', 'item2', 'item3'];
  
  $scope.open = function (size, id) {

    var modalInstance = $modal.open({
      templateUrl: 'app/partials/create.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        }
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };
  
  $scope.openFilter = function (size, id) {

    var modalInstance = $modal.open({
      templateUrl: 'app/partials/filter.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        }
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };  

  $scope.openEdit = function (size,id) {
      
    var modalInstance = $modal.open({
      templateUrl: 'app/partials/edit.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        }
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };   
  
  
  
  
    
  $scope.confirmDelete = function (size,id) {
      
    var modalInstance = $modal.open({
      templateUrl: 'app/partials/delete.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
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



app.controller('ModalInstanceCtrl', function ($scope, $modalInstance, items, requestsService, id) {
  $scope.id = id;
  if (id > 0){$scope.request = requestsService.getRequest(id);}
  $scope.items = items;
  $scope.selected = {
    item: $scope.items[0]
  };
    $scope.insertRequest = function () {  
        var toBeInserted = {
            cas:$scope.request['cas'],
            chimTag:$scope.request['chimTag'],
            date:$scope.request['date'],
            dateSupplied:$scope.request['dateSupplied'],
            destinationRoom:$scope.request['destinationRoom'],
            email:$scope.request['email'],
            itemDescription:$scope.request['itemDescription'],
            location:$scope.request['location'],
            name:$scope.request['name'],
            notes:$scope.request['notes'],
            quality:$scope.request['quality'],
            quantity:$scope.request['quantity'],
            size:$scope.request['size'],
            status:$scope.request['status']};


        requestsService.insertRequest(toBeInserted);
        $scope.request = {};
    };

    $scope.deleteRequest = function (id) {
        requestsService.deleteRequest(id);
    };     
    
  $scope.ok = function () {
    $modalInstance.close($scope.selected.item);
  };

  $scope.cancel = function () {
    $modalInstance.dismiss('cancel');
  };
  

  
  $scope.go = function ( path ) {
  $location.path( path );
    };
  
  
  
  
});




