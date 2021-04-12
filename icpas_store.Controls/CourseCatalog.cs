using icpas_store.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace icpas_store.Controls
{
	public class CourseCatalog : UserControl
	{
		protected ScriptManager ScriptManager1;
		protected HtmlGenericControl AlertWindow;
		protected TextBox txtKeywords;
		protected DropDownList ddlTopicCode;
		protected DropDownList ddlCreditType;
		protected DropDownList ddlInstructor;
		protected DropDownList ddlProgramType;
		protected DropDownList ddlMemberMiles;
		protected TextBox txtZip;
		protected DropDownList ddlCourseLevel;
		protected DropDownList ddlLocation;
        protected DropDownList ddlDeliveryTypes;
		protected TextBox txtStartDate;
		protected TextBox txtEndDate;
		protected CheckBox chkExcludeChapter;
		protected CheckBox chkAccessPass;
        protected CheckBox chkJustAdded;
		protected Button btnSearch;
		protected Button btnReset;
		protected UpdatePanel updResults;
		protected HtmlGenericControl liInPerson;
		protected LinkButton btnInPerson;
		protected HtmlGenericControl lblInPerson;
		protected HtmlGenericControl liOnDemand;
		protected LinkButton btnOnDemand;
		protected HtmlGenericControl lblOnDemand;
		protected HtmlGenericControl Results;
		protected ListView ListViewResults;
		protected DataPager lvDataPager1;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (base.IsPostBack)
			{
				return;
			}
			this.LoadSearchOptions();
			if (base.Request.QueryString["t"] != "" && base.Request.QueryString["t"] != null)
			{
				this.ddlTopicCode.SelectedIndex = this.ddlTopicCode.Items.IndexOf(this.ddlTopicCode.Items.FindByText(base.Request.QueryString["t"]));
			}
			if (base.Request.QueryString["p"] != "" && base.Request.QueryString["p"] != null)
			{
				this.ddlProgramType.SelectedIndex = this.ddlProgramType.Items.IndexOf(this.ddlProgramType.Items.FindByText(base.Request.QueryString["p"]));
			}
			this.DoSearch(false, false);
		}
		public void DoSearch(bool initial, bool usersearch)
		{
			string value = this.txtKeywords.Text.ToUpper();
			string value2 = this.ddlTopicCode.SelectedValue;
			string value3 = this.ddlCreditType.SelectedValue;
			string value4 = this.ddlLocation.SelectedValue;
			string value5 = this.ddlProgramType.SelectedValue;
			string value6 = this.ddlMemberMiles.SelectedValue;
			string value7 = this.txtZip.Text;
			string value8 = this.ddlInstructor.SelectedValue;
			string value9 = this.txtStartDate.Text;
			string value10 = this.txtEndDate.Text;
			bool flag = this.chkAccessPass.Checked;
			bool flag2 = this.chkExcludeChapter.Checked;
            bool flag3 = this.chkJustAdded.Checked;
			string value11 = this.ddlCourseLevel.SelectedValue;
            string value13 = this.ddlDeliveryTypes.SelectedValue;
			if (initial)
			{
				value2 = string.Empty;
				value3 = string.Empty;
				value4 = string.Empty;
				value5 = string.Empty;
				value6 = string.Empty;
				value7 = string.Empty;
				value8 = string.Empty;
				value9 = string.Empty;
				value10 = string.Empty;
				flag = false;
				flag2 = false;
                flag3 = false;
				value11 = string.Empty;
                value13 = string.Empty;
			}
			this.AlertWindow.Visible = false;
			if (!string.IsNullOrEmpty(value6) && string.IsNullOrEmpty(value7))
			{
				this.AlertWindow.Visible = true;
				this.AlertWindow.InnerText = "Please enter a valid Zip.";
				return;
			}
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/meetingex/search";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.POST);
			restRequest.AddParameter("SearchText", value);
			if (!string.IsNullOrEmpty(value9))
			{
				restRequest.AddParameter("StartDate", value9);
			}
			if (!string.IsNullOrEmpty(value10))
			{
				restRequest.AddParameter("EndDate", value10);
			}
			if (!string.IsNullOrEmpty(value2))
			{
				restRequest.AddParameter("TopicId", value2);
			}
			if (!string.IsNullOrEmpty(value3))
			{
				restRequest.AddParameter("CreditTypeId", value3);
			}
			if (!string.IsNullOrEmpty(value5))
			{
				restRequest.AddParameter("ProgramTypeId", value5);
			}
			if (!string.IsNullOrEmpty(value4))
			{
				restRequest.AddParameter("LocationArea", value4);
			}
			if (!string.IsNullOrEmpty(value5))
			{
				restRequest.AddParameter("ProgramType", value5);
			}
			if (!string.IsNullOrEmpty(value6) && !string.IsNullOrEmpty(value7))
			{
				restRequest.AddParameter("MilesDistance", value6);
			}
			if (!string.IsNullOrEmpty(value7))
			{
				restRequest.AddParameter("Zip", value7);
			}
			if (!string.IsNullOrEmpty(value8))
			{
				restRequest.AddParameter("InstructorId", value8);
			}
			if (flag)
			{
				restRequest.AddParameter("AccessPass", true);
			}
			if (flag2)
			{
				restRequest.AddParameter("ExcludeChapter", true);
			}
            if (flag3)
            {
                restRequest.AddParameter("JustAdded", true);
            }
			if (!string.IsNullOrEmpty(value11))
			{
				restRequest.AddParameter("CourseLevel", value11);
			}
            if (!string.IsNullOrEmpty(value13))
            {
                restRequest.AddParameter("DeliveryType", value13);
            }
			IRestResponse restResponse = restClient.Execute(restRequest);
			if (restResponse.StatusCode != HttpStatusCode.OK)
			{
				throw new HttpException(500, "Internal Server Error");
			}
			List<MeetingEx> source = JsonConvert.DeserializeObject<List<MeetingEx>>(restResponse.Content);
			List<MeetingEx> list = new List<MeetingEx>();
			List<MeetingEx> list2 = new List<MeetingEx>();
			foreach (MeetingEx current in 
				from result in source
				where result.Product.PrimaryVendor == "Illinois CPA Society"
				select result)
			{
				current.Product.PrimaryVendor = "ICPAS";
			}
			foreach (MeetingEx current2 in 
				from result in source
				where result.GroupId >= 0
				select result)
			{
				switch (current2.GroupId)
				{
				case 0:
					list.Add(current2);
					break;
				case 1:
					list.Add(current2);
					break;
				case 2:
					current2.StartDate = DateTime.MinValue;
					list2.Add(current2);
					break;
				default:
					list.Add(current2);
					break;
				}
			}
			base.Session["inPersonData"] = list;
			List<MeetingEx> value12 = this.SortList(list2);
			base.Session["onDemandData"] = value12;
			if (list.Count == 0)
			{
				this.SetOnDemand();
				base.Session["searchResultData"] = value12;
			}
			else
			{
				this.SetInPerson();
				base.Session["searchResultData"] = list;
			}
			this.lblInPerson.InnerText = string.Format("{0} results found", list.Count);
			this.lblOnDemand.InnerText = string.Format("{0} results found", list2.Count);
			this.lvDataPager1.SetPageProperties(0, 25, true);
			this.BindListView();
			if (usersearch)
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "anchor", "location.hash = '#courseresults';", true);
			}
		}
		public List<MeetingEx> SortList(IList<MeetingEx> list)
		{
			List<MeetingEx> list2 = list.ToList<MeetingEx>();
			list2.Sort((MeetingEx x, MeetingEx y) => string.CompareOrdinal(x.MeetingTitle, y.MeetingTitle));
			return list2;
		}
		public List<Param> SortList(IList<Param> list)
		{
			List<Param> list2 = list.ToList<Param>();
			list2.Sort((Param x, Param y) => string.CompareOrdinal(x.Name, y.Name));
			foreach (Param current in list2)
			{
				current.Name = current.Name.Trim();
			}
			return list2;
		}
		public List<NameParam> SortList(IList<NameParam> list)
		{
			List<NameParam> list2 = list.ToList<NameParam>();
			list2.Sort((NameParam x, NameParam y) => string.CompareOrdinal(x.Name, y.Name));
			foreach (NameParam current in list2)
			{
				current.Name = current.Name.Trim();
			}
			return list2;
		}
		public void LoadSearchOptions()
		{
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/meetingex/getsearchparams";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest request = new RestRequest(Method.GET);
			IRestResponse restResponse = restClient.Execute(request);
			SearchParams searchParams = JsonConvert.DeserializeObject<SearchParams>(restResponse.Content);
			this.ddlProgramType.DataSource = this.SortList(searchParams.ProgramTypes);
			this.ddlProgramType.DataBind();
			this.ddlProgramType.Items.Insert(0, new ListItem("Select One", ""));
			this.ddlTopicCode.DataSource = this.SortList(searchParams.TopicCodes);
			this.ddlTopicCode.DataBind();
			this.ddlTopicCode.Items.Insert(0, new ListItem("Select One", ""));
			this.ddlCreditType.DataSource = this.SortList(searchParams.CreditTypes);
			this.ddlCreditType.DataBind();
			this.ddlCreditType.Items.Insert(0, new ListItem("Select One", ""));
			this.ddlLocation.DataSource = this.SortList(searchParams.Locations);
			this.ddlLocation.DataBind();
			this.ddlLocation.Items.Insert(0, new ListItem("Select One", ""));
			this.ddlCourseLevel.DataSource = this.SortList(searchParams.CourseLevels);
			this.ddlCourseLevel.DataBind();
			this.ddlCourseLevel.Items.Insert(0, new ListItem("Select One", ""));
			this.ddlInstructor.DataSource = this.SortList(searchParams.Speakers);
			this.ddlInstructor.DataBind();
			this.ddlInstructor.Items.Insert(0, new ListItem("Select One", ""));
            this.ddlDeliveryTypes.DataSource = this.SortList(searchParams.DeliveryTypes);
            this.ddlDeliveryTypes.DataBind();
            this.ddlDeliveryTypes.Items.Insert(0, new ListItem("Select One", ""));
		}
		protected void btnSearch_OnClick(object sender, EventArgs e)
		{
			this.AlertWindow.Visible = false;
			this.DoSearch(false, true);
		}
		protected void btnInPerson_OnClick(object sender, EventArgs e)
		{
			if (base.Session["inPersonData"] == null)
			{
				this.DoSearch(false, false);
			}
			base.Session["searchResultData"] = base.Session["inPersonData"];
			this.SetInPerson();
			this.BindListView();
		}
		protected void btnOnDemand_OnClick(object sender, EventArgs e)
		{
			if (base.Session["onDemandData"] == null)
			{
				this.DoSearch(false, false);
			}
			base.Session["searchResultData"] = base.Session["onDemandData"];
			this.SetOnDemand();
			this.BindListView();
		}
		protected void SetInPerson()
		{
			this.liInPerson.Attributes.Add("class", "tab-current");
			this.liOnDemand.Attributes.Remove("class");
			this.lvDataPager1.SetPageProperties(0, 25, false);
		}
		protected void SetOnDemand()
		{
			this.liInPerson.Attributes.Remove("class");
			this.liOnDemand.Attributes.Add("class", "tab-current");
			this.lvDataPager1.SetPageProperties(0, 25, false);
		}
		protected void btnReset_OnClick(object sender, EventArgs e)
		{
			base.Response.Redirect("eventscalendar");
		}
		protected void ListViewResults_OnPagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
		{
			this.lvDataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
			this.BindListView();
		}
		private void BindListView()
		{
			this.ListViewResults.DataSource = base.Session["searchResultData"];
			this.ListViewResults.DataBind();
			this.updResults.Update();
		}
	}
}
