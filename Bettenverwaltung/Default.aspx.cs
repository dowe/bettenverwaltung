using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bettenverwaltung
{
    public partial class _Default : Page
    {
        private const int NUMBER_OF_BEDS = 200;

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
			//throw new System.NotImplementedException();
            // TEST
            for (int i = 0; i < 50; i++)
            {
                LinkButton btn = new LinkButton();
                btn.CssClass = "btnBedOccupied";
                btn.Click += Bed_Buttons_Click;
                btn.ID = i.ToString();
                divStationPaediatrie.Controls.Add(btn);
            }
		}

		protected virtual void Bed_Buttons_Click(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Dismiss_Patient_Click(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Add_Patient_Click(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
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
			throw new System.NotImplementedException();
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
    }
}