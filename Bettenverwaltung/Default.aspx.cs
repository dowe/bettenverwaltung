﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bettenverwaltung
{
    public partial class _Default : Page
    {
        // Local Keys (must be unique)
        // Keys for values that represent the current state of the view
        private const string KEY_SELECTED_BED_INDEX_ONE_BASED = "selectedBedIndex";
        private const string KEY_ACCEPTED_NOT_ID = "acceptedNotId";

        // Keys for the objects for recreation of controls after postback
        private const string KEY_POSTBACK_BEDS = "postbackBeds";
        private const string KEY_POSTBACK_RELOCATIONS = "postbackRelocations";
        private const string KEY_POSTBACK_SEARCH_RESULTS = "postbackSearchResults";
        private const string KEY_POSTBACK_MESSAGEBOX_TEXT = "postbackMessageBoxText";
        private const string KEY_POSTBACK_MESSAGEBOX_SHOW = "postbackMessageBoxShow";

        // Magic values for default state
        private const int VAL_SELECTED_BED_INDEX_ONE_BASED_NONE = -1;
        private const int VAL_ACCEPTED_NOT_ID_NONE = -1;

        // Constants for validation of user input
        private const int MAX_VARCHAR_LENGTH = 128;
        private readonly Color INVALID_COLOR = Color.Yellow;
        private readonly Color VALID_COLOR = Color.White;

        // Prefixes for id of dynamically generated controls (must be unique)
        private const string DYN_PREFIX_BED = "bed";
        private const string DYN_PREFIX_NOT = "not";
        private const string DYN_PREFIX_NOT_CANCEL = "notCancel";
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
                // Is No Postback
                InitSiteState();
                SwitchToDetailsTab();
                InitAllFromDB();
            }
            else
            {
                // Is Postback
                System.Diagnostics.Debug.WriteLine("Postback " + DateTime.Now.ToString());
                InitAllFromSiteState();
                if ((bool)GetSiteState(KEY_POSTBACK_MESSAGEBOX_SHOW))
                {
                    DisplayMessageBox((string)GetSiteState(KEY_POSTBACK_MESSAGEBOX_TEXT));
                }
            }
        }

        #region Init

        private void InitSiteState()
        {
            SetSiteState(KEY_SELECTED_BED_INDEX_ONE_BASED, VAL_SELECTED_BED_INDEX_ONE_BASED_NONE);
            SetSiteState(KEY_ACCEPTED_NOT_ID, VAL_ACCEPTED_NOT_ID_NONE);
            SetSiteState(KEY_POSTBACK_BEDS, null);
            SetSiteState(KEY_POSTBACK_RELOCATIONS, null);
            SetSiteState(KEY_POSTBACK_SEARCH_RESULTS, null);
            SetSiteState(KEY_POSTBACK_MESSAGEBOX_SHOW, false);
        }

        private void InitAllFromDB()
        {
            InitBedControlsFromDB();
            InitSearchResultControlsFromDB();
            InitRelocationControlsFromDB();
            InitTriggers();
        }
        /// <summary>
        /// Restores the dynamic controls from the SiteState if there are entries.
        /// If not the controls are recreated from the database.
        /// </summary>
        private void InitAllFromSiteState()
        {
            InitBedControlsFromSiteState();
            InitSearchResultControlsFromSiteState();
            InitRelocationControlsFromSiteState();
            InitTriggers();
        }

        private void ClearBedControls()
        {
            divStationPaediatrieBeds.Controls.Clear();
            divStationGynaekologieBeds.Controls.Clear();
            divStationInnereMedizinBeds.Controls.Clear();
            divStationOrthopaedieBeds.Controls.Clear();
        }
        private void ClearSearchResultControls()
        {
            divSearchResultList.Controls.Clear();
        }
        private void ClearRelocationControls()
        {
            divNotifications.Controls.Clear();
        }

        private void InitBedControlsFromDB()
        {
            try
            {
                List<IBedView> beds = controller.GetBettList();
                ClearBedControls();

                AddBedControls(beds.ToArray());

                // Store in SiteState
                SetSiteState(KEY_POSTBACK_BEDS, beds.ToArray());
            }
            catch (BedException e)
            {
                PrintErrorMessage(e);
            }
        }
        private void InitBedControlsFromSiteState()
        {
            if (GetSiteState(KEY_POSTBACK_BEDS) != null)
            {
                ClearBedControls();
                IBedView[] beds = (IBedView[])GetSiteState(KEY_POSTBACK_BEDS);
                AddBedControls(beds);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Beds where not in SiteState -> load from DB.");
                InitBedControlsFromDB();
            }
        }
        private void ReInitBedControlFromDB(int bedId)
        {
            IBedView[] beds = (IBedView[])GetSiteState(KEY_POSTBACK_BEDS);
            bool found = false;
            for (int i = 0; i < beds.Length && !found; i++)
            {
                if (beds[i].GetBedId() == bedId)
                {
                    beds[i] = controller.GetBedFromId(bedId);
                    found = true;
                }
            }
            if (found)
            {
                SetSiteState(KEY_POSTBACK_BEDS, beds);
                InitBedControlsFromSiteState();
            }
            else
            {
                InitBedControlsFromDB();
            }
        }
        private void AddBedControls(IBedView[] beds)
        {
            foreach (IBedView b in beds)
            {
                AddBedControl(b);
            }
        }
        private void AddBedControl(IBedView bed)
        {
            LinkButton btn = new LinkButton();

            // select CssClass
            if (bed.IsEmpty())
            {
                if (bed.IsGettingCleaned())
                {
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
            if (bed.GetBedId() == (int)GetSiteState(KEY_SELECTED_BED_INDEX_ONE_BASED))
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

        private void InitRelocationControlsFromDB()
        {
            try
            {
                ClearRelocationControls();
                List<Relocation> relocations = controller.GetActiveRelocationList();
                // first of all update deprecated accepted relocation id in SiteState
                if ((int)GetSiteState(KEY_ACCEPTED_NOT_ID) != VAL_ACCEPTED_NOT_ID_NONE)
                {
                    // this client has accepted a notification
                    bool foundAcceptedRelocation = false;
                    foreach (Relocation relocation in relocations)
                    {
                        if (relocation.GetId() == (int)GetSiteState(KEY_ACCEPTED_NOT_ID) && relocation.IsAccepted())
                        {
                            // this is the relocation accepted by this client and it is still active
                            foundAcceptedRelocation = true;
                        }
                    }
                    if (!foundAcceptedRelocation)
                    {
                        // the relocation accepted by this client is not active anymore or has been deleted completely from db
                        SetSiteState(KEY_ACCEPTED_NOT_ID, VAL_ACCEPTED_NOT_ID_NONE);
                    }
                }

                // display the relocations that shall be displayed for this client
                AddRelocationControls(relocations.ToArray());
                SetSiteState(KEY_POSTBACK_RELOCATIONS, relocations.ToArray());
            }
            catch (BedException exception)
            {
                PrintErrorMessage(exception);
            }
        }
        private void InitRelocationControlsFromSiteState()
        {
            if (GetSiteState(KEY_POSTBACK_RELOCATIONS) != null)
            {
                ClearRelocationControls();
                Relocation[] relocations = (Relocation[])GetSiteState(KEY_POSTBACK_RELOCATIONS);
                AddRelocationControls(relocations);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Relocations where not stored -> load from DB.");
                InitRelocationControlsFromDB();
            }
        }

        private void AddRelocationControls(Relocation[] relocations)
        {
            foreach (Relocation relocation in relocations)
            {
                if (shallRelocationBeDisplayed(relocation))
                {
                    AddRelocationControl(relocation);
                }
            }
        }
        private bool shallRelocationBeDisplayed(Relocation relocation)
        {
            bool res = false;
            // display this relocation ?
            if (GetSiteState(KEY_ACCEPTED_NOT_ID) != null && (int)GetSiteState(KEY_ACCEPTED_NOT_ID) != VAL_ACCEPTED_NOT_ID_NONE)
            {
                // this client has accepted a notification
                if (relocation.GetId() == (int)GetSiteState(KEY_ACCEPTED_NOT_ID))
                {
                    // this is the relocation accepted by this client
                    res = true;
                }
            }
            else
            {
                // this client has not accepted a notification
                if (relocation.IsAccepted() == false)
                {
                    // only display relocations that are not accepted by other clients
                    res = true;
                }
            }

            return res;
        }
        private void AddRelocationControl(Relocation relocation)
        {
            Panel panel = new Panel();
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
            string btnIdCancel = DYN_PREFIX_NOT_CANCEL + relocation.GetId().ToString();

            if (relocation.IsAccepted())
            {
                LinkButton btnCancel = new LinkButton();
                btnCancel.Text = "Abbrechen";
                btnCancel.ID = btnIdCancel;
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

        private void InitSearchResultControlsFromDB()
        {
            try
            {
                ClearSearchResultControls();
                List<IBedView> results = controller.SearchPatient(txtBoxSearchQuery.Text);
                AddSearchResultControls(results.ToArray());

                SetSiteState(KEY_POSTBACK_SEARCH_RESULTS, results.ToArray());
            }
            catch (BedException ex)
            {
                PrintErrorMessage(ex);
            }
        }
        private void InitSearchResultControlsFromSiteState()
        {
            if (GetSiteState(KEY_POSTBACK_SEARCH_RESULTS) != null)
            {
                ClearSearchResultControls();
                IBedView[] results = (IBedView[])GetSiteState(KEY_POSTBACK_SEARCH_RESULTS);
                AddSearchResultControls(results);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Relocations where not saved -> load from DB.");
                InitSearchResultControlsFromDB();
            }
        }
        private void AddSearchResultControls(IBedView[] results)
        {
            if (results.Length == 0)
            {
                AddSearchResultNoFound();
            }
            foreach (IBedView res in results)
            {
                AddSearchResultControl(res);
            }
        }
        private void AddSearchResultControl(IBedView bed)
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
            sb.Append(", ");
            sb.Append(bed.GetPatient().GetFirstName());
            sb.Append("; Station ");
            sb.Append(ConvertStationToString(bed.GetStation()));
            lblPatText.Text = sb.ToString();
            resultLinkButton.Controls.Add(lblPatText);

            divSearchResultList.Controls.Add(resultLinkButton);
        }

        private void AddSearchResultNoFound()
        {
            Label lblNoResults = new Label();
            lblNoResults.Text = "Kein passender Patient gefunden.";

            divSearchResultList.Controls.Add(lblNoResults);
        }

        private void InitTriggers()
        {
            InitOverviewTriggers();
            InitTabTriggers();
            InitRelocationTriggers();
            InitMessageTriggers();
        }

        private void InitOverviewTriggers()
        {
            UpdatePanel updatePanel = updatePanelOverview;
            updatePanel.Triggers.Clear();

            // bedButtons
            AddBedButtonsTriggers(updatePanel);

            // resTriggers
            AddSearchResultTriggers(updatePanel);
        }
        private void InitTabTriggers()
        {
            UpdatePanel updatePanel = updatePanelTabs;
            updatePanel.Triggers.Clear();

            AddSearchResultTriggers(updatePanel);
        }
        private void InitRelocationTriggers()
        {
            UpdatePanel updatePanel = updatePanelNot;
            updatePanel.Triggers.Clear();

            AddRelocationButtonsTriggers(updatePanel);
        }

        private void InitMessageTriggers()
        {
            UpdatePanel updatePanel = updatePanelMessage;
            updatePanel.Triggers.Clear();

            AddRelocationButtonsTriggers(updatePanel);
            AddBedButtonsTriggers(updatePanel);
            AddSearchResultTriggers(updatePanel);
        }

        private void AddBedButtonsTriggers(UpdatePanel updatePanel)
        {
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
        }

        private void AddRelocationButtonsTriggers(UpdatePanel updatePanel)
        {
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

        private void AddSearchResultTriggers(UpdatePanel updatePanel)
        {
            var resButtons = divSearchResultList.Controls.OfType<LinkButton>();
            foreach (LinkButton btn in resButtons)
            {
                AddTrigger(btn, updatePanel);
            }
        }

        private void AddTrigger(Control ctrl, UpdatePanel panel)
        {
            UpdatePanelTrigger trigger = CreateTrigger(ctrl.ID);
            panel.Triggers.Add(trigger);
        }
        private UpdatePanelTrigger CreateTrigger(string controlId)
        {
            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = controlId;
            return trigger;
        }

        #endregion Init

        protected virtual void Bed_Buttons_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnSender = (LinkButton)sender;
                string id = btnSender.ID;
                id = id.Remove(0, DYN_PREFIX_BED.Length);
                int idOneBased = Int32.Parse(id);
                DisplayBed(controller.GetBedFromId(idOneBased));
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
                if ((int)GetSiteState(KEY_SELECTED_BED_INDEX_ONE_BASED) != VAL_SELECTED_BED_INDEX_ONE_BASED_NONE)
                {
                    int selectedBedId = (int)GetSiteState(KEY_SELECTED_BED_INDEX_ONE_BASED);
                    DisplayBed(controller.DismissPatient(selectedBedId));
                    ReInitBedControlFromDB(selectedBedId);
                }
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
                        ReInitBedControlFromDB(destBed.GetBedId());
                        DisplayBed(destBed);
                    }

                }
                else
                {
                    ShowMessageBox("Bitte überprüfen sie die eingegebenen Daten noch einmal.");
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
            InitSearchResultControlsFromDB();
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

                Relocation rel = controller.AcceptRelocation(relId);
                SetSiteState(KEY_ACCEPTED_NOT_ID, relId);

                ReInitBedControlFromDB(rel.GetDestinationBed().GetBedId());
                InitRelocationControlsFromDB();
            }
            catch (BedException ex)
            {
                PrintErrorMessage(ex);
            }
        }

        protected virtual void Cancel_Relocation_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                int relId = Int32.Parse(btn.ID.Remove(0, DYN_PREFIX_NOT_CANCEL.Length));

                Relocation rel = controller.CancelRelocation(relId);
                SetSiteState(KEY_ACCEPTED_NOT_ID, VAL_ACCEPTED_NOT_ID_NONE);

                ReInitBedControlFromDB(rel.GetDestinationBed().GetBedId());
                InitRelocationControlsFromDB();
            }
            catch (BedException ex)
            {
                PrintErrorMessage(ex);
                InitBedControlsFromDB();
                InitRelocationControlsFromDB();
            }
        }

        protected virtual void Confirm_Relocation_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                int relId = Int32.Parse(btn.ID.Remove(0, DYN_PREFIX_NOT.Length));

                controller.ConfirmRelocation(relId);
                SetSiteState(KEY_ACCEPTED_NOT_ID, VAL_ACCEPTED_NOT_ID_NONE);

                InitBedControlsFromDB();
                InitRelocationControlsFromDB();
            }
            catch (BedException ex)
            {
                InitBedControlsFromDB();
                PrintErrorMessage(ex);
            }
        }

        protected virtual void Search_Item_Click(object sender, EventArgs e)
        {
            int bedIdOneBased;
            LinkButton btn = (LinkButton)sender;
            bedIdOneBased = Int32.Parse(btn.ID.Remove(0, DYN_PREFIX_SEARCH_RESULT.Length));
            DisplayBed(controller.GetBedFromId(bedIdOneBased));
        }

        protected virtual void MessageBox_Okay_Click(object sender, EventArgs e)
        {
            HideMessageBox();
        }

        protected virtual void Update_Overview_And_Notifications_Tick(object sender, EventArgs e)
        {
            InitBedControlsFromDB();
            InitRelocationControlsFromDB();
            InitTriggers();
        }

        protected virtual void PrintErrorMessage(BedException e)
        {
            ShowMessageBox(e.Message);
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
                if (pat.IsFemale())
                {
                    genderString = "w";
                }
                else
                {
                    genderString = "m";
                }
                txtBoxDetailsPatGender.Text = genderString;
                txtBoxDetailsPatBirthday.Text = pat.GetBirthday().ToShortDateString();

                txtBoxDetailsPatCorrectStation.Text = ConvertStationToString(pat.GetCorrectStation());

                txtBoxDetailsPatHistory.Text = "";
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < pat.GetHistory().GetSize(); i++)
                {
                    HistoryItem historyItem = pat.GetHistory().GetHistoryItem(i);
                    sb.AppendLine(historyItem.GetText());
                }
                txtBoxDetailsPatHistory.Text = sb.ToString();
            }
            else
            {
                // no patient is in the bed -> clear fields
                txtBoxDetailsPatId.Text = "";
                txtBoxDetailsPatName.Text = "";
                txtBoxDetailsPatBirthday.Text = "";
                txtBoxDetailsPatGender.Text = "";
                txtBoxDetailsPatCorrectStation.Text = "";
                txtBoxDetailsPatHistory.Text = "";
            }
            // display bed info in any case
            txtBoxDetailsBedId.Text = bed.GetBedId().ToString();
            txtBoxDetailsBedStation.Text = ConvertStationToString(bed.GetStation());

            // mark the bed as selected
            SetSiteState(KEY_SELECTED_BED_INDEX_ONE_BASED, bed.GetBedId());

            InitBedControlsFromSiteState();

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
            System.Diagnostics.Debug.WriteLine("Validierung fehlgeschlagen.");
            //System.Web.HttpContext.Current.Response.Write("<script>alert('" + message + "')</script>");
            DisplayMessageBox(message);
            SetSiteState(KEY_POSTBACK_MESSAGEBOX_TEXT, message);
            SetSiteState(KEY_POSTBACK_MESSAGEBOX_SHOW, true);
        }

        private void DisplayMessageBox(string message)
        {
            divOverlay.Visible = true;
            lblMessageBoxText.Text = message;
            btnMessageBoxCancel.Visible = false;
            System.Diagnostics.Debug.WriteLine("MessageBox: " + message);
        }

        private void HideMessageBox()
        {
            divOverlay.Visible = false;
            SetSiteState(KEY_POSTBACK_MESSAGEBOX_SHOW, false);
        }

        private void SetSiteState(string key, object o)
        {
            // Cache Solution
            //if (o == null) Cache.Remove(key);
            //else Cache[key] = o;

            // ViewState Solution
            ViewState[key] = o;
        }

        private object GetSiteState(string key)
        {
            // Cache Solution
            //return Cache[key];

            // ViewState Solution
            return ViewState[key];
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
    }
}