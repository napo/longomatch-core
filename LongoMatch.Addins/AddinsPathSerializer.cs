//
//  Copyright (C) 2016 
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace LongoMatch.Addins.AddinsPathSerializer
{
	[GeneratedCodeAttribute ("xsd", "4.6.1055.0")]
	[SerializableAttribute ()]
	[DebuggerStepThroughAttribute ()]
	[DesignerCategoryAttribute ("code")]
	[XmlTypeAttribute (AnonymousType = true)]
	[XmlRootAttribute (Namespace = "", IsNullable = false)]
	public partial class Addins
	{
		private object[] itemsField;

		/// <remarks/>
		[XmlElementAttribute ("Directory", typeof(AddinsDirectory), Form = XmlSchemaForm.Unqualified, IsNullable = true)]
		[XmlElementAttribute ("Exclude", typeof(AddinsExclude), Form = XmlSchemaForm.Unqualified, IsNullable = true)]
		public object[] Items {
			get {
				return this.itemsField;
			}
			set {
				this.itemsField = value;
			}
		}
	}

	/// <remarks/>
	[GeneratedCodeAttribute ("xsd", "4.6.1055.0")]
	[SerializableAttribute ()]
	[DebuggerStepThroughAttribute ()]
	[DesignerCategoryAttribute ("code")]
	[XmlTypeAttribute (AnonymousType = true)]
	public partial class AddinsDirectory
	{
		private string includesubdirsField;

		private string valueField;

		/// <remarks/>
		[XmlAttributeAttribute ("include-subdirs")]
		public string includesubdirs {
			get {
				return this.includesubdirsField;
			}
			set {
				this.includesubdirsField = value;
			}
		}

		/// <remarks/>
		[XmlTextAttribute ()]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}

	/// <remarks/>
	[GeneratedCodeAttribute ("xsd", "4.6.1055.0")]
	[SerializableAttribute ()]
	[DebuggerStepThroughAttribute ()]
	[DesignerCategoryAttribute ("code")]
	[XmlTypeAttribute (AnonymousType = true)]
	public partial class AddinsExclude
	{
		private string valueField;

		/// <remarks/>
		[XmlTextAttribute ()]
		public string Value {
			get {
				return this.valueField;
			}
			set {
				this.valueField = value;
			}
		}
	}
}
