
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



app.controller('ModalInstanceCtrl', function ($scope, $modalInstance, $compile, items, requestsService, id) {
  $scope.id = id;
  if (id > 0){$scope.request = requestsService.getRequest(id); $scope.request.dateSupplied = "";}
  $scope.items = items;
  $scope.selected = {
    item: $scope.items[0]
  };
    $scope.insertRequest = function () {  
        var toBeInserted = {
            cas:$scope.request['cas'],
            vertere:$scope.request['vertere'],
            date:$scope.request['date'],
            dateSupplied:$scope.request['dateSupplied'],
            destinationRoom:$scope.request['destinationRoom'],
            type:$scope.request['type'],
            email:$scope.request['email'],
            itemDescription:$scope.request['itemDescription'],
            location:$scope.request['location'],
            name:$scope.request['name'],
            notes:$scope.request['notes'],
            quality:$scope.request['quality'],
            quantity:$scope.request['quantity'],
            size:$scope.request['size'],
            status:$scope.request['status'],
            accountNumber:$scope.request['accountNumber']};


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
  
  
    //for autocomplete
    $scope.changeClass = function (options) {
        var widget = options.methods.widget();
        // remove default class, use bootstrap style
        widget.removeClass('ui-menu ui-corner-all ui-widget-content').addClass('dropdown-menu');
    }; 
    
    $scope.descriptionOptions = {
        options: {
            html: true,
            minLength: 1,
            onlySelectValid: true,
            outHeight: 50,
            source: function (request, response) {
                var data = [
                            "This is a description of the item",
                            "A recent item description",
                            "Another recent item description",
                            "This one",
                            "Descriptions get saved for suggestions automatically when you make a new request"
                    ];
                    
                    data = $scope.descriptionOptions.methods.filter(data, request.term);

                    if (!data.length) {
                        data.push({
                            label: 'not found',
                            value: null
                        });
                    }
                    // add "Add Language" button to autocomplete menu bottom
                    /*data.push({
                        label: $compile('<a class="ui-menu-add" ng-click="add()">Add Language</a>')($scope),
                        value: null
                    });*/
                    response(data);
                }
            },
            events: {
                change: function (event, ui) {
                    //console.log('change', event, ui);
                },
                select: function (event, ui) {
                    //console.log('select', event, ui);
                }
            }
        };
        
        
    $scope.destinationRoomOptions = {
        options: {
            html: true,
            minLength: 1,
            onlySelectValid: true,
            outHeight: 50,
            source: function (request, response) {
                var data = [
                            "SC B 1",
                            "SC B 2",
                            "SC B 3",
                            "SC B 4",
                            "SC C 1",
                            "SC C 2",
                            "SC C 3",
                            "SC C 4",
                            "SC D 1",
                            "SC D 2",
                            "SC D 3",
                            "SC D 4",
                            "SC A 1",
                            "SC A 2",
                            "SC A 3",
                            "SC A 4"
                    ];
                    
                    data = $scope.destinationRoomOptions.methods.filter(data, request.term);

                    if (!data.length) {
                        data.push({
                            label: 'not found',
                            value: null
                        });
                    }
                    // add "Add Language" button to autocomplete menu bottom
                    /*data.push({
                        label: $compile('<a class="ui-menu-add" ng-click="add()">Add Language</a>')($scope),
                        value: null
                    });*/
                    response(data);
                }
            },
            events: {
                change: function (event, ui) {
                    //console.log('change', event, ui);
                },
                select: function (event, ui) {
                    //console.log('select', event, ui);
                }
            }
        };        
        
        
        
        
  
});
