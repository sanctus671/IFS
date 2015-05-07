app.service('requestsService', function ($http, $q) {

var url = "http://localhost:49245/api/";

 //putting the defer out here means we don't constantly reload the requests when switching pages

//globally stored data
var requests = [];
var requestsLength = 0;
var request = [];
var suppliers = [];
var supplier = [];
var analysisCodes = [];
var analysisCode = [];
var users = []; //for system admin
var user = []; //for system admin
var currentUser = []; //is the current active user
var descriptions = [];
var codes = [];
var rooms = [];
var tooltips = [];
var permissionTypes = [];
var groupNames = [];

var filterString = "";
var searchString = "";

    
/* CURRENT USER FUNCTIONS */         
    this.getCurrentUser = function(){
        if (currentUser.length > 0){
            return currentUser;
        }
        var userDefer = $q.defer();
        $http.get(url + "MockUser?userCode=1").then(function(response){
            userDefer.resolve(response.data);
        });            
        var  promise = userDefer.promise;
        promise.then(function(data){
            currentUser = data;
        });            
        return promise;
    };
      
    
/* END CURRENT USER FUNCTIONS */ 
    


/*AUTO COMPLETE / VALIDATION FUNCTIONS */

    this.getDescriptions = function(){
        var descriptionsDefer = $q.defer();
        $http.get(url + "AutoComplete?type=descriptions").then(function(response){
            descriptionsDefer.resolve(response.data);
        });            
        var  promise = descriptionsDefer.promise;
        promise.then(function(data){
            descriptions = data;
        });            
        return promise;
    };
    
    this.getAnalysisCodesAutoComplete = function(){
        var analysisCodesDefer = $q.defer();  
        $http.get(url + "AutoComplete?type=analysiscodes").then(function(response){
            analysisCodesDefer.resolve(response.data);
        });            
        var  promise = analysisCodesDefer.promise;
        promise.then(function(data){
            codes = data;
        });            
        return promise;
    };     

    this.getRooms = function(){
        var roomsDefer = $q.defer();
        $http.get(url + "AutoComplete?type=rooms").then(function(response){
            roomsDefer.resolve(response.data);
        });            
        var  promise = roomsDefer.promise;
        promise.then(function(data){
            rooms = data;
        });            
        return promise;
    };
    
    this.getPermissionTypes = function(){
        var permissionTypesDefer = $q.defer();
        $http.get(url + "AutoComplete?type=permissions").then(function(response){
            permissionTypesDefer.resolve(response.data);
        });            
        var  promise = permissionTypesDefer.promise;
        promise.then(function(data){
            permissionTypes = data;
        });            
        return promise;
    };
    
    this.getGroupNames = function(){
        var groupNamesDefer = $q.defer();
        $http.get(url + "AutoComplete?type=groups").then(function(response){
            groupNamesDefer.resolve(response.data);
        });            
        var  promise = groupNamesDefer.promise;
        promise.then(function(data){
            groupNames = data;
        });            
        return promise;
    };    
    
     
    
    this.checkAccount = function(accountNumber){
        if (accountNumber.length < 9){
            return false;
        }
        return true;
    };


/* END AUTO COMPLETE / VALIDATION FUNCTIONS */ 


/*ANALYSIS CODE FUNCTIONS */
    this.getAnalysisCodes = function(){
        var analysisCodesDefer = $q.defer();    
        $http.get(url + "AnalysisCode").then(function(response){
            analysisCodesDefer.resolve(response.data);
        });            
        var  promise = analysisCodesDefer.promise;
        promise.then(function(data){
            analysisCodes = data;
        });            
        return promise;           

    }; 
    
    this.getAnalysisCode = function(id){
        var AnalysisCodeDefer = $q.defer();
        $http.get(url + "AnalysisCode/" + id).then(function(response){

            AnalysisCodeDefer.resolve(response.data);
            
        });            
        var  promise = AnalysisCodeDefer.promise;
        promise.then(function(data){

            analysisCode = data;


        });            
        return promise;
    };
    
    this.insertAnalysisCode = function(data){
        var topID = 1;
        if (analysisCodes.length > 0){
            topID = analysisCodes[0].id + 1;
        }
        data["id"] = topID;
        
        $http.post(url + "AnalysisCode", data).then(function(){
            
        });    
        analysisCodes.push(data);
        return analysisCodes;
        
    };     
    this.updateAnalysisCode = function(id,data){
        for (var i = 0; i < analysisCodes.length; i++) {
            if (analysisCodes[i].id === id) {
                for (var x in data){
                    analysisCodes[i][x] = data[x];
                }

            }
        }
        $http.put(url + "AnalysisCode/" + id, analysisCodes[i]);                
        return analysisCodes;        
    }; 
    
    this.deleteAnalysisCode = function(id){
        for (var i = analysisCodes.length - 1; i >= 0; i--) {
            if (analysisCodes[i].id === id) {
                analysisCodes.splice(i, 1);
                
                break;
            }
        }
        $http.delete(url + "AnalysisCode/" + id);
        return analysisCodes;
    };     
    
    
    
/*END ANALYSIS CODE FUNCTIONS */



/* SUPPLIERS FUNCTIONS */    
    this.getSuppliers = function(){
        var suppliersDefer = $q.defer();    
        $http.get(url + "Supplier").then(function(response){
            suppliersDefer.resolve(response.data);
        });            
        var  promise = suppliersDefer.promise;
        promise.then(function(data){
            suppliers = data;
        });            
        return promise;        
    };
    
     this.getSupplier = function (id) {
        var supplierDefer = $q.defer();
        $http.get(url + "Supplier/" + id).then(function(response){

            supplierDefer.resolve(response.data);
            
        });            
        var  promise = supplierDefer.promise;
        promise.then(function(data){

            supplier = data;


        });            
        return promise;
    };   
    
    
    this.insertSupplier = function(data){
        var topID = 1;
        if (suppliers.length > 0){
            topID = suppliers[0].id + 1;
        }
        
        data["id"] = topID;
        $http.post(url + "Supplier", data).then(function(){

        });  
        suppliers.push(data);
        return suppliers;

        
    };     
    this.updateSupplier = function(id,data){
        for (var i = 0; i < suppliers.length; i++) {
            if (suppliers[i].id === id) {
                for (var x in data){
                    suppliers[i][x] = data[x];
                }
                
                $http.put(url + "Supplier/" + id, suppliers[i]);
                return suppliers;
            }
        }
    }; 
    
    this.deleteSupplier = function(id){
        for (var i = suppliers.length - 1; i >= 0; i--) {
            if (suppliers[i].id === id) {
                suppliers.splice(i, 1);
                break;
            }
        }
        $http.delete(url + "Supplier/" + id);
        return suppliers;
    };     
    
/* END SUPPLIERS FUNCTIONS */      
    
/* USERS FUNCTIONS */    
    this.getUsers = function(){
        var usersDefer = $q.defer();    
        $http.get(url + "User").then(function(response){
            usersDefer.resolve(response.data);
        });            
        var  promise = usersDefer.promise;
        promise.then(function(data){
            users = data;
        });            
        return promise;        
    };
    
     this.getUser = function (id) {
        var userDefer = $q.defer();
        $http.get(url + "User/" + id).then(function(response){

            userDefer.resolve(response.data);
            
        });            
        var  promise = userDefer.promise;
        promise.then(function(data){

            user = data;


        });            
        return promise;
    };   
    
    
    this.insertUser = function(data){
        var topID = 1;
        if (users.length > 0){
            topID = users[0].id + 1;
        }
        
        data["id"] = topID;
        $http.post(url + "User", data).then(function(){

        });  
        users.push(data);
        return users;

        
    };     
    this.updateUser = function(id,data){
        for (var i = 0; i < users.length; i++) {
            if (users[i].id === id) {
                for (var x in data){
                    users[i][x] = data[x];
                }
                
                $http.put(url + "User/" + id, users[i]);
                return users;
            }
        }
    }; 
    
    this.deleteUser = function(id){
        for (var i = users.length - 1; i >= 0; i--) {
            if (users[i].id === id) {
                users.splice(i, 1);
                break;
            }
        }
        $http.delete(url + "User/" + id);
        return users;
    };     
    
/* END USERS FUNCTIONS */  


/* REQUESTS FUNCTIONS */   

    this.getRequests = function (offset, limit) {
        var requestsDefer = $q.defer();
        var urlString = url + "Request?offset=" + offset + "&limit=" + limit;
        if (searchString !== "" && filterString !== ""){
            urlString = urlString + "&search=" + searchString + "&filterString=" + filterString;    
        }
        else if (searchString !== ""){
            urlString = urlString + "&search=" + searchString;  
        }  
        else if(filterString !== ""){
            urlString = urlString + "&filterString=" + filterString;  
        }

        urlString = urlString + "&userCode=" + currentUser.userCode;

        $http.get(urlString).then(function(response){
            requestsDefer.resolve(response.data);
        });            
        var  promise = requestsDefer.promise;
        promise.then(function(data){
            requestsLength = data.count;
            requests = data.items;
            for (var index in requests){
                if (requests[index]["name"].length < 1 && requests[index]["userCode"].length > 0){
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
                    requests[index]["name"] = retrievedUser.name;
                    requests[index]["email"] = retrievedUser.email;
                    requests[index]["phone"] = retrievedUser.phone;
                    requests[index]["userCode"] = retrievedUser.userCode;
                    
                }
            }
            currentUser = data.user; //sets the current user when retrieving all requests
        });            
        return promise;
    };
    
    

    this.insertRequest = function (data) {
        requestsLength += 1;
        
        var topID = requestsLength;
        data["id"] = topID;
        data["status"] = "Requested";
        
        //get current date in correct format
        var today = new Date();var dd = today.getDate();var mm = today.getMonth()+1;var yyyy = today.getFullYear();
        if(dd<10) {dd='0'+dd;} if(mm<10) {mm='0'+mm;} today = dd+'/'+mm+'/'+yyyy;       
        
        
        data["date"] = new Date();
        
        data["userCode"] = currentUser.userCode;
        data["name"] = currentUser.name; //eventually get rid of these
        data["phone"] = currentUser.phone; //eventually get rid of these
        data["email"] = currentUser.email; //eventually get rid of these
        
        
        $http.post(url + "Request", data).then(function(){
            
        });  
        requests.push(data);
        return requests;
        
    };
    
    this.updateRequest = function (id,data) {
        for (var i = 0; i < requests.length; i++) {
            if (requests[i].id === id) {
                for (var x in data){
                    requests[i][x] = data[x];
                }
                

            }
        }
        $http.put(url + "Request/" + id, data);
        return requests;        

    };
    
    this.deleteRequest = function (id) {
        //$http.delete()
        for (var i = requests.length - 1; i >= 0; i--) {
            if (requests[i].id === id) {
                requests.splice(i, 1);
                break;
            }
        }
        $http.delete(url + "Request/" + id);
        return requests;
    };

    this.getRequest = function (id) {
        var requestDefer = $q.defer();

        $http.get(url + "Request/" + id).then(function(response){

            requestDefer.resolve(response.data);
            
        });            
        var  promise = requestDefer.promise;
        promise.then(function(data){

            request = data;
             if (request.name.length < 1 && requests.userCode.length > 0){
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
                request.name = retrievedUser.name;
                requests.email = retrievedUser.email;
                requests.phone = retrievedUser.phone;
                requests.userCode = retrievedUser.userCode;

            }
            //console.log(request.accountNumber.match(/(\d+|[^\d]+)/g));
            
            if (request.dateSupplied === "0001-01-01T00:00:00"){
                //actually is empty
                request.dateSupplied = "";
            }

        });            
        return promise;
    };
    
    this.filterRequests = function (field, option, value, date1, date2, searchText, offset, limit){
        //check the search form matches what we have already saved from previous search
        if (searchText !== searchString){
            //change it if it isn't
            searchString = searchText;
        }
        
        //build filter string for api
        filterString = field + ";" + option + ";";
        
        if(option === "after" || option === "before"){
            filterString = filterString + date1;
        }
        else if (option === "between" || option === "notbetween"){
            filterString = filterString + date1 + ";" + date2;
        }
        else{
            filterString = filterString + value;
        }
            
        //make request to get new set of requests
        var requestsDefer = $q.defer();
        
        var urlString = url + "Request?offset=" + offset + "&limit=" + limit;
        
        
        if (searchString !== ""){
            urlString = urlString + "&search=" + searchString + "&filterString=" + filterString;    
        }
        else{
            urlString = urlString + "&filterString=" + filterString;  
        }
        
        urlString = urlString + "&userCode=" + currentUser.userCode;
        $http.get(urlString).then(function(response){
            requestsDefer.resolve(response.data);
        });            
        var  promise = requestsDefer.promise;
        promise.then(function(data){
            requestsLength = data.count;
            requests = data.items;
            for (var index in requests){
                if (requests[index]["name"].length < 1 && requests[index]["userCode"].length > 0){
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
                    requests[index]["name"] = retrievedUser.name;
                    requests[index]["email"] = retrievedUser.email;
                    requests[index]["phone"] = retrievedUser.phone;
                    requests[index]["userCode"] = retrievedUser.userCode;
                    
                }
            }            
        });            
        return promise;       

    };

    this.searchRequests = function (search, offset, limit){

        searchString = search;
        
        var requestsDefer = $q.defer();
        
        var urlString = url + "Request?offset=" + offset + "&limit=" + limit;
        if (filterString !== ""){
            urlString = urlString + "&search=" + searchString + "&filterString=" + filterString;    
        }
        else{
            urlString = urlString + "&search=" + searchString;  
        }
        
        urlString = urlString + "&userCode=" + currentUser.userCode;
        $http.get(urlString).then(function(response){
            requestsDefer.resolve(response.data);
        });            
        var  promise = requestsDefer.promise;
        promise.then(function(data){
            requestsLength = data.count;
            requests = data.items;
            for (var index in requests){
                if (requests[index]["name"].length < 1 && requests[index]["userCode"].length > 0){
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
                    requests[index]["name"] = retrievedUser.name;
                    requests[index]["email"] = retrievedUser.email;
                    requests[index]["phone"] = retrievedUser.phone;
                    requests[index]["userCode"] = retrievedUser.userCode;
                    
                }
            }            
        });            
        return promise;        

    };    

    this.resetRequests = function (){

        searchString = "";
        filterString= "";
 
    };     
    
/* END REQUESTS FUNCTIONS */     
    

/* TOOLTIP FUNCTIONS */

    this.getTooltips = function(){
        var tooltipsDefer = $q.defer();
        $http.get(url + "Miscellaneous").then(function(response){
            tooltipsDefer.resolve(response.data);
        });            
        var  promise = tooltipsDefer.promise;
        promise.then(function(data){
            tooltips = data;
        });            
        return promise;
    
    };


/* END TOOLTIP FUNCTIONS */



/* DEFINE DUMMY DATA - TO BE REPLACED BY API CALLS TO GET REAL DATA */



    //USER DATA (get from sharepoint)
    var currentUser = {id: 1, name:"John Doe", userCode:"1", phone:"1234567", email:"johndoe@massey.ac.nz", permissions:"admin", groupid:2};






    //TOOLTIPS (get from db)
  /*  
    var tooltips = {type:"Classification of the requestt",
        itemDescription:"Description of the item being requested",
        quality:"Level of purity",
        size:"The amount per item",
        quantity:"How many of this item in this request",
        permit:"For orders which require a permit e.g. a chemical from overseas",
        permitNumber:"The permit number (if known)",
        destinationRoom:"The room your wish for this request to be delivered to",
        cas:"Cas number for the item being requested",
        accountNumber:"Account number for the account this request should be charged to",
        analysisCode:"5 alpha-numeric characters designated as your analysis code",
        notes:"Extra information for the admin viewing the request",
        }
    
    */
/*

    //SUPPLIERS (get from db)        
    var suppliers = [
        {
            id: 1, name:"supplier1"

        } ,
        {
            id: 2, name:"supplier2"

        }];


    //ROOMS (get from db)    
    var rooms = ["SC B 1",
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
        "SC A 4"];


    //ANALYSIS CODES (get from db)    
    var analysisCodes = [
        {
            id: 1, code:"abcde"

        } ,
        {
            id: 2, code:"fghijk"

        }];    

        


        

    //REQUESTS (get from db)    
    
    var requests = [
        {
            id: 1, status: 'Supplied',date: new Date('2014-12-20'), adminName: 'John Doe', userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:new Date('2014-12-21'), adminNotes: 'This has been ordered', 
            adminName:'Bob', cost: '$123', supplier: 'Person'

        } ,
        {
            id: 2, status: 'Requested',date: new Date('2014-12-21'), adminName: 'John Doe', userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        },
        {
            id: 3, status: 'Supplied',date: new Date('2014-12-22'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC C 4',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:new Date('2014-12-23'), adminNotes: 'This has been ordered', 
            adminName:'Bob', cost: '$123', supplier: 'Person'

        } ,
        {
            id: 4, status: 'Requested',date: new Date('2014-12-22'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        },
        {
            id: 5, status: 'Supplied',date: new Date('2014-12-22'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:new Date('2014-12-23'), adminNotes: '', 
            cost: '', supplier: ''

        },
        {
            id: 6, status: 'Requested',date: new Date('2014-12-22'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC D 4',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        } ,
        {
            id: 7, status: 'Received',date: new Date('2014-12-23'), adminName: 'John Doe', userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: 'Almost ready', 
            cost: '$560', supplier: 'Guy'

        },
        {
            id: 8, status: 'Requested',date: new Date('2014-12-23'),  userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        },
        {
            id: 9, status: 'Cancelled',date: new Date('2014-12-23'), adminName: 'John Doe', userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        } ,
        {
            id: 10, status: 'Supplied',date: new Date('2014-12-23'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied: new Date('2014-12-24'), adminNotes: '', 
            cost: '', supplier: ''

        },
        {
            id: 11, status: 'Requested',date: new Date('2014-12-24'),  userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC D 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        },
        {
            id: 12, status: 'Requested',date: new Date('2014-12-27'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        } ,
        {
            id: 13, status: 'Incorrect',date: new Date('2015-01-05'), adminName: 'John Doe', userid: 1, name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC A 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        },
        {
            id: 14, status: 'Requested',date: new Date('2015-01-05'), userid: 2, name:'Jane Doe', phone:'1234567', email:'janedoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC A 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        },
        {
            id: 15, status: 'Requested',date: new Date('2015-01-05'), userid: 2, name:'Jane Doe', phone:'1234567', email:'janedoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC A 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: '123456789', accountNumberPrefix: 'GL', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''

        }        


    ];
    */
   
   

/* END DUMMY DATA */    
    
});