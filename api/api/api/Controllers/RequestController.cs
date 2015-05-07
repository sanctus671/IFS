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
//using api.DataAccess;

namespace api.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class RequestController : ApiController
    {
        // GET: api/Request
        public RequestPaginate Get(int offset = 0, int limit = 1000, string search = null, string filterString = null, string userCode = null)
        {
            RequestPaginate returnRequests = new RequestPaginate();



            using (var db = new EModelsContext()){


                if (String.IsNullOrWhiteSpace(userCode)){
                    return returnRequests;
                }
                

                
                //data retrieved via sharepoint:
                var currentUser = new User //TODO: RETRIEVE EXTERNALLY
                {
                    name = "John Doe",
                    email = "doe.j@massey.ac.nz",
                    phone = "0222342342",
                    userCode = "2",
                    permission = "systemadmin", //angular app permission
                    permissionid = 2, //request api permission (1 = standard, 2 = manager, 3,4 = admin)
                    groupid = null, //the last 3 properties only set if the user is a manager
                    group = null, //group name
                    groupUsers = new List<string> {"1", "2"}//list of strings with all user codes contained in their group

                };


                returnRequests.user = currentUser; //return the user while retrieving requests

                



                var result = new List<request>();


                /*ADMIN USER GET REQUESTS */
                if (currentUser.permissionid == 4 || currentUser.permissionid == 3)
                {


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
                    else
                    {
                        result = db.requests.OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }

                }

                /*END ADMIN USER GET REQUESTS */


                /* START MANAGER GET REQUESTS*/
                if (currentUser.permissionid == 2)
                {
                    List<string> userCodes = new List<string>();
                    if (currentUser.groupUsers != null)
                    {
                        userCodes = currentUser.groupUsers;
                    }
                    userCodes.Add(currentUser.userCode);

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
                                    && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                    && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                    && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                    && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                    && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                    && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                    && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                    && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                && userCodes.Contains(req.sharepoint_users.user_code.ToString())
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
                                result = db.requests.Where(req => req.request_suppliers.OrderBy(s => s.id).FirstOrDefault().supplier.name.Contains(filter.value) && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();

                            }
                            else if (filter.field.Equals("accountNumber"))
                            {
                                result = db.requests.Where(req => req.account.number.Contains(filter.value) && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                            else if (filter.field.Equals("type"))
                            {
                                result = db.requests.Where(req => req.request_items.OrderBy(i => i.id).FirstOrDefault().description.type.Contains(filter.value) && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                            else if (filter.field.Equals("description"))
                            {
                                result = db.requests.Where(req => req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(filter.value) && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                        }


                        else if (filter.option.Equals("notcontain"))
                        {
                            filter.value = filterArray[2];
                            if (filter.field.Equals("supplier"))
                            {
                                result = db.requests.Where(req => !req.request_suppliers.OrderBy(s => s.id).FirstOrDefault().supplier.name.Contains(filter.value) && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();

                            }
                            else if (filter.field.Equals("accountNumber"))
                            {
                                result = db.requests.Where(req => !req.account.number.Contains(filter.value) && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                            else if (filter.field.Equals("type"))
                            {
                                result = db.requests.Where(req => !req.request_items.OrderBy(i => i.id).FirstOrDefault().description.type.Contains(filter.value) && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                            else if (filter.field.Equals("description"))
                            {
                                result = db.requests.Where(req => !req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(filter.value) && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                        }


                        else if (filter.option.Equals("after"))
                        {
                            filter.date1 = Convert.ToDateTime(filterArray[2]);
                            result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date1 && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.option.Equals("before"))
                        {
                            filter.date1 = Convert.ToDateTime(filterArray[2]);
                            result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date1 && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.option.Equals("between"))
                        {
                            filter.date1 = Convert.ToDateTime(filterArray[2]);
                            filter.date2 = Convert.ToDateTime(filterArray[3]);
                            result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date1 && (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date2 && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.option.Equals("notbetween"))
                        {
                            filter.date1 = Convert.ToDateTime(filterArray[2]);
                            filter.date2 = Convert.ToDateTime(filterArray[3]);
                            result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date1 || (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date2 && userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                    }

                    //normal
                    else
                    {
                        result = db.requests.Where(req => userCodes.Contains(req.sharepoint_users.user_code.ToString())).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }

                }

                /*END MANAGER USER GET REQUESTS */

                /*START STANDARD USER GET REQUESTS */


                else if (currentUser.permissionid == 1)
                {


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
                                    req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                    && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                    req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                    && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                    req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                    && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                    req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                    && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                    req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                    && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                    req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                    && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                    req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                    && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                    req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                    && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                req.request_status.OrderBy(t => t.date).FirstOrDefault().status.Contains(search)
                                && req.sharepoint_users.user_code.Equals(currentUser.userCode))
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
                                result = db.requests.Where(req => req.request_suppliers.OrderBy(s => s.id).FirstOrDefault().supplier.name.Contains(filter.value) && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();

                            }
                            else if (filter.field.Equals("accountNumber"))
                            {
                                result = db.requests.Where(req => req.account.number.Contains(filter.value) && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                            else if (filter.field.Equals("type"))
                            {
                                result = db.requests.Where(req => req.request_items.OrderBy(i => i.id).FirstOrDefault().description.type.Contains(filter.value) && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                            else if (filter.field.Equals("description"))
                            {
                                result = db.requests.Where(req => req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(filter.value) && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                        }


                        else if (filter.option.Equals("notcontain"))
                        {
                            filter.value = filterArray[2];
                            if (filter.field.Equals("supplier"))
                            {
                                result = db.requests.Where(req => !req.request_suppliers.OrderBy(s => s.id).FirstOrDefault().supplier.name.Contains(filter.value) && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();

                            }
                            else if (filter.field.Equals("accountNumber"))
                            {
                                result = db.requests.Where(req => !req.account.number.Contains(filter.value) && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                            else if (filter.field.Equals("type"))
                            {
                                result = db.requests.Where(req => !req.request_items.OrderBy(i => i.id).FirstOrDefault().description.type.Contains(filter.value) && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                            else if (filter.field.Equals("description"))
                            {
                                result = db.requests.Where(req => !req.request_items.OrderBy(i => i.id).FirstOrDefault().description.description1.Contains(filter.value) && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                            }
                        }


                        else if (filter.option.Equals("after"))
                        {
                            filter.date1 = Convert.ToDateTime(filterArray[2]);
                            result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date1 && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.option.Equals("before"))
                        {
                            filter.date1 = Convert.ToDateTime(filterArray[2]);
                            result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date1 && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.option.Equals("between"))
                        {
                            filter.date1 = Convert.ToDateTime(filterArray[2]);
                            filter.date2 = Convert.ToDateTime(filterArray[3]);
                            result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date1 && (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date2 && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                        else if (filter.option.Equals("notbetween"))
                        {
                            filter.date1 = Convert.ToDateTime(filterArray[2]);
                            filter.date2 = Convert.ToDateTime(filterArray[3]);
                            result = db.requests.Where(req => (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date < filter.date1 || (DateTime)req.request_status.OrderBy(i => i.id).FirstOrDefault().date > filter.date2 && req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                        }
                    }

                    //normal
                    else
                    {
                        result = db.requests.Where(req => req.sharepoint_users.user_code.Equals(currentUser.userCode)).OrderByDescending(req => req.id).Skip(offset).Take(limit).ToList();
                    }

                }

                /*END STANDARD USER GET REQUESTS */


                returnRequests.count = result.Count();

                foreach (var eRequest in result)
                {
                    
                    
                        Request returnRequest = new Request();
                        returnRequest.id = eRequest.id;
                        returnRequest.status = eRequest.request_status.LastOrDefault() == null ? String.Empty : eRequest.request_status.LastOrDefault().status;
                        returnRequest.statusArray = new List<Status>();
                        foreach (var eStatus in eRequest.request_status)
                        {
                            Status status = new Status();
                            status.status = eStatus.status;
                            status.name = eStatus.sharepoint_users.name;
                            status.date = eStatus.date;
                            returnRequest.statusArray.Add(status);
                        }
                        returnRequest.date = eRequest.request_status.LastOrDefault() == null ? DateTime.MinValue : eRequest.request_status.LastOrDefault().date;
                        returnRequest.name = eRequest.sharepoint_users.name == null ? String.Empty : eRequest.sharepoint_users.name;
                        returnRequest.phone = eRequest.sharepoint_users.phone == null ? String.Empty : eRequest.sharepoint_users.phone ;
                        returnRequest.email = eRequest.sharepoint_users.email == null ? String.Empty : eRequest.sharepoint_users.email;
                        returnRequest.userCode = eRequest.sharepoint_users.user_code == null ? String.Empty : eRequest.sharepoint_users.user_code;
                        returnRequest.type = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().description.type;
                        returnRequest.destinationRoom = eRequest.room == null ? String.Empty : eRequest.room.room1;
                        returnRequest.itemDescription = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().description.description1;
                        returnRequest.quality = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().quality;
                        returnRequest.size = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().size;
                        returnRequest.quantity = eRequest.request_items.LastOrDefault() == null ? 0 : (int)eRequest.request_items.LastOrDefault().quantity;
                        returnRequest.vertere = eRequest.request_items.LastOrDefault() == null ? 0 : (int)eRequest.request_items.LastOrDefault().vertere;
                        returnRequest.notes = eRequest.notes;
                        returnRequest.cas = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().cas;
                        returnRequest.accountNumber = eRequest.account == null ? String.Empty : eRequest.account.number;
                        returnRequest.supplier = eRequest.request_suppliers.Count < 1 ? String.Empty : eRequest.request_suppliers.LastOrDefault().supplier.name;
                        returnRequest.dateSupplied = eRequest.request_status.Count < 1 ? DateTime.MinValue : eRequest.request_status.LastOrDefault(s => s.status == "Supplied") == null ? DateTime.MinValue : eRequest.request_status.LastOrDefault(s => s.status == "Supplied").date;
                        returnRequest.adminName = eRequest.request_status.LastOrDefault(s => s.status != "Requested") == null ? String.Empty : eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.name == null ? String.Empty : eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.name;
                        returnRequest.adminUserCode = eRequest.request_status.LastOrDefault(s => s.status != "Requested") == null ? String.Empty : eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.user_code == null ? String.Empty : eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.user_code;
                        returnRequest.cost = eRequest.request_payments.LastOrDefault() == null ? 0 : (decimal)eRequest.request_payments.LastOrDefault().cost;
                        returnRequest.adminNotes = eRequest.request_admin_notes.Count < 1 ? String.Empty : eRequest.request_admin_notes.LastOrDefault().note;
                        returnRequest.analysisCode = eRequest.analysis_codes == null ? String.Empty : eRequest.analysis_codes.code;
                        returnRequest.permit = eRequest.request_permits.Count > 0;
                        returnRequest.permitNumber = eRequest.request_permits.Count < 1 ? String.Empty : eRequest.request_permits.LastOrDefault().number;
                        returnRequest.paymentType = eRequest.request_payments.LastOrDefault() == null ? String.Empty : eRequest.request_payments.LastOrDefault().type;
                        returnRequest.pnNumber = eRequest.request_payments.LastOrDefault() == null ? String.Empty : eRequest.request_payments.LastOrDefault().pnnumber;
                        returnRequest.invoiceDetails = eRequest.request_payments.LastOrDefault() == null ? String.Empty : eRequest.request_payments.LastOrDefault().invoice;

                        returnRequests.items.Add(returnRequest);
                    

                    
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
                returnRequest.status = eRequest.request_status.LastOrDefault() == null ? String.Empty : eRequest.request_status.LastOrDefault().status;
                returnRequest.statusArray = new List<Status>();
                foreach (var eStatus in eRequest.request_status)
                {
                    Status status = new Status();
                    status.status = eStatus.status;
                    status.name = eStatus.sharepoint_users.name;
                    status.date = eStatus.date;
                    returnRequest.statusArray.Add(status);
                }
                returnRequest.date = eRequest.request_status.LastOrDefault() == null ? DateTime.MinValue : eRequest.request_status.LastOrDefault().date;
                returnRequest.name = eRequest.sharepoint_users.name == null ? String.Empty : eRequest.sharepoint_users.name;
                returnRequest.phone = eRequest.sharepoint_users.phone == null ? String.Empty : eRequest.sharepoint_users.phone;
                returnRequest.email = eRequest.sharepoint_users.email == null ? String.Empty : eRequest.sharepoint_users.email;
                returnRequest.userCode = eRequest.sharepoint_users.user_code == null ? String.Empty : eRequest.sharepoint_users.user_code;
                returnRequest.type = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().description.type;
                returnRequest.destinationRoom = eRequest.room == null ? String.Empty : eRequest.room.room1;
                returnRequest.itemDescription = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().description.description1;
                returnRequest.quality = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().quality;
                returnRequest.size = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().size;
                returnRequest.quantity = eRequest.request_items.LastOrDefault() == null ? 0 : (int)eRequest.request_items.LastOrDefault().quantity;
                returnRequest.vertere = eRequest.request_items.LastOrDefault() == null ? 0 : (int)eRequest.request_items.LastOrDefault().vertere;
                returnRequest.notes = eRequest.notes;
                returnRequest.cas = eRequest.request_items.LastOrDefault() == null ? String.Empty : eRequest.request_items.LastOrDefault().cas;
                returnRequest.accountNumber = eRequest.account == null ? String.Empty : eRequest.account.number;
                returnRequest.supplier = eRequest.request_suppliers.Count < 1 ? String.Empty : eRequest.request_suppliers.LastOrDefault().supplier.name;
                returnRequest.dateSupplied = eRequest.request_status.Count < 1 ? DateTime.MinValue : eRequest.request_status.LastOrDefault(s => s.status == "Supplied") == null ? DateTime.MinValue : eRequest.request_status.LastOrDefault(s => s.status == "Supplied").date;
                returnRequest.adminName = eRequest.request_status.LastOrDefault(s => s.status != "Requested") == null ? String.Empty : eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.name == null ? String.Empty : eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.name;
                returnRequest.adminUserCode = eRequest.request_status.LastOrDefault(s => s.status != "Requested") == null ? String.Empty : eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.user_code == null ? String.Empty : eRequest.request_status.LastOrDefault(s => s.status != "Requested").sharepoint_users.user_code;
                returnRequest.cost = eRequest.request_payments.LastOrDefault() == null ? 0 : (decimal)eRequest.request_payments.LastOrDefault().cost;
                returnRequest.adminNotes = eRequest.request_admin_notes.Count < 1 ? String.Empty : eRequest.request_admin_notes.LastOrDefault().note;
                returnRequest.analysisCode = eRequest.analysis_codes == null ? String.Empty : eRequest.analysis_codes.code;
                returnRequest.permit = eRequest.request_permits.Count > 0;
                returnRequest.permitNumber = eRequest.request_permits.Count < 1 ? String.Empty : eRequest.request_permits.LastOrDefault().number;
                returnRequest.paymentType = eRequest.request_payments.LastOrDefault() == null ? String.Empty : eRequest.request_payments.LastOrDefault().type;
                returnRequest.pnNumber = eRequest.request_payments.LastOrDefault() == null ? String.Empty : eRequest.request_payments.LastOrDefault().pnnumber;
                returnRequest.invoiceDetails = eRequest.request_payments.LastOrDefault() == null ? String.Empty : eRequest.request_payments.LastOrDefault().invoice;

            }

            

            return returnRequest;


        }


        // POST: api/Request
        public void Post([FromBody]Request data)
        {
        
            //throw new Exception(data.ToString());
            using (var db = new EModelsContext())
            {
                int accountid = 0;
                try
                {
                    accountid = db.accounts.Where(acc => acc.number.Equals(data.accountNumber)).SingleOrDefault().id;
                }
                catch
                {
                    //no account found
                }
                if (accountid == 0 && !String.IsNullOrWhiteSpace(data.accountNumber)){
                    account insertAccount = new account{
                        number = data.accountNumber
                    };
                    db.accounts.Add(insertAccount);
                    db.SaveChanges();

                    accountid = insertAccount.id;
                }


                int codeid = 0;
                try
                {
                    codeid = db.analysis_codes.Where(code => code.code.Equals(data.analysisCode)).SingleOrDefault().id;
                }
                catch
                {
                    //no code found
                }
                if (codeid == 0 && !String.IsNullOrWhiteSpace(data.analysisCode))
                {
                    analysis_codes insertCode = new analysis_codes{
                        code = data.analysisCode
                    };
                    db.analysis_codes.Add(insertCode);
                    db.SaveChanges();

                    codeid = insertCode.id;
                }

                int roomid = 0;
                try
                {
                    roomid = db.rooms.Where(room => room.room1.Equals(data.destinationRoom)).SingleOrDefault().id;
                }
                catch
                {
                    //no room found
                }
                if (roomid == 0 && !String.IsNullOrWhiteSpace(data.destinationRoom))
                {
                    room insertRoom = new room{
                        room1 = data.destinationRoom
                    };
                    db.rooms.Add(insertRoom);
                    db.SaveChanges();

                    roomid = insertRoom.id;
                }


                int userid = 0;
                try
                {
                    userid = db.sharepoint_users.Where(user => (user.name.Equals(data.name) && user.email.Equals(data.email) && user.phone.Equals(data.phone)) || user.user_code.Equals(data.userCode)).SingleOrDefault().id;
                }
                catch
                {
                    //no user found
                }
                if (userid == 0)
                {
                    sharepoint_users insertUser = new sharepoint_users{
                        name = data.name,
                        phone = data.phone,
                        email = data.email,
                        user_code = data.userCode,
                        permissionid = 1, //standard
                        groupid = 1 //default
                    };
                    db.sharepoint_users.Add(insertUser);
                    db.SaveChanges();

                    userid = insertUser.id;
                }


                //add main request data

                request request = new request{
                    notes = data.notes,
                    accountid = accountid,
                    codeid = codeid,
                    roomid = roomid,
                    userid = userid

                };

                db.requests.Add(request);
                db.SaveChanges();

                int requestid = request.id;


                //if there is data for all required fields, create new foreign item with requestid
                //foreign keys 1:*

                //admin notes
                if (!String.IsNullOrWhiteSpace(data.adminNotes))
                {
                    request_admin_notes request_admin_note = new request_admin_notes
                    {
                        requestid = requestid,
                        note = data.adminNotes

                    };
                    db.request_admin_notes.Add(request_admin_note);
                    db.SaveChanges();

                }

                //add description if it doesn't already exist
                int descriptionid = 0;
                try
                {
                    descriptionid = db.descriptions.Where(des => des.description1.Equals(data.itemDescription) && des.type.Equals(data.type)).SingleOrDefault().id;
                }
                catch
                {
                    //no description found
                }


                if (descriptionid == 0 && !String.IsNullOrWhiteSpace(data.itemDescription) && !String.IsNullOrWhiteSpace(data.type))
                {
                    description insertDescription = new description
                    {
                        description1 = data.itemDescription,
                        type = data.type

                    };
                    db.descriptions.Add(insertDescription);
                    db.SaveChanges();

                    descriptionid = insertDescription.id;
                }


                //request items
                if (descriptionid != 0)
                {
                    request_items request_item = new request_items
                    {
                        requestid = requestid,
                        descriptionid = descriptionid,
                        cas = data.cas,
                        quality = data.quality,
                        quantity = data.quantity,
                        size = data.size,
                        vertere = data.vertere
                    };

                    db.request_items.Add(request_item);
                    db.SaveChanges();
                }


                //payments
                if (String.IsNullOrWhiteSpace(data.paymentType))
                {
                    data.paymentType = "Internal";
                }
                request_payments request_payment = new request_payments
                    {
                        requestid = requestid,
                        type = data.paymentType,
                        pnnumber = data.pnNumber,
                        invoice = data.invoiceDetails,
                        cost = data.cost
                    };
                db.request_payments.Add(request_payment);
                db.SaveChanges();


                //permits
                if (!String.IsNullOrWhiteSpace(data.permitNumber))
                {
                    request_permits request_permit = new request_permits
                        {
                            number = data.permitNumber
                        };
                    db.request_permits.Add(request_permit);
                    db.SaveChanges();
                }
                

                //status
                if (!data.status.Equals("Requested"))
                {
                    userid = 0;
                    try
                    {
                        userid = db.sharepoint_users.Where(user => user.name.Equals(data.adminName) || user.user_code.Equals(data.adminUserCode)).SingleOrDefault().id;
                    }
                    catch
                    {
                        //no user found
                    }
                    if (userid == 0)
                    {
                        sharepoint_users insertAdminUser = new sharepoint_users
                        {
                            name = data.name,
                            permissionid = 1, //standard
                            groupid = 1, //default
                            user_code = data.userCode
                        };
                        db.sharepoint_users.Add(insertAdminUser);
                        db.SaveChanges();

                        userid = insertAdminUser.id;
                    }
                }

                if (!String.IsNullOrWhiteSpace(data.status))
                {
                    request_status request_status = new request_status
                    {

                    requestid = requestid,
                    status = data.status,
                    date = DateTime.Now,
                    userid = userid
                };

                    db.request_status.Add(request_status);
                    db.SaveChanges();
                }


                //supplier
                if (!String.IsNullOrWhiteSpace(data.supplier))
                {
                    int supplierid = 0;
                    try
                    {
                        supplierid = db.suppliers.Where(sup => sup.name.Equals(data.supplier)).SingleOrDefault().id;
                    }
                    catch
                    {
                        //no supplier found
                    }
                    if (supplierid == 0)
                    {
                        supplier insertSupplier = new supplier
                        {
                            name = data.supplier,

                        };
                        db.suppliers.Add(insertSupplier);
                        db.SaveChanges();

                        supplierid = insertSupplier.id;
                    }
                    request_suppliers request_supplier = new request_suppliers
                    {
                        supplierid = supplierid,
                        requestid = requestid
                    };

                    db.request_suppliers.Add(request_supplier);
                    db.SaveChanges();
                }



            }

            


        }

        // PUT: api/Request/5
        public void Put(int id, [FromBody]Request data)
        {

            //throw new Exception(data.ToString());
            using (var db = new EModelsContext())
            {
                int accountid = 0;
                try
                {
                    accountid = db.accounts.Where(acc => acc.number.Equals(data.accountNumber)).SingleOrDefault().id;
                }
                catch
                {
                    //no account found
                }
                if (accountid == 0 && !String.IsNullOrWhiteSpace(data.accountNumber))
                {
                    account insertAccount = new account
                    {
                        number = data.accountNumber
                    };
                    db.accounts.Add(insertAccount);
                    db.SaveChanges();

                    accountid = insertAccount.id;
                }


                int codeid = 0;
                try
                {
                    codeid = db.analysis_codes.Where(code => code.code.Equals(data.analysisCode)).SingleOrDefault().id;
                }
                catch
                {
                    //no code found
                }
                if (codeid == 0 && !String.IsNullOrWhiteSpace(data.analysisCode))
                {
                    analysis_codes insertCode = new analysis_codes
                    {
                        code = data.analysisCode
                    };
                    db.analysis_codes.Add(insertCode);
                    db.SaveChanges();

                    codeid = insertCode.id;
                }

                int roomid = 0;
                try
                {
                    roomid = db.rooms.Where(room => room.room1.Equals(data.destinationRoom)).SingleOrDefault().id;
                }
                catch
                {
                    //no room found
                }
                if (roomid == 0 && !String.IsNullOrWhiteSpace(data.destinationRoom))
                {
                    room insertRoom = new room
                    {
                        room1 = data.destinationRoom
                    };
                    db.rooms.Add(insertRoom);
                    db.SaveChanges();

                    roomid = insertRoom.id;
                }


                int userid = 0;
                try
                {
                    userid = db.sharepoint_users.Where(user => user.name.Equals(data.name) && user.email.Equals(data.email) && user.phone.Equals(data.phone) || user.user_code.Equals(data.userCode)).SingleOrDefault().id;
                }
                catch
                {
                    //no user found
                }
                if (userid == 0)
                {
                    sharepoint_users insertUser = new sharepoint_users
                    {
                        name = data.name,
                        phone = data.phone,
                        email = data.email,
                        user_code = data.userCode,
                        permissionid = 1, //standard
                        groupid = 1 //default
                    };
                    db.sharepoint_users.Add(insertUser);
                    db.SaveChanges();

                    userid = insertUser.id;
                }


                //add main request data
                var eRequest = db.requests.Where(req => req.id.Equals(id)).SingleOrDefault();

                if (eRequest != null)
                {

                    eRequest.notes = data.notes;
                    eRequest.accountid = accountid;
                    eRequest.codeid = codeid;
                    eRequest.roomid = roomid;
                    eRequest.userid = userid;

                };

                db.SaveChanges();

                int requestid = eRequest.id;


                //if there is data for all required fields, create new foreign item with requestid
                //foreign keys 1:*

                //admin notes
                var eAdminNotes = db.request_admin_notes.Where(an => an.requestid.Equals(requestid)).SingleOrDefault();
                if (!String.IsNullOrWhiteSpace(data.adminNotes))
                {
                    if (eAdminNotes != null)
                    {
                        eAdminNotes.note = data.adminNotes;

                    }
                    else
                    {

                        request_admin_notes request_admin_note = new request_admin_notes
                        {
                            requestid = requestid,
                            note = data.adminNotes

                        };
                        db.request_admin_notes.Add(request_admin_note);
                    }

                    db.SaveChanges();

                }

                //add description if it doesn't already exist
                int descriptionid = 0;
                try
                {
                    descriptionid = db.descriptions.Where(des => des.description1.Equals(data.itemDescription) && des.type.Equals(data.type)).SingleOrDefault().id;
                }
                catch
                {
                    //no description found
                }


                if (descriptionid == 0 && !String.IsNullOrWhiteSpace(data.itemDescription) && !String.IsNullOrWhiteSpace(data.type))
                {
                    description insertDescription = new description
                    {
                        description1 = data.itemDescription,
                        type = data.type

                    };
                    db.descriptions.Add(insertDescription);
                    db.SaveChanges();

                    descriptionid = insertDescription.id;
                }


                //request items
                var eRequestItem = db.request_items.Where(ri => ri.requestid.Equals(requestid)).SingleOrDefault();
                if (descriptionid != 0)
                {
                    if (eRequestItem != null)
                    {
                        eRequestItem.descriptionid = descriptionid;
                        eRequestItem.cas = data.cas;
                        eRequestItem.quality = data.quality;
                        eRequestItem.quantity = data.quantity;
                        eRequestItem.size = data.size;
                        eRequestItem.vertere = data.vertere;
                    }
                    else
                    {
                        request_items request_item = new request_items
                        {
                            requestid = requestid,
                            descriptionid = descriptionid,
                            cas = data.cas,
                            quality = data.quality,
                            quantity = data.quantity,
                            size = data.size,
                            vertere = data.vertere
                        };

                        db.request_items.Add(request_item);
                    }
                    db.SaveChanges();
                }


                //payments
                var ePayment = db.request_payments.Where(rp => rp.requestid.Equals(requestid)).SingleOrDefault();
                if (String.IsNullOrWhiteSpace(data.paymentType))
                {
                    data.paymentType = "Internal";
                }
                if (ePayment != null)
                {
                    ePayment.requestid = requestid;
                    ePayment.type = data.paymentType;
                    ePayment.pnnumber = data.pnNumber;
                    ePayment.invoice = data.invoiceDetails;
                    ePayment.cost = data.cost;
                }
                else
                {
                    request_payments request_payment = new request_payments
                    {
                        requestid = requestid,
                        type = data.paymentType,
                        pnnumber = data.pnNumber,
                        invoice = data.invoiceDetails,
                        cost = data.cost
                    };
                    db.request_payments.Add(request_payment);
                }
                db.SaveChanges();


                //permits
                var ePermit = db.request_permits.Where(rpe => rpe.requestid.Equals(requestid)).SingleOrDefault();
                if (!String.IsNullOrWhiteSpace(data.permitNumber))
                {
                    if (ePermit != null)
                    {
                        ePermit.number = data.permitNumber;
                    }
                    else
                    {
                        request_permits request_permit = new request_permits
                        {
                            number = data.permitNumber
                        };
                        db.request_permits.Add(request_permit);
                    }
                    db.SaveChanges();
                }


                //status
                var eStatus = db.request_status.Where(rpe => rpe.requestid.Equals(requestid)).OrderByDescending(rpe => rpe.date).FirstOrDefault();
                if (!data.status.Equals("Requested"))
                {
                    userid = 0;
                    try
                    {
                        userid = db.sharepoint_users.Where(user => user.name.Equals(data.adminName) || user.user_code.Equals(data.adminUserCode)).SingleOrDefault().id;
                    }
                    catch
                    {
                        //no user found
                    }
                    if (userid == 0)
                    {
                        sharepoint_users insertAdminUser = new sharepoint_users
                        {
                            name = data.name,
                            user_code = data.userCode,
                            permissionid = 3, //admin
                            groupid = 1 //default
                        };
                        db.sharepoint_users.Add(insertAdminUser);
                        db.SaveChanges();

                        userid = insertAdminUser.id;
                    }
                }

                if (!String.IsNullOrWhiteSpace(data.status))
                {
                    if (!eStatus.status.Equals(data.status))
                    {
                        request_status request_status = new request_status
                        {

                            requestid = requestid,
                            status = data.status,
                            date = DateTime.Now,
                            userid = userid
                        };

                        db.request_status.Add(request_status);
                    }
                    db.SaveChanges();
                }


                //supplier
                var eSupplier = db.request_suppliers.Where(rs => rs.requestid.Equals(requestid)).SingleOrDefault();
                if (!String.IsNullOrWhiteSpace(data.supplier))
                {
                    int supplierid = 0;
                    try
                    {
                        supplierid = db.suppliers.Where(sup => sup.name.Equals(data.supplier)).SingleOrDefault().id;
                    }
                    catch
                    {
                        //no supplier found
                    }
                    if (supplierid == 0)
                    {
                        supplier insertSupplier = new supplier
                        {
                            name = data.supplier,

                        };
                        db.suppliers.Add(insertSupplier);
                        db.SaveChanges();

                        supplierid = insertSupplier.id;
                    }

                    if (eSupplier != null)
                    {
                        eSupplier.supplierid = supplierid;
                    }
                    else
                    {

                        request_suppliers request_supplier = new request_suppliers
                        {
                            supplierid = supplierid,
                            requestid = requestid
                        };

                        db.request_suppliers.Add(request_supplier);
                        
                    }
                    db.SaveChanges();
                }



            }



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
