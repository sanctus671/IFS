using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models;
using System.Data.SqlClient;
using System.Web.Http.Cors;

namespace api.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class RequestController : ApiController
    {
        // GET: api/Request
        public IEnumerable<Request> Get(int offset = 0, int limit = 1000, List<string> usergroup = null)
        {

            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand requests = new SqlCommand(@"SELECT * FROM requests 
                                                    INNER JOIN rooms ON requests.roomid = rooms.id 
                                                    INNER JOIN accounts ON requests.accountid = accounts.id 
                                                    INNER JOIN sharepoint_users ON requests.userid = sharepoint_users.id 
                                                    INNER JOIN request_items ON requests.id = request_items.requestid 
                                                    INNER JOIN items ON request_items.itemid = items.id 
                                                    INNER JOIN descriptions ON items.descriptionid = descriptions.id
                                                    LEFT JOIN analysis_codes ON requests.codeid = analysis_codes.id  ORDER BY requests.id DESC OFFSET " + offset + " ROWS FETCH NEXT " + limit + " ROWS ONLY", con);
            requests.CommandTimeout = 0;
            SqlDataReader requestsReader = requests.ExecuteReader();

            List<Request> items = new List<Request>();

            while (requestsReader.Read())
            {
                Request data = new Request();


                //fill in what we have from initial SQL call
                data.id = (int)requestsReader["id"];
                data.notes = (string)requestsReader["notes"];
                data.destinationRoom = (string)requestsReader["room"];
                data.accountNumber = (string)requestsReader["number"];


                data.name = (requestsReader["name"] ?? "").ToString();
                data.phone = (requestsReader["phone"] ?? "").ToString();
                data.email = (requestsReader["email"] ?? "").ToString(); 
                
                data.type = (string)requestsReader["type"];
                data.cas = (requestsReader["cas"] ?? "").ToString();

                data.itemDescription = (requestsReader["description"] ?? "").ToString();
                data.quality = (requestsReader["quality"] ?? "").ToString();
                data.size = (requestsReader["size"] ?? "").ToString();
                data.quantity = (int)(requestsReader["quantity"] ?? 0);
                data.vertere = (int)(requestsReader["vertere"] ?? 0);

                //could be null
                data.analysisCode = (requestsReader["code"] ?? "").ToString();


                
                //find the status' (could be many)

                SqlCommand statuses = new SqlCommand(@"SELECT * FROM request_status  
                                                    INNER JOIN sharepoint_users ON request_status.userid = sharepoint_users.id 
                                                    WHERE request_status.requestid = " + data.id + " ORDER BY request_status.date ASC", con);
                statuses.CommandTimeout = 0;
                SqlDataReader statusesReader = statuses.ExecuteReader();
                
                List<Status> dataStatuses = new List<Status>();

                int x = 0;

                //need to define status out here to read the last status once done
                Status status = new Status();
                while (statusesReader.Read())
                {

                    //retreive all statuses
                    

                    status.requestid = (int)statusesReader["requestid"];
                    status.name = (string)statusesReader["name"];
                    status.status = (string)statusesReader["status"];
                    status.date = (DateTime)statusesReader["date"];

                    //first status = request date
                    if (x == 0)
                    {
                        data.date = status.Clone().date;
                    }
                    //set required fields for supplier if required
                    if (status.status.Equals("Supplied", StringComparison.OrdinalIgnoreCase))
                    {
                        data.adminName = status.Clone().name;
                        data.dateSupplied = status.Clone().date;
                    }

                    //add clone of the current object (else causes object reference errors)
                    dataStatuses.Add(status.Clone());

                    x++;
                }

                //latest status = request status
                data.status = status.status;


                //set all statuses
                data.statusArray = dataStatuses;




                //find all payments (could be 1 or none)

                SqlCommand payments = new SqlCommand(@"SELECT * FROM request_payments  
                                                    WHERE requestid = " + data.id, con);

                SqlDataReader paymentsReader = payments.ExecuteReader();



                x = 0;


                while (paymentsReader.Read())
                {
                    (requestsReader["name"] ?? "").ToString();
                    //there's a payment, and they'll only ever be one so set the data array directly
                    data.cost = (decimal)paymentsReader["cost"];
                    data.paymentType = (string)paymentsReader["type"];
                    data.pnNumber = (paymentsReader["pnnumber"] ?? "").ToString();
                    data.invoiceDetails = (paymentsReader["invoice"] ?? "").ToString();
                    x++;
                }

                if (x == 0)
                {
                    //no payments yet, add some fillers
                    data.cost = (decimal)0;
                    data.paymentType = (string)"";
                    data.pnNumber = (string)"";
                    data.invoiceDetails = (string)"";
                }



                //find all permits (could be 1 or none)

                SqlCommand permits = new SqlCommand(@"SELECT * FROM request_permits  
                                                    WHERE requestid = " + data.id, con);

                SqlDataReader permitsReader = permits.ExecuteReader();



                x = 0;


                while (permitsReader.Read())
                {
                    //there's a permit, and they'll only ever be one so set the data array directly
                    data.permit = true;
                    data.permitNumber = (string)permitsReader["number"];
                    x++;
                }

                if (x == 0)
                {
                    //no permits yet, add some fillers
                    data.permit = false;
                    data.permitNumber = (string)"";
                }



                //find all suppliers (could be 1 or none)

                SqlCommand suppliers = new SqlCommand(@"SELECT * FROM request_suppliers INNER JOIN suppliers ON request_suppliers.supplierid = suppliers.id  
                                                    WHERE request_suppliers.requestid = " + data.id, con);

                SqlDataReader suppliersReader = suppliers.ExecuteReader();



                x = 0;


                while (suppliersReader.Read())
                {
                    //there's a supplier, and they'll only ever be one so set the data array directly
                    data.supplier = (string)suppliersReader["name"];
                    x++;
                }

                if (x == 0)
                {
                    //no suppliers yet, add some fillers
                    data.supplier = (string)"";
                }



                //find all admin notes (could be 1 or none)

                SqlCommand adminNotes = new SqlCommand(@"SELECT * FROM request_admin_notes   
                                                    WHERE requestid = " + data.id, con);

                SqlDataReader adminNotesReader = adminNotes.ExecuteReader();



                x = 0;


                while (adminNotesReader.Read())
                {
                    //there's a admin note, and they'll only ever be one so set the data array directly
                    data.adminNotes = (string)adminNotesReader["note"];
                    x++;
                }

                if (x == 0)
                {
                    //no admin notes yet, add some fillers
                    data.adminNotes = (string)"";
                }



                items.Add(data);
            }

            //your code here;
            con.Close();
            return items;
        }

        private object List<T1>()
        {
            throw new NotImplementedException();
        }

        private object List<T1>()
        {
            throw new NotImplementedException();
        }

        // GET: api/Request/5
        public Request Get(int id)
        {

            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand requests = new SqlCommand(@"SELECT * FROM requests 
                                                    INNER JOIN rooms ON requests.roomid = rooms.id 
                                                    INNER JOIN accounts ON requests.accountid = accounts.id 
                                                    INNER JOIN sharepoint_users ON requests.userid = sharepoint_users.id 
                                                    INNER JOIN request_items ON requests.id = request_items.requestid 
                                                    INNER JOIN items ON request_items.itemid = items.id 
                                                    INNER JOIN descriptions ON items.descriptionid = descriptions.id
                                                    LEFT JOIN analysis_codes ON requests.codeid = analysis_codes.id WHERE requests.id = " + id, con);
            requests.CommandTimeout = 0;
            SqlDataReader requestsReader = requests.ExecuteReader();


            while (requestsReader.Read())
            {
                Request data = new Request();


                //fill in what we have from initial SQL call
                data.id = (int)requestsReader["id"];
                data.notes = (string)requestsReader["notes"];
                data.destinationRoom = (string)requestsReader["room"];
                data.accountNumber = (string)requestsReader["number"];


                data.name = (requestsReader["name"] ?? "").ToString();
                data.phone = (requestsReader["phone"] ?? "").ToString();
                data.email = (requestsReader["email"] ?? "").ToString();

                data.type = (string)requestsReader["type"];
                data.cas = (requestsReader["cas"] ?? "").ToString();

                data.itemDescription = (requestsReader["description"] ?? "").ToString();
                data.quality = (requestsReader["quality"] ?? "").ToString();
                data.size = (requestsReader["size"] ?? "").ToString();
                data.quantity = (int)(requestsReader["quantity"] ?? 0);
                data.vertere = (int)(requestsReader["vertere"] ?? 0);

                //could be null
                data.analysisCode = (requestsReader["code"] ?? "").ToString();



                //find the status' (could be many)

                SqlCommand statuses = new SqlCommand(@"SELECT * FROM request_status  
                                                    INNER JOIN sharepoint_users ON request_status.userid = sharepoint_users.id 
                                                    WHERE request_status.requestid = " + data.id + " ORDER BY request_status.date ASC", con);
                statuses.CommandTimeout = 0;
                SqlDataReader statusesReader = statuses.ExecuteReader();

                List<Status> dataStatuses = new List<Status>();

                int x = 0;

                //need to define status out here to read the last status once done
                Status status = new Status();
                while (statusesReader.Read())
                {

                    //retreive all statuses


                    status.requestid = (int)statusesReader["requestid"];
                    status.name = (string)statusesReader["name"];
                    status.status = (string)statusesReader["status"];
                    status.date = (DateTime)statusesReader["date"];

                    //first status = request date
                    if (x == 0)
                    {
                        data.date = status.Clone().date;
                    }
                    //set required fields for supplier if required
                    if (status.status.Equals("Supplied", StringComparison.OrdinalIgnoreCase))
                    {
                        data.adminName = status.Clone().name;
                        data.dateSupplied = status.Clone().date;
                    }

                    //add clone of the current object (else causes object reference errors)
                    dataStatuses.Add(status.Clone());

                    x++;
                }

                //latest status = request status
                data.status = status.status;


                //set all statuses
                data.statusArray = dataStatuses;




                //find all payments (could be 1 or none)

                SqlCommand payments = new SqlCommand(@"SELECT * FROM request_payments  
                                                    WHERE requestid = " + data.id, con);

                SqlDataReader paymentsReader = payments.ExecuteReader();



                x = 0;


                while (paymentsReader.Read())
                {
                    (requestsReader["name"] ?? "").ToString();
                    //there's a payment, and they'll only ever be one so set the data array directly
                    data.cost = (decimal)paymentsReader["cost"];
                    data.paymentType = (string)paymentsReader["type"];
                    data.pnNumber = (paymentsReader["pnnumber"] ?? "").ToString();
                    data.invoiceDetails = (paymentsReader["invoice"] ?? "").ToString();
                    x++;
                }

                if (x == 0)
                {
                    //no payments yet, add some fillers
                    data.cost = (decimal)0;
                    data.paymentType = (string)"";
                    data.pnNumber = (string)"";
                    data.invoiceDetails = (string)"";
                }



                //find all permits (could be 1 or none)

                SqlCommand permits = new SqlCommand(@"SELECT * FROM request_permits  
                                                    WHERE requestid = " + data.id, con);

                SqlDataReader permitsReader = permits.ExecuteReader();



                x = 0;


                while (permitsReader.Read())
                {
                    //there's a permit, and they'll only ever be one so set the data array directly
                    data.permit = true;
                    data.permitNumber = (string)permitsReader["number"];
                    x++;
                }

                if (x == 0)
                {
                    //no permits yet, add some fillers
                    data.permit = false;
                    data.permitNumber = (string)"";
                }



                //find all suppliers (could be 1 or none)

                SqlCommand suppliers = new SqlCommand(@"SELECT * FROM request_suppliers INNER JOIN suppliers ON request_suppliers.supplierid = suppliers.id  
                                                    WHERE request_suppliers.requestid = " + data.id, con);

                SqlDataReader suppliersReader = suppliers.ExecuteReader();



                x = 0;


                while (suppliersReader.Read())
                {
                    //there's a supplier, and they'll only ever be one so set the data array directly
                    data.supplier = (string)suppliersReader["name"];
                    x++;
                }

                if (x == 0)
                {
                    //no suppliers yet, add some fillers
                    data.supplier = (string)"";
                }



                //find all admin notes (could be 1 or none)

                SqlCommand adminNotes = new SqlCommand(@"SELECT * FROM request_admin_notes   
                                                    WHERE requestid = " + data.id, con);

                SqlDataReader adminNotesReader = adminNotes.ExecuteReader();



                x = 0;


                while (adminNotesReader.Read())
                {
                    //there's a admin note, and they'll only ever be one so set the data array directly
                    data.adminNotes = (string)adminNotesReader["note"];
                    x++;
                }

                if (x == 0)
                {
                    //no admin notes yet, add some fillers
                    data.adminNotes = (string)"";
                }



                return data;
            }

            
            con.Close();

            return new Request();

        }

        // POST: api/Request
        public void Post([FromBody]Request data)
        {
            

            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();


            //check if user exists from the data given
            SqlCommand users = new SqlCommand(@"SELECT id FROM sharepoint_users WHERE name = '" + data.name + "' AND email = '" + data.name + "' AND phone = '"+ data.phone + "'", con);



            SqlDataReader usersReader = users.ExecuteReader();

            int userid = 436; //default empty user
            try{
                usersReader.Read();
                userid = (int)usersReader["id"];
            }
            catch {
                //need to create new user
                SqlCommand newUser = new SqlCommand(@"INSERT INTO sharepoint_users (name,phone,email) OUTPUT INSERTED.id VALUES (@name, @phone, @email)", con);
                newUser.Parameters.AddWithValue("@name", data.name);
                newUser.Parameters.AddWithValue("@phone", data.phone);
                newUser.Parameters.AddWithValue("@email", data.email);

                //retreive the userid
                userid = (int)newUser.ExecuteScalar();
            }




            //check if room exists
            int roomid = 426; //default empty room
            if (!string.IsNullOrEmpty(data.destinationRoom))
            {


                SqlCommand rooms = new SqlCommand(@"SELECT id FROM rooms WHERE room = '" + data.destinationRoom + "'", con);
                rooms.Parameters.AddWithValue("@room", data.destinationRoom);

                SqlDataReader roomsReader = rooms.ExecuteReader();

                try{
                    roomsReader.Read();
                
                    roomid = (int)roomsReader["id"];


                }

                catch
                {
                    //need to create new room
                    SqlCommand newRoom = new SqlCommand(@"INSERT INTO rooms (room) OUTPUT INSERTED.id VALUES (@room)", con);
                    newRoom.Parameters.AddWithValue("@room", data.destinationRoom);

                    //retreive the roomid
                    roomid = (int)newRoom.ExecuteScalar();
                }
            }




            //check if account exists
            int accountid = 425; //default empty account
            if (!string.IsNullOrEmpty(data.accountNumber))
            {
                SqlCommand accounts = new SqlCommand(@"SELECT id FROM accounts WHERE number = '" + data.accountNumber + "'", con);

                //throw new Exception(@"SELECT id FROM accounts WHERE number = '" + data.accountNumber + "'");
                SqlDataReader accountsReader = accounts.ExecuteReader();

   
                
                try{
                    
                    accountsReader.Read();
                
                    accountid = (int)accountsReader["id"];


                }

                catch
                {
                    //need to create new account
                    SqlCommand newAccount = new SqlCommand(@"INSERT INTO accounts (number) OUTPUT INSERTED.id VALUES (@number)", con);
                    newAccount.Parameters.AddWithValue("@number", data.accountNumber);

                    //retreive the accountid
                    accountid = (int)newAccount.ExecuteScalar();
                }
            }


            //check if analysis code exists
            int codeid = 1; //default empty analysis code
            if (!string.IsNullOrEmpty(data.analysisCode))
            {
                SqlCommand analysisCodes = new SqlCommand(@"SELECT id FROM analysis_codes WHERE code = '" + data.analysisCode + "'", con);
                SqlDataReader analysisCodesReader = analysisCodes.ExecuteReader();

                try{
                    analysisCodesReader.Read();
                
                    codeid = (int)analysisCodesReader["id"];


                }

                catch
                {
                    //need to create new code
                    SqlCommand newAnalysisCode = new SqlCommand(@"INSERT INTO analysis_codes (code) OUTPUT INSERTED.id VALUES (@code)", con);
                    newAnalysisCode.Parameters.AddWithValue("@code", data.analysisCode ?? "");

                    //retreive the accountid
                    codeid = (int)newAnalysisCode.ExecuteScalar();
                }

            }



            //ready to insert main request
            SqlCommand newRequest = new SqlCommand(@"INSERT INTO requests (userid,roomid,accountid,codeid,notes) OUTPUT INSERTED.id VALUES (@userid,@roomid,@accountid,@codeid,@notes)", con);
            newRequest.Parameters.AddWithValue("@userid", userid);
            newRequest.Parameters.AddWithValue("@roomid", roomid);
            newRequest.Parameters.AddWithValue("@accountid", accountid);
            newRequest.Parameters.AddWithValue("@codeid", codeid);
            newRequest.Parameters.AddWithValue("@notes", data.notes ?? "");

            int requestid = (int)newRequest.ExecuteScalar();


            //now insert optional things


            //insert admin notes if there are any
            if (!string.IsNullOrEmpty(data.adminNotes))
            {
                SqlCommand newAdminNote = new SqlCommand(@"INSERT INTO request_admin_notes (requestid,note) VALUES (@requestid,@note)", con);
                newAdminNote.Parameters.AddWithValue("@requestid", requestid);
                newAdminNote.Parameters.AddWithValue("@note", data.adminNotes);

                newAdminNote.ExecuteNonQuery();

            }

            //insert payment if there is one
            if (!string.IsNullOrEmpty(data.paymentType))
            {
                SqlCommand newPayment = new SqlCommand(@"INSERT INTO request_payments (requestid,type,cost,pnnumber,invoice) VALUES (@requestid,@type,@cost,@pnnumber,@invoice)", con);
                newPayment.Parameters.AddWithValue("@requestid", requestid);
                newPayment.Parameters.AddWithValue("@type", data.paymentType ?? "internal");
                newPayment.Parameters.AddWithValue("@cost", data.cost);
                newPayment.Parameters.AddWithValue("@pnnumber", data.pnNumber ?? "");
                newPayment.Parameters.AddWithValue("@invoice", data.invoiceDetails ?? "");

                newPayment.ExecuteNonQuery();

            }

            //insert permits if there is one
            if (data.permit)
            {
                SqlCommand newPermit = new SqlCommand(@"INSERT INTO request_permits (requestid,number) VALUES (@requestid,@number)", con);
                newPermit.Parameters.AddWithValue("@requestid", requestid);
                newPermit.Parameters.AddWithValue("@number", data.permitNumber);


                newPermit.ExecuteNonQuery();

            }


            //insert status if there is one (there always will be at least 1)
            if (!string.IsNullOrEmpty(data.status))
            {
                //check if admin changed the status (ie admin name exists)
                if (!string.IsNullOrEmpty(data.adminName))
                {
                    SqlCommand adminUsers = new SqlCommand(@"SELECT id FROM sharepoint_users WHERE name = '" + data.adminName + "'", con);

                    SqlDataReader adminUsersReader = adminUsers.ExecuteReader();

                    int size = 0;

                    while (adminUsersReader.Read())
                    {
                        userid = (int)adminUsersReader["id"];
                        size++;

                    }

                    if (size < 1)
                    {
                        //need to create new admin user
                        SqlCommand newAdminUser = new SqlCommand(@"INSERT INTO sharepoint_users (name) OUTPUT INSERTED.id VALUES (@name)", con);
                        newAdminUser.Parameters.AddWithValue("@name", data.adminName);

                        //retreive the userid
                        userid = (int)newAdminUser.ExecuteScalar();
                    }
                }

                SqlCommand newStatus = new SqlCommand(@"INSERT INTO request_status (requestid,userid,date,status) VALUES (@requestid,@userid,CURRENT_TIMESTAMP,@status)", con);
                newStatus.Parameters.AddWithValue("@requestid", requestid);
                newStatus.Parameters.AddWithValue("@userid", userid);
                newStatus.Parameters.AddWithValue("@status", data.status ?? "Requested");


                newStatus.ExecuteNonQuery();

            }


            //insert item if there is one
            if (!string.IsNullOrEmpty(data.itemDescription))
            {
                SqlCommand descriptions = new SqlCommand(@"SELECT id FROM descriptions WHERE description = '" + data.itemDescription + "'", con);
                descriptions.Parameters.AddWithValue("@description", data.itemDescription);
                SqlDataReader descriptionsReader = descriptions.ExecuteReader();


                int descriptionid = 6743;
                int itemid = 7167;
                try{
                    descriptionsReader.Read();
                
                    descriptionid = (int)descriptionsReader["id"];

                    SqlCommand items = new SqlCommand(@"SELECT id FROM items WHERE descriptionid = " + descriptionid, con);

                    itemid = (int)items.ExecuteScalar();

                }

                catch
                {
                    //need to create new description
                    SqlCommand newDescription = new SqlCommand(@"INSERT INTO descriptions (description) OUTPUT INSERTED.id VALUES (@description)", con);
                    newDescription.Parameters.AddWithValue("@description", data.itemDescription);

                    //retreive the descriptionid
                    descriptionid = (int)newDescription.ExecuteScalar();

                    //also need to create new item to hold the description


                    SqlCommand newItem = new SqlCommand(@"INSERT INTO items (descriptionid,type,cas) OUTPUT INSERTED.id VALUES (@descriptionid, @type, @cas)", con);
                    newItem.Parameters.AddWithValue("@descriptionid", descriptionid);
                    newItem.Parameters.AddWithValue("@type", data.type ?? "Other");
                    newItem.Parameters.AddWithValue("@cas", data.cas ?? "0");

                    //retreive the itemid
                    itemid = (int)newItem.ExecuteScalar();

                }




                SqlCommand newRequestItem = new SqlCommand(@"INSERT INTO request_items (requestid,itemid,quantity,quality,size,vertere) VALUES (@requestid,@itemid,@quantity,@quality, @size, @vertere)", con);
                newRequestItem.Parameters.AddWithValue("@requestid", requestid);
                newRequestItem.Parameters.AddWithValue("@itemid", itemid);
                newRequestItem.Parameters.AddWithValue("@quantity", data.quantity);
                newRequestItem.Parameters.AddWithValue("@quality", data.quality ?? "");
                newRequestItem.Parameters.AddWithValue("@size", data.size ?? "");
                newRequestItem.Parameters.AddWithValue("@vertere", data.vertere);



                newRequestItem.ExecuteNonQuery();

            }

            //insert supplier if there is one
            if (!string.IsNullOrEmpty(data.supplier))
            {
                SqlCommand suppliers = new SqlCommand(@"SELECT id FROM suppliers WHERE name = '" + data.supplier + "'", con);

                SqlDataReader suppliersReader = suppliers.ExecuteReader();


                int supplierid = 1;

                try
                {
                    suppliersReader.Read();

                    supplierid = (int)suppliersReader["id"];


                }

                catch
                {
                    //need to create new supplier
                    SqlCommand newSupplier = new SqlCommand(@"INSERT INTO suppliers (name) OUTPUT INSERTED.id VALUES (@name)", con);
                    newSupplier.Parameters.AddWithValue("@name", data.supplier);

                    //retreive the supplierid
                    supplierid = (int)newSupplier.ExecuteScalar();



                }




                SqlCommand newRequestSupplier = new SqlCommand(@"INSERT INTO request_suppliers (requestid,supplierid) VALUES (@requestid,@supplierid)", con);
                newRequestSupplier.Parameters.AddWithValue("@requestid", requestid);
                newRequestSupplier.Parameters.AddWithValue("@supplierid", supplierid);




                newRequestSupplier.ExecuteNonQuery();

            }



            con.Close();








        }

        // PUT: api/Request/5
        public void Put(int id, [FromBody]Request data)
        {
            Request request = this.Get(id);

            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();


            //check if user has changed
            if (!(data.name.Equals(request.name, StringComparison.OrdinalIgnoreCase) && data.phone.Equals(request.name, StringComparison.OrdinalIgnoreCase) && data.email.Equals(request.name, StringComparison.OrdinalIgnoreCase)))
            {

                //UPDATE/INSERT NEW USER

                //check if user exists from the data given
                SqlCommand users = new SqlCommand(@"SELECT id FROM sharepoint_users WHERE name = '" + data.name + "' AND email = '" + data.name + "' AND phone = '" + data.phone + "'", con);



                SqlDataReader usersReader = users.ExecuteReader();


                int userid = 436; //default empty user
                try
                {
                    usersReader.Read();
                    userid = (int)usersReader["id"];
                }
                catch
                {
                    //need to create new user
                    SqlCommand newUser = new SqlCommand(@"INSERT INTO sharepoint_users (name,phone,email) OUTPUT INSERTED.id VALUES (@name, @phone, @email)", con);
                    newUser.Parameters.AddWithValue("@name", data.name);
                    newUser.Parameters.AddWithValue("@phone", data.phone);
                    newUser.Parameters.AddWithValue("@email", data.email);

                    //retreive the userid
                    userid = (int)newUser.ExecuteScalar();
                }

                //perform update on main request
                SqlCommand updateRequestUser = new SqlCommand(@"UPDATE requests SET userid = @userid WHERE id = @requestid", con);
                updateRequestUser.Parameters.AddWithValue("@userid", userid);
                updateRequestUser.Parameters.AddWithValue("@requestid", id);

                updateRequestUser.ExecuteScalar();
            }


            //check if room has changed 
            if (!(data.destinationRoom.Equals(request.destinationRoom, StringComparison.OrdinalIgnoreCase)))
            {
                //UPDATE/INSERT NEW USER
                //check if room exists
                int roomid = 426; //default empty room
                if (!string.IsNullOrEmpty(data.destinationRoom))
                {


                    SqlCommand rooms = new SqlCommand(@"SELECT id FROM rooms WHERE room = '" + data.destinationRoom + "'", con);
                    rooms.Parameters.AddWithValue("@room", data.destinationRoom);

                    SqlDataReader roomsReader = rooms.ExecuteReader();

                    try
                    {
                        roomsReader.Read();

                        roomid = (int)roomsReader["id"];


                    }

                    catch
                    {
                        //need to create new room
                        SqlCommand newRoom = new SqlCommand(@"INSERT INTO rooms (room) OUTPUT INSERTED.id VALUES (@room)", con);
                        newRoom.Parameters.AddWithValue("@room", data.destinationRoom);

                        //retreive the roomid
                        roomid = (int)newRoom.ExecuteScalar();
                    }
                }
                //perform update on main request
                SqlCommand updateRequestRoom = new SqlCommand(@"UPDATE requests SET roomid = @roomid WHERE id = @requestid", con);
                updateRequestRoom.Parameters.AddWithValue("@roomid", roomid);
                updateRequestRoom.Parameters.AddWithValue("@requestid", id);

                updateRequestRoom.ExecuteScalar();
            }


            //check if account has changed

            if (!(data.accountNumber.Equals(request.accountNumber, StringComparison.OrdinalIgnoreCase)))
            {
                //UPDATE/INSERT NEW ACCOUNT
                //check if account exists
                int accountid = 425; //default empty account
                if (!string.IsNullOrEmpty(data.accountNumber))
                {
                    SqlCommand accounts = new SqlCommand(@"SELECT id FROM accounts WHERE number = '" + data.accountNumber + "'", con);

                    //throw new Exception(@"SELECT id FROM accounts WHERE number = '" + data.accountNumber + "'");
                    SqlDataReader accountsReader = accounts.ExecuteReader();



                    try
                    {

                        accountsReader.Read();

                        accountid = (int)accountsReader["id"];


                    }

                    catch
                    {
                        //need to create new account
                        SqlCommand newAccount = new SqlCommand(@"INSERT INTO accounts (number) OUTPUT INSERTED.id VALUES (@number)", con);
                        newAccount.Parameters.AddWithValue("@number", data.accountNumber);

                        //retreive the accountid
                        accountid = (int)newAccount.ExecuteScalar();
                    }
                }
                //perform update on main request
                SqlCommand updateRequestAccount = new SqlCommand(@"UPDATE requests SET accountid = @accountid WHERE id = @requestid", con);
                updateRequestAccount.Parameters.AddWithValue("@accountid", accountid);
                updateRequestAccount.Parameters.AddWithValue("@requestid", id);

                updateRequestAccount.ExecuteScalar();
            }

            //check if analysis code has changed

            if (!(data.analysisCode.Equals(request.analysisCode, StringComparison.OrdinalIgnoreCase)))
            {
                //check if analysis code exists
                int codeid = 1; //default empty analysis code
                if (!string.IsNullOrEmpty(data.analysisCode))
                {
                    SqlCommand analysisCodes = new SqlCommand(@"SELECT id FROM analysis_codes WHERE code = '" + data.analysisCode + "'", con);
                    SqlDataReader analysisCodesReader = analysisCodes.ExecuteReader();

                    try
                    {
                        analysisCodesReader.Read();

                        codeid = (int)analysisCodesReader["id"];


                    }

                    catch
                    {
                        //need to create new code
                        SqlCommand newAnalysisCode = new SqlCommand(@"INSERT INTO analysis_codes (code) OUTPUT INSERTED.id VALUES (@code)", con);
                        newAnalysisCode.Parameters.AddWithValue("@code", data.analysisCode ?? "");

                        //retreive the accountid
                        codeid = (int)newAnalysisCode.ExecuteScalar();
                    }

                }
                //perform update on main request
                SqlCommand updateRequestCode = new SqlCommand(@"UPDATE requests SET codeid = @codeid WHERE id = @requestid", con);
                updateRequestCode.Parameters.AddWithValue("@codeid", codeid);
                updateRequestCode.Parameters.AddWithValue("@requestid", id);

                updateRequestCode.ExecuteScalar();
            }


            //check if admin notes have changed

            if (!(data.adminNotes.Equals(request.adminNotes, StringComparison.OrdinalIgnoreCase)))
            {
                //insert admin notes if there are any
                if (string.IsNullOrEmpty(request.adminNotes))
                {
                    SqlCommand newAdminNote = new SqlCommand(@"INSERT INTO request_admin_notes (requestid,note) VALUES (@requestid,@note)", con);
                    newAdminNote.Parameters.AddWithValue("@requestid", id);
                    newAdminNote.Parameters.AddWithValue("@note", data.adminNotes);

                    newAdminNote.ExecuteNonQuery();

                }
                else
                {
                    //is update existing admin note
                    SqlCommand updateAdminNote = new SqlCommand(@"UPDATE request_admin_notes SET note = @note WHERE requestid = @requestid", con);
                    updateAdminNote.Parameters.AddWithValue("@requestid", id);
                    updateAdminNote.Parameters.AddWithValue("@note", data.adminNotes);

                    updateAdminNote.ExecuteNonQuery();
                }
            }


            //check if payment has changed
            if (!(data.paymentType.Equals(request.paymentType, StringComparison.OrdinalIgnoreCase) && data.cost == request.cost && data.pnNumber.Equals(request.pnNumber, StringComparison.OrdinalIgnoreCase) && data.invoiceDetails.Equals(request.invoiceDetails, StringComparison.OrdinalIgnoreCase)))
            {
                //insert payment if there is one
                if (string.IsNullOrEmpty(request.paymentType))
                {
                    SqlCommand newPayment = new SqlCommand(@"INSERT INTO request_payments (requestid,type,cost,pnnumber,invoice) VALUES (@requestid,@type,@cost,@pnnumber,@invoice)", con);
                    newPayment.Parameters.AddWithValue("@requestid", id);
                    newPayment.Parameters.AddWithValue("@type", data.paymentType ?? "internal");
                    newPayment.Parameters.AddWithValue("@cost", data.cost);
                    newPayment.Parameters.AddWithValue("@pnnumber", data.pnNumber ?? "");
                    newPayment.Parameters.AddWithValue("@invoice", data.invoiceDetails ?? "");

                    newPayment.ExecuteNonQuery();

                }
                else
                {
                    //is an update to existing payment
                    SqlCommand updatePayment = new SqlCommand(@"UPDATE request_payments SET type = @type,cost = @cost,pnnumber = @pnnumber,invoice = @invoice WHERE id = @requestid", con);
                    updatePayment.Parameters.AddWithValue("@requestid", id);
                    updatePayment.Parameters.AddWithValue("@type", data.paymentType ?? "internal");
                    updatePayment.Parameters.AddWithValue("@cost", data.cost);
                    updatePayment.Parameters.AddWithValue("@pnnumber", data.pnNumber ?? "");
                    updatePayment.Parameters.AddWithValue("@invoice", data.invoiceDetails ?? "");

                    updatePayment.ExecuteNonQuery();

                }
            }



            //check if permit has changed
            if (!(data.permitNumber.Equals(request.permitNumber, StringComparison.OrdinalIgnoreCase)))
            {
                //insert permits if there is one
                if (!request.permit)
                {
                    SqlCommand newPermit = new SqlCommand(@"INSERT INTO request_permits (requestid,number) VALUES (@requestid,@number)", con);
                    newPermit.Parameters.AddWithValue("@requestid",  id);
                    newPermit.Parameters.AddWithValue("@number", data.permitNumber);


                    newPermit.ExecuteNonQuery();

                }
                else
                {
                    SqlCommand updatePermit = new SqlCommand(@"UPDATE request_permits SET  number = @number WHERE requestid = @requestid", con);
                    updatePermit.Parameters.AddWithValue("@requestid", id);
                    updatePermit.Parameters.AddWithValue("@number", data.permitNumber);


                    updatePermit.ExecuteNonQuery();
                }

            }

            //check if status is the same or not
            if (!(data.status.Equals(request.status, StringComparison.OrdinalIgnoreCase)))
            {
                //insert status if there is one (there always will be at least 1)
                if (!string.IsNullOrEmpty(data.status))
                {
                    //check if admin changed the status (ie admin name exists)
                    int userid = 436;
                    if (!string.IsNullOrEmpty(data.adminName))
                    {
                        SqlCommand adminUsers = new SqlCommand(@"SELECT id FROM sharepoint_users WHERE name = '" + data.adminName + "'", con);

                        SqlDataReader adminUsersReader = adminUsers.ExecuteReader();

                        int size = 0;
                        
                        while (adminUsersReader.Read())
                        {
                            userid = (int)adminUsersReader["id"];
                            size++;

                        }

                        if (size < 1)
                        {
                            //need to create new admin user
                            SqlCommand newAdminUser = new SqlCommand(@"INSERT INTO sharepoint_users (name) OUTPUT INSERTED.id VALUES (@name)", con);
                            newAdminUser.Parameters.AddWithValue("@name", data.adminName);

                            //retreive the userid
                            userid = (int)newAdminUser.ExecuteScalar();
                        }
                    }

                    SqlCommand newStatus = new SqlCommand(@"INSERT INTO request_status (requestid,userid,date,status) VALUES (@requestid,@userid,CURRENT_TIMESTAMP,@status)", con);
                    newStatus.Parameters.AddWithValue("@requestid", id);
                    newStatus.Parameters.AddWithValue("@userid", userid);
                    newStatus.Parameters.AddWithValue("@status", data.status ?? "Requested");


                    newStatus.ExecuteNonQuery();

                }
            }

            //check if item has changed
            if (!(data.itemDescription.Equals(request.itemDescription, StringComparison.OrdinalIgnoreCase)))
            {
                //insert item if there is one
                if (!string.IsNullOrEmpty(data.itemDescription))
                {
                    SqlCommand descriptions = new SqlCommand(@"SELECT id FROM descriptions WHERE description = '" + data.itemDescription + "'", con);
                    descriptions.Parameters.AddWithValue("@description", data.itemDescription);
                    SqlDataReader descriptionsReader = descriptions.ExecuteReader();


                    int descriptionid = 6743;
                    int itemid = 7167;
                    try
                    {
                        descriptionsReader.Read();

                        descriptionid = (int)descriptionsReader["id"];

                        SqlCommand items = new SqlCommand(@"SELECT id FROM items WHERE descriptionid = " + descriptionid, con);

                        itemid = (int)items.ExecuteScalar();

                    }

                    catch
                    {
                        //need to create new description
                        SqlCommand newDescription = new SqlCommand(@"INSERT INTO descriptions (description) OUTPUT INSERTED.id VALUES (@description)", con);
                        newDescription.Parameters.AddWithValue("@description", data.itemDescription);

                        //retreive the descriptionid
                        descriptionid = (int)newDescription.ExecuteScalar();

                        //also need to create new item to hold the description


                        SqlCommand newItem = new SqlCommand(@"INSERT INTO items (descriptionid,type,cas) OUTPUT INSERTED.id VALUES (@descriptionid, @type, @cas)", con);
                        newItem.Parameters.AddWithValue("@descriptionid", descriptionid);
                        newItem.Parameters.AddWithValue("@type", data.type ?? "Other");
                        newItem.Parameters.AddWithValue("@cas", data.cas ?? "0");

                        //retreive the itemid
                        itemid = (int)newItem.ExecuteScalar();

                    }




                    SqlCommand updateRequestItem = new SqlCommand(@"UPDATE request_items SET requestid = @requestid, itemid = @itemid, quantity = @quantity, quality = @quality, size = @size, vertere  =@vertere WHERE requestid = @requestid", con);
                    updateRequestItem.Parameters.AddWithValue("@requestid", id);
                    updateRequestItem.Parameters.AddWithValue("@itemid", itemid);
                    updateRequestItem.Parameters.AddWithValue("@quantity", data.quantity);
                    updateRequestItem.Parameters.AddWithValue("@quality", data.quality ?? "");
                    updateRequestItem.Parameters.AddWithValue("@size", data.size ?? "");
                    updateRequestItem.Parameters.AddWithValue("@vertere", data.vertere);



                    updateRequestItem.ExecuteNonQuery();

                }
            }


            //check if supplier has changed
            if (!(data.supplier.Equals(request.supplier, StringComparison.OrdinalIgnoreCase)))
            {

            //insert supplier if there is one
                if (!string.IsNullOrEmpty(data.supplier))
                {
                    SqlCommand suppliers = new SqlCommand(@"SELECT id FROM suppliers WHERE name = '" + data.supplier + "'", con);

                    SqlDataReader suppliersReader = suppliers.ExecuteReader();


                    int supplierid = 1;

                    try
                    {
                        suppliersReader.Read();

                        supplierid = (int)suppliersReader["id"];


                    }

                    catch
                    {
                        //need to create new supplier
                        SqlCommand newSupplier = new SqlCommand(@"INSERT INTO suppliers (name) OUTPUT INSERTED.id VALUES (@name)", con);
                        newSupplier.Parameters.AddWithValue("@name", data.supplier);

                        //retreive the supplierid
                        supplierid = (int)newSupplier.ExecuteScalar();



                    }





                    SqlCommand newRequestSupplier = new SqlCommand(@"UPDATE request_suppliers SET supplierid = @supplierid WHERE requestid = @requestid", con);
                    newRequestSupplier.Parameters.AddWithValue("@requestid", id);
                    newRequestSupplier.Parameters.AddWithValue("@supplierid", supplierid);




                    newRequestSupplier.ExecuteNonQuery();
                }

            }



            con.Close();




        }

        // DELETE: api/Request/5
        public void Delete(int id)
        {

            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();
            //will also remove all associated items with this request
            SqlCommand deleteRequest = new SqlCommand(@"DELETE FROM requests WHERE id = @requestid", con);
            deleteRequest.Parameters.AddWithValue("@requestid", id);

            deleteRequest.ExecuteNonQuery();
        }
    }
}
