using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models;
using System.Data.SqlClient;
using System.Web.Http.Cors;
using api.EModels;

namespace api.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class RequestController : ApiController
    {
        // GET: api/Request
        public RequestPaginate Get(int offset = 0, int limit = 1000, string search = null, string filterString = null)
        {
            RequestPaginate returnRequests = new RequestPaginate();



            using (var db = new EModelsContext()){

                var result = new List<request>();

                //search and filtering
                if (!String.IsNullOrWhiteSpace(search) && filterString != null)
                {
                    var stringProperties = typeof(request).GetProperties().Where(prop => prop.PropertyType == search.GetType());

                    var filterArray = filterString.Split(';');

                    Filter filter = new Filter();
                    filter.field = filterArray[0];
                    filter.option = filterArray[1];

                    //filter is array containing field, option, values (array, could be value,date,date1,date2)


                    var field = typeof(request).GetProperty(filter.field);
                    if (filter.option.Equals("contain"))
                    {
                        filter.value = filterArray[2];
                        if (filter.field.Equals("supplier"))
                        {
                            result = db.requests.Where(req => req.request_suppliers.OrderBy(s => s.id).FirstOrDefault().supplier.name.Contains(filter.value) &&
                                (req.sharepoint_users.name.Contains(search) ||
                                req.account.number.Contains(search) ||
                                req.room.room1.Contains(search) ||
                                req.sharepoint_users.email.Contains(search) ||
                                req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                                ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();

                        }
                        else if (filter.field.Equals("accountNumber"))
                        {
                            result = db.requests.Where(req => req.account.number.Contains(filter.value) &&
                                (req.sharepoint_users.name.Contains(search) ||
                                req.account.number.Contains(search) ||
                                req.room.room1.Contains(search) ||
                                req.sharepoint_users.email.Contains(search) ||
                                req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                                ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.field.Equals("type"))
                        {
                            result = db.requests.Where(req => req.request_items.OrderBy(i => i.id).FirstOrDefault().description.type.Contains(filter.value) &&
                                (req.sharepoint_users.name.Contains(search) ||
                                req.account.number.Contains(search) ||
                                req.room.room1.Contains(search) ||
                                req.sharepoint_users.email.Contains(search) ||
                                req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                                ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.field.Equals("description"))
                        {
                            result = db.requests.Where(req => req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(filter.value) &&
                                (req.sharepoint_users.name.Contains(search) ||
                                req.account.number.Contains(search) ||
                                req.room.room1.Contains(search) ||
                                req.sharepoint_users.email.Contains(search) ||
                                req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                                ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                    }





                    else if (filter.option.Equals("notcontain"))
                    {
                        filter.value = filterArray[2];
                        if (filter.field.Equals("supplier"))
                        {
                            result = db.requests.Where(req => !req.request_suppliers.OrderBy(s => s.id).FirstOrDefault().supplier.name.Contains(filter.value) &&
                                (req.sharepoint_users.name.Contains(search) ||
                                req.account.number.Contains(search) ||
                                req.room.room1.Contains(search) ||
                                req.sharepoint_users.email.Contains(search) ||
                                req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                                ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();

                        }
                        else if (filter.field.Equals("accountNumber"))
                        {
                            result = db.requests.Where(req => !req.account.number.Contains(filter.value) &&
                                (req.sharepoint_users.name.Contains(search) ||
                                req.account.number.Contains(search) ||
                                req.room.room1.Contains(search) ||
                                req.sharepoint_users.email.Contains(search) ||
                                req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                                ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.field.Equals("type"))
                        {
                            result = db.requests.Where(req => !req.request_items.OrderBy(i => i.id).FirstOrDefault().description.type.Contains(filter.value) &&
                                (req.sharepoint_users.name.Contains(search) ||
                                req.account.number.Contains(search) ||
                                req.room.room1.Contains(search) ||
                                req.sharepoint_users.email.Contains(search) ||
                                req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                                ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.field.Equals("description"))
                        {
                            result = db.requests.Where(req => !req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(filter.value) &&
                                (req.sharepoint_users.name.Contains(search) ||
                                req.account.number.Contains(search) ||
                                req.room.room1.Contains(search) ||
                                req.sharepoint_users.email.Contains(search) ||
                                req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                                ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                    }







                    else if (filter.option.Equals("after"))
                    {
                        filter.date1 = Convert.ToDateTime(filterArray[2]);
                        result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date1 &&
                            (req.sharepoint_users.name.Contains(search) ||
                            req.account.number.Contains(search) ||
                            req.room.room1.Contains(search) ||
                            req.sharepoint_users.email.Contains(search) ||
                            req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                            req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                            ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }
                    else if (filter.option.Equals("before"))
                    {
                        filter.date1 = Convert.ToDateTime(filterArray[2]);
                        result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date1 &&
                            (req.sharepoint_users.name.Contains(search) ||
                            req.account.number.Contains(search) ||
                            req.room.room1.Contains(search) ||
                            req.sharepoint_users.email.Contains(search) ||
                            req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                            req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                            ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }
                    else if (filter.option.Equals("between"))
                    {
                        filter.date1 = Convert.ToDateTime(filterArray[2]);
                        filter.date2 = Convert.ToDateTime(filterArray[3]);
                        result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date1 && (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date2 &&
                            (req.sharepoint_users.name.Contains(search) ||
                            req.account.number.Contains(search) ||
                            req.room.room1.Contains(search) ||
                            req.sharepoint_users.email.Contains(search) ||
                            req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                            req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                            ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }
                    else if (filter.option.Equals("notbetween"))
                    {
                        filter.date1 = Convert.ToDateTime(filterArray[2]);
                        filter.date2 = Convert.ToDateTime(filterArray[3]);
                        result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date1 || (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date2 &&
                            (req.sharepoint_users.name.Contains(search) ||
                            req.account.number.Contains(search) ||
                            req.room.room1.Contains(search) ||
                            req.sharepoint_users.email.Contains(search) ||
                            req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                            req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                            ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }
                    
                }

                //just search
                else if (!String.IsNullOrWhiteSpace(search))
                {
                    //var stringProperties = typeof(request).GetProperties().Where(prop => prop.PropertyType == search.GetType());


                    result = db.requests.Where(req =>
                            (req.sharepoint_users.name.Contains(search) ||
                            req.account.number.Contains(search) ||
                            req.room.room1.Contains(search) ||
                            req.sharepoint_users.email.Contains(search) ||
                            req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(search) ||
                            req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search))
                            ).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                }

                //just filtering
                else if (filterString != null)
                {

                    var filterArray = filterString.Split(';');

                    Filter filter = new Filter();
                    filter.field = filterArray[0];
                    filter.option = filterArray[1];

                    if (filter.option.Equals("contain"))
                    {
                        filter.value = filterArray[2];
                        if (filter.field.Equals("supplier"))
                        {
                            result = db.requests.Where(req => req.request_suppliers.OrderBy(s => s.id).FirstOrDefault().supplier.name.Contains(filter.value)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();

                        }
                        else if (filter.field.Equals("accountNumber"))
                        {
                            result = db.requests.Where(req => req.account.number.Contains(filter.value)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.field.Equals("type"))
                        {
                            result = db.requests.Where(req => req.request_items.OrderBy(i => i.id).FirstOrDefault().description.type.Contains(filter.value)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.field.Equals("description"))
                        {
                            result = db.requests.Where(req => req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(filter.value)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                    }


                    else if (filter.option.Equals("notcontain"))
                    {
                        filter.value = filterArray[2];
                        if (filter.field.Equals("supplier"))
                        {
                            result = db.requests.Where(req => !req.request_suppliers.OrderBy(s => s.id).FirstOrDefault().supplier.name.Contains(filter.value)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();

                        }
                        else if (filter.field.Equals("accountNumber"))
                        {
                            result = db.requests.Where(req => !req.account.number.Contains(filter.value)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.field.Equals("type"))
                        {
                            result = db.requests.Where(req => !req.request_items.OrderBy(i => i.id).FirstOrDefault().description.type.Contains(filter.value)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.field.Equals("description"))
                        {
                            result = db.requests.Where(req => !req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(filter.value)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                    }


                    else if (filter.option.Equals("after"))
                    {
                        filter.date1 = Convert.ToDateTime(filterArray[2]);
                        result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date1).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }
                    else if (filter.option.Equals("before"))
                    {
                        filter.date1 = Convert.ToDateTime(filterArray[2]);
                        result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date1).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }
                    else if (filter.option.Equals("between"))
                    {
                        filter.date1 = Convert.ToDateTime(filterArray[2]);
                        filter.date2 = Convert.ToDateTime(filterArray[3]);
                        result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date1 && (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date2).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }
                    else if (filter.option.Equals("notbetween"))
                    {
                        filter.date1 = Convert.ToDateTime(filterArray[2]);
                        filter.date2 = Convert.ToDateTime(filterArray[3]);
                        result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date1 || (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date2).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }
                }
                
                //normal
                else{
                    result = db.requests.OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                }
                

                returnRequests.count =  db.requests.Count();

                foreach (var eRequest in result)
                {
                    try
                    {
                        Request returnRequest = new Request();
                        returnRequest.id = eRequest.id;
                        returnRequest.status = eRequest.request_status.LastOrDefault().status;
                        returnRequest.statusArray = new List<Status>();
                        foreach (var eStatus in eRequest.request_status)
                        {
                            Status status = new Status();
                            status.status = eStatus.status;
                            status.name = eStatus.sharepoint_users.name;
                            status.date = eStatus.date;
                            returnRequest.statusArray.Add(status);
                        }
                        returnRequest.date = eRequest.request_status.LastOrDefault().date;
                        returnRequest.name = eRequest.sharepoint_users.name;
                        returnRequest.phone = eRequest.sharepoint_users.phone;
                        returnRequest.email = eRequest.sharepoint_users.email;
                        returnRequest.type = eRequest.request_items.LastOrDefault().description.type;
                        returnRequest.destinationRoom = eRequest.room.room1;
                        returnRequest.itemDescription = eRequest.request_items.LastOrDefault().description.description1;
                        returnRequest.quality = eRequest.request_items.LastOrDefault().quality;
                        returnRequest.size = eRequest.request_items.LastOrDefault().size;
                        returnRequest.quantity = (int)eRequest.request_items.LastOrDefault().quantity;
                        returnRequest.vertere = (int)eRequest.request_items.LastOrDefault().vertere;
                        returnRequest.notes = eRequest.notes;
                        returnRequest.cas = eRequest.request_items.LastOrDefault().cas;
                        returnRequest.accountNumber = eRequest.account.number;
                        returnRequest.supplier = eRequest.request_suppliers.Count < 1 ? String.Empty : eRequest.request_suppliers.LastOrDefault().supplier.name;
                        returnRequest.dateSupplied = eRequest.request_status.Count < 1 ? DateTime.MinValue : eRequest.request_status.LastOrDefault(s => s.status == "Supplied") == null ? DateTime.MinValue : eRequest.request_status.LastOrDefault(s => s.status == "Supplied").date;
                        returnRequest.adminName = eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.name;
                        returnRequest.cost = (decimal)eRequest.request_payments.LastOrDefault().cost;
                        returnRequest.adminNotes = eRequest.request_admin_notes.Count < 1 ? String.Empty : eRequest.request_admin_notes.LastOrDefault().note;
                        returnRequest.analysisCode = eRequest.analysis_codes == null ? String.Empty : eRequest.analysis_codes.code;
                        returnRequest.permit = eRequest.request_permits.Count > 0;
                        returnRequest.permitNumber = eRequest.request_permits.Count < 1 ? String.Empty : eRequest.request_permits.LastOrDefault().number;
                        returnRequest.paymentType = eRequest.request_payments.LastOrDefault().type;
                        returnRequest.pnNumber = eRequest.request_payments.LastOrDefault().pnnumber;
                        returnRequest.invoiceDetails = eRequest.request_payments.LastOrDefault().invoice;

                        returnRequests.items.Add(returnRequest);
                    }

                    catch (Exception e)
                    {
                        
                    }
                }

            } 

            return returnRequests;
        }



        // GET: api/Request/5
        public Request Get(int id)
        {
            Request returnRequest = new Request();

            using (var db = new EModelsContext())
            {

                request eRequest = db.requests.Where(req => req.id.Equals(id)).Single();

                
                returnRequest.id = eRequest.id;
                returnRequest.status = eRequest.request_status.LastOrDefault().status;
                returnRequest.statusArray = new List<Status>();
                foreach (var eStatus in eRequest.request_status)
                {
                    Status status = new Status();
                    status.status = eStatus.status;
                    status.name = eStatus.sharepoint_users.name;
                    status.date = eStatus.date;
                    returnRequest.statusArray.Add(status);
                }
                returnRequest.date = eRequest.request_status.LastOrDefault().date;
                returnRequest.name = eRequest.sharepoint_users.name;
                returnRequest.phone = eRequest.sharepoint_users.phone;
                returnRequest.email = eRequest.sharepoint_users.email;
                returnRequest.type = eRequest.request_items.LastOrDefault().description.type;
                returnRequest.destinationRoom = eRequest.room.room1;
                returnRequest.itemDescription = eRequest.request_items.LastOrDefault().description.description1;
                returnRequest.quality = eRequest.request_items.LastOrDefault().quality;
                returnRequest.size = eRequest.request_items.LastOrDefault().size;
                returnRequest.quantity = (int)eRequest.request_items.LastOrDefault().quantity;
                returnRequest.vertere = (int)eRequest.request_items.LastOrDefault().vertere;
                returnRequest.notes = eRequest.notes;
                returnRequest.cas = eRequest.request_items.LastOrDefault().cas;
                returnRequest.accountNumber = eRequest.account.number;
                returnRequest.supplier = eRequest.request_suppliers.Count < 1 ? String.Empty : eRequest.request_suppliers.LastOrDefault().supplier.name;
                returnRequest.dateSupplied = eRequest.request_status.Count < 1 ? DateTime.MinValue : eRequest.request_status.LastOrDefault(s => s.status == "Supplied") == null ? DateTime.MinValue : eRequest.request_status.LastOrDefault(s => s.status == "Supplied").date;
                returnRequest.adminName = eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.name;
                returnRequest.cost = (decimal)eRequest.request_payments.LastOrDefault().cost;
                returnRequest.adminNotes = eRequest.request_admin_notes.Count < 1 ? String.Empty : eRequest.request_admin_notes.LastOrDefault().note;
                returnRequest.analysisCode = eRequest.analysis_codes == null ? String.Empty : eRequest.analysis_codes.code;
                returnRequest.permit = eRequest.request_permits.Count > 0;
                returnRequest.permitNumber = eRequest.request_permits.Count < 1 ? String.Empty : eRequest.request_permits.LastOrDefault().number;
                returnRequest.paymentType = eRequest.request_payments.LastOrDefault().type;
                returnRequest.pnNumber = eRequest.request_payments.LastOrDefault().pnnumber;
                returnRequest.invoiceDetails = eRequest.request_payments.LastOrDefault().invoice;

            }

            

            return returnRequest;


        }


        // POST: api/Request
        public void Post([FromBody]Request data)
        {
        
            //throw new Exception(data.ToString());
            using (var db = new EModelsContext())
            {

                //add main request data
                request request = new request{
                    notes = data.notes};

                //foreign keys 1:1

                //account
                request.account = new account();
                request.account.number = data.accountNumber;
                

                //analysis code
                if (!String.IsNullOrWhiteSpace(data.analysisCode))
                {
                    request.analysis_codes = new analysis_codes();
                    request.analysis_codes.code = data.analysisCode;
                }

                //user data
                request.sharepoint_users = new sharepoint_users();
                request.sharepoint_users.name = data.name;
                request.sharepoint_users.email = data.email;
                request.sharepoint_users.phone = data.phone;

                //room
                request.room = new room();
                request.room.room1 = data.destinationRoom;


                //foreign keys 1:*

                //admin notes
                if (!String.IsNullOrWhiteSpace(data.analysisCode))
                {
                    request.request_admin_notes.Add(new request_admin_notes
                    {
                        note = data.adminNotes

                    });
                }

                //request items
                request_items request_item = new request_items();
                request_item.cas = data.cas;
                request_item.quality = data.quality;
                request_item.quantity = data.quantity;
                request_item.size = data.size;
                request_item.vertere = data.vertere;

                request_item.description = new description();
                request_item.description.description1 = data.itemDescription;
                request_item.description.type = data.type;

                request.request_items.Add(request_item);

                //payments
                request.request_payments.Add(new request_payments
                    {
                        type = data.paymentType,
                        pnnumber = data.pnNumber,
                        invoice = data.invoiceDetails,
                        cost = data.cost
                    });

                //permits
                request.request_permits.Add(new request_permits
                    {
                        number = data.permitNumber
                    });
                

                //status
                string statusName = data.name;
                if (!data.status.Equals("Requested"))
                {
                    statusName = data.adminName;
                }

                request_status request_status = new request_status();
                request_status.status = data.status;
                request_status.date = DateTime.Now;
                request_status.sharepoint_users = new sharepoint_users();
                request_status.sharepoint_users.name = statusName;

                request.request_status.Add(request_status);

                //supplier
                request_suppliers request_supplier = new request_suppliers();
                request_supplier.supplier = new supplier();
                request_supplier.supplier.name = data.supplier;

                request.request_suppliers.Add(request_supplier);


                //add and save changes
                db.requests.Add(request);

                db.SaveChanges();



            }

            


        }

        // PUT: api/Request/5
        public void Put(int id, [FromBody]Request data)
        {
            




        }

        // DELETE: api/Request/5
        public void Delete(int id)
        {
            using (var db = new EModelsContext())
            {
                var eRequest = db.requests.Where(req => req.id.Equals(id)).SingleOrDefault();
                if (eRequest != null)
                {
                    db.requests.Remove(eRequest);
                    db.SaveChanges();
                }
            }
        }





    }
}
