//This handles retrieving data and is used by controllers. 3 options (server, factory, provider) with 
//each doing the same thing just structuring the functions/data differently.
app.service('requestsService', function () {
    
    var name = "John Doe";
    var phone = "1234567";
    var email = "johndoe@massey.ac.nz";
    var permissions = "standard";
    var group = "Toms manager group";
    
        
    this.getUser = function(){
        return {
            name:name,
            phone:phone,
            email:email,
            permissions:permissions,
            group:group
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
                console.log("here");
                requests[index]["filtered"] = 0;
            }
            
            
        }
        

    };
    
    var requests = [
        {
            id: 1, status: 'Supplied',date: new Date('2014-12-20'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:new Date('2014-12-21'), adminNotes: 'This has been ordered', 
            adminName:'Bob', cost: '$123', supplier: 'Person'
            
        } ,
        {
            id: 2, status: 'Ordered',date: new Date('2014-12-21'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 3, status: 'Supplied',date: new Date('2014-12-22'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC C 4',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:new Date('2014-12-23'), adminNotes: 'This has been ordered', 
            adminName:'Bob', cost: '$123', supplier: 'Person'
            
        } ,
        {
            id: 4, status: 'Ordered',date: new Date('2014-12-22'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 5, status: 'Ordered',date: new Date('2014-12-22'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 6, status: 'Ordered',date: new Date('2014-12-22'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC D 4',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        } ,
        {
            id: 7, status: 'Received',date: new Date('2014-12-23'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: 'Almost ready', 
            cost: '$560', supplier: 'Guy'
            
        },
        {
            id: 8, status: 'Ordered',date: new Date('2014-12-23'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 9, status: 'Cancelled',date: new Date('2014-12-23'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        } ,
        {
            id: 10, status: 'Ordered',date: new Date('2014-12-23'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 11, status: 'Ordered',date: new Date('2014-12-24'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC D 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        },
        {
            id: 12, status: 'Ordered',date: new Date('2014-12-27'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC B 2',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        } ,
        {
            id: 13, status: 'Incorrect',date: new Date('2014-01-05'), name:'John Doe', phone:'1234567', email:'johndoe@massey.ac.nz', 
            type:'Chemical',itemDescription: 'This is a description of the item',quality:1,size:2, quantity:3,destinationRoom:'SC A 1',notes:'Some Notes', 
            cas:'1234444', vertere: '213234', accountNumber: 'GL123456789', analysisCode: 'wezxy' ,dateSupplied:'', adminNotes: '', 
            cost: '', supplier: ''
            
        }

        
    ];
    
    
    //clone of array to reset when filtering
    var originalRequests = requests.slice();
    
    
});