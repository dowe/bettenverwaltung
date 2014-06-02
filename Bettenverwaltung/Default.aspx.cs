﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Bettenverwaltung
{
    public partial class _Default : Page
    {
        private const int NUMBER_OF_BEDS = 200;

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
            // TEST
            not0.Style.Add("display", "none");
            not1.Style.Add("display", "none");
            // END TEST
            InitBeds();
            db = new BVContext();
            Bed b0 = new Bed();
            b0.bedId = 0;
            db.Beds.Add(b0);
            Bed b1 = new Bed();
            b1.bedId = 1;
            db.Beds.Add(b1);

            db.SaveChanges();

            var beds = from b in db.Beds orderby b.bedId select b;
            foreach (var bed in beds)
            {
                Console.WriteLine(bed.bedId.ToString() + bed.Patient);
            }
			//throw new System.NotImplementedException();
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
			throw new System.NotImplementedException();
		}

		protected virtual void Tab_Search_Click(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Tab_Add_Click(object sender, EventArgs e)
		{
			throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        private void SwitchToSearchTab()
        {
            throw new System.NotImplementedException();
        }

        private void SwitchToAddTab()
        {
            throw new System.NotImplementedException();
        }
    }
}