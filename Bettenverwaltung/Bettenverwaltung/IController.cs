﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Wenn der Code neu generiert wird, gehen alle Änderungen an dieser Datei verloren
// </auto-generated>
//------------------------------------------------------------------------------
namespace Bettenverwaltung
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public interface IController 
	{
		void AcceptRelocation(int relocationId);

		IBedView AddPatient(string firstname, string lastname, EStation station, DateTime birthday, bool isFemale);

		void CancelRelocation(int realocationId);

		void ConfirmRelocation(int realocationId);

		IBedView DismissPatient(int bedId);

		IBedView DisplayPatient(int bedId);

		List<Relocation> GetActiveRelocationList();

		List<IBedView> GetBettList();

		List<IBedView> SearchPatient(string term);

	}
}

