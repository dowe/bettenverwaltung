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
    using System.Runtime.Serialization;
    using System.Text;

	public interface IBedView
	{
		int GetBedId();

		Patient GetPatient();

		EStation GetStation();

		bool IsEmpty();

		bool IsInRelocation();

		bool IsGettingCleaned();

	}
}

