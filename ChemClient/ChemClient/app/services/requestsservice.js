﻿//This handles retrieving data and is used by controllers. 3 options (server, factory, provider) with 
//each doing the same thing just structuring the functions/data differently.
app.service('requestsService', function () {
    
    var name = "John Doe";
    var phone = "1234567";
    var email = "johndoe@massey.ac.nz";
    var permissions = "standard";
    
        
    this.getUser = function(){
        return {
            name:name,
            phone:phone,
            email:email,
            permissions:permissions
        };
    };
    
    this.getRequests = function () {
        return requests;
    };
    
    

    this.insertRequest = function (data) {
        //data options = id, date, firstName, lastName, phone extension, chemical type, chemical desciption, quality, size, quantity, destination room, notes, chIM Tag, Location now, status, date supplied
        var topID = requests.length + 1;
        data["id"] = topID;
        data["status"] = "New";
        
        //get current date in correct format
        var today = new Date();var dd = today.getDate();var mm = today.getMonth()+1;var yyyy = today.getFullYear();
        if(dd<10) {dd='0'+dd;} if(mm<10) {mm='0'+mm;} today = dd+'/'+mm+'/'+yyyy;       
        
        
        data["date"] = new Date();
        

        data["name"] = name;
        data["phone"] = phone;
        data["email"] = email;
        requests.push(data);
    };
    
    this.updateRequest = function (id,data) {
        for (var i = 0; i < requests.length; i++) {
            if (requests[i].id === id) {
                for (var x in data){
                    requests[i][x] = data[x];
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
            if(option === "Contains" && requests[index][field] && requests[index][field].indexOf(value) === -1){
                requests[index]["filtered"] = 1;
            }
            else if (option === "Does not contain" && requests[index][field] && requests[index][field].indexOf(value) > -1){
                
                requests[index]["filtered"] = 1;
            }
            else if(option === "Is after" && requests[index][field] && requests[index][field] < new Date(date1)){
                requests[index]["filtered"] = 1;
            }
            else if(option === "Is before" && requests[index][field] && requests[index][field] > new Date(date1)){
                requests[index]["filtered"] = 1;
            }            
            else if(option === "Is not between" && requests[index][field] && requests[index][field] > new Date(date1) && requests[index][field] < new Date(date2)){
                requests[index]["filtered"] = 1;
            } 
            else if(option === "Is between" && requests[index][field] && (requests[index][field] < new Date(date1) || requests[index][field] > new Date(date2))){
                requests[index]["filtered"] = 1;
            } 
            else{
                console.log("here");
                requests[index]["filtered"] = 0;
            }
            
            
        }
        

    };
    
    var requests = [
        {
            id: 1, status: 'Completed',date: new Date('2014-12-20'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location',dateSupplied:new Date('2014-12-21')
            
        } ,
        {
            id: 2, status: 'Processing',date:new Date('2014-12-21'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        },
        {
            id: 3, status: 'Completed',date:new Date('2014-12-22'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location',dateSupplied:new Date('2014-12-23')
            
        } ,
        {
            id: 4, status: 'New',date:new Date('2014-12-22'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        },
        {
            id: 5, status: 'New',date: new Date('2014-12-22'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        },
        {
            id: 6, status: 'New',date:new Date('2014-12-22'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        } ,
        {
            id: 7, status: 'New',date:new Date('2014-12-23'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        },
        {
            id: 8, status: 'New',date:new Date('2014-12-23'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        },
        {
            id: 9, status: 'New',date:new Date('2014-12-23'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        } ,
        {
            id: 10, status: 'New',date:new Date('2014-12-23'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        },
        {
            id: 11, status: 'New',date:new Date('2014-12-24'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        },
        {
            id: 12, status: 'New',date:new Date('2014-12-27'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type',itemDescription: 'Item Description',quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        } ,
        {
            id: 13, status: 'New',date:new Date('2014-12-28'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            chemicalType:'Chemical Type', itemDescription: 'Item Description', quality:'Quality',size:'Size', quantity:'Quantity',destinationRoom:'Destination Room',notes:'Notes', 
            cas:'CAS', chimTag: 'Chim Tag', location:'Location'
            
        }

        
    ];
    
    
    //clone of array to reset when filtering
    var originalRequests = requests.slice();
    
    
});