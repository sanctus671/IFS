
app.controller('MainController', function ($scope, $rootScope, requestsService,$location) {

    
    

    function init() {
        
        $scope.user = {
            name: "",
            phone: "",
            email: "",
            permission: "standard",
            userCode: ""
            
        }
        
        $scope.permissions = "standard";
        $scope.orderByField = "date";
        //check we the correct page is loaded based on the users permissions

       

        
        
        $scope.currentPage = 1;
        $scope.pageSize = 10;  
        $scope.totalRequests = 0;
        $scope.searchString = "";
        $scope.requests = [];
        $scope.getPaginatedRequests();
        $scope.pagination = {
            current: 1
        };        
        
        $scope.sortReverse = true;
        
        

        



      
    }
    

    $scope.changeOrder = function (header) {
        $scope.orderByField = header; 
        $scope.sortReverse = !$scope.sortReverse;

    };
    
    $scope.pageChanged = function(newPage) {
        $scope.currentPage = newPage;
        $scope.getPaginatedRequests();
    };
    
    $scope.changePageSize = function (size) {
        
        $scope.pageSize = size; 
        //getRequests();

    };
    
    $scope.getPaginatedRequests = function () {
        var promise = requestsService.getRequests($scope.pageSize * ($scope.currentPage -1), $scope.pageSize);
        setTimeout(function(){        
        promise.then(function(data){
            $scope.requests = data.items;
            for (var index in $scope.requests){
                if ($scope.requests[index]["name"].length < 1 && $scope.requests[index]["userCode"].length > 0){
                    //get user data for this user code
                    var retrievedUser = { //TODO: RETRIEVE EXTERNALLY
                        name : "User retrieved from elsewhere",
                        email : "test@massey.ac.nz",
                        phone : "66666666666666666",
                        userCode : "0",
                        permission : "standard",
                        permissionid : 1, //1 = standard, 2 = manager, 3,4 = admin
                        groupid : null, //the last 3 properties only set if the user is a manager
                        group : null, //group name
                        groupUsers : null //list of strings with all user codes contained in their group                        
                    };
                    $scope.requests[index]["name"] = retrievedUser.name;
                    $scope.requests[index]["email"] = retrievedUser.email;
                    $scope.requests[index]["phone"] = retrievedUser.phone;
                    $scope.requests[index]["userCode"] = retrievedUser.userCode;
                    
                }
            }            
            $scope.totalRequests = data.count;
            $scope.user = data.user;
            $scope.setPath($scope.user.permission);   
            $rootScope.$broadcast('requestsChanged',data.items);
        }); 
        },250);           
    }
    
    $scope.setPath = function(permission){
        $scope.permissions = permission;
        if (permission  === 'standard' && $location.path().indexOf("standard") === -1){
            document.location.href='#/standard/';
        }        
        else if (permission  === 'manager' && $location.path().indexOf("manager") === -1){
            document.location.href='#/manager/';
        }
        else if (permission  === 'accountant' && $location.path().indexOf("accountant") === -1){
            $scope.orderByField = "dateSupplied"
            document.location.href='#/accountant/';
        }
        else if (permission  === 'systemadmin' && $location.path().indexOf("systemadmin") === -1){
            document.location.href='#/systemadmin/';
        }         
        else if (permission  === 'admin' && $location.path().indexOf("admin") === -1){
            document.location.href='#/admin/';
        }
        
        
    }
    
    
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
        var promise = requestsService.filterRequests(field, options1 ? options1 : options2, value, date1, date2,$scope.searchString,$scope.pageSize * ($scope.currentPage -1), $scope.pageSize);
        setTimeout(function(){        
        promise.then(function(data){
            $scope.requests = data.items;
            for (var index in $scope.requests){
                if ($scope.requests[index]["name"].length < 1 && $scope.requests[index]["userCode"].length > 0){
                    //get user data for this user code
                    var retrievedUser = { //TODO: RETRIEVE EXTERNALLY
                        name : "User retrieved from elsewhere",
                        email : "test@massey.ac.nz",
                        phone : "66666666666666666",
                        userCode : "0",
                        permission : "standard",
                        permissionid : 1, //1 = standard, 2 = manager, 3,4 = admin
                        groupid : null, //the last 3 properties only set if the user is a manager
                        group : null, //group name
                        groupUsers : null //list of strings with all user codes contained in their group                        
                    };
                    $scope.requests[index]["name"] = retrievedUser.name;
                    $scope.requests[index]["email"] = retrievedUser.email;
                    $scope.requests[index]["phone"] = retrievedUser.phone;
                    $scope.requests[index]["userCode"] = retrievedUser.userCode;
                    
                }
            }            
            $scope.totalRequests = data.count;
            $rootScope.$broadcast('requestsChanged',data.items);
        }); 
        },250); 
  };
  
  $scope.searchRequests = function(){
        var promise = requestsService.searchRequests($scope.searchString,$scope.pageSize * ($scope.currentPage -1), $scope.pageSize);
        
        promise.then(function(data){
            $scope.requests = data.items;
            for (var index in $scope.requests){ //TODO: RETRIEVE EXTERNALLY
                if ($scope.requests[index]["name"].length < 1 && $scope.requests[index]["userCode"].length > 0){
                    //get user data for this user code
                    var retrievedUser = {
                        name : "User retrieved from elsewhere",
                        email : "test@massey.ac.nz",
                        phone : "66666666666666666",
                        userCode : "0",
                        permission : "standard",
                        permissionid : 1, //1 = standard, 2 = manager, 3,4 = admin
                        groupid : null, //the last 3 properties only set if the user is a manager
                        group : null, //group name
                        groupUsers : null //list of strings with all user codes contained in their group                        
                    };
                    $scope.requests[index]["name"] = retrievedUser.name;
                    $scope.requests[index]["email"] = retrievedUser.email;
                    $scope.requests[index]["phone"] = retrievedUser.phone;
                    $scope.requests[index]["userCode"] = retrievedUser.userCode;
                    
                }
            }            
            $scope.totalRequests = data.count;
            $rootScope.$broadcast('requestsChanged',data.items);
        
        }); 
  }; 
  
  $scope.resetRequests = function(){
        $scope.currentPage = 1;
        $scope.pageSize = 10;  
        $scope.totalRequests = 0;
        $scope.searchString = "";      
        requestsService.resetRequests();
        $scope.getPaginatedRequests();
        
  };  
  
  $rootScope.$on('requestsChanged', function (event, data){ 
      console.log(data);
      $scope.requests = data;
  });
  
  init();
             
});


app.controller('MainViewController', function ($scope, $rootScope, $routeParams, requestsService,$location) {
    
    $scope.permissions = requestsService.getCurrentUser().permissions;

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
    
        $scope.requestID = ($routeParams.reqID) ? parseInt($routeParams.reqID) : 0;
        if ($scope.requestID > 0) {

 
            var promise = requestsService.getRequest($scope.requestID);
                promise.then(function(data){
                $scope.request = data;
                if ($scope.request.name.length < 1 && $scope.request.userCode.length > 0){
                    //get user data for this user code
                    var retrievedUser = { //TODO: RETRIEVE EXTERNALLY
                        name : "User retrieved from elsewhere",
                        email : "test@massey.ac.nz",
                        phone : "66666666666666666",
                        userCode : "0",
                        permission : "standard",
                        permissionid : 1, //1 = standard, 2 = manager, 3,4 = admin
                        groupid : null, //the last 3 properties only set if the user is a manager
                        group : null, //group name
                        groupUsers : null //list of strings with all user codes contained in their group                        
                    };
                    $scope.request.name = retrievedUser.name;
                    $scope.request.email = retrievedUser.email;
                    $scope.request.phone = retrievedUser.phone;
                    $scope.request.userCode = retrievedUser.userCode;

                                }                
            });  
        }
    
        
        
    }
    
    $rootScope.$on('requestsChanged', function (event, data){
        for (var i = 0; i < data.length; i++) {
            if (data[i].id === $scope.requestID) {
                $scope.request = data[i];
            }
        }       
  });
  

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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
        },
        userdata: function() {
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
  
  $scope.openUser = function (size, id) {

    var modalInstance = $modal.open({
      templateUrl: 'app/partials/createuser.html',
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
            return false;
        },
        userdata: function() {
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
  
  $scope.openEditUser = function (size,id) {
      
    var modalInstance = $modal.open({
      templateUrl: 'app/partials/edituser.html',
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
            return false;
        },
        userdata: function() {
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
  
  
  
  
    
  $scope.confirmDeleteUser = function (size,id) {
      
    var modalInstance = $modal.open({
      templateUrl: 'app/partials/deleteuser.html',
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
            return false;
        },
        userdata: function() {
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



app.controller('ModalInstanceCtrl', function ($scope, $rootScope, $modalInstance, $compile, items, requestsService, id, supplier, code, userdata) {
  $scope.id = id;
  $scope.user = requestsService.getCurrentUser();
  $scope.permissions = $scope.user.permissions;

  var promise = requestsService.getTooltips();
  promise.then(function(data){
    $scope.tooltips = data;
   });
   
  var promise = requestsService.getSuppliers();
  promise.then(function(data){
    $scope.suppliers = data;
   });
  
  var promise = requestsService.getPermissionTypes();
      promise.then(function(data){
      $scope.permissionTypes = data; 
    });
    
  var promise = requestsService.getGroupNames();
      promise.then(function(data){
      $scope.groupNames = data; 
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
    
  else if (userdata && id > 0) {
      
      var promise = requestsService.getUser(id);
      promise.then(function(data){
      $scope.userdata = data; 
        });
    }    

  else if (id > 0){

      var promise = requestsService.getRequest(id);
      promise.then(function(data){
        $scope.request = data;
        var parts = $scope.request.accountNumber.match(/(\d+|[^\d]+)/g);
        if (parts.length > 1){
            $scope.request.accountNumberPrefix = parts.shift();
            $scope.request.accountNumber = parts.join("");
        }
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
            accountNumber:$scope.request['accountNumberPrefix'] + $scope.request['accountNumber'],
            permit:$scope.request['permit'],
            permitNumber:$scope.request['permitNumber'],
            analysisCode:$scope.request['analysisCode'],
            };



        if (requestsService.checkAccount($scope.request['accountNumber'])){
            var requests = requestsService.insertRequest(toBeInserted);
            $rootScope.$broadcast('requestsChanged', requests);
            $scope.request = {};            
            $scope.ok();
        }
        else{
            alert("Account Number was invalid");
        }
        
        
    };
    
    $scope.updateRequest = function(){
        $scope.request["accountNumber"] = $scope.request["accountNumberPrefix"] + $scope.request["accountNumber"]; 
        
        if ($scope.permissions === "admin"){
            $scope.request["adminName"] = $scope.user.name;
            if ($scope.request['status'] === "Supplied"){
                var today = new Date();var dd = today.getDate();var mm = today.getMonth()+1;var yyyy = today.getFullYear();
                if(dd<10) {dd='0'+dd;} if(mm<10) {mm='0'+mm;} today = dd+'/'+mm+'/'+yyyy;  
                
                $scope.request["dateSupplied"] = today;
            }
        }       
        var requests = requestsService.updateRequest(id, $scope.request);
        $rootScope.$broadcast('requestsChanged', requests);
        $scope.ok();
    };   
    

    $scope.deleteRequest = function () {
        $scope.request.status = "Cancelled";
        if ($scope.permissions === "admin"){
            $scope.request.adminName = $scope.user.name;
        }
        var requests = requestsService.updateRequest(id, $scope.request);
        $rootScope.$broadcast('requestsChanged', requests);
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
            $scope.suppliers = requestsService.insertSupplier($scope.supplier);
            $rootScope.$broadcast('suppliersChanged', $scope.suppliers);
            $scope.supplier = {};            
            $scope.ok();            
        };
        
        $scope.editSupplier = function(){
            $scope.suppliers = requestsService.updateSupplier(id,$scope.supplier);
            $rootScope.$broadcast('suppliersChanged', $scope.suppliers);            
            $scope.supplier = {};            
            $scope.ok();              
        };
        $scope.deleteSupplier = function(){
            $scope.suppliers = requestsService.deleteSupplier(id); 
            $rootScope.$broadcast('suppliersChanged', $scope.suppliers);
            $scope.ok();            
        }; 
        
        
        
        $scope.insertCode = function(){
            var codes = requestsService.insertAnalysisCode($scope.code);
            $rootScope.$broadcast('codesChanged', codes);
            $scope.code = {};            
            $scope.ok();            
        };
        
        $scope.editCode = function(){
            var codes = requestsService.updateAnalysisCode(id,$scope.code);
            $rootScope.$broadcast('codesChanged', codes);
            $scope.code = {};            
            $scope.ok();              
        };
        $scope.deleteCode = function(){
            var codes = requestsService.deleteAnalysisCode(id);    
            $rootScope.$broadcast('codesChanged', codes);
            $scope.ok();            
        };   
        
        $scope.insertUser = function(){
            var codes = requestsService.insertUser($scope.userdata);
            $rootScope.$broadcast('usersChanged', codes);
            $scope.code = {};            
            $scope.ok();            
        };
        
        $scope.editUser = function(){
            var codes = requestsService.updateUser(id,$scope.userdata);
            $rootScope.$broadcast('usersChanged', codes);
            $scope.code = {};            
            $scope.ok();              
        };
        $scope.deleteUser = function(){
            var codes = requestsService.deleteUser(id);    
            $rootScope.$broadcast('usersChanged', codes);
            $scope.ok();            
        };          
});




app.controller('SupplierController', function ($scope, $rootScope, requestsService,$location) {
 
    init();
    
    function init(){
        $scope.currentPage = 1;
        $scope.pageSize = 10;        
        $scope.orderByField = "id";
        $scope.sortReverse = true;
        $scope.suppliers = [];
        setTimeout(function(){
        var promise = requestsService.getSuppliers();
        promise.then(function(data){
            $scope.suppliers = data;
        });
        },250);

        
     $rootScope.$on('suppliersChanged', function(event, data) {
         console.log(data);
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




app.controller('CodeController', function ($scope, $rootScope, requestsService,$location) {
    

    init();
    
    function init(){
        $scope.currentPage = 1;
        $scope.pageSize = 10;        
        $scope.orderByField = "id";
        $scope.sortReverse = true;
        $scope.user = requestsService.getCurrentUser();
        $scope.permissions = $scope.user.permissions;     
        
        setTimeout(function(){
        var promise = requestsService.getAnalysisCodes();
            promise.then(function(data){
            $scope.codes = data;
        });
        },250);        
        
     $rootScope.$on('codesChanged', function(event, args) {
         console.log("herecode");
        $scope.codes = args;
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


app.controller('UserController', function ($scope, $rootScope, requestsService,$location) {
    

    init();
    
    function init(){
        $scope.currentPage = 1;
        $scope.pageSize = 10;        
        $scope.orderByField = "id";
        $scope.sortReverse = true;
        $scope.user = requestsService.getCurrentUser();
        $scope.permissions = $scope.user.permissions;     
        
        setTimeout(function(){
        var promise = requestsService.getUsers();
            promise.then(function(data){
            $scope.users = data;
        });
        },250);        
        
     $rootScope.$on('usersChanged', function(event, args) {
        $scope.users = args;
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