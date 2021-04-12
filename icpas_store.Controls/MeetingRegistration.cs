using icpas_store.Controls.General;
using icpas_store.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace icpas_store.Controls
{
	public class MeetingRegistration : UserControl
	{
		public class GetCPASessionGroups
		{
			public int ID
			{
				get;
				set;
			}
			public int MeetingID
			{
				get;
				set;
			}
			public int Sequence
			{
				get;
				set;
			}
			public string SessionIDs
			{
				get;
				set;
			}
		}
		protected Panel Panel1;
		protected Label Title;
		protected Label Price;
		protected Panel pnlAccPassMsg;
		protected Label lblAccPassMsg;
		protected Label Attendee;
		protected Label Label1;
		protected TextBox txtEmail;
		protected RequiredFieldValidator RequiredFieldValidator1;
		protected Label Label2;
		protected TextBox txtAreaHomePhone;
		protected TextBox txtHomePhone;
		protected TextBox txtPhoneExt;
		protected RequiredFieldValidator RequiredFieldValidator2;
		protected RequiredFieldValidator RequiredFieldValidator4;
		protected Label lblName1;
		protected TextBox txtName1;
		protected Label lblCompany;
		protected TextBox txtCompany;
		protected Label lblTitle;
		protected TextBox txtTitle;
		protected Label Label3;
		protected TextBox txtHomeAddressLine1;
		protected RequiredFieldValidator RequiredFieldValidator3;
		protected TextBox txtHomeAddressLine2;
		protected Label Label4;
		protected TextBox txtHomeCity;
		protected DropDownList ddbPersonalInformationState;
		protected TextBox txtHomeZip;
		protected Label Label5;
		protected DropDownList ddlPersonalInformationCountry;
		protected RequiredFieldValidator RequiredFieldValidator5;
		protected RequiredFieldValidator RequiredFieldValidator6;
		protected RequiredFieldValidator rfv1;
		protected Panel pnlSessions;
		protected ListView ListViewSessions;
		protected Label lblStatus;
		protected ValidationSummary ValidationSummary1;
		protected LinkButton btnSubmit;
		protected HiddenField hidSessSelectedProdIDs;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.LoadFormData();
				this.PersonDetails();
				this.LoadSessionData();
				decimal d;
				bool flag = decimal.TryParse(this.Price.Text.Replace("$", ""), out d);
				if (flag && d > 0m)
				{
					this.GetEventAccessPassResult();
				}
			}
		}
		private void LoadFormData()
		{
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/PopulateCountries/GetCountries";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest request = new RestRequest(Method.GET);
			IRestResponse restResponse = restClient.Execute(request);
			DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(restResponse.Content);
			if (dataTable.Rows.Count > 0)
			{
				this.ddlPersonalInformationCountry.DataSource = dataTable;
				this.ddlPersonalInformationCountry.DataTextField = "Country";
				this.ddlPersonalInformationCountry.DataValueField = "ID";
				this.ddlPersonalInformationCountry.DataBind();
				this.ddlPersonalInformationCountry.ClearSelection();
				this.SetComboValue(ref this.ddlPersonalInformationCountry, "United States");
				this.PopulateStateByCountryId();
			}
		}
		public void PersonDetails()
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			PersonEx personById = Helpers.GetPersonById(Convert.ToInt32(base.Session["PersonID"]), auraId);
			this.txtName1.Text = Helpers.TrimOrNull(personById.FirstName) + " " + Helpers.TrimOrNull(personById.LastName);
			this.Attendee.Text = this.txtName1.Text;
			this.txtTitle.Text = Helpers.TrimOrNull(personById.Title);
			this.txtEmail.Text = Helpers.TrimOrNull(personById.Email);
			if (personById.Company != null)
			{
				this.txtCompany.Text = Helpers.TrimOrNull(personById.Company.Name);
			}
			if (personById.PreferredAddress.Contains("Home") && personById.HomeAddress != null)
			{
				this.txtHomeAddressLine1.Text = Helpers.TrimOrNull(personById.HomeAddress.Line1);
				this.txtHomeAddressLine2.Text = Helpers.TrimOrNull(personById.HomeAddress.Line2);
				this.txtHomeCity.Text = Helpers.TrimOrNull(personById.HomeAddress.City);
				this.txtHomeZip.Text = Helpers.TrimOrNull(personById.HomeAddress.PostalCode);
				this.SetComboValue(ref this.ddlPersonalInformationCountry, personById.HomeAddress.Country);
				this.SetComboValue(ref this.ddbPersonalInformationState, personById.HomeAddress.StateProvince);
			}
			else
			{
				if (personById.BusinessAddress != null)
				{
					this.txtHomeAddressLine1.Text = Helpers.TrimOrNull(personById.BusinessAddress.Line1);
					this.txtHomeAddressLine2.Text = Helpers.TrimOrNull(personById.BusinessAddress.Line2);
					this.txtHomeCity.Text = Helpers.TrimOrNull(personById.BusinessAddress.City);
					this.txtHomeZip.Text = Helpers.TrimOrNull(personById.BusinessAddress.PostalCode);
					this.SetComboValue(ref this.ddlPersonalInformationCountry, personById.BusinessAddress.Country);
					this.SetComboValue(ref this.ddbPersonalInformationState, personById.BusinessAddress.StateProvince);
				}
			}
			this.PopulateStateByCountryId();
			this.txtAreaHomePhone.Text = Helpers.TrimOrNull(personById.PhoneAreaCode);
			this.txtHomePhone.Text = Helpers.TrimOrNull(personById.Phone);
			this.txtPhoneExt.Text = Helpers.TrimOrNull(personById.PhoneExtension);
		}
		public void LoadSessionData()
		{
			if (base.Request.QueryString["OL"] == null)
			{
				return;
			}
			string ol = base.Request.QueryString["OL"];
			if (string.IsNullOrEmpty(ol))
			{
				throw new HttpException(400, "Bad Request, QueryString Parameter Missing.");
			}
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			WebShoppingCartEx webShoppingCartEx = Helpers.GetCart(auraId).FirstOrDefault<WebShoppingCartEx>();
			if (webShoppingCartEx == null || webShoppingCartEx.Lines == null)
			{
				base.Response.Redirect("/productcatalog/viewcart");
				return;
			}
			if (webShoppingCartEx.Lines.FirstOrDefault((WebShoppingCartDetails x) => x.Id == Convert.ToInt32(ol)) == null)
			{
				this.btnSubmit.Visible = false;
				return;
			}
			base.Session["CartLine"] = webShoppingCartEx.Lines.First((WebShoppingCartDetails x) => x.Id == Convert.ToInt32(ol));
			int productId = webShoppingCartEx.Lines.First((WebShoppingCartDetails x) => x.Id == Convert.ToInt32(ol)).ProductId;
			MeetingEx meetingByProductId = Helpers.GetMeetingByProductId(productId.ToString());
			List<MeetingEx> sessionsByMeetingId = Helpers.GetSessionsByMeetingId(meetingByProductId.Id.ToString());
			this.Price.Text = Helpers.GetProductPrice(productId.ToString(), base.Session["PersonID"].ToString()).ToString("C");
			this.Title.Text = meetingByProductId.MeetingTitle;
			if (sessionsByMeetingId.Count > 0)
			{
				this.ListViewSessions.DataSource = sessionsByMeetingId;
				this.ListViewSessions.DataBind();
				this.pnlSessions.Visible = true;
				return;
			}
			this.pnlSessions.Visible = false;
		}
		private void GetEventAccessPassResult()
		{
			WebShoppingCartDetails webShoppingCartDetails = (WebShoppingCartDetails)base.Session["CartLine"];
			if (webShoppingCartDetails != null)
			{
				string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
				if (string.IsNullOrEmpty(auraId))
				{
					return;
				}
				WebShoppingCartEx webShoppingCartEx = Helpers.GetCart(auraId).FirstOrDefault<WebShoppingCartEx>();
				string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/GetEventAccessPassResult";
				RestClient restClient = new RestClient(baseUrl);
				RestRequest restRequest = new RestRequest(Method.GET);
				restRequest.AddParameter("lgAttendeeID", Convert.ToInt64(base.Session["PersonID"]));
				restRequest.AddParameter("lgProductID", webShoppingCartDetails.ProductId.ToString());
				if (webShoppingCartEx != null)
				{
					restRequest.AddParameter("cartId", webShoppingCartEx.Id);
				}
				else
				{
					restRequest.AddParameter("cartId", null);
				}
				restRequest.AddParameter("cartdetailId", webShoppingCartDetails.Id);
				IRestResponse restResponse = restClient.Execute(restRequest);
				string text = JsonConvert.DeserializeObject<string>(restResponse.Content);
				if (Helpers.TrimOrNull(text) != "" && !Helpers.TrimOrNull(text).Contains("Error occurred"))
				{
					this.pnlAccPassMsg.Visible = true;
					this.Price.Visible = true;
					this.lblAccPassMsg.Text = "Because you are a <span style='font-style: italic;'>" + text.Trim() + "</span> holder, your price will be discounted when you register.";
					return;
				}
				this.pnlAccPassMsg.Visible = false;
			}
		}
		public bool AtLeastOneSessionChecked()
		{
			this.lblStatus.Text = "";
			bool flag = (
				from row in this.ListViewSessions.Items
				select (CheckBox)row.FindControl("SessionSelect") into chkSelected
				let productId = Convert.ToInt32(chkSelected.InputAttributes["productId"])
				select chkSelected).Any((CheckBox chkSelected) => chkSelected.Checked);
			if (!flag)
			{
				this.lblStatus.Visible = true;
				this.lblStatus.Text = "Please select at least ONE of the sessions above.";
			}
			return flag;
		}
		protected void btnSubmit_OnClick(object sender, EventArgs e)
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			WebShoppingCartEx webShoppingCartEx = Helpers.GetCart(auraId).FirstOrDefault<WebShoppingCartEx>();
			if (webShoppingCartEx == null || webShoppingCartEx.Lines == null)
			{
				base.Response.Redirect("/productcatalog/viewcart");
				return;
			}
			int rowId = Convert.ToInt32(base.Request.QueryString["OL"]);
			WebShoppingCartDetails webShoppingCartDetails = webShoppingCartEx.Lines.First((WebShoppingCartDetails x) => x.Id == rowId);
			MeetingEx meetingByProductId = Helpers.GetMeetingByProductId(webShoppingCartEx.Lines.First((WebShoppingCartDetails x) => x.Id == Convert.ToInt32(rowId)).ProductId.ToString());
			if (this.ListViewSessions.Items.Count > 0)
			{
				if (!this.CheckSessionGroup(meetingByProductId.Id))
				{
					return;
				}
				if (!this.AtLeastOneSessionChecked())
				{
					return;
				}
			}
			base.Session["CartLine"] = webShoppingCartDetails;
			webShoppingCartDetails.SessionRanks.Clear();
			for (int i = 0; i < this.ListViewSessions.Items.Count; i++)
			{
				ListViewDataItem listViewDataItem = this.ListViewSessions.Items[i];
				CheckBox checkBox = (CheckBox)listViewDataItem.FindControl("SessionSelect");
				int sessionProductID = Convert.ToInt32(checkBox.InputAttributes["productId"]);
				if (checkBox.Checked)
				{
					webShoppingCartDetails.SessionRanks.Add(new WebShoppingCartDetailsSessionRank
					{
						SessionProductID = sessionProductID,
						Sequence = i
					});
				}
			}
			webShoppingCartDetails.BadgeName = this.txtName1.Text.Trim();
			webShoppingCartDetails.JobTitle = this.txtTitle.Text.Trim();
			webShoppingCartDetails.CompanyName = this.txtCompany.Text.Trim();
			Helpers.SaveCartLine(webShoppingCartDetails, auraId);
			if (this.SavePersonalInfo())
			{
				base.Response.Redirect("/productcatalog/viewcart");
			}
		}
		protected void ListViewSessions_OnItemDataBound(object sender, ListViewItemEventArgs e)
		{
			WebShoppingCartDetails webShoppingCartDetails = (WebShoppingCartDetails)base.Session["CartLine"];
			if (e.Item.ItemType != ListViewItemType.DataItem)
			{
				return;
			}
			ListViewDataItem listViewDataItem = (ListViewDataItem)e.Item;
			MeetingEx rowView = (MeetingEx)listViewDataItem.DataItem;
			CheckBox checkBox = (CheckBox)e.Item.FindControl("SessionSelect");
			checkBox.InputAttributes.Add("productId", Convert.ToString(rowView.Product.Id));
			if (webShoppingCartDetails.SessionRanks.Any((WebShoppingCartDetailsSessionRank x) => x.SessionProductID == rowView.Product.Id))
			{
				checkBox.Checked = true;
				HtmlGenericControl htmlGenericControl = (HtmlGenericControl)e.Item.FindControl("Checklbl");
				htmlGenericControl.InnerHtml = "Selected";
			}
		}
		protected void ddlPersonalInformationCountry_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.PopulateStateByCountryId();
		}
		private void PopulateStateByCountryId()
		{
			try
			{
				string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/PopulateStates/GetStates";
				RestClient restClient = new RestClient(baseUrl);
				RestRequest restRequest = new RestRequest(Method.GET);
				if (this.ddlPersonalInformationCountry.Items.Count > 0 && this.ddlPersonalInformationCountry.SelectedValue != null)
				{
					restRequest.AddParameter("countryID", this.ddlPersonalInformationCountry.SelectedValue);
					IRestResponse restResponse = restClient.Execute(restRequest);
					DataTable dataSource = JsonConvert.DeserializeObject<DataTable>(restResponse.Content);
					this.ddbPersonalInformationState.DataSource = dataSource;
					this.ddbPersonalInformationState.DataTextField = "State";
					this.ddbPersonalInformationState.DataValueField = "State";
					this.ddbPersonalInformationState.DataBind();
				}
			}
			catch (Exception)
			{
			}
		}
		private void SetComboValue(ref DropDownList cmb, string sValue)
		{
			try
			{
				cmb.ClearSelection();
				for (int i = 0; i <= cmb.Items.Count - 1; i++)
				{
					if (string.Compare(cmb.Items[i].Value, sValue, true) == 0)
					{
						cmb.Items[i].Selected = true;
						break;
					}
					if (string.Compare(cmb.Items[i].Text, sValue, true) == 0)
					{
						cmb.Items[i].Selected = true;
						break;
					}
				}
			}
			catch (Exception)
			{
			}
		}
		private bool SavePersonalInfo()
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			this.lblStatus.Visible = false;
			string text = "";
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/ProfileUpdatePersonInfoSave/SaveProfilePersonalInfoMeetingReg";
			string value4;
			string value3;
			string value2;
			string value = value2 = (value3 = (value4 = ""));
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.POST);
			restRequest.AddHeader("x-aura-token", auraId);
			restRequest.AddParameter("PersonID", base.Session["PersonID"]);
			if (!string.IsNullOrEmpty(this.txtHomeAddressLine1.Text) | !string.IsNullOrEmpty(this.txtHomeAddressLine2.Text))
			{
				value2 = this.txtHomeAddressLine1.Text;
				value = this.txtHomeAddressLine2.Text;
			}
			string text2 = this.txtHomeCity.Text;
			string value5 = (!string.IsNullOrEmpty(this.ddbPersonalInformationState.SelectedValue)) ? this.ddbPersonalInformationState.SelectedItem.Text : "";
			int num = (!string.IsNullOrEmpty(this.ddlPersonalInformationCountry.SelectedValue)) ? Convert.ToInt32(this.ddlPersonalInformationCountry.SelectedValue) : -1;
			string value6 = (!string.IsNullOrEmpty(this.ddlPersonalInformationCountry.SelectedValue)) ? this.ddlPersonalInformationCountry.SelectedItem.Text : "";
			string value7 = this.txtHomeZip.Text.Trim();
			string value8 = this.txtEmail.Text.Trim();
			string value9 = this.txtPhoneExt.Text.Trim();
			if (!string.IsNullOrEmpty(this.txtAreaHomePhone.Text) && !string.IsNullOrEmpty(this.txtHomePhone.Text))
			{
				if (this.txtAreaHomePhone.Text.Length <= 5 & this.txtHomePhone.Text.Length <= 15)
				{
					value3 = this.txtAreaHomePhone.Text;
					value4 = this.txtHomePhone.Text;
				}
				else
				{
					text += "Please enter valid Home phone number. \n";
				}
			}
			if (text.Length > 1)
			{
				this.lblStatus.Text = text;
				this.lblStatus.Visible = true;
				return false;
			}
			PersonEx personById = Helpers.GetPersonById(Convert.ToInt32(base.Session["PersonID"]), auraId);
			restRequest.AddParameter("PreferredAddress", personById.PreferredAddress);
			restRequest.AddParameter("Email1", value8);
			restRequest.AddParameter("AddressLine1", value2);
			restRequest.AddParameter("AddressLine2", value);
			restRequest.AddParameter("City", text2);
			restRequest.AddParameter("State", value5);
			restRequest.AddParameter("Country", value6);
			restRequest.AddParameter("CountryCodeID", num);
			restRequest.AddParameter("ZipCode", value7);
			restRequest.AddParameter("AreaCode", value3);
			restRequest.AddParameter("Phone", value4);
			restRequest.AddParameter("PhoneExtension", value9);
			IRestResponse restResponse = restClient.Execute(restRequest);
			string arg_35F_0 = restResponse.Content;
			return true;
		}
		private bool CheckSessionGroup(int meetingid)
		{
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/Icpas/api/CPASessionGroups/GetCPASessionGroups?meetingid=" + meetingid;
			RestClient restClient = new RestClient(baseUrl);
			RestRequest request = new RestRequest(Method.GET);
			IRestResponse restResponse = restClient.Execute(request);
			List<MeetingRegistration.GetCPASessionGroups> list = JsonConvert.DeserializeObject<List<MeetingRegistration.GetCPASessionGroups>>(restResponse.Content);
			bool result;
			try
			{
				string text = "";
				string text2 = "";
				this.lblStatus.Text = "";
				int num = 0;
				if (list.Count > 0)
				{
					int i = 0;
					for (i = 0; i <= list.Count - 1; i++)
					{
						num++;
						string sessionIDs = list[i].SessionIDs;
						int num2 = 0;
						string text3 = "";
						text = "";
						for (int j = 0; j < this.ListViewSessions.Items.Count; j++)
						{
							ListViewDataItem listViewDataItem = this.ListViewSessions.Items[j];
							CheckBox checkBox = (CheckBox)listViewDataItem.FindControl("SessionSelect");
							int num3 = Convert.ToInt32(checkBox.InputAttributes["productId"]);
							if (checkBox.Checked)
							{
								int id = Helpers.GetMeetingByProductId(num3.ToString()).Id;
								if (sessionIDs.Contains(id.ToString()))
								{
									num2++;
									if (this.hidSessSelectedProdIDs.Value.Trim().Length == 0)
									{
										this.hidSessSelectedProdIDs.Value = id.ToString();
									}
									else
									{
										this.hidSessSelectedProdIDs.Value = "," + id;
									}
								}
							}
						}
						if (num2 != 1)
						{
							if (text3 == "")
							{
								text3 = sessionIDs;
							}
							if (text3 != "")
							{
								new DataTable();
								if (list.Count == 1)
								{
									List<int> list2 = text3.Split(new char[]
									{
										','
									}).Select(new Func<string, int>(int.Parse)).ToList<int>();
									using (List<int>.Enumerator enumerator = list2.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											int current = enumerator.Current;
											string meetingTitle = Helpers.GetMeetingByMeetingId(current.ToString()).MeetingTitle;
											if (text == "")
											{
												text = meetingTitle;
											}
											else
											{
												text = text + "<br>" + meetingTitle;
											}
										}
										goto IL_365;
									}
								}
								List<int> list3 = text3.Split(new char[]
								{
									','
								}).Select(new Func<string, int>(int.Parse)).ToList<int>();
								foreach (int current2 in list3)
								{
									string meetingTitle2 = Helpers.GetMeetingByMeetingId(current2.ToString()).MeetingTitle;
									if (text == "")
									{
										text = meetingTitle2;
									}
									else
									{
										text = text + "<br>" + meetingTitle2;
									}
								}
								if (text2 == "")
								{
									text2 = string.Concat(new object[]
									{
										"Group: ",
										num,
										"<br>",
										text
									});
								}
								else
								{
									text2 = string.Concat(new object[]
									{
										text2,
										"<br><br>Group: ",
										num,
										"<br>",
										text
									});
								}
							}
						}
						IL_365:;
					}
					if (text2 != "")
					{
						this.lblStatus.Visible = true;
						if (list.Count == 1)
						{
							this.lblStatus.Text = "Please select ONE of the sessions above: <br>" + text;
						}
						else
						{
							this.lblStatus.Text = "Please select ONE session from each group above: <br>" + text2;
						}
						result = false;
					}
					else
					{
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
			catch (Exception)
			{
				this.lblStatus.Visible = true;
				this.lblStatus.Text = "Error.";
				result = false;
			}
			return result;
		}
		public static DataTable ToDataTable<T>(IList<T> list)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
			DataTable dataTable = new DataTable();
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor = properties[i];
				dataTable.Columns.Add(propertyDescriptor.Name, Nullable.GetUnderlyingType(propertyDescriptor.PropertyType) ?? propertyDescriptor.PropertyType);
			}
			object[] array = new object[properties.Count];
			foreach (T current in list)
			{
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = (properties[j].GetValue(current) ?? DBNull.Value);
				}
				dataTable.Rows.Add(array);
			}
			return dataTable;
		}
	}
}
