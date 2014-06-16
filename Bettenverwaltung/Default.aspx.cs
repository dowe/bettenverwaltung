using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bettenverwaltung
{
    public partial class _Default : Page
    {
        private const int NUMBER_OF_BEDS = 200;

        private const int MAX_VARCHAR_LENGTH = 255;

        private readonly Color INVALID_COLOR =  Color.Yellow;
        private readonly Color VALID_COLOR =    Color.White;

        // Css Classes
        private const String CSS_CLASS_BTN_ACTIVE = "btnTabActive";
        private const String CSS_CLASS_BTN_INACTIVE = "btnTabInactive";
        private const String CSS_CLASS_DIV_ACTIVE = "divTabActive";
        private const String CSS_CLASS_DIV_INACTIVE = "divTabInactive";

        private Button[] bedButtons = new Button[NUMBER_OF_BEDS];

        private IController controller;

        //Entity Test
        //**********************************************
        BVContext db;
        //**********************************************
        public _Default()
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
                SwitchToDetailsTab();
            }
            // TEST
            not0.Style.Add("display", "none");
            not1.Style.Add("display", "none");
            // END TEST
            InitBeds();
            /*db = new BVContext();
            Bed b0 = new Bed();
            b0.bedId = 0;
            db.Beds.Add(b0);
            Bed b1 = new Bed();
            b1.bedId = 200;
            db.Beds.Add(b1);

            db.SaveChanges();

            var beds = from b in db.Beds orderby b.bedId select b;
            foreach (var bed in beds)
            {
                Console.WriteLine(bed.bedId);
            }

            DateTime dt = DateTime.Today;
            Patient p1 = new Patient("peter", "enis", dt, true, 25, 0, 0);
            db.Patients.Add(p1);
            db.SaveChanges();
            Patient patients = db.Patients.Find(25);
            
			//throw new System.NotImplementedException();*/
		}

		private void InitBeds()
		{
            return; // For Testing because controller.GetBettList() is not implemented yet
            List<IBedView> beds = controller.GetBettList();

            beds.Sort(); // make sure the beds are added in correct order

            foreach (IBedView bed in beds) {
                AddBed(bed);
            }
		}
        private void AddBed(IBedView bed)
        {
            LinkButton btn = new LinkButton();

            // select CssClass
            if (bed.IsEmpty())
            {
                if (bed.IsGettingCleaned()) {
                    btn.CssClass = "btnBedCleaning";
                }
                if (bed.IsInRelocation())
                {
                    btn.CssClass = "btnBedInRelocation";
                }
                else
                {
                    // bed is not part of a relocation and is not getting cleaned
                    btn.CssClass = "btnBedFree";
                }
            }
            else
            {
                // someone is in this bed
                btn.CssClass = "btnBedOccupied";
            }

            // assign click event
            btn.ID = bed.GetBedId().ToString();
            btn.Click += Bed_Buttons_Click;

            // add to div
            switch (bed.GetStation())
            {
                case EStation.Gynaekologie:
                    divStationGynaekologie.Controls.Add(btn);
                    break;
                case EStation.Innere_Medizin:
                    divStationInnereMedizin.Controls.Add(btn);
                    break;
                case EStation.Orthopaedie:
                    divStationOrthopaedie.Controls.Add(btn);
                    break;
                case EStation.Paediatrie:
                    divStationPaediatrie.Controls.Add(btn);
                    break;
            }
        }

		protected virtual void Bed_Buttons_Click(object sender, EventArgs e)
		{
            LinkButton btnSender = (LinkButton)sender;
            int id = Int32.Parse(btnSender.ID);
            DisplayBed(controller.DisplayPatient(id));
		}

		protected virtual void Dismiss_Patient_Click(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Add_Patient_Click(object sender, EventArgs e)
		{
            if (ValidateAddPatientInput() == true)
            {
                // there should not be any parsing errors beyond this point
                string firstName = txtBoxAddPatFirstName.Text;
                string lastName = txtBoxAddPatLastName.Text;
                EStation station;
                switch (txtBoxAddPatCorrectStation.Text)
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
                DateTime birthday = DateTime.Parse(txtBoxAddPatBirthday.Text);
                bool isFemale = txtBoxAddPatGender.Text.Equals("w");

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

        private void ResetValidationWarnings()
        {
            txtBoxAddPatFirstName.BackColor = VALID_COLOR;
            txtBoxAddPatFirstName.ToolTip = "";

            txtBoxAddPatLastName.BackColor = VALID_COLOR;
            txtBoxAddPatLastName.ToolTip = "";

            txtBoxAddPatGender.BackColor = VALID_COLOR;
            txtBoxAddPatGender.ToolTip = "";

            txtBoxAddPatBirthday.BackColor = VALID_COLOR;
            txtBoxAddPatBirthday.ToolTip = "";

            txtBoxAddPatCorrectStation.BackColor = VALID_COLOR;
            txtBoxAddPatCorrectStation.ToolTip = "";
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

            // validate gender
            if (!ValidateTextBoxNotEmpty(txtBoxAddPatGender))
            {
                res = false;
            }
            if (!txtBoxAddPatGender.Text.Equals("m") && !txtBoxAddPatGender.Text.Equals("w"))
            {
                res = false;
                txtBoxAddPatGender.BackColor = INVALID_COLOR;
                txtBoxAddPatGender.ToolTip = "Muss entweder 'm' oder 'w' sein.";
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

            // validate station
            if (!ValidateTextBoxNotEmpty(txtBoxAddPatCorrectStation))
            {
                res = false;
            }
            if (!txtBoxAddPatCorrectStation.Text.Equals("Pädiatrie") &&
                !txtBoxAddPatCorrectStation.Text.Equals("Gynäkologie") &&
                !txtBoxAddPatCorrectStation.Text.Equals("Innere Medizin") &&
                !txtBoxAddPatCorrectStation.Text.Equals("Orthopädie"))
            {
                res = false;
                txtBoxAddPatCorrectStation.BackColor = INVALID_COLOR;
                txtBoxAddPatCorrectStation.ToolTip = "Muss eine der Stationen 'Pädiatrie', 'Gynäkologie', 'Innere Medizin' oder 'Orthopädie' sein.";
            }

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
			throw new System.NotImplementedException();
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
			throw new System.NotImplementedException();
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
			throw new System.NotImplementedException();
		}

		private void InitRelocations()
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Update_Overview_Tick(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Update_Notification_Tick(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Print_Error_Message(BedException e)
		{
			throw new System.NotImplementedException();
		}

		private void CheckPatientInput()
		{
			throw new System.NotImplementedException();
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
			//throw new System.NotImplementedException();
            // display patient if there is one in the bed
            if (!bed.IsEmpty())
            {
                Patient pat = bed.GetPatient();
                txtBoxDetailsPatId.Text = pat.GetPatientId().ToString();
                txtBoxDetailsPatName.Text = pat.GetLastName() + ", " + pat.GetFirstName();
                String genderString;
                if (pat.IsFemale()) {
                    genderString = "w";
                }
                else {
                    genderString = "m";
                }
                txtBoxDetailsPatGender.Text = genderString;
                txtBoxDetailsPatBirthday.Text = pat.GetBirthday().ToString();
                // TODO: station 
            }
		}

        private void SwitchToDetailsTab()
        {
            btnTabDetails.CssClass = CSS_CLASS_BTN_ACTIVE;
            btnTabSearch.CssClass = CSS_CLASS_BTN_INACTIVE;
            btnTabAdd.CssClass = CSS_CLASS_BTN_INACTIVE;

            divTabDetails.CssClass = CSS_CLASS_DIV_ACTIVE;
            divTabSearch.CssClass = CSS_CLASS_DIV_INACTIVE;
            divTabAdd.CssClass = CSS_CLASS_DIV_INACTIVE;
        }

        private void SwitchToSearchTab()
        {
            btnTabDetails.CssClass = CSS_CLASS_BTN_INACTIVE;
            btnTabSearch.CssClass = CSS_CLASS_BTN_ACTIVE;
            btnTabAdd.CssClass = CSS_CLASS_BTN_INACTIVE;

            divTabDetails.CssClass = CSS_CLASS_DIV_INACTIVE;
            divTabSearch.CssClass = CSS_CLASS_DIV_ACTIVE;
            divTabAdd.CssClass = CSS_CLASS_DIV_INACTIVE;
        }

        private void SwitchToAddTab()
        {
            btnTabDetails.CssClass = CSS_CLASS_BTN_INACTIVE;
            btnTabSearch.CssClass = CSS_CLASS_BTN_INACTIVE;
            btnTabAdd.CssClass = CSS_CLASS_BTN_ACTIVE;

            divTabDetails.CssClass = CSS_CLASS_DIV_INACTIVE;
            divTabSearch.CssClass = CSS_CLASS_DIV_INACTIVE;
            divTabAdd.CssClass = CSS_CLASS_DIV_ACTIVE;
        }

        private void ShowMessageBox(string message)
        {
            System.Web.HttpContext.Current.Response.Write("<script>alert('" + message + "')</script>");
        }
    }
}