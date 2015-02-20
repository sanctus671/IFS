app.service('requestsService', function () {

/* USER FUNCTIONS */         
    this.getUser = function(){
        return user;
    };
    
    this.getUserGroup = function(id){
        for (var i = 0; i < userGroups.length; i++) {
            if (userGroups[i].id === id) {
                return userGroups[i];
            }
        }
        return null;
    };    
    
    this.changePermissions = function(permission){
        user.permissions = permission;
    };
/* END USER FUNCTIONS */ 
    


/*AUTO COMPLETE / VALIDATION FUNCTIONS */

    this.getDescriptions = function(){
        var descriptions = [];
        for (var index in requests){
            var description = requests[index].itemDescription;
            if ($.inArray(description, descriptions)){
                descriptions.push(requests[index].itemDescription);
            }
        }
        return descriptions;
    };
    
    this.getAnalysisCodesAutoComplete = function(){
        var codes = [];
        for (var index in analysisCodes){
            var code = analysisCodes[index].code;
            codes.push(code);
        }
        return codes;
    };     

    this.getRooms = function(){
        return rooms;
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
        
        return analysisCodes;
    }; 
    
    this.getAnalysisCode = function(id){
        for (var i = 0; i < analysisCodes.length; i++) {
            if (analysisCodes[i].id === id) {
                return analysisCodes[i];
            }
        }
        return null;
    };
    
    this.insertAnalysisCode = function(data){
        var topID = analysisCodes.length + 1;
        data["id"] = topID;
        analysisCodes.push(data);
    };     
    this.updateAnalysisCode = function(id,data){
        for (var i = 0; i < analysisCodes.length; i++) {
            if (analysisCodes[i].id === id) {
                for (var x in data){
                    analysisCodes[i][x] = data[x];
                }
                return;
            }
        }
    }; 
    
    this.deleteAnalysisCode = function(id){
        for (var i = analysisCodes.length - 1; i >= 0; i--) {
            if (analysisCodes[i].id === id) {
                analysisCodes.splice(i, 1);
                break;
            }
        }
    };     
    
    
    
/*END ANALYSIS CODE FUNCTIONS */



/* SUPPLIERS FUNCTIONS */    
    this.getSuppliers = function(){
        return suppliers;
    };
    
     this.getSupplier = function (id) {
        for (var i = 0; i < suppliers.length; i++) {
            if (suppliers[i].id === id) {
                return suppliers[i];
            }
        }
        return null;
    };   
    
    
    this.insertSupplier = function(data){
        var topID = suppliers.length + 1;
        data["id"] = topID;
        suppliers.push(data);
    };     
    this.updateSupplier = function(id,data){
        for (var i = 0; i < suppliers.length; i++) {
            if (suppliers[i].id === id) {
                for (var x in data){
                    suppliers[i][x] = data[x];
                }
                return;
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
    };     
    
/* END SUPPLIERS FUNCTIONS */      
    
    
/* REQUESTS FUNCTIONS */     
    this.getRequests = function () {
        if (user.permissions === "admin" || user.permissions === "accountant"){
            //get all requests
            return requests;
        }

        else if (user.permissions === "accountant" || user.permissions === "manager"){
            //get requests under this accountant or manaer
            var users = [];
            for (var i = 0; i < userGroups.length; i++) {
                if (userGroups[i].id === user.groupid) {
                    users = userGroups[i].users;
                }
            }
            
            requests = requests.filter(function(x){return users.indexOf(x.userid) > -1});
            return requests;
        }
        //otherwise just get the requests for this user
        requests = requests.filter(function(x){return x.userid === user.id;});
        return requests;
    };
    
    

    this.insertRequest = function (data) {
        var topID = requests.length + 1;
        data["id"] = topID;
        data["status"] = "Requested";
        
        //get current date in correct format
        var today = new Date();var dd = today.getDate();var mm = today.getMonth()+1;var yyyy = today.getFullYear();
        if(dd<10) {dd='0'+dd;} if(mm<10) {mm='0'+mm;} today = dd+'/'+mm+'/'+yyyy;       
        
        
        data["date"] = new Date();
        
        data["userid"] = user.id;
        data["name"] = user.name;
        data["phone"] = user.phone;
        data["email"] = user.email;
        requests.push(data);
    };
    
    this.updateRequest = function (id,data) {
        //need to detect it is the first time it is changed to this status
        
        if (user.permissions === "admin" && data["status"] === "Received"){
            data["adminName"] = user.name;
        }
        for (var i = 0; i < requests.length; i++) {
            if (requests[i].id === id) {


                for (var x in data){
                    if (x === 'dateSupplied' && data['status'] === 'Supplied' && user.permissions === "admin"){
                        var today = new Date();var dd = today.getDate();var mm = today.getMonth()+1;var yyyy = today.getFullYear();
                        if(dd<10) {dd='0'+dd;} if(mm<10) {mm='0'+mm;} today = dd+'/'+mm+'/'+yyyy;                             
                        //data[x] = today;
                        requests[i][x] = today;
                    }
                    else{
                        requests[i][x] = data[x];
                    }
                }
                return;
            }
        }
    };
    
    this.deleteRequest = function (id) {
        for (var i = requests.length - 1; i >= 0; i--) {
            if (requests[i].id === id) {
                requests.splice(i, 1);
                break;
            }
        }
    };

    this.getRequest = function (id) {
        for (var i = 0; i < requests.length; i++) {
            if (requests[i].id === id) {
                return requests[i];
            }
        }
        return null;
    };
    
    this.filterRequests = function (field, option, value, date1, date2){
        
        for (var index = requests.length; index--;){
            if(option === "contain" && requests[index][field] && requests[index][field].indexOf(value) === -1){
                requests[index]["filtered"] = 1;
            }
            else if (option === "notcontain" && requests[index][field] && requests[index][field].indexOf(value) > -1){
                
                requests[index]["filtered"] = 1;
            }
            else if(option === "after" && requests[index][field] && requests[index][field] < new Date(date1)){
                requests[index]["filtered"] = 1;
            }
            else if(option === "before" && requests[index][field] && requests[index][field] > new Date(date1)){
                requests[index]["filtered"] = 1;
            }            
            else if(option === "notbetween" && requests[index][field] && requests[index][field] > new Date(date1) && requests[index][field] < new Date(date2)){
                requests[index]["filtered"] = 1;
            } 
            else if(option === "between" && requests[index][field] && (requests[index][field] < new Date(date1) || requests[index][field] > new Date(date2))){
                requests[index]["filtered"] = 1;
            } 
            else{
                requests[index]["filtered"] = 0;
            }
            
            
        }
        

    };
    
/* END REQUESTS FUNCTIONS */     
    

/* TOOLTIP FUNCTIONS */

    this.getTooltips = function(){
        return tooltips;
    };


/* END TOOLTIP FUNCTIONS */


/* DEFINE DUMMY DATA - TO BE REPLACED BY API CALLS TO GET REAL DATA */



    //USER DATA (get from sharepoint)
    var user = {id: 1, name:"John Doe", phone:"1234567", email:"johndoe@massey.ac.nz", permissions:"accountant", groupid:2};


    //DEFINING USER GROUPS (get from sharepoint)
    var userGroups = [
        {
            id: 1, name:"Toms manager group",users:[1]

        } ,
        {
            id: 2, name:"Jims accoutnant group", users:[2]

        }];



    //TOOLTIPS (get from db)
    
    var tooltips = {type:"Classification of the request",
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

/* END DUMMY DATA */    
    
});