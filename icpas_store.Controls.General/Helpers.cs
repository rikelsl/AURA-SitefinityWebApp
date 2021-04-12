using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using icpas_store.Models;
using RestSharp;

namespace icpas_store.Controls.General
{
    public static class Helpers
    {
        public static string IfNull(string str, string alt)
        {
            return string.IsNullOrEmpty(str) ? alt : str;
        }

        public static string TrimOrNull(string input)
        {
            return input != null ? input.Trim() : string.Empty;
        }

        public static string GetAuraId(string guid)
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["BypassLogin"]) ? "7a924c75-b3b5-492b-ad2d-4a645d3ac7ee" : guid;
        }

        public static string GetPersonId(string id)
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["BypassLogin"]) ? "107962" : id;
        }

        public static string SetDebugQueryString(string live, string debug)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["Debug"]) && string.IsNullOrEmpty(live))
                return debug;
            return live;
        }

        public static bool IsRegistered(string personId, string meetingId, bool onlyUseUpcoming = false)
        {
            int meeting = -1;
            int.TryParse(meetingId, out meeting);
            var ids = GetRegisteredMeetings(personId, onlyUseUpcoming);
            return ids.Contains(meeting);
        }

        public static List<int> GetRegisteredMeetings(string personId, bool onlyUseUpcoming = true)
        {
            var endpoint = "/icpas/api/MeetingDetailEx/GetPersonUpcomingMeetingAttendance?personId=";
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + endpoint + personId;
            var ids = GetRegisteredMeetingHelper(apiUrl);
            if (!onlyUseUpcoming)
            {
                endpoint = "/icpas/api/MeetingDetailEx/GetPersonPastMeetingAttendance?personId=";
                apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + endpoint + personId;
                ids = ids.Concat(GetRegisteredMeetingHelper(apiUrl)).ToList();
            }
            return ids;
        }

        private static List<int> GetRegisteredMeetingHelper(string apiUrl)
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.InternalServerError) return new List<int>();
            var data = JsonConvert.DeserializeObject<List<MeetingDetailEx>>(response.Content);
            return data.Select(x => x.MeetingId).ToList();
        }

        public static bool IsClassPassCardVisible(string personId, string productId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/IsClassPassCardVisible?registrantId=" + personId + "&productId=" + productId;
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        public static string CreateRegistrantPerson(FirmAdminRegistrants person, string personId, string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/CreateRegistrantPerson?personId=" + personId;
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-aura-token", auraId);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(person);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<string>(response.Content);
        }

        public static string FormatDescriptionLine(PersonEx person, WebShoppingCartDetails line, OrderLineViewModel item)
        {
            //person id 2 is guest
            return string.Format("Registration for {0} {1}.", person.FirstName, person.LastName);
        }

        public static PersonEx GetPersonById(int id, string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/PersonEx/GetSingleById/{id}";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-aura-token", auraId);
            request.AddUrlSegment("id", id.ToString());
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<PersonEx>(response.Content);
        }

        public static MeetingEx GetMeetingByProductId(string productId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/meetingex/getbyodatasingle/?$filter=Product/Id eq '" + productId + "'";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<MeetingEx>(response.Content);
        }

        public static MeetingEx GetMeetingByMeetingId(string meetingId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/meetingex/getbyodatasingle/?$filter=Id eq '" + meetingId + "'";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<MeetingEx>(response.Content);
        }

        public static bool IsGuestRegistrationAvailable(string productId, string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/IsGuestRegistrationAvailable?productId=" + productId;
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-aura-token", auraId);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        public static List<FirmAdminRegistrants> LoadFirmAdminRegistrants(string id, string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/LoadFirmAdminRegistrants?personId=" + id;
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-aura-token", auraId);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<FirmAdminRegistrants>>(response.Content);
        }

        public static bool IsFirmAdminOrMember(string id, string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/IsFirmAdminOrMember?personId=" + id;
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-aura-token", auraId);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<bool>(response.Content);
        }

        public static List<WebShoppingCartOrderView> GetOrderDetails(string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/GetUserWebShoppingCartOrderViews";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-aura-token", auraId);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<WebShoppingCartOrderView>>(response.Content);
        }

        public static void NewCart(string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/AddShoppingCartLine";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("x-aura-token", auraId);
            request.RequestFormat = DataFormat.Json;
            var empty = new WebShoppingCartEx
            {
                Id = -1,
                Type = new WebShoppingCartType
                {
                    Id = 1,
                },
                Name = "Shopping Cart",
                Lines = new List<WebShoppingCartDetails>(),
            };
            request.AddBody(empty);
            var response = client.Execute(request);
        }

        public static List<WebShoppingCartEx> GetCart(string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/GetActiveShoppingUserCarts";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-aura-token", auraId);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<WebShoppingCartEx>>(response.Content);
        }

        public static int GetGuestRegistrantId(string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/GuestRegistrantId";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-aura-token", auraId);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<int>(response.Content);
        }

        public static Order GetOrderById(string id)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/api/Order/GetSingleById/" + id;
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<Order>(response.Content);
        }

        public static IRestResponse AddToCart(WebShoppingCartEx cart, int registrantId, int detailId, string auraId,
            string badgeName = null, int associatedId = 0, string companyName = "", string jobTitle = "")
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] +
                         "/icpas/api/WebShoppingCartEx/AddShoppingCartLine";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("x-aura-token", auraId);
            request.RequestFormat = DataFormat.Json;
            cart.Lines = new List<WebShoppingCartDetails>
            {
                new WebShoppingCartDetails
                {
                    ProductId = detailId,
                    RegistrantId = registrantId,
                    BadgeName = badgeName,
                    AssociatedLineId = associatedId,
                    JobTitle = jobTitle,
                    CompanyName = companyName,
                },
            };
            request.AddBody(cart);
            var response = client.Execute(request);
            return response;
        }

        public static List<MeetingEx> GetSessionsByMeetingId(string meetingId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/MeetingEx/GetUpcomingPlannedConferenceSessions?parentMeetingId=" + meetingId;
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<MeetingEx>>(response.Content);
        }

        public static WebShoppingCartDetails SaveCartLine(WebShoppingCartDetails cartLine, string auraId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartDetails/Save";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-aura-token", auraId);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(cartLine);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<WebShoppingCartDetails>(response.Content);
        }

        public static IRestResponse SubmitDonation(string productId, string amount,
            WebShoppingCartProcessRequestViewModel processRequest, string auraId)
        {
            var apiUrl =
                string.Format("{0}/icpas/api/WebShoppingCartEx/ProcessNonMeetingSingleOrder?productId={1}&amount={2}",
                    ConfigurationManager.AppSettings["ServicesUrl"], productId, amount);
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-aura-token", auraId);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(processRequest);
            var response = client.Execute(request);
            return response;
        }

        public static ProductEx GetProductExById(string productId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/ProductEx/GetSingleById/" + productId;
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ProductEx>(response.Content);
        }

        public static Decimal GetProductPrice(string productId, string personId)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] +
                         string.Format("/icpas/api/WebShoppingCartEx/GetProductPrice?productId={0}&personId={1}",
                             productId.ToString(), personId.ToString());

            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<Decimal>(response.Content);
        }

        public static List<PaymentType> GetPaymentTypes(List<int> productIds)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/PaymentType/icpasGetPaymentTypes";
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(productIds);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<PaymentType>>(response.Content);
        }

        /// <summary>
        /// Format an Address Block
        /// </summary>
        /// <param name="address1">Address Line 1</param>
        /// <param name="address2">Address Line 2</param>
        /// <param name="city">City</param>
        /// <param name="state">State</param>
        /// <param name="zip">Zip</param>
        /// <returns>A <see cref="string"/> containing the formatted address block</returns>
        public static string FormatAddressBlock(string address1, string address2, string city, string state, string zip)
        {
            string addressBlock = string.Empty;

            if (!string.IsNullOrEmpty(address1))
            {
                addressBlock = address1;
            }
            if (!string.IsNullOrEmpty(address2))
            {
                if (addressBlock != "")
                {
                    addressBlock += "<br />";
                }
                addressBlock += address2;
            }
            if (!string.IsNullOrEmpty(city))
            {
                if (addressBlock != "")
                {
                    addressBlock += "<br />";
                }
                addressBlock += city;
            }
            if (!string.IsNullOrEmpty(state))
            {
                if (string.IsNullOrEmpty(city))
                {
                    addressBlock += "<br />";
                }
                else
                {
                    addressBlock += ", ";
                }
                addressBlock += state;

                if (!string.IsNullOrEmpty(zip))
                {
                    addressBlock += " " + zip;
                }
            }

            return addressBlock;
        }

        /// <summary>
        /// Removes all HTML tags from string, including anything between &lt; and &gt; of the tag
        /// </summary>
        /// <param name="sourceHtml">Text to be cleaned</param>
        /// <returns>A <see cref="string" /> without html coding</returns>
        public static string StripHtmlTags(this string sourceHtml)
        {
            if (!string.IsNullOrEmpty(sourceHtml))
            {
                char[] chrArray = new char[sourceHtml.Length];
                int num = 0;
                bool flag = false;
                string str = sourceHtml;

                for (int i = 0; i < str.Length; i++)
                {
                    char chr = str[i];
                    if (chr == '<')
                    {
                        flag = true;
                    }
                    else if (chr == '>')
                    {
                        flag = false;
                    }
                    else if (!flag)
                    {
                        chrArray[num] = chr;
                        num++;
                    }
                }
                return new string(chrArray, 0, num);
            }

            return string.Empty;
        }

        /// <summary>
        /// Custom am/pm format of a.m. and p.m.
        /// </summary>
        /// <returns>The custom <see cref="DateTimeFormatInfo"/> for a.m. and p.m. </returns>
        public static DateTimeFormatInfo AMPMFormat()
        {
            DateTimeFormatInfo fi = new DateTimeFormatInfo();
            fi.AMDesignator = "a.m.";
            fi.PMDesignator = "p.m.";

            return fi;
        }

        /// <summary>
        /// Load Listbox with specific Topic Code values having the same parent ID
        /// </summary>
        /// <param name="listBox">ListBox to be populated</param>
        /// <param name="topicCode">ID of parent topic code </param>
        /// <param name="addAllOption">Include All selection with a value of All, default is true</param>
        public static void LoadTopicCodeListBox(ListBox listBox, int topicCode, bool addAllOption = true)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/TopicCode/GetTopicCodes";

            var client = new RestClient(apiUrl + "?parentID=" + topicCode.ToString());
            var request = new RestRequest(Method.GET);

            var response = client.Execute(request);

            var data = JsonConvert.DeserializeObject<DataTable>(response.Content);

            listBox.DataSource = data;
            listBox.DataTextField = "name";
            listBox.DataValueField = "id";
            listBox.DataBind();

            ListItem allItem = new ListItem();
            allItem.Value = "All";
            allItem.Text = "All";
            listBox.Items.Insert(0, allItem);
            listBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Load CheckBoxList with specific Topic Code values having the same parent ID
        /// </summary>
        /// <param name="checkBoxList">CheckBoxList to be populated</param>
        /// <param name="topicCode">ID of parent topic code </param>
        public static void LoadTopicCodeCheckBoxList(CheckBoxList checkBoxList, int topicCode)
        {
            var apiUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/TopicCode/GetTopicCodes";

            var client = new RestClient(apiUrl + "?parentID=" + topicCode.ToString());
            var request = new RestRequest(Method.GET);

            var response = client.Execute(request);

            var data = JsonConvert.DeserializeObject<DataTable>(response.Content);

            checkBoxList.DataSource = data;
            checkBoxList.DataTextField = "name";
            checkBoxList.DataValueField = "id";
            checkBoxList.DataBind();
        }

        /// <summary>
        /// Select items in a ListBox from a List&lt;string&gt; of values
        /// </summary>
        /// <param name="items">List&lt;string&gt; of values to be marked selected</param>
        /// <param name="listBox">ListBox to be populated</param>
        public static void SelectListBoxItems(List<string> items, ListBox listBox)
        {
            listBox.ClearSelection();
            foreach (ListItem listItem in listBox.Items)
            {
                if (items.Contains(listItem.Value))
                {
                    listItem.Selected = true;
                }
            }
        }

        /// <summary>
        /// Select items in a CheckBoxList from a List&lt;string&gt; of values
        /// </summary>
        /// <param name="items">List&lt;string&gt; of values to be marked selected</param>
        /// <param name="checkBoxList">CheckBoxList to be populated</param>
        public static void SelectCheckBoxListItems(List<string> items, CheckBoxList checkBoxList)
        {
            checkBoxList.ClearSelection();
            foreach (ListItem listItem in checkBoxList.Items)
            {
                if (items.Contains(listItem.Value))
                {
                    listItem.Selected = true;
                }
            }
        }

        /// <summary>
        /// Select items in a CheckBoxList from a List of TopicCode
        /// </summary>
        /// <param name="items">List of TopicCode  to be marked selected</param>
        /// <param name="checkBoxList">CheckBoxList to be populated</param>
        public static void SelectCheckBoxListItems(List<TopicCode> items, CheckBoxList checkBoxList)
        {
            checkBoxList.ClearSelection();
            if (items.Count > 0)
            {
                foreach (ListItem listItem in checkBoxList.Items)
                {
                    TopicCode selected = items.Find(i => i.Id.ToString() == listItem.Value);
                    if (selected != null)
                    {
                        listItem.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Get a comma delimited list of selected value from a Multiple Selection Mode ListBox
        /// </summary>
        /// <param name="listBox">ListBox to get the selections from</param>
        /// <returns>A comma delimated <see cref="string"/> of selected values</returns>
        public static string GetSelectedItemFromListBox(ListBox listBox)
        {
            string selectedItems = "";

            List<string> selectedIDs = new List<string>();
            foreach (ListItem listItem in listBox.Items)
            {
                if (listItem.Selected)
                {
                    selectedIDs.Add(listItem.Value);
                }
            }
            selectedItems = string.Join(",", selectedIDs);

            return selectedItems;
        }

        /// <summary>
        /// Get a comma delimited list of selected value from a CheckBoxList
        /// </summary>
        /// <param name="checkBoxList">CheckBoxList to get the selections from</param>
        /// <returns>A comma delimated <see cref="string"/> of selected values</returns>
        public static string GetSelectedItemsFromCheckBoxList(CheckBoxList checkBoxList)
        {
            string selectedItems = string.Empty;

            List<string> selectedIDs = new List<string>();
            foreach (ListItem listItem in checkBoxList.Items)
            {
                if (listItem.Selected)
                {
                    selectedIDs.Add(listItem.Value);
                }
            }
            selectedItems = string.Join(",", selectedIDs);

            return selectedItems;
        }

        /// <summary>
        /// Removes all characters not on the admitted list
        /// </summary>
        /// <param name="sourceText">Text to be cleaned</param>
        /// <param name="subChar">Character to replace banned characters, default is space</param>
        /// <param name="skip">Skip Character substitution, just remove banned character, default is true</param>
        /// <returns>A <see cref="string" /> without banner characters</returns>
        /// <remarks>Allowed Charaters: !-_+*'@#$^.()[] </remarks>
        public static string Sanitize(this string sourceText, char subChar = ' ', bool skip = true)
        {
            string admitted = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!-_+*'@#$^.()[]& ";
            StringBuilder output = new StringBuilder(sourceText.Length);
            bool found = false;

            foreach (char c in sourceText)
            {
                found = false;
                foreach (char adm in admitted)
                {
                    if (c == adm)
                    {
                        found = true;
                        output.Append(c);
                    }
                }

                if (found == false)
                {
                    if (!skip)
                        output.Append(subChar);
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Return a list in random order
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list</typeparam>
        /// <param name="list">The List</param>
        /// <returns>A randomized list</returns>
        public static IList<T> RandomizeList<T>(this IList<T> list) where T : ICloneable
        {
            IList<T> originalList = list.Select(item => (T)item.Clone()).ToList();

            IList<T> randomList = new List<T>();

            Random r = new Random();
            int randomIndex = 0;
            while (originalList.Count > 0)
            {
                randomIndex = r.Next(0, originalList.Count);
                randomList.Add(originalList[randomIndex]);
                originalList.RemoveAt(randomIndex);
            }

            return randomList;
        }
    }
}