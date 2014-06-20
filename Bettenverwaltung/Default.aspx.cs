using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bettenverwaltung
{
    public partial class _Default : Page
    {
        private const string VSKEY_SELECTED_BED_INDEX_ONE_BASED = "selectedBedIndex";
        private const string VSKEY_ACCEPTED_NOT_ID = "acceptedNotId";
        private const int VSVAL_SELECTED_BED_INDEX_ONE_BASED_NONE = -1;
        private const int VSVAL_ACCEPTED_NOT_ID_NONE = -1;

        private const int NUMBER_OF_BEDS = 200;

        private const int MAX_VARCHAR_LENGTH = 255;
        private readonly Color INVALID_COLOR =  Color.Yellow;
        private readonly Color VALID_COLOR =    Color.White;

        // dynamic ID Prefixes
        private const string DYN_PREFIX_BED = "bed";
        private const string DYN_PREFIX_NOT = "not";
        private const string DYN_PREFIX_SEARCH_RESULT = "res";

        // Css Classes
        private const String CSS_CLASS_BED_FREE = "btnBedFree";
        private const String CSS_CLASS_BED_IN_RELOCATION = "btnBedInRelocation";
        private const String CSS_CLASS_BED_WRONG_STATION = "btnBedWrongStation";
        private const String CSS_CLASS_BED_CLEANING = "btnBedCleaning";
        private const String CSS_CLASS_BED_OCCUPIED = "btnBedOccupied";
        private const String CSS_CLASS_BED_SELECTED = "btnBedSelected";

        private const String CSS_CLASS_NOT_LIST_ITEM = "divNotListItem";

        private const String CSS_CLASS_BTN_TAB_ACTIVE = "btnTabActive";
        private const String CSS_CLASS_BTN_TAB_INACTIVE = "btnTabInactive";
        private const String CSS_CLASS_DIV_TAB_ACTIVE = "divTabActive";
        private const String CSS_CLASS_DIV_TAB_INACTIVE = "divTabInactive";

        private const String CSS_CLASS_BTN_NOT_ACCEPT = "btnAcceptRel";
        private const String CSS_CLASS_BTN_NOT_CANCEL = "btnCancelRel";
        private const String CSS_CLASS_BTN_NOT_CONFIRM = "btnConfirmRel";

        private const String CSS_CLASS_LBL_PAT_ID = "lblPatId";
        private const String CSS_CLASS_BTN_SEARCH_RESULT_LIST_ITEM = "btnSearchResultListItem";

        private IController controller;

        public _Default()// : this(new TestController()) // "Mock" Test
        {
            this.controller = new Controller();
        }

        public _Default(IController controller)
        {
            this.controller = controller;
        }

		protected virtual void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                InitViewState();
                SwitchToDetailsTab();
            }

            InitAll();
		}

        private void InitAll()
        {
            InitSearchResults();
            InitBeds();
            InitRelocations();
            InitTriggers();
        }

        private void InitViewState() {
            ViewState.Add(VSKEY_SELECTED_BED_INDEX_ONE_BASED, VSVAL_SELECTED_BED_INDEX_ONE_BASED_NONE);
            ViewState.Add(VSKEY_ACCEPTED_NOT_ID, VSVAL_ACCEPTED_NOT_ID_NONE);
        }

		private void InitBeds()
		{
            try
            {
                List<IBedView> beds = controller.GetBettList();
                divStationPaediatrieBeds.Controls.Clear();
                divStationGynaekologieBeds.Controls.Clear();
                divStationInnereMedizinBeds.Controls.Clear();
                divStationOrthopaedieBeds.Controls.Clear();

                //beds.Sort(); // make sure the beds are added in correct order

                foreach (IBedView bed in beds) {
                    AddBed(bed);
                }
            }
            catch (BedException e)
            {
                PrintErrorMessage(e);
            }
		}
        private void AddBed(IBedView bed)
        {
            LinkButton btn = new LinkButton();

            // select CssClass
            if (bed.IsEmpty())
            {
                if (bed.IsGettingCleaned()) {
                    btn.CssClass = CSS_CLASS_BED_CLEANING;
                }
                else if (bed.IsInRelocation())
                {
                    btn.CssClass = CSS_CLASS_BED_IN_RELOCATION;
                }
                else
                {
                    // bed is not part of a relocation and is not getting cleaned
                    btn.CssClass = CSS_CLASS_BED_FREE;
                }
            }
            else
            {
                if (bed.GetPatient().GetCorrectStation() != bed.GetStation())
                {
                    // bed is occupied but patient belongs to another station
                    btn.CssClass = CSS_CLASS_BED_WRONG_STATION;
                }
                else
                {
                    // bed is occupied and patient is in correct station
                    btn.CssClass = CSS_CLASS_BED_OCCUPIED;
                }
            }

            // check if this bed is selected
            if (bed.GetBedId() == (int)ViewState[VSKEY_SELECTED_BED_INDEX_ONE_BASED])
            {
                StringBuilder sb = new StringBuilder(btn.CssClass);
                sb.Append(" ");
                sb.Append(CSS_CLASS_BED_SELECTED);
                btn.CssClass = sb.ToString();
            }

            // assign click event
            btn.ID = "bed" + bed.GetBedId().ToString();
            btn.Click += Bed_Buttons_Click;

            // add to div
            switch (bed.GetStation())
            {
                case EStation.Gynaekologie:
                    divStationGynaekologieBeds.Controls.Add(btn);
                    break;
                case EStation.Innere_Medizin:
                    divStationInnereMedizinBeds.Controls.Add(btn);
                    break;
                case EStation.Orthopaedie:
                    divStationOrthopaedieBeds.Controls.Add(btn);
                    break;
                case EStation.Paediatrie:
                    divStationPaediatrieBeds.Controls.Add(btn);
                    break;
            }
        }

		protected virtual void Bed_Buttons_Click(object sender, EventArgs e)
		{
            try
            {
                LinkButton btnSender = (LinkButton)sender;
                string id = btnSender.ID;
                id = id.Remove(0, DYN_PREFIX_BED.Length);
                int idOneBased = Int32.Parse(id);
                DisplayBed(controller.DisplayPatient(idOneBased));
            }
            catch (BedException exception)
            {
                PrintErrorMessage(exception);
            }
		}

		protected virtual void Dismiss_Patient_Click(object sender, EventArgs e)
		{
            try
            {
                int selectedBedId = (int)ViewState[VSKEY_SELECTED_BED_INDEX_ONE_BASED];
                DisplayBed(controller.DismissPatient(selectedBedId));
            }
            catch (BedException ex)
            {
                PrintErrorMessage(ex);
            }
		}

		protected virtual void Add_Patient_Click(object sender, EventArgs e)
		{
            try
            {
                if (ValidateAddPatientInput() == true)
                {
                    // there should not be any parsing errors beyond this point
                    string firstName = txtBoxAddPatFirstName.Text;
                    string lastName = txtBoxAddPatLastName.Text;
                    EStation station = ConvertStringToStation(dropDownListAddPatStation.Text);
                    DateTime birthday = DateTime.Parse(txtBoxAddPatBirthday.Text);
                    bool isFemale = dropDownListAddPatGender.Text.Equals("w");

                    IBedView destBed = controller.AddPatient(
                        firstName,
                        lastName,
                        station,
                        birthday,
                        isFemale);

                    if (destBed == null)
                    {
                        ShowMessageBox("Es wurde kein geeignetes Bett für den Patienten gefunden. Er muss in ein anderes Krankenhaus verlegt werden.");
                    }
                    else
                    {
                        DisplayBed(destBed);
                    }

                }
            }
            catch (BedException exception)
            {
                PrintErrorMessage(exception);
            }
		}

        private void ResetValidationWarnings()
        {
            txtBoxAddPatFirstName.BackColor = VALID_COLOR;
            txtBoxAddPatFirstName.ToolTip = "";

            txtBoxAddPatLastName.BackColor = VALID_COLOR;
            txtBoxAddPatLastName.ToolTip = "";

            txtBoxAddPatBirthday.BackColor = VALID_COLOR;
            txtBoxAddPatBirthday.ToolTip = "";
        }

        private bool ValidateAddPatientInput()
        {
            bool res = true;

            ResetValidationWarnings();
            // validate first name
            if (!ValidateTextBoxNotEmpty(txtBoxAddPatFirstName))
            {
                res = false;
            }
            if (txtBoxAddPatFirstName.Text.Length > MAX_VARCHAR_LENGTH)
            {
                res = false;
                txtBoxAddPatFirstName.BackColor = INVALID_COLOR;
                txtBoxAddPatFirstName.ToolTip = "Darf maximal " + MAX_VARCHAR_LENGTH.ToString() + " Zeichen lang sein.";
            }
            
            // validate last name
            if (!ValidateTextBoxNotEmpty(txtBoxAddPatLastName))
            {
                res = false;
            }
            if (txtBoxAddPatLastName.Text.Length > MAX_VARCHAR_LENGTH)
            {
                res = false;
                txtBoxAddPatLastName.BackColor = INVALID_COLOR;
                txtBoxAddPatLastName.ToolTip = "Darf maximal " + MAX_VARCHAR_LENGTH.ToString() + " Zeichen lang sein.";
            }

            // validate birthday
            bool parseResult;
            DateTime parseDate;
            parseResult = DateTime.TryParse(txtBoxAddPatBirthday.Text, out parseDate);
            if (!parseResult)
            {
                res = false;
                txtBoxAddPatBirthday.BackColor = INVALID_COLOR;
                txtBoxAddPatBirthday.ToolTip = "Verwenden Sie das Format 'DD:MM:YYYY'.";
            }

            // show message if necessary
            if (res == false)
            {
                ShowMessageBox("Bitte überprüfen sie die eingegebenen Daten noch einmal.");
            }
            
            return res;
        }

        private bool ValidateTextBoxNotEmpty(TextBox box)
        {
            bool res = true;

            if (box.Text.Length == 0)
            {
                res = false;
                box.BackColor = INVALID_COLOR;
                box.ToolTip = "Darf nicht leer sein.";
            }

            return res;
        }

		protected virtual void Search_Click(object sender, EventArgs e)
		{
            InitSearchResults();
		}

        private void InitSearchResults()
        {
            try
            {
                divSearchResultList.Controls.Clear();
                List<IBedView> result = controller.SearchPatient(txtBoxSearchQuery.Text);
                foreach (IBedView bed in result)
                {
                    AddSearchResultItem(bed);
                }
            }
            catch (BedException ex)
            {
                PrintErrorMessage(ex);
            }
        }

        private void AddSearchResultItem(IBedView bed)
        {
            LinkButton resultLinkButton = new LinkButton();
            resultLinkButton.ID = DYN_PREFIX_SEARCH_RESULT + bed.GetBedId();
            resultLinkButton.CssClass = CSS_CLASS_BTN_SEARCH_RESULT_LIST_ITEM;
            resultLinkButton.Click += Search_Item_Click;

            Label lblPatId = new Label();
            lblPatId.Text = bed.GetPatient().GetPatientId().ToString();
            lblPatId.CssClass = CSS_CLASS_LBL_PAT_ID;
            resultLinkButton.Controls.Add(lblPatId);

            Label lblPatText = new Label();
            StringBuilder sb = new StringBuilder();
            sb.Append(" - ");
            sb.Append(bed.GetPatient().GetLastName());
            sb.Append(" ");
            sb.Append(bed.GetPatient().GetFirstName());
            sb.Append(", Station ");
            sb.Append(bed.GetStation());
            lblPatText.Text = sb.ToString();
            resultLinkButton.Controls.Add(lblPatText);

            divSearchResultList.Controls.Add(resultLinkButton);
        }

		protected virtual void Tab_Details_Click(object sender, EventArgs e)
		{
            SwitchToDetailsTab();
		}

		protected virtual void Tab_Search_Click(object sender, EventArgs e)
		{
            SwitchToSearchTab();
		}

		protected virtual void Tab_Add_Click(object sender, EventArgs e)
		{
            SwitchToAddTab();
		}

		protected virtual void Accept_Relocation_Click(object sender, EventArgs e)
		{
            try
            {
                LinkButton btn = (LinkButton)sender;
                int relId = Int32.Parse(btn.ID.Remove(0, DYN_PREFIX_NOT.Length));
                controller.AcceptRelocation(relId);
                ViewState[VSKEY_ACCEPTED_NOT_ID] = relId;
                InitAll();
            }
            catch (BedException ex)
            {
                PrintErrorMessage(ex);
            }
		}

		protected virtual void Cancel_Relocation_Click(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Confirm_Relocation_Click(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Search_Item_Click(object sender, EventArgs e)
		{
            int bedIdOneBased;
            LinkButton btn = (LinkButton)sender;
            bedIdOneBased = Int32.Parse(btn.ID.Remove(0, DYN_PREFIX_SEARCH_RESULT.Length));
            DisplayBed(controller.DisplayPatient(bedIdOneBased));
		}

		private void InitRelocations()
		{
            try
            {
                divNotifications.Controls.Clear();
                List<Relocation> relocations = controller.GetActiveRelocationList();
                // first update deprecated accepted relocation
                if ((int)ViewState[VSKEY_ACCEPTED_NOT_ID] != VSVAL_ACCEPTED_NOT_ID_NONE)
                {
                    // this client has accepted a notification
                    bool foundAcceptedRelocation = false;
                    foreach (Relocation relocation in relocations)
                    {
                        if (relocation.GetId() == (int)ViewState[VSKEY_ACCEPTED_NOT_ID] && relocation.IsActive())
                        {
                            // this is the relocation accepted by this client and it is still active
                            foundAcceptedRelocation = true;
                        }
                    }
                    if (!foundAcceptedRelocation)
                    {
                        // the relocation accepted by this client is not active anymore or has been deleted completely from db
                        ViewState[VSKEY_ACCEPTED_NOT_ID] = VSVAL_ACCEPTED_NOT_ID_NONE;
                    }
                }

                foreach (Relocation relocation in relocations)
                {
                    // display this relocation ?
                    if (ViewState[VSKEY_ACCEPTED_NOT_ID] != null && (int)ViewState[VSKEY_ACCEPTED_NOT_ID] != VSVAL_ACCEPTED_NOT_ID_NONE)
                    {
                        // this client has accepted a notification
                        if (relocation.GetId() == (int)ViewState[VSKEY_ACCEPTED_NOT_ID])
                        {
                            // this is the relocation accepted by this client
                            AddRelocation(relocation);
                        }
                    }
                    else
                    {
                        // this client has not accepted a notification
                        if (relocation.IsAccepted() == false)
                        {
                            // only display relocations that are not accepted by other clients
                            AddRelocation(relocation);
                        }
                    }
                }
            }
            catch (BedException exception)
            {
                PrintErrorMessage(exception);
            }
		}

        private void AddRelocation(Relocation relocation)
        {
            Panel panel = new Panel();
            panel.ID = DYN_PREFIX_NOT + relocation.GetId().ToString();
            panel.CssClass = CSS_CLASS_NOT_LIST_ITEM;

            Label content = new Label();
            StringBuilder sb = new StringBuilder();
            Patient pat = relocation.GetSourceBed().GetPatient();

            sb.Append(pat.GetLastName());
            sb.Append(", ");
            sb.Append(pat.GetFirstName());
            sb.Append(" (PID ");
            sb.Append(pat.GetPatientId().ToString());
            sb.Append(")<br />");
            
            sb.Append("Derzeit: Bett ");
            sb.Append(relocation.GetSourceBed().GetBedId().ToString());
            sb.Append(", Station ");
            sb.Append(ConvertStationToString(relocation.GetSourceBed().GetStation()));
            sb.Append("<br />");
            
            sb.Append("Nach:    Bett ");
            sb.Append(relocation.GetDestinationBed().GetBedId().ToString());
            sb.Append(", Station ");
            sb.Append(ConvertStationToString(relocation.GetDestinationBed().GetStation()));
            sb.Append("<br />");

            content.Text = sb.ToString();
            panel.Controls.Add(content);

            string btnId = DYN_PREFIX_NOT + relocation.GetId().ToString();

            if (relocation.IsAccepted())
            {
                LinkButton btnCancel = new LinkButton();
                btnCancel.Text = "Abbrechen";
                btnCancel.ID = btnId;
                btnCancel.CssClass = CSS_CLASS_BTN_NOT_CANCEL;
                btnCancel.Click += Cancel_Relocation_Click;
                panel.Controls.Add(btnCancel);

                LinkButton btnConfirm = new LinkButton();
                btnConfirm.Text = "Bestätigen";
                btnConfirm.ID = btnId;
                btnConfirm.CssClass = CSS_CLASS_BTN_NOT_CONFIRM;
                btnConfirm.Click += Confirm_Relocation_Click;
                panel.Controls.Add(btnConfirm);
            }
            else
            {
                LinkButton btnAccept = new LinkButton();
                btnAccept.Text = "Annehmen";
                btnAccept.ID = btnId;
                btnAccept.CssClass = CSS_CLASS_BTN_NOT_ACCEPT;
                btnAccept.Click += Accept_Relocation_Click;
                panel.Controls.Add(btnAccept);
            }
            divNotifications.Controls.Add(panel);
        }

		protected virtual void Update_Overview_Tick(object sender, EventArgs e)
		{
			InitBeds();
            InitTriggers();
		}

		protected virtual void Update_Notification_Tick(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void PrintErrorMessage(BedException e)
		{
            ShowMessageBox(e.Message);
		}

		private int GetAcceptedRelocationId()
		{
			throw new System.NotImplementedException();
		}

		private void SetAcceptedRelocationId(int? id)
		{
			throw new System.NotImplementedException();
		}

		private void DisplayBed(IBedView bed)
		{
            // display patient if there is one in the bed
            if (!bed.IsEmpty())
            {
                Patient pat = bed.GetPatient();
                txtBoxDetailsPatId.Text = pat.GetPatientId().ToString();
                txtBoxDetailsPatName.Text = pat.GetLastName() + ", " + pat.GetFirstName();
                string genderString;
                if (pat.IsFemale()) {
                    genderString = "w";
                }
                else {
                    genderString = "m";
                }
                txtBoxDetailsPatGender.Text = genderString;
                txtBoxDetailsPatBirthday.Text = pat.GetBirthday().ToString();
                
                txtBoxDetailsPatCorrectStation.Text = ConvertStationToString(pat.GetCorrectStation());

                listBoxDetailsPatHistory.Items.Clear();
                for (int i = 0; i < pat.GetHistory().GetSize(); i++)
                {
                    HistoryItem historyItem = pat.GetHistory().GetHistoryItem(i);
                    listBoxDetailsPatHistory.Items.Add(new ListItem(historyItem.GetText()));
                }
            }
            else
            {
                // no patient is in the bed -> clear fields
                txtBoxDetailsPatId.Text = "";
                txtBoxDetailsPatName.Text = "";
                txtBoxDetailsPatBirthday.Text = "";
                txtBoxDetailsPatGender.Text = "";
                txtBoxDetailsPatCorrectStation.Text = "";
                listBoxDetailsPatHistory.Items.Clear();
            }
            // display bed info in any case
            txtBoxDetailsBedId.Text = bed.GetBedId().ToString();
            txtBoxDetailsBedStation.Text = ConvertStationToString(bed.GetStation());

            // mark the bed as selected
            ViewState.Add(VSKEY_SELECTED_BED_INDEX_ONE_BASED, bed.GetBedId());

            InitBeds();

            SwitchToDetailsTab();
		}

        private void SwitchToDetailsTab()
        {
            btnTabDetails.CssClass = CSS_CLASS_BTN_TAB_ACTIVE;
            btnTabSearch.CssClass = CSS_CLASS_BTN_TAB_INACTIVE;
            btnTabAdd.CssClass = CSS_CLASS_BTN_TAB_INACTIVE;

            divTabDetails.CssClass = CSS_CLASS_DIV_TAB_ACTIVE;
            divTabSearch.CssClass = CSS_CLASS_DIV_TAB_INACTIVE;
            divTabAdd.CssClass = CSS_CLASS_DIV_TAB_INACTIVE;
        }

        private void SwitchToSearchTab()
        {
            btnTabDetails.CssClass = CSS_CLASS_BTN_TAB_INACTIVE;
            btnTabSearch.CssClass = CSS_CLASS_BTN_TAB_ACTIVE;
            btnTabAdd.CssClass = CSS_CLASS_BTN_TAB_INACTIVE;

            divTabDetails.CssClass = CSS_CLASS_DIV_TAB_INACTIVE;
            divTabSearch.CssClass = CSS_CLASS_DIV_TAB_ACTIVE;
            divTabAdd.CssClass = CSS_CLASS_DIV_TAB_INACTIVE;
        }

        private void SwitchToAddTab()
        {
            btnTabDetails.CssClass = CSS_CLASS_BTN_TAB_INACTIVE;
            btnTabSearch.CssClass = CSS_CLASS_BTN_TAB_INACTIVE;
            btnTabAdd.CssClass = CSS_CLASS_BTN_TAB_ACTIVE;

            divTabDetails.CssClass = CSS_CLASS_DIV_TAB_INACTIVE;
            divTabSearch.CssClass = CSS_CLASS_DIV_TAB_INACTIVE;
            divTabAdd.CssClass = CSS_CLASS_DIV_TAB_ACTIVE;
        }

        private void ShowMessageBox(string message)
        {
            System.Web.HttpContext.Current.Response.Write("<script>alert('" + message + "')</script>");
        }

        private string ConvertStationToString(EStation station)
        {
            string stationString;
            switch (station)
            {
                case EStation.Paediatrie:
                    stationString = "Pädiatrie";
                    break;
                case EStation.Gynaekologie:
                    stationString = "Gynäkologie";
                    break;
                case EStation.Innere_Medizin:
                    stationString = "Innere Medizin";
                    break;
                case EStation.Orthopaedie:
                default:
                    stationString = "Orthopädie";
                    break;
            }
            return stationString;
        }

        private EStation ConvertStringToStation(string strStation)
        {
            EStation station;
            switch (dropDownListAddPatStation.Text)
            {
                case "Pädiatrie":
                    station = EStation.Paediatrie;
                    break;
                case "Gynäkologie":
                    station = EStation.Gynaekologie;
                    break;
                case "Innere Medizin":
                    station = EStation.Innere_Medizin;
                    break;
                case "Orthopädie":
                default:
                    station = EStation.Orthopaedie;
                    break;
            }
            return station;
        }

        private UpdatePanelTrigger CreateTrigger(string controlId)
        {
            PostBackTrigger trigger = new PostBackTrigger();
            trigger.ControlID = controlId;
            return trigger;
        }

        private void InitTriggers()
        {
            InitOverviewTriggers();
            InitTabTriggers();
            InitRelocationTriggers();
        }

        private void InitOverviewTriggers()
        {
            UpdatePanel updatePanel = updatePanelOverview;
            updatePanel.Triggers.Clear();
            
            // bedButtons
            foreach (LinkButton bedBtn in divStationPaediatrieBeds.Controls)
            {
                AddTrigger(bedBtn, updatePanel);
            }
            foreach (LinkButton bedBtn in divStationGynaekologieBeds.Controls)
            {
                AddTrigger(bedBtn, updatePanel);
            }
            foreach (LinkButton bedBtn in divStationInnereMedizinBeds.Controls)
            {
                AddTrigger(bedBtn, updatePanel);
            }
            foreach (LinkButton bedBtn in divStationOrthopaedieBeds.Controls)
            {
                AddTrigger(bedBtn, updatePanel);
            }

            // resTriggers
            foreach (LinkButton resBtn in divSearchResultList.Controls)
            {
                AddTrigger(resBtn, updatePanel);
            }
        }

        private void InitTabTriggers()
        {
            UpdatePanel updatePanel = updatePanelTabs;
            updatePanel.Triggers.Clear();

            foreach (LinkButton btn in divSearchResultList.Controls)
            {
                AddTrigger(btn, updatePanel);
            }
        }

        private void InitRelocationTriggers()
        {
            UpdatePanel updatePanel = updatePanelNot;
            updatePanel.Triggers.Clear();

            var updatePanelPanels = divNotifications.Controls.OfType<Panel>();
            foreach (Panel pnl in updatePanelPanels)
            {
                var btns = pnl.Controls.OfType<LinkButton>();
                foreach (LinkButton btn in btns)
                {
                    UpdatePanelTrigger trigger = CreateTrigger(btn.ID);
                    updatePanel.Triggers.Add(trigger);
                }
            }
        }

        private void AddTrigger(Control ctrl, UpdatePanel panel)
        {
            UpdatePanelTrigger trigger = CreateTrigger(ctrl.ID);
            panel.Triggers.Add(trigger);
        }
    }
}