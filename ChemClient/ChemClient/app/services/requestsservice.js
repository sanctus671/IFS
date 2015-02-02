app.service('requestsService', function () {

/* USER FUNCTIONS */         
    this.getUser = function(){
        return user;
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

    this.getRooms = function(){
        return rooms;
    };
    
    this.checkAccount = function(accountNumber){
        if (accountNumber.length < 11){
            return false;
        }
        return true;
    };


/* END AUTO COMPLETE / VALIDATION FUNCTIONS */ 






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
        if (user.permissions === "admin"){
            //get all requests
            return requests;
        }
        else if (user.permissions === "manager"){
            //get requests under this manager 
            return requests;
        }
        else if (user.permissions === "accountant"){
            //get requests under this accountant
            return requests;
        }
        //otherwise just get the requests for this user
        requests = requests.filter(function(x){return x.userid === user.id;});
        return requests;
    };
    
    

    this.insertRequest = function (data) {
        var topID = requests.length + 1;
        data["id"] = topID;
        data["status"] = "Ordered";
        
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
        for (var i = 0; i < requests.length; i++) {
            if (requests[i].id === id) {
                for (var x in data){
                    if (x === 'dateSupplied' && requests[i]['status'] === 'Supplied'){
                        var today = new Date();var dd = today.getDate();var mm = today.getMonth()+1;var yyyy = today.getFullYear();
                        if(dd<10) {dd='0'+dd;} if(mm<10) {mm='0'+mm;} today = dd+'/'+mm+'/'+yyyy;                             
                        data[x] = today;
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
    




//dumby data
    var user = {id: 1, name:"John Doe", phone:"1234567", email:"johndoe@massey.ac.nz", permissions:"admin", group:"Toms manager group"};
    
        
        
        
    var suppliers = [
        {
            id: 1, name:"supplier1"
            
        } ,
        {
            id: 2, name:"supplier2"
            
        }];
        

    
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
                            "SC A 4"]
    
    
    
    
    
    
    
    
    var requests = [
        {
            id: 1, status: 'Supplied',date: new Date('2014-12-20'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:new Date('2014-12-21'), adminNotes: 'This has been ordered', 
            adminName:'Bob', cost: '$123', supplier: 'Person'
            
        } ,
        {
            id: 2, status: 'Ordered',date: new Date('2014-12-21'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
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
            id: 4, status: 'Ordered',date: new Date('2014-12-22'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 5, status: 'Ordered',date: new Date('2014-12-22'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 6, status: 'Ordered',date: new Date('2014-12-22'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC D 4',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        } ,
        {
            id: 7, status: 'Received',date: new Date('2014-12-23'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: 'Almost ready', 
            cost: '$560', supplier: 'Guy'
            
        },
        {
            id: 8, status: 'Ordered',date: new Date('2014-12-23'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 9, status: 'Cancelled',date: new Date('2014-12-23'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        } ,
        {
            id: 10, status: 'Ordered',date: new Date('2014-12-23'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 11, status: 'Ordered',date: new Date('2014-12-24'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC D 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 12, status: 'Ordered',date: new Date('2014-12-27'), userid: 1,name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        } ,
        {
            id: 13, status: 'Incorrect',date: new Date('2015-01-05'), userid: 1, name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC A 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 14, status: 'Ordered',date: new Date('2015-01-05'), userid: 2, name:'Jane Doe', phone:'1234567', email:'janedoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC A 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 15, status: 'Ordered',date: new Date('2015-01-05'), userid: 2, name:'Jane Doe', phone:'1234567', email:'janedoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC A 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        }        

        
    ];

    
    
});