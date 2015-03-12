
app.controller('MainController', function ($scope, requestsService,$location) {

    
    init();

    function init() {
        
        $scope.user = requestsService.getUser()
        $scope.permissions = $scope.user.permissions;
        $scope.orderByField = "date";
        //check we the correct page is loaded based on the users permissions
        if ($scope.permissions  === 'standard' && $location.path().indexOf("standard") === -1){
            document.location.href='#/standard/';
        }        
        else if ($scope.permissions  === 'manager' && $location.path().indexOf("manager") === -1){
            document.location.href='#/manager/';
        }
        else if ($scope.permissions  === 'accountant' && $location.path().indexOf("accountant") === -1){
            $scope.orderByField = "dateSupplied"
            document.location.href='#/accountant/';
        }
        else if ($scope.permissions  === 'admin' && $location.path().indexOf("admin") === -1){
            document.location.href='#/admin/';
        }

        
        
        $scope.currentPage = 1;
        $scope.pageSize = 10;        
        
        $scope.sortReverse = true;
        
        var promise = requestsService.getRequests(0, 1000);
        promise.then(function(data){
            $scope.requests = data;
        });

        



      
    }
    

    $scope.changeOrder = function (header) {
        $scope.orderByField = header; 
        $scope.sortReverse = !$scope.sortReverse;

    };
    
    $scope.changePageSize = function (size) {
        
        $scope.pageSize = size; 


    };
    
    $scope.change = function (){
        requestsService.changePermissions('admin');
    };    
    
    
    
    $scope.insertRequest = function () {      

        requestsService.insertRequest($scope.newRequest);
        $scope.newRequest = {};
    };
    


    $scope.deleteRequest = function (id) {
        requestsService.deleteRequest(id);
    };
    
  $scope.filterRequests = function(field,options1,options2,value,date1,date2){
        if (options1){
            requestsService.filterRequests(field, options1, value, date1, date2);
        }
        else{
            requestsService.filterRequests(field, options2, value, date1, date2);
        }
  };
             
});


app.controller('MainViewController', function ($scope, $routeParams, requestsService,$location) {
    
    $scope.permissions = requestsService.getUser().permissions;

    //check we the correct page is loaded based on the users permissions

    if ($scope.permissions  === 'standard' && $location.path().indexOf("standard") === -1){
        document.location.href='#/standardview/' + $routeParams.reqID;
    }        
    else if ($scope.permissions  === 'manager' && $location.path().indexOf("manager") === -1){
        document.location.href='#/managerview/' + $routeParams.reqID;
    }
    else if ($scope.permissions  === 'accountant' && $location.path().indexOf("accountant") === -1){
        document.location.href='#/accountantview/' + $routeParams.reqID;
    }
    else if ($scope.permissions  === 'admin' && $location.path().indexOf("admin") === -1){
        document.location.href='#/adminview/' + $routeParams.reqID;
    }    
    
    
    
    
    $scope.request = {};



    init();

    function init() {
    
        var requestID = ($routeParams.reqID) ? parseInt($routeParams.reqID) : 0;
        if (requestID > 0) {

 
            var promise = requestsService.getRequest(requestID);
                promise.then(function(data){
                $scope.request = data; 
            });  
        }
    
        
        
    }
  

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
        },
        supplier: function() {
            return false;
        },
        code: function() {
            return false;
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
        },
        supplier: function() {
            return false;
        },
        code: function() {
            return false;
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
        },
        supplier: function() {
            return false;
        },
        code: function() {
            return false;
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
        },
        supplier: function() {
            return false;
        },
        code: function() {
            return false;
        }        
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };   
  
  
  $scope.openSupplier = function (size, id) {

    var modalInstance = $modal.open({
      templateUrl: 'app/partials/createsupplier.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        },
        supplier: function() {
            return true;
        },
        code: function() {
            return false;
        }         
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };  
  
  $scope.openEditSupplier = function (size,id) {
      
    var modalInstance = $modal.open({
      templateUrl: 'app/partials/editsupplier.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        },
        supplier: function() {
            return true;
        },
        code: function() {
            return false;
        }         
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };   
  
  
  
  
    
  $scope.confirmDeleteSupplier = function (size,id) {
      
    var modalInstance = $modal.open({
      templateUrl: 'app/partials/deletesupplier.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        },
        supplier: function() {
            return true;
        },
        code: function() {
            return false;
        }        
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };  
  
  $scope.openCode = function (size, id) {

    var modalInstance = $modal.open({
      templateUrl: 'app/partials/createcode.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        },
        supplier: function() {
            return false;
        } ,
        code: function() {
            return true;
        }         
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };  
  
  $scope.openEditCode = function (size,id) {
      
    var modalInstance = $modal.open({
      templateUrl: 'app/partials/editcode.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        },
        supplier: function() {
            return false;
        } ,
        code: function() {
            return true;
        }         
      }
    });

    modalInstance.result.then(function (selectedItem) {
      $scope.selected = selectedItem;
    }, function () {
      //$log.info('Modal dismissed at: ' + new Date());
    });
  };   
  
  
  
  
    
  $scope.confirmDeleteCode = function (size,id) {
      
    var modalInstance = $modal.open({
      templateUrl: 'app/partials/deletecode.html',
      controller: 'ModalInstanceCtrl',
      size: size,
      resolve: {
        items: function () {
          return $scope.items;
        },
        id: function() {
            return id;
        },
        supplier: function() {
            return false;
        } ,
        code: function() {
            return true;
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



app.controller('ModalInstanceCtrl', function ($scope, $modalInstance, $compile, items, requestsService, id, supplier, code) {
  $scope.id = id;
  $scope.user = requestsService.getUser();
  $scope.permissions = $scope.user.permissions;
  $scope.tooltips = requestsService.getTooltips();
  
  var promise = requestsService.getSuppliers();
  promise.then(function(data){
    $scope.suppliers = data;
   });
  
  var promise = requestsService.getTooltips();
      promise.then(function(data){
      $scope.tooltips = data; 
    });
    
  setTimeout(function(){$('[data-toggle="tooltip"]').tooltip({'placement': 'top'});}, 500);
  
  
  if ($scope.permissions !== "admin"){
      setTimeout(function(){$('.admin').css({"background-color" : "#f5f5f5"});}, 500);
      
  }  
  
  if (supplier && id > 0){
      var promise = requestsService.getSupplier(id);
      promise.then(function(data){
        $scope.supplier = data;

        });      
    }
  else if (code && id > 0) {
      
      var promise = requestsService.getAnalysisCode(id);
      promise.then(function(data){
      $scope.code = data; 
        });
    }

  else if (id > 0){

      var promise = requestsService.getRequest(id);
      promise.then(function(data){
        $scope.request = data;
        $scope.request.dateSupplied = "";
        });     
     
  }
    

  

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
            accountNumber:$scope.request['accountNumber'],
            accountNumberPrefix:$scope.request['accountNumberPrefix'],
            permit:$scope.request['permit'],
            permitNumber:$scope.request['permitNumber']};



        if (requestsService.checkAccount($scope.request['accountNumber'])){
            requestsService.insertRequest(toBeInserted);
            $scope.request = {};            
            $scope.ok();
        }
        else{
            alert("Account Number was invalid");
        }
        
        
    };
    
    $scope.updateRequest = function(){
        requestsService.updateRequest(id, $scope.request);
         $scope.ok();
    };   
    

    $scope.deleteRequest = function () {
        $scope.request.status = "Cancelled";
        if ($scope.permissions === "admin"){
            $scope.request.adminName = $scope.user.name;
        }
        requestsService.updateRequest(id, $scope.request);
        //requestsService.deleteRequest(id);
    };     
    
  $scope.ok = function () {
    if ($scope.open){
        setTimeout(function(){$('.newModal').click();}, 500);
    }
    $modalInstance.close($scope.selected.item);
  };

  $scope.cancel = function () {
    $modalInstance.dismiss('cancel');
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

                    var promise = requestsService.getDescriptions();
                    promise.then(function(descriptions){
                        var data = descriptions;                    
                        data = $scope.descriptionOptions.methods.filter(data, request.term);

                        if (!data.length) {
                            data.push({
                                label: 'Not Found',
                                value: null
                            });
                        }
                        // add "Add Language" button to autocomplete menu bottom
                        /*data.push({
                            label: $compile('<a class="ui-menu-add" ng-click="add()">Add Language</a>')($scope),
                            value: null
                        });*/
                        response(data);
                    });
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
                

                    var promise = requestsService.getRooms();
                    promise.then(function(rooms){
                        var data = rooms;
                    
                        data = $scope.destinationRoomOptions.methods.filter(data, request.term);

                        if (!data.length) {
                            data.push({
                                label: 'Not Found',
                                value: null
                            });
                        }
                        // add "Add Language" button to autocomplete menu bottom
                        /*data.push({
                            label: $compile('<a class="ui-menu-add" ng-click="add()">Add Language</a>')($scope),
                            value: null
                        });*/
                        response(data);
                    });
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
        
        
    $scope.analysisCodeOptions = {
        options: {
            html: true,
            minLength: 1,
            onlySelectValid: true,
            outHeight: 50,
            source: function (request, response) {
                    var promise = requestsService.getAnalysisCodesAutoComplete();
                    promise.then(function(codes){
                        var data = codes;               

                        data = $scope.analysisCodeOptions.methods.filter(data, request.term);

                        if (!data.length) {
                            data.push({
                                label: 'Not Found',
                                value: null
                            });
                        }
                        // add "Add Language" button to autocomplete menu bottom
                        /*data.push({
                            label: $compile('<a class="ui-menu-add" ng-click="add()">Add Language</a>')($scope),
                            value: null
                        });*/
                        response(data);
                });
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
        
        
        $scope.insertSupplier = function(){
            requestsService.insertSupplier($scope.supplier);
            $scope.supplier = {};            
            $scope.ok();            
        };
        
        $scope.editSupplier = function(){
            requestsService.updateSupplier(id,$scope.supplier);
            $scope.supplier = {};            
            $scope.ok();              
        };
        $scope.deleteSupplier = function(){
            requestsService.deleteSupplier(id);          
            $scope.ok();            
        }; 
        
        
        
        $scope.insertCode = function(){
            requestsService.insertAnalysisCode($scope.code);
            $scope.code = {};            
            $scope.ok();            
        };
        
        $scope.editCode = function(){
            requestsService.updateAnalysisCode(id,$scope.code);
            $scope.code = {};            
            $scope.ok();              
        };
        $scope.deleteCode = function(){
            requestsService.deleteAnalysisCode(id);          
            $scope.ok();            
        };         
});




app.controller('SupplierController', function ($scope, requestsService,$location) {
    
    init();
    
    function init(){
        $scope.currentPage = 1;
        $scope.pageSize = 10;        
        $scope.orderByField = "id";
        $scope.sortReverse = true;
        
 
        var promise = requestsService.getSuppliers();
        promise.then(function(data){
            $scope.suppliers = data;
        });
        
        

      
    }
    

    
    $scope.changeOrder = function (header) {
        $scope.orderByField = header; 
        $scope.sortReverse = !$scope.sortReverse;

    };
    
    $scope.changePageSize = function (size) {
        
        $scope.pageSize = size; 


    };    
    

});









app.controller('SupplierViewController', function ($scope, $routeParams, requestsService) {
    $scope.supplier = {};

    

    init();

    function init() {
    
        var supplierID = ($routeParams.supID) ? parseInt($routeParams.supID) : 0;
        if (supplierID > 0) {


            var promise = requestsService.getSupplier(supplierID);
            promise.then(function(data){
                $scope.supplier = data; 
            });            
        }
    
        
        
    }    
});




app.controller('CodeController', function ($scope, requestsService,$location) {
    

    init();
    
    function init(){
        $scope.currentPage = 1;
        $scope.pageSize = 10;        
        $scope.orderByField = "id";
        $scope.sortReverse = true;
        $scope.user = requestsService.getUser();
        $scope.permissions = $scope.user.permissions;          

        var promise = requestsService.getAnalysisCodes();
            promise.then(function(data){
            $scope.codes = data;
        });
        
        

      
    }
    

    
    $scope.changeOrder = function (header) {
        $scope.orderByField = header; 
        $scope.sortReverse = !$scope.sortReverse;

    };
    
    $scope.changePageSize = function (size) {
        
        $scope.pageSize = size; 


    };    
    

});



app.controller('CodeViewController', function ($scope, $routeParams, requestsService) {
    $scope.code = {};

    

    init();

    function init() {
    
        var codeID = ($routeParams.codeID) ? parseInt($routeParams.codeID) : 0;
        if (codeID > 0) {

            
            var promise = requestsService.getAnalysisCode(codeID);
            promise.then(function(data){
                $scope.code = data; 
            });
        }
    
        
        
    }    
});